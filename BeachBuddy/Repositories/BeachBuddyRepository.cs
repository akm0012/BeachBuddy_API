using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeachBuddy.DbContexts;
using BeachBuddy.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeachBuddy.Repositories
{
    public class BeachBuddyRepository : IBeachBuddyRepository, IDisposable
    {
        private readonly BeachBuddyContext _context;

        public BeachBuddyRepository(BeachBuddyContext buddyContext)
        {
            _context = buddyContext ?? throw new ArgumentNullException(nameof(buddyContext));
        }
        
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.OrderBy(user => user.FirstName).ToListAsync();
        }

        public async Task<User> GetUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);
        }

        public async Task AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await _context.Users.AddAsync(user);
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

        public async Task<bool> UserExists(Guid userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return await _context.Users.AnyAsync(user => user.Id == userId);
        }

        public async Task<IEnumerable<Item>> GetItems()
        {
            return await _context.Items.OrderBy(item => item.Name).ToListAsync();
        }

        public async Task<Item> GetItem(Guid itemId)
        {
            if (itemId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(itemId));
            }

            return await _context.Items.FirstOrDefaultAsync(item => item.Id == itemId);
        }

        public async Task AddItem(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await _context.Items.AddAsync(item);
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

        public async Task<bool> ItemExists(Guid itemId)
        {
            if (itemId == null)
            {
                throw new ArgumentNullException(nameof(itemId));
            }

            return await _context.Items.AnyAsync(item => item.Id == itemId);
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() >= 0;
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