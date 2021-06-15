using Microsoft.EntityFrameworkCore.Migrations;

namespace Appointments_Express_Backend.Migrations
{
    public partial class EditedPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "id",
                keyValue: 1,
                column: "name",
                value: "Delete Store");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "id",
                keyValue: 1,
                column: "name",
                value: "Create Store");
        }
    }
}
