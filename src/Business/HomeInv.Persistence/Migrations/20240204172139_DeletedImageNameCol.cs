using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeInv.Persistence.Migrations
{
    public partial class DeletedImageNameCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "ItemDefinitions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "ItemDefinitions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
