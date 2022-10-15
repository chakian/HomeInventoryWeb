using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeInv.Persistence.Migrations
{
    public partial class RequestedByUserColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequestedByUserId",
                table: "HomeUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomeUsers_RequestedByUserId",
                table: "HomeUsers",
                column: "RequestedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeUsers_AspNetUsers_RequestedByUserId",
                table: "HomeUsers",
                column: "RequestedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeUsers_AspNetUsers_RequestedByUserId",
                table: "HomeUsers");

            migrationBuilder.DropIndex(
                name: "IX_HomeUsers_RequestedByUserId",
                table: "HomeUsers");

            migrationBuilder.DropColumn(
                name: "RequestedByUserId",
                table: "HomeUsers");
        }
    }
}
