using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystemRepository.Migrations
{
    /// <inheritdoc />
    public partial class init25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "FileUploads",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileUploads_ManagerId",
                table: "FileUploads",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileUploads_Managers_ManagerId",
                table: "FileUploads",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileUploads_Managers_ManagerId",
                table: "FileUploads");

            migrationBuilder.DropIndex(
                name: "IX_FileUploads_ManagerId",
                table: "FileUploads");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "FileUploads");
        }
    }
}
