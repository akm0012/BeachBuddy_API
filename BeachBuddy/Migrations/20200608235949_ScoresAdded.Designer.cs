﻿// <auto-generated />
using System;
using BeachBuddy.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BeachBuddy.Migrations
{
    [DbContext(typeof(BeachBuddyContext))]
    [Migration("20200608235949_ScoresAdded")]
    partial class ScoresAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BeachBuddy.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(1500)")
                        .HasMaxLength(1500);

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Items");

                    b.HasData(
                        new
                        {
                            Id = new Guid("2a434202-5736-4e04-8e21-c307d372bde0"),
                            Count = 12,
                            Description = "A delicious and refreshing lime beverage.",
                            ImageUrl = "https://www.lacroixwater.com/wp-content/uploads/2019/01/LaCroix_Can-Flavors_Lime_A_Vertical-683x1024.jpg",
                            Name = "LaCroix (Lime)"
                        },
                        new
                        {
                            Id = new Guid("c9a51e68-29c2-4c70-b800-94555a399640"),
                            Count = 12,
                            Description = "A delicious and refreshing coconut beverage.",
                            ImageUrl = "https://www.lacroixwater.com/wp-content/uploads/2019/01/LaCroix_Can-Flavors_Coconut_A_Vertical-683x1024.jpg",
                            Name = "LaCroix (Coconut)"
                        },
                        new
                        {
                            Id = new Guid("9f5d21ed-b194-4de5-890a-e69626246d48"),
                            Count = 6,
                            Description = "A delicious and refreshing adult beverage.",
                            ImageUrl = "https://cdn.justwineapp.com/assets/beer/bottle/1st-republic-brewing-company-corona-light_1477953503.png",
                            Name = "Corona Light"
                        },
                        new
                        {
                            Id = new Guid("f3b2cb40-67eb-4384-8350-ee8637bcfdb7"),
                            Count = 2,
                            Description = "The real MVP of the beach trip.",
                            ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/71alXyECmDL._SL1500_.jpg",
                            Name = "Sun Screen SPF 30"
                        });
                });

            modelBuilder.Entity("BeachBuddy.Entities.Score", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("WinCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Scores");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c9b9b770-659a-4543-8229-4af6a551e252"),
                            Name = "KanJam",
                            UserId = new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                            WinCount = 0
                        },
                        new
                        {
                            Id = new Guid("5754bf85-8b41-4556-b0b3-653b2370b9af"),
                            Name = "Mario Party",
                            UserId = new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                            WinCount = 0
                        },
                        new
                        {
                            Id = new Guid("3ee79f5c-e83e-426d-a417-b8922798f85c"),
                            Name = "KanJam",
                            UserId = new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"),
                            WinCount = 0
                        },
                        new
                        {
                            Id = new Guid("9a01985b-f9c8-4a0d-b85d-24612fe5ef75"),
                            Name = "Mario Party",
                            UserId = new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"),
                            WinCount = 0
                        },
                        new
                        {
                            Id = new Guid("e171ab20-97d5-4e6a-819a-38e619e20614"),
                            Name = "KanJam",
                            UserId = new Guid("2902b665-1190-4c70-9915-b9c2d7680450"),
                            WinCount = 0
                        },
                        new
                        {
                            Id = new Guid("c26565ae-5b99-40d4-8324-1f309bac0b0b"),
                            Name = "Mario Party",
                            UserId = new Guid("2902b665-1190-4c70-9915-b9c2d7680450"),
                            WinCount = 0
                        },
                        new
                        {
                            Id = new Guid("27042c2a-33f5-4bdd-aa57-840eb6dd68fb"),
                            Name = "KanJam",
                            UserId = new Guid("102b566b-ba1f-404c-b2df-e2cde39ade09"),
                            WinCount = 0
                        },
                        new
                        {
                            Id = new Guid("b01f871c-7b0a-482d-88cc-8fe7a4d83a20"),
                            Name = "Mario Party",
                            UserId = new Guid("102b566b-ba1f-404c-b2df-e2cde39ade09"),
                            WinCount = 0
                        },
                        new
                        {
                            Id = new Guid("f17263b8-2d84-4308-8998-721168e7a369"),
                            Name = "KanJam",
                            UserId = new Guid("5b3621c0-7b12-4e80-9c8b-3398cba7ee05"),
                            WinCount = 0
                        },
                        new
                        {
                            Id = new Guid("303d5b5e-f805-4aee-afac-6562cadede8c"),
                            Name = "Mario Party",
                            UserId = new Guid("5b3621c0-7b12-4e80-9c8b-3398cba7ee05"),
                            WinCount = 0
                        },
                        new
                        {
                            Id = new Guid("79fdafa6-d77b-4415-b7b3-610337b59352"),
                            Name = "KanJam",
                            UserId = new Guid("2aadd2df-7caf-45ab-9355-7f6332985a87"),
                            WinCount = 0
                        },
                        new
                        {
                            Id = new Guid("945a0a7b-dd65-4215-9e21-d05153712dd8"),
                            Name = "Mario Party",
                            UserId = new Guid("2aadd2df-7caf-45ab-9355-7f6332985a87"),
                            WinCount = 0
                        });
                });

            modelBuilder.Entity("BeachBuddy.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                            FirstName = "Andrew",
                            LastName = "Marshall"
                        },
                        new
                        {
                            Id = new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"),
                            FirstName = "Lena",
                            LastName = "Brottman"
                        },
                        new
                        {
                            Id = new Guid("2902b665-1190-4c70-9915-b9c2d7680450"),
                            FirstName = "Clayton",
                            LastName = "French"
                        },
                        new
                        {
                            Id = new Guid("102b566b-ba1f-404c-b2df-e2cde39ade09"),
                            FirstName = "Erica",
                            LastName = "Moore"
                        },
                        new
                        {
                            Id = new Guid("5b3621c0-7b12-4e80-9c8b-3398cba7ee05"),
                            FirstName = "Stephen",
                            LastName = "Elkourie"
                        },
                        new
                        {
                            Id = new Guid("2aadd2df-7caf-45ab-9355-7f6332985a87"),
                            FirstName = "Lacey",
                            LastName = "Gibbs"
                        });
                });

            modelBuilder.Entity("BeachBuddy.Entities.Score", b =>
                {
                    b.HasOne("BeachBuddy.Entities.User", "User")
                        .WithMany("Scores")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
