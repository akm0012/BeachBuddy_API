using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeachBuddy.Entities;
using BeachBuddy.Enums;
using BeachBuddy.Models;
using BeachBuddy.Models.Files;
using BeachBuddy.Repositories;
using BeachBuddy.Services;
using BeachBuddy.Services.Notification;
using BeachBuddy.Services.Twilio;
using BeachBuddy.Services.Weather;
using Microsoft.Extensions.Logging;

namespace BeachBuddy.Twilio
{
    public class TwilioWebhookRepository : ITwilioWebhookRepository
    {
        private const string StatusCommand = "status";
        private const string ViewErrorCommand = "errors";
        
        private readonly ILogger<TwilioWebhookRepository> _logger;
        private readonly IBeachBuddyRepository _beachBuddyRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly ITwilioService _twilioService;
        private readonly INotificationService _notificationService;
        private readonly IWeatherService _weatherService;
        private readonly BackgroundTaskQueue _backgroundTaskQueue;

        public TwilioWebhookRepository(ILogger<TwilioWebhookRepository> logger,
            IBeachBuddyRepository beachBuddyRepository, 
            IStatusRepository statusRepository, 
            ITwilioService twilioService,
            INotificationService notificationService,
            BackgroundTaskQueue backgroundTaskQueue)
        {
            _logger = logger;
            _beachBuddyRepository = beachBuddyRepository;
            _statusRepository = statusRepository;
            _twilioService = twilioService;
            _notificationService = notificationService;
            _backgroundTaskQueue = backgroundTaskQueue;
        }

        public async Task MessageReceived(string fromNumber, string toNumber, string text, List<RemoteFile> files)
        {
            _logger.LogInformation($"New Incoming SMS from {fromNumber}: {text}");

            IEnumerable<User> users = new List<User>();
            try
            {
                users = await _beachBuddyRepository.GetUsers(new UserResourceParameters()
                {
                    PhoneNumber = fromNumber
                });
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while looking up User: {e.Message}");
            }

            var userWhoSentMessage = users.FirstOrDefault();
            if (userWhoSentMessage == null)
            {
                // If the DB is down and we are trying to look up the Status, we should let this through.
                if (text.ToLower().Equals(ViewErrorCommand) || text.ToLower().Equals(StatusCommand))
                {    
                    _logger.LogInformation("Database may be down. Letting unknown User through.");
                }
                else
                {
                    await _twilioService.SendSms(fromNumber, "Sorry, I don't know who you are.");
                    return;
                }
            }

            var words = text.Split(" ");
            var firstWordOfMessage = words[0].ToLower();

            var firstChar = firstWordOfMessage.ToCharArray()[0];
            if (firstChar == '+' || firstChar == '-')
            {
                await ProcessScoreUpdate(text, userWhoSentMessage, fromNumber, toNumber);
                return;
            }

            string secondWordOfMessage = null;
            if (words.Length >= 2)
            {
                secondWordOfMessage = text.Split(" ")[1].ToLower();
            }

            switch (firstWordOfMessage.ToLower())
            {
                case "done":
                    _backgroundTaskQueue.QueueSunscreenReminderForUser(userWhoSentMessage.Id);
                    await _twilioService.SendSms(fromNumber, "Great! I'll let you know once it's dry and then again when it's time to reapply. ⏱");
                    return;
                
                case "refresh":
                    await _notificationService.sendNotification(null, NotificationType.DashboardPulledToRefresh, null, null, true);
                    return;
                
                case "remove":
                    await RemoveItems(fromNumber, toNumber, text, firstWordOfMessage);
                    // Send data notification so app will update  
                    await _notificationService.sendNotification(null, NotificationType.RequestedItemRemoved, null, null, true);
                    return;

                case "list":
                    await ShowAllItems(fromNumber);
                    return;

                case "h":
                    await ShowHelp(fromNumber);
                    return;

                case "nukefromorbit":
                    await RemoveItems(fromNumber, toNumber, text, firstWordOfMessage, true);
                    // Send data notification so app will update
                    await _notificationService.sendNotification(null, NotificationType.RequestedItemRemoved, null, null, true);
                    return;

                case "bal":
                    await GetBalance(fromNumber);
                    return;

                case StatusCommand:
                    await CheckSystemStatus(fromNumber);
                    return;
                
                case ViewErrorCommand:
                    await SendSystemStatusErrors(fromNumber);
                    return;
                
                case "add":
                    if (secondWordOfMessage != null && secondWordOfMessage == "game")
                    {
                        var gameName = text.Substring("add game".Length).Trim();
                        await AddGame(gameName, fromNumber);
                        return;
                    }

                    break;

                case "leaderboard":
                    await ShowLeaderBoard(fromNumber);
                    return;
            }

            var requestedItemToSave = new RequestedItem();

            // Check if there is a quantity 
            if (int.TryParse(firstWordOfMessage, out var quantity))
            {
                text = text.Substring(firstWordOfMessage.Length).Trim();
            }
            else
            {
                quantity = 1;
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                await _twilioService.SendSms(fromNumber,
                    $"Sorry, I didn't detect anything to add to the list.");
                return;
            }

            requestedItemToSave.Id = Guid.NewGuid();
            requestedItemToSave.Count = quantity;
            requestedItemToSave.CreatedDateTime = DateTimeOffset.UtcNow;
            requestedItemToSave.RequestedByUserId = userWhoSentMessage.Id;
            requestedItemToSave.RequestedByUser = userWhoSentMessage;
            requestedItemToSave.Name = text;

            try
            {
                await _beachBuddyRepository.AddRequestedItem(requestedItemToSave);
                await _beachBuddyRepository.Save();
            }
            catch (Exception e)
            {
                _logger.LogError($"Something did not work when trying to save a requested item via Twilio: {e.Message}",
                    e);
                await _twilioService.SendSms(fromNumber,
                    $"Sorry, I couldn't add {text} to the list. Try again.)");
                return;
            }

            await _notificationService.sendNotification(
                await _beachBuddyRepository.GetRequestedItem(requestedItemToSave.Id),
                NotificationType.RequestedItemAdded,
                $"\"{requestedItemToSave.Name}\" added to list", $"{userWhoSentMessage.FirstName} " +
                                                                 $"added {requestedItemToSave.Count} {requestedItemToSave.Name} to the Beach List.",
                false);

            await _twilioService.SendSms(fromNumber, $"\"{text}\" was added to the list!");
        }

        private async Task RemoveItems(string fromNumber, string toNumber, string text, string firstWordOfMessage,
            bool nuke = false)
        {
            text = text.Substring(firstWordOfMessage.Length).Trim();

            if (string.IsNullOrWhiteSpace(text) && !nuke)
            {
                await _twilioService.SendSms(fromNumber,
                    $"Nothing was removed. If you want to remove everything send, \"NukeFromOrbit\"");
                return;
            }

            var numItemsDeleted = 0;
            var itemsToDelete = await _beachBuddyRepository.GetRequestedItems(text);
            if (nuke)
            {
                itemsToDelete = await _beachBuddyRepository.GetNotCompletedRequestedItems();
            }

            foreach (var item in itemsToDelete)
            {
                _beachBuddyRepository.DeleteRequestedItem(item);
                numItemsDeleted++;
            }

            await _beachBuddyRepository.Save();

            await _twilioService.SendSms(fromNumber, $"Removed {numItemsDeleted} item(s) from the list.");
        }

        private async Task ShowAllItems(string fromNumber)
        {
            var outstandingItems = await _beachBuddyRepository.GetNotCompletedRequestedItems();
            var namesOfOutstandingItems =
                outstandingItems.Aggregate("", (current, item) => current + $"{item.Name} ({item.Count})\n");

            if (string.IsNullOrWhiteSpace(namesOfOutstandingItems))
            {
                namesOfOutstandingItems = "(empty list)";
            }

            await _twilioService.SendSms(fromNumber, $"{namesOfOutstandingItems.Trim()}");
        }

        private async Task ShowHelp(string fromNumber)
        {
            var helpText = "You can send these commands:\n\n" +
                                    "{quantity} {itemName} - Will add a new item. {quantity} is optional.\n\n" +
                                    "remove {itemName} - Will remove {nameOfItem} from the list name.\n\n" +
                                    "list - Will show all the uncompleted items in the list.\n\n" +
                                    "add game {gameName} - Will add a new game.\n\n" +
                                    "leaderboard - Will show all the scores.\n\n" +
                                    "+1 {userName} {gameName} - Will increment your score. {userName} is optional.\n\n" +
                                    "done - Will indicate you are done putting on sunscreen.\n\n" +
                                    "refresh - Will force refresh the TV Dashboard.\n\n" +
                                    "bal - Will show current Twilio balance.\n\n" +
                                    $"{StatusCommand} - Will show system status.\n\n" +
                                    "NukeFromOrbit - Will delete all beach list items.";

            await _twilioService.SendSms(fromNumber, $"{helpText.Trim()}");
        }

        /**
         * Checks the status of the various services and repositories to make sure things are still working. 
         */
        private async Task CheckSystemStatus(string fromNumber)
        {
            var systemStatus = await _statusRepository.GetSystemStatus();
            
            var statusString = new StringBuilder();
            statusString.Append(GetStatusString("Database", systemStatus.IsDatabaseOk));
            statusString.AppendLine();
            statusString.Append(GetStatusString("Beach Conditions", systemStatus.IsBeachConditionsOk));
            statusString.AppendLine();
            statusString.Append(GetStatusString("Weather Forecast", systemStatus.IsWeatherOk));
            statusString.AppendLine();
            statusString.Append(GetStatusString("Current UV", systemStatus.IsCurrentUvIndexOk));
            statusString.AppendLine();
            statusString.Append(GetStatusString("Users", systemStatus.IsGetUsersOk));
            statusString.AppendLine();
            statusString.Append(GetStatusString("Twilio", systemStatus.IsCurrentUvIndexOk));
            statusString.AppendLine();
            statusString.Append($"Twilio balance: ${systemStatus.TwilioBalance}");

            if (systemStatus.ErrorMessages.Count > 0)
            {
                statusString.AppendLine();
                statusString.Append("Errors were present. ☠️ Send 'errors' to see them.");
            }

            await _twilioService.SendSms(fromNumber, statusString.ToString());
        }

        /**
         * Sends any System Status errors. 
         */
        private async Task SendSystemStatusErrors(string fromNumber)
        {
            var systemStatus = await _statusRepository.GetSystemStatus();

            if (systemStatus.ErrorMessages.Count > 0)
            {
                var errorString = new StringBuilder("Errors:");

                foreach (var error in systemStatus.ErrorMessages)
                {
                    errorString.AppendLine();
                    errorString.Append(error);
                }
                await _twilioService.SendSms(fromNumber, errorString.ToString());
            }
            else
            {
                await _twilioService.SendSms(fromNumber, "No errors found. ✅");
            }
        }
        
        private string GetStatusString(string statusName, bool isOk)
        {
            return isOk ? $"✅ {statusName}" : $"❌ {statusName}";
        }
        
        private async Task GetBalance(string fromNumber)
        {
            var account = await _twilioService.GetAccountBalance();

            await _twilioService.SendSms(fromNumber, $"Current balance: ${account.Balance}");
        }

        private async Task ShowLeaderBoard(string fromNumber)
        {
            var users = await _beachBuddyRepository.GetUsers();

            var sb = new StringBuilder();

            foreach (var user in users)
            {
                sb.Append(user.FirstName);

                var userScores = user.Scores.OrderBy(x => x.Name).ToList();
                foreach (var score in userScores)
                {
                    sb.Append($"\n{score.Name} ({score.WinCount})");
                }

                sb.Append("\n\n");
            }

            var textToSend = sb.ToString().Trim();

            await _twilioService.SendSms(fromNumber, textToSend);
        }

        private async Task AddGame(string gameName, string fromNumber)
        {
            if (string.IsNullOrWhiteSpace(gameName))
            {
                await _twilioService.SendSms(fromNumber,
                    $"The game name can not be empty.");
                return;
            }

            if (gameName.Length > 15)
            {
                await _twilioService.SendSms(fromNumber,
                    "The game name can not be longer than 15 characters.");
                return;
            }

            var users = await _beachBuddyRepository.GetUsers();

            foreach (var user in users)
            {
                var scoreToAdd = new Score()
                {
                    Id = Guid.NewGuid(),
                    Name = gameName,
                    UserId = user.Id,
                    WinCount = 0,
                    User = user
                };

                user.Scores.Add(scoreToAdd);

                _beachBuddyRepository.UpdateUser(user);
                await _beachBuddyRepository.AddScore(scoreToAdd);
            }

            await _beachBuddyRepository.Save();

            await _twilioService.SendSms(fromNumber,
                $"{gameName} was added! If you play outside, don't forget sunscreen!");
        }

        private async Task ProcessScoreUpdate(String text,
            User userWhoSentMessage,
            string fromNumber,
            string toNumber)
        {
            var words = text.Split(" ");
            if (words.Length < 2)
            {
                await _twilioService.SendSms(fromNumber,
                    "I'm not sure what game you are referring to... Send 'h' for help.");
                return;
            }

            var firstWordOfMessage = words[0].ToLower();
            var firstChar = firstWordOfMessage.ToCharArray()[0];

            var secondWordOfMessage = text.Split(" ")[1].ToLower();

            var userWhoseScoreToManipulate = userWhoSentMessage;
            
            var gameName = text.Substring(firstWordOfMessage.Length).Trim();
            
            var users = await _beachBuddyRepository.GetUsers(new UserResourceParameters()
            {
                Name = secondWordOfMessage
            });

            var userMentionedInMessage = users.FirstOrDefault();
            if (userMentionedInMessage != null)
            {
                userWhoseScoreToManipulate = userMentionedInMessage;
                
                // We need to recalculate the Game Name so we can take out the Users name
                gameName = text.Substring(firstWordOfMessage.Length + " ".Length + secondWordOfMessage.Length).Trim();
            }

            var scoreToEdit = await _beachBuddyRepository.GetScore(userWhoseScoreToManipulate.Id, gameName);
            if (scoreToEdit == null)
            {
                await _twilioService.SendSms(fromNumber,
                    $"Sorry, I could not find the game, {gameName}. You may need to add it first. Send 'h' for help.");
                return;
            }

            switch (firstChar)
            {
                case '+':
                {
                    Double.TryParse(firstWordOfMessage, out var scoreChange);

                    scoreToEdit.WinCount += Convert.ToInt32(scoreChange);
                    break;
                }
                case '-':
                {
                    Double.TryParse(firstWordOfMessage, out var score);
                    var scoreChange = Math.Abs(score);

                    var winCount = Convert.ToInt32(scoreChange);

                    if (winCount <= 0)
                    {
                        await _twilioService.SendSms(fromNumber,
                            $"{userWhoseScoreToManipulate.FirstName} already has 0 wins in {gameName}");
                        return;
                    }
                
                    scoreToEdit.WinCount -= winCount;
                    break;
                }
            }
            
            _beachBuddyRepository.UpdateScore(scoreToEdit);
            await _beachBuddyRepository.Save();
            
            await _notificationService.sendNotification(null, NotificationType.ScoreUpdated, null, null, true);
            await _twilioService.SendSms(fromNumber,
                $"{userWhoseScoreToManipulate.FirstName} now has {scoreToEdit.WinCount} win(s) in {gameName}!");
        }
    }
}