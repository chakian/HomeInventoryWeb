using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeInv.Persistence.Migrations
{
    public partial class ChangeShopListItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListItems_SizeUnits_SizeUnitId",
                table: "ShoppingListItems");

            migrationBuilder.AlterColumn<int>(
                name: "SizeUnitId",
                table: "ShoppingListItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListItems_SizeUnits_SizeUnitId",
                table: "ShoppingListItems",
                column: "SizeUnitId",
                principalTable: "SizeUnits",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListItems_SizeUnits_SizeUnitId",
                table: "ShoppingListItems");

            migrationBuilder.AlterColumn<int>(
                name: "SizeUnitId",
                table: "ShoppingListItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListItems_SizeUnits_SizeUnitId",
                table: "ShoppingListItems",
                column: "SizeUnitId",
                principalTable: "SizeUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
