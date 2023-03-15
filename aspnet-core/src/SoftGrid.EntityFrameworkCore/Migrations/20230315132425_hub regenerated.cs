using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class hubregenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "Hubs");

            migrationBuilder.AddColumn<long>(
                name: "PictureMediaLibraryId",
                table: "Hubs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hubs_PictureMediaLibraryId",
                table: "Hubs",
                column: "PictureMediaLibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hubs_MediaLibraries_PictureMediaLibraryId",
                table: "Hubs",
                column: "PictureMediaLibraryId",
                principalTable: "MediaLibraries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hubs_MediaLibraries_PictureMediaLibraryId",
                table: "Hubs");

            migrationBuilder.DropIndex(
                name: "IX_Hubs_PictureMediaLibraryId",
                table: "Hubs");

            migrationBuilder.DropColumn(
                name: "PictureMediaLibraryId",
                table: "Hubs");

            migrationBuilder.AddColumn<Guid>(
                name: "PictureId",
                table: "Hubs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
