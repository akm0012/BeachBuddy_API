using System;
using System.Collections.Generic;
using BeachBuddy.Entities;

namespace BeachBuddy.Repositories
{
    public interface IBeachBuddyRepository
    {
        IEnumerable<User> GetUsers();
        User GetUser(Guid userId);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        bool UserExists(Guid userId);
        
        IEnumerable<Item> GetItems();
        Item GetItem(Guid itemId);
        void AddItem(Item item);
        void UpdateItem(Item item);
        void DeleteItem(Item item);
        bool ItemExists(Guid itemId);
        
        bool Save();
    }
}