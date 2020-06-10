using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeachBuddy.Migrations
{
    public partial class AddedRequesteditemupdates2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestedItems_Users_RequesterId",
                table: "RequestedItems");

            migrationBuilder.DropIndex(
                name: "IX_RequestedItems_RequesterId",
                table: "RequestedItems");

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("1db7f5c4-f13c-47a7-93af-c804eb5b58db"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("2cf007d8-78b6-491a-a84b-ec6f9e45a74c"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("62732cfd-4bdd-493e-9b9a-360e38b6f11f"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("fbbb4646-f4e3-4eea-a4ec-eaa2a6dbcc4f"));

            migrationBuilder.DropColumn(
                name: "RequesterId",
                table: "RequestedItems");

            migrationBuilder.AddColumn<Guid>(
                name: "RequestedByUserId",
                table: "RequestedItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Count", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("3a3bc1f1-d3a4-41d7-a94a-10325da5cd16"), 12, "A delicious and refreshing lime beverage.", "https://www.lacroixwater.com/wp-content/uploads/2019/01/LaCroix_Can-Flavors_Lime_A_Vertical-683x1024.jpg", "LaCroix (Lime)" },
                    { new Guid("15c3e56f-a228-4171-a46c-cfa813e4ad26"), 12, "A delicious and refreshing coconut beverage.", "https://www.lacroixwater.com/wp-content/uploads/2019/01/LaCroix_Can-Flavors_Coconut_A_Vertical-683x1024.jpg", "LaCroix (Coconut)" },
                    { new Guid("ee9e5db4-ef2f-4808-a867-1488610e2b80"), 6, "A delicious and refreshing adult beverage.", "https://cdn.justwineapp.com/assets/beer/bottle/1st-republic-brewing-company-corona-light_1477953503.png", "Corona Light" },
                    { new Guid("e8a462cc-d744-4b7b-8b42-20d6e67ca7b9"), 2, "The real MVP of the beach trip.", "https://images-na.ssl-images-amazon.com/images/I/71alXyECmDL._SL1500_.jpg", "Sun Screen SPF 30" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestedItems_RequestedByUserId",
                table: "RequestedItems",
                column: "RequestedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestedItems_Users_RequestedByUserId",
                table: "RequestedItems",
                column: "RequestedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestedItems_Users_RequestedByUserId",
                table: "RequestedItems");

            migrationBuilder.DropIndex(
                name: "IX_RequestedItems_RequestedByUserId",
                table: "RequestedItems");

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("15c3e56f-a228-4171-a46c-cfa813e4ad26"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("3a3bc1f1-d3a4-41d7-a94a-10325da5cd16"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("e8a462cc-d744-4b7b-8b42-20d6e67ca7b9"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("ee9e5db4-ef2f-4808-a867-1488610e2b80"));

            migrationBuilder.DropColumn(
                name: "RequestedByUserId",
                table: "RequestedItems");

            migrationBuilder.AddColumn<Guid>(
                name: "RequesterId",
                table: "RequestedItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Count", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("62732cfd-4bdd-493e-9b9a-360e38b6f11f"), 12, "A delicious and refreshing lime beverage.", "https://www.lacroixwater.com/wp-content/uploads/2019/01/LaCroix_Can-Flavors_Lime_A_Vertical-683x1024.jpg", "LaCroix (Lime)" },
                    { new Guid("2cf007d8-78b6-491a-a84b-ec6f9e45a74c"), 12, "A delicious and refreshing coconut beverage.", "https://www.lacroixwater.com/wp-content/uploads/2019/01/LaCroix_Can-Flavors_Coconut_A_Vertical-683x1024.jpg", "LaCroix (Coconut)" },
                    { new Guid("1db7f5c4-f13c-47a7-93af-c804eb5b58db"), 6, "A delicious and refreshing adult beverage.", "https://cdn.justwineapp.com/assets/beer/bottle/1st-republic-brewing-company-corona-light_1477953503.png", "Corona Light" },
                    { new Guid("fbbb4646-f4e3-4eea-a4ec-eaa2a6dbcc4f"), 2, "The real MVP of the beach trip.", "https://images-na.ssl-images-amazon.com/images/I/71alXyECmDL._SL1500_.jpg", "Sun Screen SPF 30" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestedItems_RequesterId",
                table: "RequestedItems",
                column: "RequesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestedItems_Users_RequesterId",
                table: "RequestedItems",
                column: "RequesterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
