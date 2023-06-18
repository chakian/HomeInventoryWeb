using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeInv.Persistence.Migrations
{
    public partial class AddedItemDefIdToShopListItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemDefinitionId",
                table: "ShoppingListItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListItems_ItemDefinitionId",
                table: "ShoppingListItems",
                column: "ItemDefinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListItems_ItemDefinitions_ItemDefinitionId",
                table: "ShoppingListItems",
                column: "ItemDefinitionId",
                principalTable: "ItemDefinitions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListItems_ItemDefinitions_ItemDefinitionId",
                table: "ShoppingListItems");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingListItems_ItemDefinitionId",
                table: "ShoppingListItems");

            migrationBuilder.DropColumn(
                name: "ItemDefinitionId",
                table: "ShoppingListItems");
        }
    }
}
