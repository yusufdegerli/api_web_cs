using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_for_sambapos.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserRolesTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserRoles_UserRole_Id",
                table: "Users");
            /*migrationBuilder.DropForeignKey(
                name: "FK_Users_UserRole_UserRole_Id",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole");

            migrationBuilder.RenameTable(
                name: "UserRole",
                newName: "UserRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserRoles_UserRole_Id",
                table: "Users",
                column: "UserRole_Id",
                principalTable: "UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);*/
            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserRoles_UserRole_Id",
                table: "Users",
                column: "UserRole_Id",
                principalTable: "UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
            {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserRoles_UserRole_Id",
                table: "Users"
                );
            /*migrationBuilder.DropForeignKey(
                name: "FK_Users_UserRoles_UserRole_Id",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserRole");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserRole_UserRole_Id",
                table: "Users",
                column: "UserRole_Id",
                principalTable: "UserRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);*/
            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserRoles_UserRole_Id",
                table: "Users",
                column: "UserRole_Id",
                principalTable: "UserRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
                );
        }
    }
}
