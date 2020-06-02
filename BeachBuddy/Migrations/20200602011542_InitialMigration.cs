using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeachBuddy.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "Users",
                columns: new[] { "Id", "FirstName", "KanJamWinCount", "LastName", "StarCount" },
                values: new object[,]
                {
                    { new Guid("1783ec3d-e689-4249-9e08-7ea71590eb27"), "Andrew", 0, "Marshall", 0 },
                    { new Guid("6966b63f-de4e-4854-aaf1-27d3ea963097"), "Lena", 0, "Brottman", 0 },
                    { new Guid("be21613c-249c-4703-8571-e7a371133c91"), "Clayton", 0, "French", 0 },
                    { new Guid("1b301c75-dbb9-436e-996d-9bff2cf0d5a3"), "Erica", 0, "Moore", 0 },
                    { new Guid("0ddeea52-cf23-4e91-9b65-fce283aa3a11"), "Stephen", 0, "Elkourie", 0 },
                    { new Guid("bfe1bb10-a683-4408-8bdb-d771248c695f"), "Lacey", 0, "Gibbs", 0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
