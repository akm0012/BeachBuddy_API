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
        public DbSet<Item> Items { get; set; }

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

            modelBuilder.Entity<Item>().HasData(
            new Item()
            {
                Id = Guid.NewGuid(),
                Name = "LaCroix (Lime)",
                Description = "A delicious and refreshing lime beverage.",
                Count = 12,
                ImageUrl = "https://www.lacroixwater.com/wp-content/uploads/2019/01/LaCroix_Can-Flavors_Lime_A_Vertical-683x1024.jpg"
            },
            new Item()
            {
                Id = Guid.NewGuid(),
                Name = "LaCroix (Coconut)",
                Description = "A delicious and refreshing coconut beverage.",
                Count = 12,
                ImageUrl = "https://www.lacroixwater.com/wp-content/uploads/2019/01/LaCroix_Can-Flavors_Coconut_A_Vertical-683x1024.jpg"
            },
            new Item()
            {
                Id = Guid.NewGuid(),
                Name = "Corona Light",
                Description = "A delicious and refreshing adult beverage.",
                Count = 6,
                ImageUrl = "https://cdn.justwineapp.com/assets/beer/bottle/1st-republic-brewing-company-corona-light_1477953503.png"
            },
            new Item()
            {
                Id = Guid.NewGuid(),
                Name = "Sun Screen SPF 30",
                Description = "The real MVP of the beach trip.",
                Count = 2,
                ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/71alXyECmDL._SL1500_.jpg"
            }
            );
            
            base.OnModelCreating(modelBuilder);
        }
    }
}