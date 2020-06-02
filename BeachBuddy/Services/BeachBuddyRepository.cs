using System;
using System.Collections.Generic;
using System.Linq;
using BeachBuddy.DbContexts;
using BeachBuddy.Entities;

namespace BeachBuddy.Services
{
    public class BeachBuddyRepository : IBeachBuddyRepository, IDisposable
    {
        private readonly BeachBuddyContext _context;

        public BeachBuddyRepository(BeachBuddyContext buddyContext)
        {
            _context = buddyContext ?? throw new ArgumentNullException(nameof(buddyContext));
        }
        
        public IEnumerable<User> GetUsers()
        {
            return _context.Users.OrderBy(user => user.FirstName).ToList();
        }

        public User GetUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return _context.Users.FirstOrDefault(user => user.Id == userId);
        }

        public void AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _context.Users.Add(user);
        }

        public void UpdateUser(User user)
        {
            // no code in this implementation
            // Auto Mapper will change the instance and it will save it for us 
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
        }

        public bool UserExists(Guid userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return _context.Users.Any(user => user.Id == userId);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }
    }
}