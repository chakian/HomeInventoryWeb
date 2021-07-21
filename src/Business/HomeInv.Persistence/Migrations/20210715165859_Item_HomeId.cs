using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeInv.Persistence.Migrations
{
    public partial class Item_HomeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HomeId",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Items_HomeId",
                table: "Items",
                column: "HomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Homes_HomeId",
                table: "Items",
                column: "HomeId",
                principalTable: "Homes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Homes_HomeId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_HomeId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "HomeId",
                table: "Items");
        }
    }
}
