using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeInv.Persistence.Migrations
{
    public partial class CategoryHomeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "ItemStocks",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 7, 12, 21, 37, 18, 703, DateTimeKind.Utc).AddTicks(7984));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "Items",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 7, 12, 21, 37, 18, 703, DateTimeKind.Utc).AddTicks(7242));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "HomeUsers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 7, 12, 21, 37, 18, 703, DateTimeKind.Utc).AddTicks(6629));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "Homes",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 7, 12, 21, 37, 18, 703, DateTimeKind.Utc).AddTicks(5792));

            migrationBuilder.AddColumn<int>(
                name: "HomeId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "Areas",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 7, 12, 21, 37, 18, 701, DateTimeKind.Utc).AddTicks(5492));

            migrationBuilder.CreateIndex(
                name: "IX_Categories_HomeId",
                table: "Categories",
                column: "HomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Homes_HomeId",
                table: "Categories",
                column: "HomeId",
                principalTable: "Homes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Homes_HomeId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_HomeId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "HomeId",
                table: "Categories");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "ItemStocks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 7, 12, 21, 37, 18, 703, DateTimeKind.Utc).AddTicks(7984),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "Items",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 7, 12, 21, 37, 18, 703, DateTimeKind.Utc).AddTicks(7242),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "HomeUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 7, 12, 21, 37, 18, 703, DateTimeKind.Utc).AddTicks(6629),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "Homes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 7, 12, 21, 37, 18, 703, DateTimeKind.Utc).AddTicks(5792),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsertTime",
                table: "Areas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 7, 12, 21, 37, 18, 701, DateTimeKind.Utc).AddTicks(5492),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");
        }
    }
}
