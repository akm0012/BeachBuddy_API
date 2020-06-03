using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeachBuddy.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 1500, nullable: true),
                    Count = table.Column<int>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    StarCount = table.Column<int>(nullable: false),
                    KanJamWinCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Count", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("9287e8eb-3f81-4f3a-b54e-fd6ec8e39d7a"), 12, "A delicious and refreshing lime beverage.", "https://www.lacroixwater.com/wp-content/uploads/2019/01/LaCroix_Can-Flavors_Lime_A_Vertical-683x1024.jpg", "LaCroix (Lime)" },
                    { new Guid("2b7f8bb5-ec72-4f3f-9ba9-5974366bd4c0"), 12, "A delicious and refreshing coconut beverage.", "https://www.lacroixwater.com/wp-content/uploads/2019/01/LaCroix_Can-Flavors_Coconut_A_Vertical-683x1024.jpg", "LaCroix (Coconut)" },
                    { new Guid("de761cf2-a73f-480a-8697-87ed2fec9691"), 6, "A delicious and refreshing adult beverage.", "https://cdn.justwineapp.com/assets/beer/bottle/1st-republic-brewing-company-corona-light_1477953503.png", "Corona Light" },
                    { new Guid("3c8166d3-24eb-4d54-81b7-d4417c72d281"), 2, "The real MVP of the beach trip.", "https://images-na.ssl-images-amazon.com/images/I/71alXyECmDL._SL1500_.jpg", "Sun Screen SPF 30" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FirstName", "KanJamWinCount", "LastName", "StarCount" },
                values: new object[,]
                {
                    { new Guid("0a035c88-d2b2-484f-8a7b-a1d35f9abc78"), "Andrew", 0, "Marshall", 0 },
                    { new Guid("d9aa2704-d604-400f-97e7-5efe501da039"), "Lena", 0, "Brottman", 0 },
                    { new Guid("db3916d0-e4c1-471a-bf03-08785d110d61"), "Clayton", 0, "French", 0 },
                    { new Guid("3ba590b5-24ab-453a-b381-fc8ccd42aff9"), "Erica", 0, "Moore", 0 },
                    { new Guid("8016c161-b76d-42a0-b30a-f51e5ae69f54"), "Stephen", 0, "Elkourie", 0 },
                    { new Guid("80d20a13-0dcb-4351-8a33-4067785cd93c"), "Lacey", 0, "Gibbs", 0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
