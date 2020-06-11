using System;
using BeachBuddy.Entities;
using BeachBuddy.Enums;
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
        public DbSet<Score> Scores { get; set; }
        public DbSet<RequestedItem> RequestedItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                    FirstName = "Andrew",
                    LastName = "Marshall",
                    SkinType = SkinType.One,
                    PhoneNumber = "+17703557591",
                    PhotoUrl = "StaticFiles/images/andrew.jpeg"
                },
                new User()
                {
                    Id = Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96"),
                    FirstName = "Lena",
                    LastName = "Brottman",
                    SkinType = SkinType.Two,
                    PhoneNumber = "+18474945909",
                    PhotoUrl = "StaticFiles/images/lena.jpeg"
                },
                new User()
                {
                    Id = Guid.Parse("2902b665-1190-4c70-9915-b9c2d7680450"),
                    FirstName = "Clayton",
                    LastName = "French",
                    SkinType = SkinType.Three,
                    PhoneNumber = "+16784691861‬",
                    PhotoUrl = "StaticFiles/images/clayton.jpeg"
                },
                new User()
                {
                    Id = Guid.Parse("102b566b-ba1f-404c-b2df-e2cde39ade09"),
                    FirstName = "Erica",
                    LastName = "Moore",
                    SkinType = SkinType.Three,
                    PhoneNumber = "+16782662654",
                    PhotoUrl = "StaticFiles/images/erica.jpeg"
                },
                new User()
                {
                    Id = Guid.Parse("5b3621c0-7b12-4e80-9c8b-3398cba7ee05"),
                    FirstName = "Stephen",
                    LastName = "Elkourie",
                    SkinType = SkinType.Five,
                    PhoneNumber = "+1‭6782343314",
                    PhotoUrl = "StaticFiles/images/stephen.jpeg"
                },
                new User()
                {
                    Id = Guid.Parse("2aadd2df-7caf-45ab-9355-7f6332985a87"),
                    FirstName = "Lacey",
                    LastName = "Gibbs",
                    SkinType = SkinType.Three,
                    PhoneNumber = "+12563935211‬",
                    PhotoUrl = "StaticFiles/images/lacey.jpeg"
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