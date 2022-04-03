using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeInv.Persistence.Migrations
{
    public partial class ItemStockSizeAndRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_SizeUnits_SizeUnitId",
                table: "Items");

            migrationBuilder.AddColumn<decimal>(
                name: "Size",
                table: "ItemStocks",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SizeUnitId",
                table: "ItemStocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ItemStocks_ItemId",
                table: "ItemStocks",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemStocks_SizeUnitId",
                table: "ItemStocks",
                column: "SizeUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_SizeUnits_SizeUnitId",
                table: "Items",
                column: "SizeUnitId",
                principalTable: "SizeUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemStocks_Items_ItemId",
                table: "ItemStocks",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemStocks_SizeUnits_SizeUnitId",
                table: "ItemStocks",
                column: "SizeUnitId",
                principalTable: "SizeUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.InsertData("SizeUnits", new string[] { "Name", "Description", "IsBaseUnit", "BaseUnitId", "ConversionMultiplierToBase", "IsActive" }, new object[] { "l", "Litre", true, null, 1, true });
            migrationBuilder.InsertData("SizeUnits", new string[] { "Name", "Description", "IsBaseUnit", "BaseUnitId", "ConversionMultiplierToBase", "IsActive" }, new object[] { "dl", "Desilitre", false, 1, 10, true });
            migrationBuilder.InsertData("SizeUnits", new string[] { "Name", "Description", "IsBaseUnit", "BaseUnitId", "ConversionMultiplierToBase", "IsActive" }, new object[] { "cl", "Santilitre", false, 1, 100, true });
            migrationBuilder.InsertData("SizeUnits", new string[] { "Name", "Description", "IsBaseUnit", "BaseUnitId", "ConversionMultiplierToBase", "IsActive" }, new object[] { "ml", "Mililitre", false, 1, 1000, true });
            migrationBuilder.InsertData("SizeUnits", new string[] { "Name", "Description", "IsBaseUnit", "BaseUnitId", "ConversionMultiplierToBase", "IsActive" }, new object[] { "kg", "Kilogram", true, null, 1, true });
            migrationBuilder.InsertData("SizeUnits", new string[] { "Name", "Description", "IsBaseUnit", "BaseUnitId", "ConversionMultiplierToBase", "IsActive" }, new object[] { "g", "Gram", false, 5, 1000, true });
            migrationBuilder.InsertData("SizeUnits", new string[] { "Name", "Description", "IsBaseUnit", "BaseUnitId", "ConversionMultiplierToBase", "IsActive" }, new object[] { "Tane", null, true, null, 1, true });
            migrationBuilder.InsertData("SizeUnits", new string[] { "Name", "Description", "IsBaseUnit", "BaseUnitId", "ConversionMultiplierToBase", "IsActive" }, new object[] { "Paket", null, true, null, 1, true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_SizeUnits_SizeUnitId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemStocks_Items_ItemId",
                table: "ItemStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemStocks_SizeUnits_SizeUnitId",
                table: "ItemStocks");

            migrationBuilder.DropIndex(
                name: "IX_ItemStocks_ItemId",
                table: "ItemStocks");

            migrationBuilder.DropIndex(
                name: "IX_ItemStocks_SizeUnitId",
                table: "ItemStocks");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "ItemStocks");

            migrationBuilder.DropColumn(
                name: "SizeUnitId",
                table: "ItemStocks");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_SizeUnits_SizeUnitId",
                table: "Items",
                column: "SizeUnitId",
                principalTable: "SizeUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
