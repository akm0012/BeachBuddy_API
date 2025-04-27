using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeachBuddy.Entities;
using BeachBuddy.Enums;
using BeachBuddy.Repositories;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;

namespace BeachBuddy.Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IBeachBuddyRepository _beachBuddyRepository;

        public NotificationService(ILogger<NotificationService> logger, IBeachBuddyRepository beachBuddyRepository)
        {
            _logger = logger;
            _beachBuddyRepository = beachBuddyRepository;
        }

        public async Task sendNotification(RequestedItem requestedItem, 
            NotificationType notificationType,
            string notificationTitle,
            string notificationMessage, 
            bool dataOnly)
        {
            var devices = await _beachBuddyRepository.GetDevices();

            Dictionary<string, string> data;
            
            if (dataOnly)
            {
                data = new Dictionary<string, string>
                {
                    {"notificationType", notificationType.ToString()},
                    {"updateOnly", "true"}
                };  
            }
            else
            {
                data = new Dictionary<string, string>
                {
                    {"notificationType", notificationType.ToString()},
                    {"updateOnly", "false"},
                    {"itemId", $"{requestedItem.Id}"},
                    {"name", $"{requestedItem.Name}"},
                    {"count", $"{requestedItem.Count}"},
                    {"sentByUserId", $"{requestedItem.RequestedByUserId}"}
                };                
            }

            var deviceList = devices.ToList();
            var results = await SendFcmNotification(deviceList, notificationTitle, notificationMessage, dataOnly, data);

            for (var i = 0; i < results.Count; i++)
            {
                var response = results[i];
                var device = deviceList[i];
                if (response.IsSuccess)
                {
                    _logger.LogDebug($"Message was sent!");
                    // Woohoo!
                }
                else
                {
                    _logger.LogWarning(response.Exception.InnerException,
                        $"Error sending notification to device {device.DeviceToken}");
                    if (response.Exception.MessagingErrorCode.HasValue &&
                        response.Exception.MessagingErrorCode == MessagingErrorCode.Unregistered)
                    {
                        // the token has been unregistered, must delete the device record
                        _beachBuddyRepository.DeleteDevice(device);
                        await _beachBuddyRepository.Save();
                        _logger.LogDebug($"Device {device.DeviceToken} has been unregistered");
                    }
                }
            }
        }

        private async Task<IReadOnlyList<SendResponse>> SendFcmNotification(IReadOnlyCollection<Device> devices,
            string notificationTitle, string notificationMessage, bool dataOnly,
            IReadOnlyDictionary<string, string> data)
        {
            if (devices.Count > 0)
            {
                var tokens = devices.Select(d => d.DeviceToken).ToList();
                var allData = new Dictionary<string, string> {{"messageText", notificationMessage}}
                    .Concat(data)
                    .ToDictionary(x => x.Key, x => x.Value);

                var firebaseMessage = new MulticastMessage
                {
                    Tokens = tokens,
                    Data = allData
                };

                if (!dataOnly)
                {
                    firebaseMessage.Apns = new ApnsConfig {Aps = new Aps {Sound = "seagulls"}}; // This sound does not work on Android
                    firebaseMessage.Android = new AndroidConfig
                    {
                        TimeToLive = new TimeSpan(12,0, 0),
                        Notification = new AndroidNotification
                        {
                            ChannelId = "RequestedItemsChannel",
                            Sound = "seagulls"
                        }
                    };
                    firebaseMessage.Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = notificationTitle,
                        Body = notificationMessage
                    };
                }

                var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(firebaseMessage);
                return response.Responses;
            }

            return new List<SendResponse>();
        }
    }
}