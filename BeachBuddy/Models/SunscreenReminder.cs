using System;

namespace BeachBuddy.Models
{
    public class SunscreenReminder
    {
        public Guid UserId { get; set; }
        
        public bool HasIsDryReminderBeenSent { get; set; }
        public long IsDryReminderTimeSeconds { get; set; }

        public bool HasReapplyReminderBeenSent { get; set; }
        public long ReapplyReminderTimeSeconds { get; set; }
    }
}