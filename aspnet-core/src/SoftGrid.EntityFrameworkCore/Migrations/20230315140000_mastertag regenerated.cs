using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class mastertagregenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "MasterTags");

            migrationBuilder.AddColumn<long>(
                name: "PictureMediaLibraryId",
                table: "MasterTags",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MasterTags_PictureMediaLibraryId",
                table: "MasterTags",
                column: "PictureMediaLibraryId",
                unique: true,
                filter: "[PictureMediaLibraryId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterTags_MediaLibraries_PictureMediaLibraryId",
                table: "MasterTags",
                column: "PictureMediaLibraryId",
                principalTable: "MediaLibraries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterTags_MediaLibraries_PictureMediaLibraryId",
                table: "MasterTags");

            migrationBuilder.DropIndex(
                name: "IX_MasterTags_PictureMediaLibraryId",
                table: "MasterTags");

            migrationBuilder.DropColumn(
                name: "PictureMediaLibraryId",
                table: "MasterTags");

            migrationBuilder.AddColumn<Guid>(
                name: "PictureId",
                table: "MasterTags",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
