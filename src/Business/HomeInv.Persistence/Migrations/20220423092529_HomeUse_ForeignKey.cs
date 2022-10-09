using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeInv.Persistence.Migrations
{
    public partial class HomeUse_ForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeUsers_AspNetUsers_UserId",
                table: "HomeUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeUsers_AspNetUsers_UserId",
                table: "HomeUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeUsers_AspNetUsers_UserId",
                table: "HomeUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeUsers_AspNetUsers_UserId",
                table: "HomeUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
