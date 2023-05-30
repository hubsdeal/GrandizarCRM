using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class masternavigationmenuupdated30052023 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaLink",
                table: "MasterNavigationMenus");

            migrationBuilder.AddColumn<string>(
                name: "ContentLink",
                table: "MasterNavigationMenus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplaySequence",
                table: "MasterNavigationMenus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NavigationLink",
                table: "MasterNavigationMenus",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentLink",
                table: "MasterNavigationMenus");

            migrationBuilder.DropColumn(
                name: "DisplaySequence",
                table: "MasterNavigationMenus");

            migrationBuilder.DropColumn(
                name: "NavigationLink",
                table: "MasterNavigationMenus");

            migrationBuilder.AddColumn<Guid>(
                name: "MediaLink",
                table: "MasterNavigationMenus",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
