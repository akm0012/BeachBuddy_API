using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeachBuddy.Entities;
using BeachBuddy.Models;
using BeachBuddy.Models.Files;
using BeachBuddy.Repositories;
using BeachBuddy.Services.Notification;
using BeachBuddy.Services.Twilio;
using BeachBuddy.Services.Weather;
using Microsoft.Extensions.Logging;

namespace BeachBuddy.Twilio
{
    public class TwilioWebhookRepository : ITwilioWebhookRepository
    {
        private readonly ILogger<TwilioWebhookRepository> _logger;
        private readonly IBeachBuddyRepository _beachBuddyRepository;
        private readonly ITwilioService _twilioService;
        private readonly INotificationService _notificationService;

        public TwilioWebhookRepository(ILogger<TwilioWebhookRepository> logger,
            IBeachBuddyRepository beachBuddyRepository, ITwilioService twilioService,
            INotificationService notificationService)
        {
            _logger = logger;
            _beachBuddyRepository = beachBuddyRepository;
            _twilioService = twilioService;
            _notificationService = notificationService;
        }

        public async Task MessageReceived(string fromNumber, string toNumber, string text, List<RemoteFile> files)
        {
            _logger.LogInformation($"New Incoming SMS from {fromNumber}: {text}");

            var users = await _beachBuddyRepository.GetUsers(new UserResourceParameters()
            {
                PhoneNumber = fromNumber
            });

            var userWhoSentMessage = users.FirstOrDefault();
            if (userWhoSentMessage == null)
            {
                await _twilioService.SendSms(toNumber, fromNumber, "Sorry, I don't know who you are.");
                return;
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
                case "remove":
                    await RemoveItems(fromNumber, toNumber, text, firstWordOfMessage);
                    // Send data notification so app will update
                    await _notificationService.sendNotification(null, null, null, true);
                    return;

                case "list":
                    await ShowAllItems(fromNumber, toNumber);
                    return;

                case "h":
                    await ShowHelp(fromNumber, toNumber);
                    return;

                case "nukefromorbit":
                    await RemoveItems(fromNumber, toNumber, text, firstWordOfMessage, true);
                    // Send data notification so app will update
                    await _notificationService.sendNotification(null, null, null, true);
                    return;

                case "bal":
                    await GetBalance(fromNumber, toNumber);
                    return;

                case "add":
                    if (secondWordOfMessage != null && secondWordOfMessage == "game")
                    {
                        var gameName = text.Substring("add game".Length).Trim();
                        await AddGame(gameName, fromNumber, toNumber);
                        return;
                    }

                    break;

                case "leaderboard":
                    await ShowLeaderBoard(fromNumber, toNumber);
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
                await _twilioService.SendSms(toNumber, fromNumber,
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
                await _twilioService.SendSms(toNumber, fromNumber,
                    $"Sorry, I couldn't add {text} to the list. Try again.)");
                return;
            }

            await _notificationService.sendNotification(
                await _beachBuddyRepository.GetRequestedItem(requestedItemToSave.Id),
                $"\"{requestedItemToSave.Name}\" added to list", $"{userWhoSentMessage.FirstName} " +
                                                                 $"added {requestedItemToSave.Count} {requestedItemToSave.Name} to the Beach List.",
                false);

            await _twilioService.SendSms(toNumber, fromNumber, $"\"{text}\" was added to the list!");
        }

        private async Task RemoveItems(string fromNumber, string toNumber, string text, string firstWordOfMessage,
            bool nuke = false)
        {
            text = text.Substring(firstWordOfMessage.Length).Trim();

            if (string.IsNullOrWhiteSpace(text) && !nuke)
            {
                await _twilioService.SendSms(toNumber, fromNumber,
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

            await _twilioService.SendSms(toNumber, fromNumber, $"Removed {numItemsDeleted} item(s) from the list.");
        }

        private async Task ShowAllItems(string fromNumber, string toNumber)
        {
            var outstandingItems = await _beachBuddyRepository.GetNotCompletedRequestedItems();
            var namesOfOutstandingItems =
                outstandingItems.Aggregate("", (current, item) => current + $"{item.Name} ({item.Count})\n");

            if (string.IsNullOrWhiteSpace(namesOfOutstandingItems))
            {
                namesOfOutstandingItems = "(empty list)";
            }

            await _twilioService.SendSms(toNumber, fromNumber, $"{namesOfOutstandingItems.Trim()}");
        }

        private async Task ShowHelp(string fromNumber, string toNumber)
        {
            const string helpText = "You can send these commands:\n\n" +
                                    "{quantity} {itemName} - Will add a new item. {quantity} is optional.\n\n" +
                                    "remove {itemName} - Will remove {nameOfItem} from the list name.\n\n" +
                                    "list - Will show all the uncompleted items in the list.\n\n" +
                                    "add game {gameName} - Will add a new game.\n\n" +
                                    "leaderboard - Will show all the scores.\n\n" +
                                    "+1 {userName} {gameName} - Will increment your score. {userName} is optional.\n\n" +
                                    "bal - Will show current Twilio balance.\n\n" +
                                    "NukeFromOrbit - Will delete all beach list items.";

            await _twilioService.SendSms(toNumber, fromNumber, $"{helpText.Trim()}");
        }

        private async Task GetBalance(string fromNumber, string toNumber)
        {
            var account = await _twilioService.GetAccountBalance();

            await _twilioService.SendSms(toNumber, fromNumber, $"Current balance: ${account.Balance}");
        }

        private async Task ShowLeaderBoard(string fromNumber, string toNumber)
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

            await _twilioService.SendSms(toNumber, fromNumber, textToSend);
        }

        private async Task AddGame(string gameName, string fromNumber, string toNumber)
        {
            if (string.IsNullOrWhiteSpace(gameName))
            {
                await _twilioService.SendSms(toNumber, fromNumber,
                    $"The game name can not be empty.");
                return;
            }

            if (gameName.Length > 15)
            {
                await _twilioService.SendSms(toNumber, fromNumber,
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

            await _twilioService.SendSms(toNumber, fromNumber,
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
                await _twilioService.SendSms(toNumber, fromNumber,
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
                await _twilioService.SendSms(toNumber, fromNumber,
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
                        await _twilioService.SendSms(toNumber, fromNumber,
                            $"{userWhoseScoreToManipulate.FirstName} already has 0 wins in {gameName}");
                        return;
                    }
                
                    scoreToEdit.WinCount -= winCount;
                    break;
                }
            }
            
            _beachBuddyRepository.UpdateScore(scoreToEdit);
            await _beachBuddyRepository.Save();
            
            await _twilioService.SendSms(toNumber, fromNumber,
                $"{userWhoseScoreToManipulate.FirstName} now has {scoreToEdit.WinCount} win(s) in {gameName}!");
        }
    }
}