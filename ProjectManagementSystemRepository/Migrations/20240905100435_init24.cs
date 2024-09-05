using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystemRepository.Migrations
{
    /// <inheritdoc />
    public partial class init24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FileUploadId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_FileUploadId",
                table: "Projects",
                column: "FileUploadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_FileUploads_FileUploadId",
                table: "Projects",
                column: "FileUploadId",
                principalTable: "FileUploads",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_FileUploads_FileUploadId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_FileUploadId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "FileUploadId",
                table: "Projects");
        }
    }
}
