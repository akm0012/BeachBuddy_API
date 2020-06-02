using System;
using BeachBuddy.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeachBuddy.DbContexts
{
    public class BeachBuddyContext : DbContext
    {
        public BeachBuddyContext(DbContextOptions<BeachBuddyContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Andrew",
                    LastName = "Marshall",
                    KanJamWinCount = 0,
                    StarCount = 0
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Lena",
                    LastName = "Brottman",
                    KanJamWinCount = 0,
                    StarCount = 0
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Clayton",
                    LastName = "French",
                    KanJamWinCount = 0,
                    StarCount = 0
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Erica",
                    LastName = "Moore",
                    KanJamWinCount = 0,
                    StarCount = 0
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Stephen",
                    LastName = "Elkourie",
                    KanJamWinCount = 0,
                    StarCount = 0
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Lacey",
                    LastName = "Gibbs",
                    KanJamWinCount = 0,
                    StarCount = 0
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}