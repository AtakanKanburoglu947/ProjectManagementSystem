using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystemRepository.Migrations
{
    /// <inheritdoc />
    public partial class init17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Files_FileId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_UserIdentities_UserIdentityId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Files_FileId",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "Projects",
                newName: "FileUploadId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_FileId",
                table: "Projects",
                newName: "IX_Projects_FileUploadId");

            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "Comments",
                newName: "FileUploadId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_FileId",
                table: "Comments",
                newName: "IX_Comments_FileUploadId");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserIdentityId",
                table: "Files",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Files_FileUploadId",
                table: "Comments",
                column: "FileUploadId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_UserIdentities_UserIdentityId",
                table: "Files",
                column: "UserIdentityId",
                principalTable: "UserIdentities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Files_FileUploadId",
                table: "Projects",
                column: "FileUploadId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Files_FileUploadId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_UserIdentities_UserIdentityId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Files_FileUploadId",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "FileUploadId",
                table: "Projects",
                newName: "FileId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_FileUploadId",
                table: "Projects",
                newName: "IX_Projects_FileId");

            migrationBuilder.RenameColumn(
                name: "FileUploadId",
                table: "Comments",
                newName: "FileId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_FileUploadId",
                table: "Comments",
                newName: "IX_Comments_FileId");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserIdentityId",
                table: "Files",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Files_FileId",
                table: "Comments",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_UserIdentities_UserIdentityId",
                table: "Files",
                column: "UserIdentityId",
                principalTable: "UserIdentities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Files_FileId",
                table: "Projects",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id");
        }
    }
}
