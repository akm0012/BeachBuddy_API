using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace BeachBuddy.Services.Twilio
{
    public class TwilioService : ITwilioService
    {
        private readonly ILogger<TwilioService> _logger;
        private readonly ITwilioRestClient _client;
        
        public TwilioService(ILogger<TwilioService> logger, ITwilioRestClient client)
        {
            _logger = logger;
            _client = client;
        }
        
        public async Task<MessageResource> SendSms(string from, string to, string text,
            IEnumerable<Uri> mediaUrls = null)
        {
            try
            {
                return await MessageResource.CreateAsync(new CreateMessageOptions(new PhoneNumber(to))
                {
                    From = new PhoneNumber(from),
                    Body = text,
                    MediaUrl = mediaUrls?.ToList(),
                    StatusCallback = null
                }, _client);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.InnerException?.Message);
                return null;
            }
        }
    }
}