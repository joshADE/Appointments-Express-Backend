using Microsoft.EntityFrameworkCore.Migrations;

namespace Appointments_Express_Backend.Migrations
{
    public partial class QuickProfileAndClosedStoreHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isQuickProfile",
                table: "Stores",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isOpen",
                table: "StoreHours",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isQuickProfile",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "isOpen",
                table: "StoreHours");
        }
    }
}
