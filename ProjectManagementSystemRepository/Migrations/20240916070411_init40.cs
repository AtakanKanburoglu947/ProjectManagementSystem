using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystemRepository.Migrations
{
    /// <inheritdoc />
    public partial class init40 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Managers_ManagerId",
                table: "Jobs");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Managers_ManagerId",
                table: "Jobs",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Managers_ManagerId",
                table: "Jobs");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Managers_ManagerId",
                table: "Jobs",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id");
        }
    }
}
