using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystemRepository.Migrations
{
    /// <inheritdoc />
    public partial class init23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Comments_CommentId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Projects_ProjectId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_UserIdentities_UserIdentityId",
                table: "Files");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Files",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_CommentId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_ProjectId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Files");

            migrationBuilder.RenameTable(
                name: "Files",
                newName: "FileUploads");

            migrationBuilder.RenameIndex(
                name: "IX_Files_UserIdentityId",
                table: "FileUploads",
                newName: "IX_FileUploads_UserIdentityId");

            migrationBuilder.AddColumn<Guid>(
                name: "FileUploadId",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileUploads",
                table: "FileUploads",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_FileUploadId",
                table: "Comments",
                column: "FileUploadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_FileUploads_FileUploadId",
                table: "Comments",
                column: "FileUploadId",
                principalTable: "FileUploads",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileUploads_UserIdentities_UserIdentityId",
                table: "FileUploads",
                column: "UserIdentityId",
                principalTable: "UserIdentities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_FileUploads_FileUploadId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_FileUploads_UserIdentities_UserIdentityId",
                table: "FileUploads");

            migrationBuilder.DropIndex(
                name: "IX_Comments_FileUploadId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FileUploads",
                table: "FileUploads");

            migrationBuilder.DropColumn(
                name: "FileUploadId",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "FileUploads",
                newName: "Files");

            migrationBuilder.RenameIndex(
                name: "IX_FileUploads_UserIdentityId",
                table: "Files",
                newName: "IX_Files_UserIdentityId");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Files",
                table: "Files",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Files_UserIdentities_UserIdentityId",
                table: "Files",
                column: "UserIdentityId",
                principalTable: "UserIdentities",
                principalColumn: "Id");
        }
    }
}
