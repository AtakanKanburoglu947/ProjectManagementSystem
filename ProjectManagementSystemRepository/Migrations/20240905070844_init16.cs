using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystemRepository.Migrations
{
    /// <inheritdoc />
    public partial class init16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Image_ImageId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Image_ImageId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.RenameColumn(
                name: "ImageId",
                table: "Projects",
                newName: "FileId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_ImageId",
                table: "Projects",
                newName: "IX_Projects_FileId");

            migrationBuilder.RenameColumn(
                name: "ImageId",
                table: "Comments",
                newName: "FileId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ImageId",
                table: "Comments",
                newName: "IX_Comments_FileId");

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    UserIdentityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_UserIdentities_UserIdentityId",
                        column: x => x.UserIdentityId,
                        principalTable: "UserIdentities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_UserIdentityId",
                table: "Files",
                column: "UserIdentityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Files_FileId",
                table: "Comments",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Files_FileId",
                table: "Projects",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Files_FileId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Files_FileId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "Projects",
                newName: "ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_FileId",
                table: "Projects",
                newName: "IX_Projects_ImageId");

            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "Comments",
                newName: "ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_FileId",
                table: "Comments",
                newName: "IX_Comments_ImageId");

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserIdentityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Bytes = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Image_UserIdentities_UserIdentityId",
                        column: x => x.UserIdentityId,
                        principalTable: "UserIdentities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Image_UserIdentityId",
                table: "Image",
                column: "UserIdentityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Image_ImageId",
                table: "Comments",
                column: "ImageId",
                principalTable: "Image",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Image_ImageId",
                table: "Projects",
                column: "ImageId",
                principalTable: "Image",
                principalColumn: "Id");
        }
    }
}
