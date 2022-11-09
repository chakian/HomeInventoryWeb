using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeInv.Persistence.Migrations
{
    public partial class CreateItemStockActionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemStockActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemStockId = table.Column<int>(type: "int", nullable: false),
                    ItemStockActionTypeId = table.Column<int>(type: "int", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActionTarget = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    InsertUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemStockActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemStockActions_AspNetUsers_InsertUserId",
                        column: x => x.InsertUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemStockActions_AspNetUsers_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemStockActions_ItemStockActionTypes_ItemStockActionTypeId",
                        column: x => x.ItemStockActionTypeId,
                        principalTable: "ItemStockActionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemStockActions_ItemStocks_ItemStockId",
                        column: x => x.ItemStockId,
                        principalTable: "ItemStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemStockActions_InsertUserId",
                table: "ItemStockActions",
                column: "InsertUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemStockActions_ItemStockActionTypeId",
                table: "ItemStockActions",
                column: "ItemStockActionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemStockActions_ItemStockId",
                table: "ItemStockActions",
                column: "ItemStockId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemStockActions_UpdateUserId",
                table: "ItemStockActions",
                column: "UpdateUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemStockActions");
        }
    }
}
