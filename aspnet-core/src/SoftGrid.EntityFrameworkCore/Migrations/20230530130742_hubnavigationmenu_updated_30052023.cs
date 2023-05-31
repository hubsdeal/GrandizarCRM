using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class hubnavigationmenuupdated30052023 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplaySequence",
                table: "HubNavigationMenus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasParentMenu",
                table: "HubNavigationMenus",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ParentMenuId",
                table: "HubNavigationMenus",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplaySequence",
                table: "HubNavigationMenus");

            migrationBuilder.DropColumn(
                name: "HasParentMenu",
                table: "HubNavigationMenus");

            migrationBuilder.DropColumn(
                name: "ParentMenuId",
                table: "HubNavigationMenus");
        }
    }
}
