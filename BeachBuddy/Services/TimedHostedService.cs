using System;
using System.Threading;
using System.Threading.Tasks;
using BeachBuddy.Objects;
using BeachBuddy.Repositories;
using BeachBuddy.Services.Twilio;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BeachBuddy.Services
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private const int TIMER_PERIOD_SEC = 5;

        private readonly ILogger<TimedHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly BackgroundTaskQueue _backgroundTaskQueue;
        private Timer _timer;
        private readonly SunscreenReminderQueue _sunscreenReminderQueue;

        public TimedHostedService(ILogger<TimedHostedService> logger,
            IServiceProvider serviceProvider,
            BackgroundTaskQueue backgroundTaskQueue)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _backgroundTaskQueue = backgroundTaskQueue;
            _sunscreenReminderQueue = new SunscreenReminderQueue(logger);
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

            // Add any new Reminders
            var remindersToAdd = _backgroundTaskQueue.DequeueSunscreenReminders();
            foreach (var reminderToAdd in remindersToAdd)
            {
                _sunscreenReminderQueue.AddReminder(reminderToAdd);
            }
            
            // Process existing ones
            var sunscreenIsDryReminders = _sunscreenReminderQueue.GetSunscreenIsDryRemindersThatAreDue();
            var reapplySunscreenReminders = _sunscreenReminderQueue.GetSunscreenReapplyRemindersThatAreDue();

            if (sunscreenIsDryReminders.Count > 0)
            {
                foreach (var reminder in sunscreenIsDryReminders)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetService<IBeachBuddyRepository>();
                        var user = context.GetUser(reminder.UserId).Result;

                        var twilioService = scope.ServiceProvider.GetService<ITwilioService>();
                        _logger.LogInformation($"{user.FirstName} sunscreen should be dry. Sending text...");
                        twilioService.SendSms(user.PhoneNumber, $"{user.FirstName}, your sunscreen should be dry by now. Go get in that sun! 😎");
                    }
                }
            }

            if (reapplySunscreenReminders.Count > 0)
            {
                foreach (var reminder in reapplySunscreenReminders)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetService<IBeachBuddyRepository>();
                        var user = context.GetUser(reminder.UserId).Result;
                        
                        var twilioService = scope.ServiceProvider.GetService<ITwilioService>();
                        _logger.LogInformation($"{user.FirstName} needs to reapply. Sending text...");
                        twilioService.SendSms(user.PhoneNumber, $"{user.FirstName}, it's time to reapply sunscreen! 🥵 \n\nText \"done\" if you want to be reminded in another 2 hours.");
                    }
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