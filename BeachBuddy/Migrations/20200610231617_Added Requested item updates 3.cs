using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeachBuddy.Migrations
{
    public partial class AddedRequesteditemupdates3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Count", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("3a65d43d-b00e-490f-aadf-055c6df97b0f"), 12, "A delicious and refreshing lime beverage.", "https://www.lacroixwater.com/wp-content/uploads/2019/01/LaCroix_Can-Flavors_Lime_A_Vertical-683x1024.jpg", "LaCroix (Lime)" },
                    { new Guid("4b38c822-3e56-48d7-8b02-94a3951f2dab"), 12, "A delicious and refreshing coconut beverage.", "https://www.lacroixwater.com/wp-content/uploads/2019/01/LaCroix_Can-Flavors_Coconut_A_Vertical-683x1024.jpg", "LaCroix (Coconut)" },
                    { new Guid("66a82a35-3849-423b-a891-3778fe824a6f"), 6, "A delicious and refreshing adult beverage.", "https://cdn.justwineapp.com/assets/beer/bottle/1st-republic-brewing-company-corona-light_1477953503.png", "Corona Light" },
                    { new Guid("e16ddb8b-0e47-4fd6-ab6b-f5fd22a24c7c"), 2, "The real MVP of the beach trip.", "https://images-na.ssl-images-amazon.com/images/I/71alXyECmDL._SL1500_.jpg", "Sun Screen SPF 30" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("3a65d43d-b00e-490f-aadf-055c6df97b0f"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("4b38c822-3e56-48d7-8b02-94a3951f2dab"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("66a82a35-3849-423b-a891-3778fe824a6f"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("e16ddb8b-0e47-4fd6-ab6b-f5fd22a24c7c"));

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
        }
    }
}
