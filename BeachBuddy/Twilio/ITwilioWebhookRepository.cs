using System.Collections.Generic;
using System.Threading.Tasks;
using BeachBuddy.Models.Files;

namespace BeachBuddy.Twilio
{
    public interface ITwilioWebhookRepository
    {
        Task MessageReceived(string fromNumber, string toNumber, string text, List<RemoteFile> files);
    }
}