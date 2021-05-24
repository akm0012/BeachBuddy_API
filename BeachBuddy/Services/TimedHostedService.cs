using System;
using System.Threading;
using System.Threading.Tasks;
using BeachBuddy.Models;
using BeachBuddy.Objects;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BeachBuddy.Services
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private const int TIMER_PERIOD_SEC = 5;

        private int _executionCount = 0;
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer;
        private readonly SunscreenReminderQueue _sunscreenReminderQueue;
        
        public TimedHostedService(ILogger<TimedHostedService> logger)
        {
            _logger = logger;
            _sunscreenReminderQueue = new SunscreenReminderQueue(logger);
        }

        public void AddSunscreenReminderToQueue(SunscreenReminder sunscreenReminder)
        {
            _sunscreenReminderQueue.AddReminder(sunscreenReminder);
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, 
                TimeSpan.FromSeconds(TIMER_PERIOD_SEC));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var currentTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            _logger.LogInformation("Checking if any SunscreenReminders need to be sent out: " + currentTime);
            
            // var count = Interlocked.Increment(ref _executionCount);

            var sunscreenIsDryReminders = _sunscreenReminderQueue.GetSunscreenIsDryRemindersThatAreDue();
            var reapplySunscreenReminders = _sunscreenReminderQueue.GetSunscreenReapplyRemindersThatAreDue();

            if (sunscreenIsDryReminders.Count > 0)
            {
                foreach (var reminder in sunscreenIsDryReminders)
                {
                    _logger.LogInformation("Todo: Send out 'Sunscreen is dry' reminders to User: %s", reminder.UserId);
                }
            }
            
            if (reapplySunscreenReminders.Count > 0)
            {
                foreach (var reminder in reapplySunscreenReminders)
                {
                    _logger.LogInformation("Todo: Send out 'Reapply' reminders to User: %s", reminder.UserId);
                }
            }
        }
        
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}