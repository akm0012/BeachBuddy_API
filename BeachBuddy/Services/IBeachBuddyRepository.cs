using System;
using System.Collections.Generic;
using BeachBuddy.Entities;

namespace BeachBuddy.Services
{
    public interface IBeachBuddyRepository
    {
        IEnumerable<User> GetUsers();
        User GetUser(Guid userId);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        bool UserExists(Guid userId);
        bool Save();
    }
}