using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeInv.Persistence.Migrations
{
    public partial class AddHomeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Homes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Homes_AspNetUsers_InsertUserId",
                        column: x => x.InsertUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Homes_AspNetUsers_UpdateUserId",
                        column: x => x.UpdateUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Homes_InsertUserId",
                table: "Homes",
                column: "InsertUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Homes_UpdateUserId",
                table: "Homes",
                column: "UpdateUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Homes");
        }
    }
}
