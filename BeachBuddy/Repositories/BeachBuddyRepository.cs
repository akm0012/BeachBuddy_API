using System;
using System.Collections.Generic;
using System.Linq;
using BeachBuddy.DbContexts;
using BeachBuddy.Entities;

namespace BeachBuddy.Repositories
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
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            
            _context.Users.Update(user);
        }

        public void DeleteUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
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

        public IEnumerable<Item> GetItems()
        {
            return _context.Items.OrderBy(item => item.Name).ToList();
        }

        public Item GetItem(Guid itemId)
        {
            if (itemId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(itemId));
            }

            return _context.Items.FirstOrDefault(item => item.Id == itemId);
        }

        public void AddItem(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _context.Items.Add(item);
        }

        public void UpdateItem(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _context.Items.Update(item);
        }

        public void DeleteItem(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _context.Items.Remove(item);
        }

        public bool ItemExists(Guid itemId)
        {
            if (itemId == null)
            {
                throw new ArgumentNullException(nameof(itemId));
            }

            return _context.Items.Any(item => item.Id == itemId);
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