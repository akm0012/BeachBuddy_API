using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeachBuddy.Migrations
{
    public partial class ScoresAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("2b7f8bb5-ec72-4f3f-9ba9-5974366bd4c0"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("3c8166d3-24eb-4d54-81b7-d4417c72d281"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("9287e8eb-3f81-4f3a-b54e-fd6ec8e39d7a"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("de761cf2-a73f-480a-8697-87ed2fec9691"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0a035c88-d2b2-484f-8a7b-a1d35f9abc78"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3ba590b5-24ab-453a-b381-fc8ccd42aff9"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8016c161-b76d-42a0-b30a-f51e5ae69f54"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("80d20a13-0dcb-4351-8a33-4067785cd93c"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d9aa2704-d604-400f-97e7-5efe501da039"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("db3916d0-e4c1-471a-bf03-08785d110d61"));

            migrationBuilder.DropColumn(
                name: "KanJamWinCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "StarCount",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    WinCount = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scores_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Count", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("2a434202-5736-4e04-8e21-c307d372bde0"), 12, "A delicious and refreshing lime beverage.", "https://www.lacroixwater.com/wp-content/uploads/2019/01/LaCroix_Can-Flavors_Lime_A_Vertical-683x1024.jpg", "LaCroix (Lime)" },
                    { new Guid("c9a51e68-29c2-4c70-b800-94555a399640"), 12, "A delicious and refreshing coconut beverage.", "https://www.lacroixwater.com/wp-content/uploads/2019/01/LaCroix_Can-Flavors_Coconut_A_Vertical-683x1024.jpg", "LaCroix (Coconut)" },
                    { new Guid("9f5d21ed-b194-4de5-890a-e69626246d48"), 6, "A delicious and refreshing adult beverage.", "https://cdn.justwineapp.com/assets/beer/bottle/1st-republic-brewing-company-corona-light_1477953503.png", "Corona Light" },
                    { new Guid("f3b2cb40-67eb-4384-8350-ee8637bcfdb7"), 2, "The real MVP of the beach trip.", "https://images-na.ssl-images-amazon.com/images/I/71alXyECmDL._SL1500_.jpg", "Sun Screen SPF 30" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[,]
                {
                    { new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), "Andrew", "Marshall" },
                    { new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), "Lena", "Brottman" },
                    { new Guid("2902b665-1190-4c70-9915-b9c2d7680450"), "Clayton", "French" },
                    { new Guid("102b566b-ba1f-404c-b2df-e2cde39ade09"), "Erica", "Moore" },
                    { new Guid("5b3621c0-7b12-4e80-9c8b-3398cba7ee05"), "Stephen", "Elkourie" },
                    { new Guid("2aadd2df-7caf-45ab-9355-7f6332985a87"), "Lacey", "Gibbs" }
                });

            migrationBuilder.InsertData(
                table: "Scores",
                columns: new[] { "Id", "Name", "UserId", "WinCount" },
                values: new object[,]
                {
                    { new Guid("c9b9b770-659a-4543-8229-4af6a551e252"), "KanJam", new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), 0 },
                    { new Guid("5754bf85-8b41-4556-b0b3-653b2370b9af"), "Mario Party", new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), 0 },
                    { new Guid("3ee79f5c-e83e-426d-a417-b8922798f85c"), "KanJam", new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), 0 },
                    { new Guid("9a01985b-f9c8-4a0d-b85d-24612fe5ef75"), "Mario Party", new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), 0 },
                    { new Guid("e171ab20-97d5-4e6a-819a-38e619e20614"), "KanJam", new Guid("2902b665-1190-4c70-9915-b9c2d7680450"), 0 },
                    { new Guid("c26565ae-5b99-40d4-8324-1f309bac0b0b"), "Mario Party", new Guid("2902b665-1190-4c70-9915-b9c2d7680450"), 0 },
                    { new Guid("27042c2a-33f5-4bdd-aa57-840eb6dd68fb"), "KanJam", new Guid("102b566b-ba1f-404c-b2df-e2cde39ade09"), 0 },
                    { new Guid("b01f871c-7b0a-482d-88cc-8fe7a4d83a20"), "Mario Party", new Guid("102b566b-ba1f-404c-b2df-e2cde39ade09"), 0 },
                    { new Guid("f17263b8-2d84-4308-8998-721168e7a369"), "KanJam", new Guid("5b3621c0-7b12-4e80-9c8b-3398cba7ee05"), 0 },
                    { new Guid("303d5b5e-f805-4aee-afac-6562cadede8c"), "Mario Party", new Guid("5b3621c0-7b12-4e80-9c8b-3398cba7ee05"), 0 },
                    { new Guid("79fdafa6-d77b-4415-b7b3-610337b59352"), "KanJam", new Guid("2aadd2df-7caf-45ab-9355-7f6332985a87"), 0 },
                    { new Guid("945a0a7b-dd65-4215-9e21-d05153712dd8"), "Mario Party", new Guid("2aadd2df-7caf-45ab-9355-7f6332985a87"), 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scores_UserId",
                table: "Scores",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("2a434202-5736-4e04-8e21-c307d372bde0"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("9f5d21ed-b194-4de5-890a-e69626246d48"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("c9a51e68-29c2-4c70-b800-94555a399640"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("f3b2cb40-67eb-4384-8350-ee8637bcfdb7"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("102b566b-ba1f-404c-b2df-e2cde39ade09"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("2902b665-1190-4c70-9915-b9c2d7680450"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("2aadd2df-7caf-45ab-9355-7f6332985a87"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("5b3621c0-7b12-4e80-9c8b-3398cba7ee05"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"));

            migrationBuilder.AddColumn<int>(
                name: "KanJamWinCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StarCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
