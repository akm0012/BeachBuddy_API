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
    [Migration("20200610223358_Added Requested items")]
    partial class AddedRequesteditems
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
                            Id = new Guid("ec4b3810-609c-4253-9494-6529ecff549e"),
                            Count = 12,
                            Description = "A delicious and refreshing lime beverage.",
                            ImageUrl = "https://www.lacroixwater.com/wp-content/uploads/2019/01/LaCroix_Can-Flavors_Lime_A_Vertical-683x1024.jpg",
                            Name = "LaCroix (Lime)"
                        },
                        new
                        {
                            Id = new Guid("1c250228-c925-4fe1-8c75-78e8288c69c2"),
                            Count = 12,
                            Description = "A delicious and refreshing coconut beverage.",
                            ImageUrl = "https://www.lacroixwater.com/wp-content/uploads/2019/01/LaCroix_Can-Flavors_Coconut_A_Vertical-683x1024.jpg",
                            Name = "LaCroix (Coconut)"
                        },
                        new
                        {
                            Id = new Guid("68fc8bc5-d95c-410f-8a51-6861ed3e1fd9"),
                            Count = 6,
                            Description = "A delicious and refreshing adult beverage.",
                            ImageUrl = "https://cdn.justwineapp.com/assets/beer/bottle/1st-republic-brewing-company-corona-light_1477953503.png",
                            Name = "Corona Light"
                        },
                        new
                        {
                            Id = new Guid("f58e9663-b80f-4c35-ad61-7668432dc0d5"),
                            Count = 2,
                            Description = "The real MVP of the beach trip.",
                            ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/71alXyECmDL._SL1500_.jpg",
                            Name = "Sun Screen SPF 30"
                        });
                });

            modelBuilder.Entity("BeachBuddy.Entities.RequestedItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("CreatedDateTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<Guid>("RequesterId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RequesterId");

                    b.ToTable("RequestedItems");
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

            modelBuilder.Entity("BeachBuddy.Entities.RequestedItem", b =>
                {
                    b.HasOne("BeachBuddy.Entities.User", "RequestedByUser")
                        .WithMany()
                        .HasForeignKey("RequesterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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
