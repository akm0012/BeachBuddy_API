using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeachBuddy.Models;

namespace BeachBuddy.Services
{
    public class BackgroundTaskQueue
    {
        private const int DRY_TIME_SEC = 20 * 60; // 20 minutes
        private const int REAPPLY_TIME_SEC = 120 * 60; // 120 minutes (2 hours)
        
        private readonly List<SunscreenReminder> _sunscreenReminders = new List<SunscreenReminder>();

        public void QueueSunscreenReminderForUser(Guid userId)
        {
            var reminder = new SunscreenReminder
            {
                UserId = userId,
                IsDryReminderTimeSeconds = DateTimeOffset.Now.ToUnixTimeSeconds() + 10, // todo change to const
                ReapplyReminderTimeSeconds = DateTimeOffset.Now.ToUnixTimeSeconds() + 15, // todo change to const
                HasIsDryReminderBeenSent = false,
                HasReapplyReminderBeenSent = false
            };
            
            _sunscreenReminders.Add(reminder);
        }

        public List<SunscreenReminder> DequeueSunscreenReminders()
        {
            var listToReturn = new List<SunscreenReminder>(_sunscreenReminders);
            
            _sunscreenReminders.Clear();
            
            return listToReturn;
        }
    }
}