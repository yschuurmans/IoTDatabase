using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IoT.Migrations
{
    public partial class AddedApiKeyItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateTime",
                table: "Item",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LastUpdaterFK",
                table: "Item",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Item_LastUpdaterFK",
                table: "Item",
                column: "LastUpdaterFK");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiKey_Item",
                table: "Item",
                column: "LastUpdaterFK",
                principalTable: "ApiKey",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiKey_Item",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_LastUpdaterFK",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "LastUpdateTime",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "LastUpdaterFK",
                table: "Item");
        }
    }
}
