using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010;
using Twilio.Rest.Api.V2010.Account;

namespace BeachBuddy.Services.Twilio
{
    public interface ITwilioService
    {
        Task<MessageResource> SendSms(string from, string to, string text, IEnumerable<Uri> mediaUrls = null);
        Task<BalanceResource> GetAccountBalance();
    }
}