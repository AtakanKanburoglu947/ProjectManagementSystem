using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystemRepository.Migrations
{
    /// <inheritdoc />
    public partial class init20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Files_CommentId",
                table: "Files",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_ProjectId",
                table: "Files",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Comments_CommentId",
                table: "Files",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Projects_ProjectId",
                table: "Files",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Comments_CommentId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Projects_ProjectId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_CommentId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_ProjectId",
                table: "Files");
        }
    }
}
