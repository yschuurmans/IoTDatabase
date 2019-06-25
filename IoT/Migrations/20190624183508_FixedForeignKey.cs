using Microsoft.EntityFrameworkCore.Migrations;

namespace IoT.Migrations
{
    public partial class FixedForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_Source",
                table: "Item");

            migrationBuilder.AddForeignKey(
                name: "FK_Collection_Item",
                table: "Item",
                column: "CollectionFK",
                principalTable: "Collection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collection_Item",
                table: "Item");

            migrationBuilder.AddForeignKey(
                name: "FK_Article_Source",
                table: "Item",
                column: "CollectionFK",
                principalTable: "Collection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
