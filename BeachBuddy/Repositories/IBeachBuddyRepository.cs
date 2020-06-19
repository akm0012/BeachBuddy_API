using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeachBuddy.Entities;
using BeachBuddy.Models;

namespace BeachBuddy.Repositories
{
    public interface IBeachBuddyRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(Guid userId);
        Task<IEnumerable<User>> GetUsers(UserResourceParameters userResourceParameters);
        Task AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        Task<bool> UserExists(Guid userId);
        
        Task<IEnumerable<Item>> GetItems();
        Task<Item> GetItem(Guid itemId);
        Task AddItem(Item item);
        void UpdateItem(Item item);
        void DeleteItem(Item item);
        Task<bool> ItemExists(Guid itemId);
        
        Task<IEnumerable<Score>> GetScores();
        Task<Score> GetScore(Guid scoreId);
        Task AddScore(Score score);
        void UpdateScore(Score score);
        void DeleteScore(Score score);
        Task<bool> ScoreExists(Guid scoreId);
        
        Task<IEnumerable<RequestedItem>> GetRequestedItems();
        Task<IEnumerable<RequestedItem>> GetRequestedItems(string nameQuery);
        Task<IEnumerable<RequestedItem>> GetNotCompletedRequestedItems();
        Task<RequestedItem> GetRequestedItem(Guid requestedItemId);
        Task AddRequestedItem(RequestedItem requestedItem);
        void UpdateRequestedItem(RequestedItem requestedItem);
        void DeleteRequestedItem(RequestedItem requestedItem);
        Task<bool> RequestedItemExists(Guid requestedItemId);
        
        Task<IEnumerable<Device>> GetDevices();
        Task<Device> GetDevice(string deviceToken);
        Task AddDevice(Device device);
        void DeleteDevice(Device device);
        
        Task<bool> Save();
    }
}