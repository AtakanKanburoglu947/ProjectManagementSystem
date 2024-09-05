using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystemRepository.Migrations
{
    /// <inheritdoc />
    public partial class init30 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FileUploadId",
                table: "Jobs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Jobs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_FileUploadId",
                table: "Jobs",
                column: "FileUploadId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ManagerId",
                table: "Jobs",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ProjectId",
                table: "Jobs",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_FileUploads_FileUploadId",
                table: "Jobs",
                column: "FileUploadId",
                principalTable: "FileUploads",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Managers_ManagerId",
                table: "Jobs",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Projects_ProjectId",
                table: "Jobs",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_FileUploads_FileUploadId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Managers_ManagerId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Projects_ProjectId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_FileUploadId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_ManagerId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_ProjectId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "FileUploadId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Jobs");
        }
    }
}
