using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Appointments_Express_Backend.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(300)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    location = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    minTimeBlock = table.Column<int>(type: "int", nullable: false),
                    maxTimeBlock = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    username = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    roleId = table.Column<int>(type: "int", nullable: false),
                    permissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.roleId, x.permissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_permissionId",
                        column: x => x.permissionId,
                        principalTable: "Permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_roleId",
                        column: x => x.roleId,
                        principalTable: "Roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    storeId = table.Column<int>(type: "int", nullable: false),
                    customerId = table.Column<int>(type: "int", nullable: false),
                    start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end = table.Column<DateTime>(type: "datetime2", nullable: false),
                    title = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Appointments_Customers_customerId",
                        column: x => x.customerId,
                        principalTable: "Customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Stores_storeId",
                        column: x => x.storeId,
                        principalTable: "Stores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClosedDaysTimes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    storeId = table.Column<int>(type: "int", nullable: false),
                    from = table.Column<DateTime>(type: "datetime2", nullable: false),
                    to = table.Column<DateTime>(type: "datetime2", nullable: false),
                    repeat = table.Column<bool>(type: "bit", nullable: false),
                    repeatInterval = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClosedDaysTimes", x => x.id);
                    table.ForeignKey(
                        name: "FK_ClosedDaysTimes_Stores_storeId",
                        column: x => x.storeId,
                        principalTable: "Stores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreHours",
                columns: table => new
                {
                    storeId = table.Column<int>(type: "int", nullable: false),
                    dayOfWeek = table.Column<int>(type: "int", nullable: false),
                    open = table.Column<TimeSpan>(type: "time", nullable: false),
                    close = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreHours", x => new { x.storeId, x.dayOfWeek });
                    table.ForeignKey(
                        name: "FK_StoreHours_Stores_storeId",
                        column: x => x.storeId,
                        principalTable: "Stores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStoreRoles",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false),
                    storeId = table.Column<int>(type: "int", nullable: false),
                    roleId = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStoreRoles", x => new { x.userId, x.storeId, x.roleId });
                    table.ForeignKey(
                        name: "FK_UserStoreRoles_Roles_roleId",
                        column: x => x.roleId,
                        principalTable: "Roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStoreRoles_Stores_storeId",
                        column: x => x.storeId,
                        principalTable: "Stores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStoreRoles_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Create Store" },
                    { 2, "Edit Store Details" },
                    { 3, "Edit Store Hours" },
                    { 4, "Edit Closed Times" },
                    { 5, "Assign Managers" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { 1, "Owner/Creator of the store", "Owner" },
                    { 2, "Manager of the store and appointment times", "Manager" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_customerId",
                table: "Appointments",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_storeId",
                table: "Appointments",
                column: "storeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClosedDaysTimes_storeId",
                table: "ClosedDaysTimes",
                column: "storeId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_name",
                table: "Permissions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_permissionId",
                table: "RolePermissions",
                column: "permissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_name",
                table: "Roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_email",
                table: "Users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_username",
                table: "Users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserStoreRoles_roleId",
                table: "UserStoreRoles",
                column: "roleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStoreRoles_storeId",
                table: "UserStoreRoles",
                column: "storeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "ClosedDaysTimes");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "StoreHours");

            migrationBuilder.DropTable(
                name: "UserStoreRoles");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
