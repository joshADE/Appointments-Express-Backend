using Microsoft.EntityFrameworkCore.Migrations;

namespace Appointments_Express_Backend.Migrations
{
    public partial class AppointmentManagementChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "id", "name" },
                values: new object[] { 6, "Edit Appointments" });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "permissionId", "roleId" },
                values: new object[,]
                {
                    { 6, 1 },
                    { 6, 2 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "permissionId", "roleId" },
                keyValues: new object[] { 6, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "permissionId", "roleId" },
                keyValues: new object[] { 6, 2 });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "id",
                keyValue: 6);
        }
    }
}
