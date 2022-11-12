using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeInv.Persistence.Migrations
{
    public partial class MoveSizeUnitId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SizeUnitId",
                table: "ItemDefinitions",
                type: "int",
                nullable: true);

            UpdateSizeIds(migrationBuilder);

            migrationBuilder.DropForeignKey(
                name: "FK_ItemStocks_SizeUnits_SizeUnitId",
                table: "ItemStocks");

            migrationBuilder.DropIndex(
                name: "IX_ItemStocks_SizeUnitId",
                table: "ItemStocks");

            migrationBuilder.DropColumn(
                name: "SizeUnitId",
                table: "ItemStocks");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDefinitions_SizeUnitId",
                table: "ItemDefinitions",
                column: "SizeUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDefinitions_SizeUnits_SizeUnitId",
                table: "ItemDefinitions",
                column: "SizeUnitId",
                principalTable: "SizeUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemDefinitions_SizeUnits_SizeUnitId",
                table: "ItemDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_ItemDefinitions_SizeUnitId",
                table: "ItemDefinitions");

            migrationBuilder.DropColumn(
                name: "SizeUnitId",
                table: "ItemDefinitions");

            migrationBuilder.AddColumn<int>(
                name: "SizeUnitId",
                table: "ItemStocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ItemStocks_SizeUnitId",
                table: "ItemStocks",
                column: "SizeUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemStocks_SizeUnits_SizeUnitId",
                table: "ItemStocks",
                column: "SizeUnitId",
                principalTable: "SizeUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
