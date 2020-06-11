using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeachBuddy.Models.Files;
using Microsoft.AspNetCore.Mvc;
using Twilio.AspNet.Common;

namespace BeachBuddy.Twilio
{
    [ApiController]
    [Route("twilio/event")]
    public class TwilioHttpController : ControllerBase
    {
        private readonly ITwilioWebhookRepository _twilioWebhookRepository;

        public TwilioHttpController(ITwilioWebhookRepository twilioWebhookRepository)
        {
            _twilioWebhookRepository = twilioWebhookRepository;
        }
        
        [HttpPost("sms")]
        public async Task SmsReceived([FromForm] SmsRequest request, [FromForm] int numMedia)
        {
            var media = GetMediaFromRequest(numMedia);
            await _twilioWebhookRepository.MessageReceived(request.From, request.To, request.Body, media);
        }
      
        private List<RemoteFile> GetMediaFromRequest(int numMedia)
        {
            var media = new List<RemoteFile>();
            for (var i = 0; i < numMedia; i++)
            {
                var contentType = Request.Form[$"MediaContentType{i}"].FirstOrDefault();
                var url = Request.Form[$"MediaUrl{i}"].FirstOrDefault();
                media.Add(new RemoteFile {Url = url, ContentType = contentType});
            }

            return media;
        }
    }
}