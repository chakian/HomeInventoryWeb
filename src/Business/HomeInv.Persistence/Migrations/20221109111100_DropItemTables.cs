using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeInv.Persistence.Migrations
{
    public partial class DropItemTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemStocks");

            migrationBuilder.DropTable(
                name: "Items");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    HomeId = table.Column<int>(type: "int", nullable: false),
                    InsertUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SizeUnitId = table.Column<int>(type: "int", nullable: false),
                    UpdateUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsContainer = table.Column<bool>(type: "bit", nullable: false),
                    IsExpirable = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_AspNetUsers_InsertUserId",
                        column: x => x.InsertUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_AspNetUsers_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Items_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Homes_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Homes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_SizeUnits_SizeUnitId",
                        column: x => x.SizeUnitId,
                        principalTable: "SizeUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemStocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AreaId = table.Column<int>(type: "int", nullable: false),
                    ContainerId = table.Column<int>(type: "int", nullable: true),
                    InsertUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    SizeUnitId = table.Column<int>(type: "int", nullable: false),
                    UpdateUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemStocks_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemStocks_AspNetUsers_InsertUserId",
                        column: x => x.InsertUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemStocks_AspNetUsers_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemStocks_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemStocks_ItemStocks_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "ItemStocks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemStocks_SizeUnits_SizeUnitId",
                        column: x => x.SizeUnitId,
                        principalTable: "SizeUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryId",
                table: "Items",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_HomeId",
                table: "Items",
                column: "HomeId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_InsertUserId",
                table: "Items",
                column: "InsertUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_SizeUnitId",
                table: "Items",
                column: "SizeUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_UpdateUserId",
                table: "Items",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemStocks_AreaId",
                table: "ItemStocks",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemStocks_ContainerId",
                table: "ItemStocks",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemStocks_InsertUserId",
                table: "ItemStocks",
                column: "InsertUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemStocks_ItemId",
                table: "ItemStocks",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemStocks_SizeUnitId",
                table: "ItemStocks",
                column: "SizeUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemStocks_UpdateUserId",
                table: "ItemStocks",
                column: "UpdateUserId");
        }
    }
}
