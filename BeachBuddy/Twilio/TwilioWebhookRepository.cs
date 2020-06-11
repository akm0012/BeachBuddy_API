using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeachBuddy.Entities;
using BeachBuddy.Models;
using BeachBuddy.Models.Files;
using BeachBuddy.Repositories;
using BeachBuddy.Services.Twilio;
using Microsoft.Extensions.Logging;

namespace BeachBuddy.Twilio
{
    public class TwilioWebhookRepository : ITwilioWebhookRepository
    {
        private readonly ILogger<TwilioWebhookRepository> _logger;
        private readonly IBeachBuddyRepository _beachBuddyRepository;
        private readonly ITwilioService _twilioService;

        public TwilioWebhookRepository(ILogger<TwilioWebhookRepository> logger,
            IBeachBuddyRepository beachBuddyRepository, ITwilioService twilioService)
        {
            _logger = logger;
            _beachBuddyRepository = beachBuddyRepository;
            _twilioService = twilioService;
        }

        public async Task MessageReceived(string fromNumber, string toNumber, string text, List<RemoteFile> files)
        {
            var users = await _beachBuddyRepository.GetUsers(new UserResourceParameters()
            {
            PhoneNumber = fromNumber 
            });

            var userWhoSentMessage = users.FirstOrDefault();
            if (userWhoSentMessage == null)
            {
                await _twilioService.SendSms(toNumber, fromNumber, "Sorry, I don't know who you are.");
            }
            
            // Todo: do logic to create a new Requested Item
            
            // var requestedItem = new RequestedItem()
            // {
            //     Name = "Todo"
            // };
            //
            // await _beachBuddyRepository.AddRequestedItem(requestedItem);
        }
        
    }
}