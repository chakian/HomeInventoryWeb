using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeInv.Persistence.Migrations
{
    public partial class CreateItemUnitPriceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemUnitPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemStockActionId = table.Column<int>(type: "int", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "money", nullable: false),
                    ItemDefinitionId = table.Column<int>(type: "int", nullable: true),
                    PriceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemUnitPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemUnitPrices_ItemDefinitions_ItemDefinitionId",
                        column: x => x.ItemDefinitionId,
                        principalTable: "ItemDefinitions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemUnitPrices_ItemStockActions_ItemStockActionId",
                        column: x => x.ItemStockActionId,
                        principalTable: "ItemStockActions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemUnitPrices_ItemDefinitionId",
                table: "ItemUnitPrices",
                column: "ItemDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemUnitPrices_ItemStockActionId",
                table: "ItemUnitPrices",
                column: "ItemStockActionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemUnitPrices");
        }
    }
}
