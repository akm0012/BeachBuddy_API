using System;
using System.Threading;
using System.Threading.Tasks;
using BeachBuddy.Enums;
using BeachBuddy.Models;
using BeachBuddy.Repositories;
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
        private readonly IBeachBuddyRepository _beachBuddyRepository;
        private readonly BackgroundTaskQueue _backgroundTaskQueue;

        public SunscreenReminderController(
            IBeachBuddyRepository beachBuddyRepository,
            BackgroundTaskQueue backgroundTaskQueue,
            ILogger<SunscreenReminderController> logger)
        {
            _beachBuddyRepository = beachBuddyRepository ??
                                    throw new ArgumentNullException(nameof(beachBuddyRepository));
            _backgroundTaskQueue = backgroundTaskQueue;
            _logger = logger;
        }

        [HttpPost("sunscreenApplied/{userId}")]
        public async Task<ActionResult> AddSunscreenReminder(Guid userId)
        {
            var userExists = await _beachBuddyRepository.UserExists(userId);
            if (!userExists)
            {
                return NotFound();
            }

            _backgroundTaskQueue.QueueSunscreenReminderForUser(userId);

            return Ok();
        }
    }
}