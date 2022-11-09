using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeInv.Persistence.Migrations
{
    public partial class CreateItemStockActionTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemDefinition_AspNetUsers_InsertUserId",
                table: "ItemDefinition");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDefinition_AspNetUsers_UpdateUserId",
                table: "ItemDefinition");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDefinition_Categories_CategoryId",
                table: "ItemDefinition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemDefinition",
                table: "ItemDefinition");

            migrationBuilder.RenameTable(
                name: "ItemDefinition",
                newName: "ItemDefinitions");

            migrationBuilder.RenameIndex(
                name: "IX_ItemDefinition_UpdateUserId",
                table: "ItemDefinitions",
                newName: "IX_ItemDefinitions_UpdateUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemDefinition_InsertUserId",
                table: "ItemDefinitions",
                newName: "IX_ItemDefinitions_InsertUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemDefinition_CategoryId",
                table: "ItemDefinitions",
                newName: "IX_ItemDefinitions_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemDefinitions",
                table: "ItemDefinitions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ItemStockActionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemStockActionTypes", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDefinitions_AspNetUsers_InsertUserId",
                table: "ItemDefinitions",
                column: "InsertUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDefinitions_AspNetUsers_UpdateUserId",
                table: "ItemDefinitions",
                column: "UpdateUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDefinitions_Categories_CategoryId",
                table: "ItemDefinitions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.InsertData("ItemStockActionTypes", new string[] { "Name", "Description", "IsActive" }, new object[] { "Purchased", null, true });
            migrationBuilder.InsertData("ItemStockActionTypes", new string[] { "Name", "Description", "IsActive" }, new object[] { "Consumed", null, true });
            migrationBuilder.InsertData("ItemStockActionTypes", new string[] { "Name", "Description", "IsActive" }, new object[] { "Sold", null, true });
            migrationBuilder.InsertData("ItemStockActionTypes", new string[] { "Name", "Description", "IsActive" }, new object[] { "Dismissed", null, true });
            migrationBuilder.InsertData("ItemStockActionTypes", new string[] { "Name", "Description", "IsActive" }, new object[] { "Broken", null, true });
            migrationBuilder.InsertData("ItemStockActionTypes", new string[] { "Name", "Description", "IsActive" }, new object[] { "Lost", null, true });
            migrationBuilder.InsertData("ItemStockActionTypes", new string[] { "Name", "Description", "IsActive" }, new object[] { "GiftedIn", null, true });
            migrationBuilder.InsertData("ItemStockActionTypes", new string[] { "Name", "Description", "IsActive" }, new object[] { "GiftedOut", null, true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemDefinitions_AspNetUsers_InsertUserId",
                table: "ItemDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDefinitions_AspNetUsers_UpdateUserId",
                table: "ItemDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDefinitions_Categories_CategoryId",
                table: "ItemDefinitions");

            migrationBuilder.DropTable(
                name: "ItemStockActionTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemDefinitions",
                table: "ItemDefinitions");

            migrationBuilder.RenameTable(
                name: "ItemDefinitions",
                newName: "ItemDefinition");

            migrationBuilder.RenameIndex(
                name: "IX_ItemDefinitions_UpdateUserId",
                table: "ItemDefinition",
                newName: "IX_ItemDefinition_UpdateUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemDefinitions_InsertUserId",
                table: "ItemDefinition",
                newName: "IX_ItemDefinition_InsertUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemDefinitions_CategoryId",
                table: "ItemDefinition",
                newName: "IX_ItemDefinition_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemDefinition",
                table: "ItemDefinition",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDefinition_AspNetUsers_InsertUserId",
                table: "ItemDefinition",
                column: "InsertUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDefinition_AspNetUsers_UpdateUserId",
                table: "ItemDefinition",
                column: "UpdateUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDefinition_Categories_CategoryId",
                table: "ItemDefinition",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
