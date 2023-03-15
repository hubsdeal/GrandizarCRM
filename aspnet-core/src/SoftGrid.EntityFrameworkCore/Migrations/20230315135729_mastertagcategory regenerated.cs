using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class mastertagcategoryregenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "MasterTagCategories");

            migrationBuilder.AddColumn<long>(
                name: "PictureMediaLibraryId",
                table: "MasterTagCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MasterTagCategories_PictureMediaLibraryId",
                table: "MasterTagCategories",
                column: "PictureMediaLibraryId",
                unique: true,
                filter: "[PictureMediaLibraryId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterTagCategories_MediaLibraries_PictureMediaLibraryId",
                table: "MasterTagCategories",
                column: "PictureMediaLibraryId",
                principalTable: "MediaLibraries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterTagCategories_MediaLibraries_PictureMediaLibraryId",
                table: "MasterTagCategories");

            migrationBuilder.DropIndex(
                name: "IX_MasterTagCategories_PictureMediaLibraryId",
                table: "MasterTagCategories");

            migrationBuilder.DropColumn(
                name: "PictureMediaLibraryId",
                table: "MasterTagCategories");

            migrationBuilder.AddColumn<Guid>(
                name: "PictureId",
                table: "MasterTagCategories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
