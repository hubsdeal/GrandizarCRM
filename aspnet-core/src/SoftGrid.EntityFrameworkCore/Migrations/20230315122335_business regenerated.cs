using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class businessregenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Businesses");

            migrationBuilder.AddColumn<long>(
                name: "LogoMediaLibraryId",
                table: "Businesses",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_LogoMediaLibraryId",
                table: "Businesses",
                column: "LogoMediaLibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_MediaLibraries_LogoMediaLibraryId",
                table: "Businesses",
                column: "LogoMediaLibraryId",
                principalTable: "MediaLibraries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_MediaLibraries_LogoMediaLibraryId",
                table: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_LogoMediaLibraryId",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "LogoMediaLibraryId",
                table: "Businesses");

            migrationBuilder.AddColumn<Guid>(
                name: "Logo",
                table: "Businesses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
