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
                IsDryReminderTimeSeconds = DateTimeOffset.Now.ToUnixTimeSeconds() + DRY_TIME_SEC, 
                ReapplyReminderTimeSeconds = DateTimeOffset.Now.ToUnixTimeSeconds() + REAPPLY_TIME_SEC,
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