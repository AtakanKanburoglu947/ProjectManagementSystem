using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystemRepository.Migrations
{
    /// <inheritdoc />
    public partial class init4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Manager_ManagerId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Manager_Roles_RoleId",
                table: "Manager");

            migrationBuilder.DropForeignKey(
                name: "FK_Manager_UserIdentities_UserIdentityId",
                table: "Manager");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Manager_ManagerId",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Manager",
                table: "Manager");

            migrationBuilder.RenameTable(
                name: "Manager",
                newName: "Managers");

            migrationBuilder.RenameIndex(
                name: "IX_Manager_UserIdentityId",
                table: "Managers",
                newName: "IX_Managers_UserIdentityId");

            migrationBuilder.RenameIndex(
                name: "IX_Manager_RoleId",
                table: "Managers",
                newName: "IX_Managers_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Managers",
                table: "Managers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Managers_ManagerId",
                table: "Comments",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_Roles_RoleId",
                table: "Managers",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Managers_UserIdentities_UserIdentityId",
                table: "Managers",
                column: "UserIdentityId",
                principalTable: "UserIdentities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Managers_ManagerId",
                table: "Projects",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Managers_ManagerId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_Roles_RoleId",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_Managers_UserIdentities_UserIdentityId",
                table: "Managers");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Managers_ManagerId",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Managers",
                table: "Managers");

            migrationBuilder.RenameTable(
                name: "Managers",
                newName: "Manager");

            migrationBuilder.RenameIndex(
                name: "IX_Managers_UserIdentityId",
                table: "Manager",
                newName: "IX_Manager_UserIdentityId");

            migrationBuilder.RenameIndex(
                name: "IX_Managers_RoleId",
                table: "Manager",
                newName: "IX_Manager_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Manager",
                table: "Manager",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Manager_ManagerId",
                table: "Comments",
                column: "ManagerId",
                principalTable: "Manager",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Manager_Roles_RoleId",
                table: "Manager",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Manager_UserIdentities_UserIdentityId",
                table: "Manager",
                column: "UserIdentityId",
                principalTable: "UserIdentities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Manager_ManagerId",
                table: "Projects",
                column: "ManagerId",
                principalTable: "Manager",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
