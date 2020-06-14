using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeachBuddy.Entities;
using BeachBuddy.Models;
using BeachBuddy.Models.Files;
using BeachBuddy.Repositories;
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

        public TwilioWebhookRepository(ILogger<TwilioWebhookRepository> logger,
            IBeachBuddyRepository beachBuddyRepository, ITwilioService twilioService)
        {
            _logger = logger;
            _beachBuddyRepository = beachBuddyRepository;
            _twilioService = twilioService;
        }

        public async Task MessageReceived(string fromNumber, string toNumber, string text, List<RemoteFile> files)
        {
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

            var firstWordOfMessage = text.Split(" ")[0];

            switch (firstWordOfMessage.ToLower())
            {
                case "remove":
                    await RemoveItems(fromNumber, toNumber, text, firstWordOfMessage);
                    return;

                case "list":
                    await ShowAllItems(fromNumber, toNumber);
                    return;

                case "h":
                    await ShowHelp(fromNumber, toNumber);
                    return;
                
                case "nukefromorbit":
                    await RemoveItems(fromNumber, toNumber, text, firstWordOfMessage, true);
                    return;
                
                case "bal":
                    await GetBalance(fromNumber, toNumber);
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

            await _twilioService.SendSms(toNumber, fromNumber, $"\"{text}\" was added to the list!");
        }

        private async Task RemoveItems(string fromNumber, string toNumber, string text, string firstWordOfMessage, bool nuke = false)
        {
            text = text.Substring(firstWordOfMessage.Length).Trim();

            if (string.IsNullOrWhiteSpace(text) && !nuke)
            {
                await _twilioService.SendSms(toNumber, fromNumber, $"Nothing was removed. If you want to remove everything send, \"NukeFromOrbit\"");
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
                                    "bal - Will show current Twilio balance.\n\n" +
                                    "NukeFromOrbit - Will delete all items.";

            await _twilioService.SendSms(toNumber, fromNumber, $"{helpText.Trim()}");
        }

        private async Task GetBalance(string fromNumber, string toNumber)
        {
            var account = await _twilioService.GetAccountBalance();
            
            await _twilioService.SendSms(toNumber, fromNumber, $"Current balance: ${account.Balance}");
        }
    }
}