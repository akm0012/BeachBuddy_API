using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeachBuddy.Entities;

namespace BeachBuddy.Repositories
{
    public interface IBeachBuddyRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(Guid userId);
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
        
        Task<bool> Save();
    }
}