using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class ContactReGenerated220323 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Contacts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApplicant",
                table: "Contacts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PictureId",
                table: "Contacts",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "IsApplicant",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "Contacts");
        }
    }
}
