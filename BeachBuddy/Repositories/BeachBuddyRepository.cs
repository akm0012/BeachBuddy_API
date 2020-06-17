using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeachBuddy.DbContexts;
using BeachBuddy.Entities;
using BeachBuddy.Models;
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
            return await _context.Users
                .Include(user => user.Scores)
                .OrderBy(user => user.FirstName).ToListAsync();
        }

        public async Task<User> GetUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);
        }

        public async Task<IEnumerable<User>> GetUsers(UserResourceParameters userResourceParameters)
        {
            var phoneNumber = userResourceParameters.PhoneNumber;
            var name = userResourceParameters.Name;

            if (string.IsNullOrWhiteSpace(phoneNumber) && string.IsNullOrWhiteSpace(name))
            {
                return await GetUsers();
            }

            var users = _context.Users as IQueryable<User>;

            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                phoneNumber = phoneNumber.Trim();
                users = users.Where(user => user.PhoneNumber.Contains(phoneNumber));
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                users = users.Where(user => user.FirstName.Contains(name)
                                            || user.LastName.Contains(name));
            }

            return await users.ToListAsync();
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

        public async Task<IEnumerable<Score>> GetScores()
        {
            return await _context.Scores.OrderBy(score => score.Name).ToListAsync();
        }

        public async Task<Score> GetScore(Guid scoreId)
        {
            if (scoreId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(scoreId));
            }

            return await _context.Scores.FirstOrDefaultAsync(score => score.Id == scoreId);
        }

        public async Task AddScore(Score score)
        {
            if (score == null)
            {
                throw new ArgumentNullException(nameof(score));
            }

            await _context.Scores.AddAsync(score);
        }

        public void UpdateScore(Score score)
        {
            if (score == null)
            {
                throw new ArgumentNullException(nameof(score));
            }

            _context.Scores.Update(score);
        }

        public void DeleteScore(Score score)
        {
            if (score == null)
            {
                throw new ArgumentNullException(nameof(score));
            }

            _context.Scores.Remove(score);
        }

        public async Task<bool> ScoreExists(Guid scoreId)
        {
            if (scoreId == null)
            {
                throw new ArgumentNullException(nameof(scoreId));
            }

            return await _context.Scores.AnyAsync(score => score.Id == scoreId);
        }

        public async Task<IEnumerable<RequestedItem>> GetRequestedItems()
        {
            return await _context.RequestedItems
                .Include(item => item.RequestedByUser)
                .OrderBy(r => r.CreatedDateTime).ToListAsync();
        }

        public async Task<IEnumerable<RequestedItem>> GetRequestedItems(string nameQuery)
        {
            return await _context.RequestedItems
                .Where(r => r.Name.Equals(nameQuery))
                .ToListAsync();
        }

        public async Task<IEnumerable<RequestedItem>> GetNotCompletedRequestedItems()
        {
            return await _context.RequestedItems
                .Include(item => item.RequestedByUser)
                .Where(r => !r.IsRequestCompleted)
                .OrderBy(r => r.CreatedDateTime).ToListAsync();
        }

        public async Task<RequestedItem> GetRequestedItem(Guid requestedItemId)
        {
            if (requestedItemId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(requestedItemId));
            }

            return await _context.RequestedItems
                .Include(item => item.RequestedByUser)
                .FirstOrDefaultAsync(item => item.Id == requestedItemId);
        }

        public async Task AddRequestedItem(RequestedItem requestedItem)
        {
            if (requestedItem == null)
            {
                throw new ArgumentNullException(nameof(requestedItem));
            }

            await _context.RequestedItems.AddAsync(requestedItem);
        }

        public void UpdateRequestedItem(RequestedItem requestedItem)
        {
            if (requestedItem == null)
            {
                throw new ArgumentNullException(nameof(requestedItem));
            }

            _context.RequestedItems.Update(requestedItem);
        }

        public void DeleteRequestedItem(RequestedItem requestedItem)
        {
            if (requestedItem == null)
            {
                throw new ArgumentNullException(nameof(requestedItem));
            }

            _context.RequestedItems.Remove(requestedItem);
        }

        public async Task<bool> RequestedItemExists(Guid requestedItemId)
        {
            if (requestedItemId == null)
            {
                throw new ArgumentNullException(nameof(requestedItemId));
            }

            return await _context.RequestedItems.AnyAsync(item => item.Id == requestedItemId);
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