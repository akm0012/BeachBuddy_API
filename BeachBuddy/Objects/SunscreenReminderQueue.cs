using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeachBuddy.Models;
using Microsoft.Extensions.Logging;

namespace BeachBuddy.Objects
{
    /**
     * This is a container that will hold all the Sunscreen reminders. You can only have one
     * reminder per UserId. 
     */
    public class SunscreenReminderQueue
    {
        private readonly Dictionary<Guid, SunscreenReminder> _reminderDict = new Dictionary<Guid, SunscreenReminder>();

        private readonly ILogger _logger;
        public SunscreenReminderQueue(ILogger logger)
        {
            _logger = logger;
        }

        /**
         * Adds a new Sunscreen reminder to the Queue. If there is already a reminder for that User
         * it will be overrided with the new reminder.  
         */
        public void AddReminder(SunscreenReminder sunscreenReminder)
        {
            _logger.LogInformation("Adding a reminder....");
            _logger.LogInformation("Dry reminder time: " + sunscreenReminder.IsDryReminderTimeSeconds);
            _logger.LogInformation("Reapply reminder time: " + sunscreenReminder.ReapplyReminderTimeSeconds);

            if (_reminderDict.ContainsKey(sunscreenReminder.UserId))
            {
                // This User already has a reminder. Remove it before we add the new one.
                _logger.LogInformation("User %s already had a reminder. Replacing it with the new one.", sunscreenReminder.UserId);
                _reminderDict.Remove(sunscreenReminder.UserId);
            }

            _reminderDict.Add(sunscreenReminder.UserId, sunscreenReminder);
        }

        /**
         * Will return a list of "Sunscreen is Dry" Reminders that are due to be processed.
         *
         * When reminders are returned here, their flag will be switched indicating that they have been processed.
         *
         * The will remain in the Queue until the reapply reminder has been sent. 
         */
        public List<SunscreenReminder> GetSunscreenIsDryRemindersThatAreDue()
        {
            var dueReminders = new List<SunscreenReminder>();
            var currentTimeSeconds = DateTimeOffset.Now.ToUnixTimeSeconds();

            // Look for reminders that are due and have not been sent yet...
            foreach (var entry in _reminderDict.Where(
                entry => entry.Value.IsDryReminderTimeSeconds <= currentTimeSeconds && !entry.Value.HasIsDryReminderBeenSent))
            {
                _logger.LogInformation("Time's up! User %s sunscreen is dry!", entry.Value.UserId);
                entry.Value.HasIsDryReminderBeenSent = true;
                dueReminders.Add(entry.Value);
            }

            return dueReminders;
        }
        
        /**
         * Will return a list of "Reapply Sunscreen" Reminders that are due to be processed.
         *
         * When reminders are returned here, their flag will be switched indicating that they have been processed.
         *
         * When the reminders are returned, they will be removed from the Queue.
         */
        public List<SunscreenReminder> GetSunscreenReapplyRemindersThatAreDue()
        {
            var dueReminders = new List<SunscreenReminder>();
            var currentTimeSeconds = DateTimeOffset.Now.ToUnixTimeSeconds();

            // Look for reminders that are due and have not been sent yet...
            foreach (var entry in _reminderDict.Where(
                entry => entry.Value.ReapplyReminderTimeSeconds <= currentTimeSeconds && !entry.Value.HasReapplyReminderBeenSent))
            {
                _logger.LogInformation("Time's up! User %s should reapply!", entry.Value.UserId);
                entry.Value.HasReapplyReminderBeenSent = true;
                dueReminders.Add(entry.Value);
             
                // Remove the item from the dictionary  
                _reminderDict.Remove(entry.Key);
            }

            return dueReminders;
        }
    }
}