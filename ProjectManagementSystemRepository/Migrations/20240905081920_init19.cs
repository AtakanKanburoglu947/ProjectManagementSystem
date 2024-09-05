using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystemRepository.Migrations
{
    /// <inheritdoc />
    public partial class init19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Files_FileUploadId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Files_FileUploadId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_FileUploadId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Comments_FileUploadId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "FileUploadId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "FileUploadId",
                table: "Comments");

            migrationBuilder.AddColumn<Guid>(
                name: "CommentId",
                table: "Files",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Files",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Files");

            migrationBuilder.AddColumn<Guid>(
                name: "FileUploadId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FileUploadId",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_FileUploadId",
                table: "Projects",
                column: "FileUploadId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_FileUploadId",
                table: "Comments",
                column: "FileUploadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Files_FileUploadId",
                table: "Comments",
                column: "FileUploadId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Files_FileUploadId",
                table: "Projects",
                column: "FileUploadId",
                principalTable: "Files",
                principalColumn: "Id");
        }
    }
}
