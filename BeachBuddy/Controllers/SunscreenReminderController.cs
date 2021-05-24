using System;
using System.Threading;
using System.Threading.Tasks;
using BeachBuddy.Enums;
using BeachBuddy.Models;
using BeachBuddy.Services;
using BeachBuddy.Services.Notification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BeachBuddy.Controllers
{
    [ApiController]
    [Route("api/sunscreenReminder")]
    public class SunscreenReminderController : ControllerBase
    {
        private readonly ILogger<SunscreenReminderController> _logger;
        private readonly INotificationService _notificationService;
        private readonly TimedHostedService _timedHostedService;

        public SunscreenReminderController(
            TimedHostedService timedHostedService,
            INotificationService notificationService,
            ILogger<SunscreenReminderController> logger)
        {
            _timedHostedService = timedHostedService;
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpPost("timerTest")]
        public async Task<ActionResult> TimerTest()
        {
            var reminder = new SunscreenReminder
            {
                UserId = Guid.NewGuid(),
                IsDryReminderTimeSeconds = DateTimeOffset.Now.ToUnixTimeSeconds() + 10,
                ReapplyReminderTimeSeconds = DateTimeOffset.Now.ToUnixTimeSeconds() + 15,
                HasIsDryReminderBeenSent = false,
                HasReapplyReminderBeenSent = false
            };

            _timedHostedService.AddSunscreenReminderToQueue(reminder);

            return Ok();
        }
    }
}