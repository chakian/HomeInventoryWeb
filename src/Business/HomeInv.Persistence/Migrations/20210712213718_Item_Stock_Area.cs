using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeInv.Persistence.Migrations
{
    public partial class Item_Stock_Area : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "HomeUsers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "HomeUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 7, 12, 21, 37, 18, 703, DateTimeKind.Utc).AddTicks(6629),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "Homes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "Homes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 7, 12, 21, 37, 18, 703, DateTimeKind.Utc).AddTicks(5792),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HomeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    InsertUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2021, 7, 12, 21, 37, 18, 701, DateTimeKind.Utc).AddTicks(5492)),
                    UpdateUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Areas_AspNetUsers_InsertUserId",
                        column: x => x.InsertUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Areas_AspNetUsers_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Areas_Homes_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Homes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SizeUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsBaseUnit = table.Column<bool>(type: "bit", nullable: false),
                    BaseUnitId = table.Column<int>(type: "int", nullable: true),
                    ConversionMultiplierToBase = table.Column<decimal>(type: "decimal(28,14)", precision: 28, scale: 14, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SizeUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemStocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: false),
                    ContainerId = table.Column<int>(type: "int", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    InsertUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2021, 7, 12, 21, 37, 18, 703, DateTimeKind.Utc).AddTicks(7984)),
                    UpdateUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemStocks_AspNetUsers_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemStocks_ItemStocks_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "ItemStocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SizeUnitId = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsExpirable = table.Column<bool>(type: "bit", nullable: false),
                    IsContainer = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    InsertUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2021, 7, 12, 21, 37, 18, 703, DateTimeKind.Utc).AddTicks(7242)),
                    UpdateUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_AspNetUsers_InsertUserId",
                        column: x => x.InsertUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_AspNetUsers_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_SizeUnits_SizeUnitId",
                        column: x => x.SizeUnitId,
                        principalTable: "SizeUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Areas_HomeId",
                table: "Areas",
                column: "HomeId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_InsertUserId",
                table: "Areas",
                column: "InsertUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_UpdateUserId",
                table: "Areas",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryId",
                table: "Items",
                column: "CategoryId");

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
                name: "IX_ItemStocks_UpdateUserId",
                table: "ItemStocks",
                column: "UpdateUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "ItemStocks");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "SizeUnits");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "HomeUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "HomeUsers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 7, 12, 21, 37, 18, 703, DateTimeKind.Utc).AddTicks(6629));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "Homes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "Homes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 7, 12, 21, 37, 18, 703, DateTimeKind.Utc).AddTicks(5792));
        }
    }
}
