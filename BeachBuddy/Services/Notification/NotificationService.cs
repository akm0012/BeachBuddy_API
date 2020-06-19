using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeachBuddy.Entities;
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

        public async Task sendNotification(RequestedItem requestedItem, string notificationTitle,
            string notificationMessage, bool dataOnly)
        {
            var devices = new List<Device>();

            var data = new Dictionary<string, string>
            {
                {"myFirstDataField", "Hello World!"}
            };

            var results = await SendFcmNotification(devices, notificationTitle, notificationMessage, dataOnly, data);

            for (var i = 0; i < results.Count; i++)
            {
                var response = results[i];
                var device = devices[i];
                if (response.IsSuccess)
                {
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

        private async Task<IReadOnlyList<SendResponse>> SendFcmNotification(List<Device> devices,
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
                    firebaseMessage.Apns = new ApnsConfig {Aps = new Aps {Sound = "default"}};
                    firebaseMessage.Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = notificationTitle,
                        Body = notificationMessage
                    };
                }

                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(firebaseMessage);
                return response.Responses;
            }

            return new List<SendResponse>();
        }
    }
}