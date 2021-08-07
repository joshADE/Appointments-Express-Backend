using Microsoft.EntityFrameworkCore.Migrations;

namespace Appointments_Express_Backend.Migrations
{
    public partial class Images : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "avatarPublicId",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "avatarUrl",
                table: "Users",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avatarPublicId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "avatarUrl",
                table: "Users");
        }
    }
}
