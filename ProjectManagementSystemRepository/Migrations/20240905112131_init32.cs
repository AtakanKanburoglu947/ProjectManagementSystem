using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystemRepository.Migrations
{
    /// <inheritdoc />
    public partial class init32 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserIdentityId",
                table: "Jobs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_UserIdentityId",
                table: "Jobs",
                column: "UserIdentityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_UserIdentities_UserIdentityId",
                table: "Jobs",
                column: "UserIdentityId",
                principalTable: "UserIdentities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_UserIdentities_UserIdentityId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_UserIdentityId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "UserIdentityId",
                table: "Jobs");
        }
    }
}
