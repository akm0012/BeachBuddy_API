using System.Threading.Tasks;
using Twilio.Clients;
using Twilio.Http;

namespace BeachBuddy.Services.Twilio
{
    public class TwilioClient : ITwilioRestClient
    {
        private readonly ITwilioRestClient _innerClient;

        public TwilioClient(System.Net.Http.HttpClient httpClient)
        {
            _innerClient = new TwilioRestClient(
                APIKeys.TwilioAccoutSid,
                APIKeys.TwilioAuthToken,
                httpClient: new SystemNetHttpClient(httpClient)
            );
        }

        public Response Request(Request request) => _innerClient.Request(request);
        public Task<Response> RequestAsync(Request request) => _innerClient.RequestAsync(request);

        public string AccountSid => _innerClient.AccountSid;
        public string Region => _innerClient.Region;
        public HttpClient HttpClient => _innerClient.HttpClient;
    }
}