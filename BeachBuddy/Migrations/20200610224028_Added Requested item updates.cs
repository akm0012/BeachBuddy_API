using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeachBuddy.Migrations
{
    public partial class AddedRequesteditemupdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("1c250228-c925-4fe1-8c75-78e8288c69c2"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("68fc8bc5-d95c-410f-8a51-6861ed3e1fd9"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("ec4b3810-609c-4253-9494-6529ecff549e"));

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: new Guid("f58e9663-b80f-4c35-ad61-7668432dc0d5"));

            migrationBuilder.AddColumn<bool>(
                name: "IsRequestCompleted",
                table: "RequestedItems",
                nullable: false,
                defaultValue: false);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "IsRequestCompleted",
                table: "RequestedItems");

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Count", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("ec4b3810-609c-4253-9494-6529ecff549e"), 12, "A delicious and refreshing lime beverage.", "https://www.lacroixwater.com/wp-content/uploads/2019/01/LaCroix_Can-Flavors_Lime_A_Vertical-683x1024.jpg", "LaCroix (Lime)" },
                    { new Guid("1c250228-c925-4fe1-8c75-78e8288c69c2"), 12, "A delicious and refreshing coconut beverage.", "https://www.lacroixwater.com/wp-content/uploads/2019/01/LaCroix_Can-Flavors_Coconut_A_Vertical-683x1024.jpg", "LaCroix (Coconut)" },
                    { new Guid("68fc8bc5-d95c-410f-8a51-6861ed3e1fd9"), 6, "A delicious and refreshing adult beverage.", "https://cdn.justwineapp.com/assets/beer/bottle/1st-republic-brewing-company-corona-light_1477953503.png", "Corona Light" },
                    { new Guid("f58e9663-b80f-4c35-ad61-7668432dc0d5"), 2, "The real MVP of the beach trip.", "https://images-na.ssl-images-amazon.com/images/I/71alXyECmDL._SL1500_.jpg", "Sun Screen SPF 30" }
                });
        }
    }
}
