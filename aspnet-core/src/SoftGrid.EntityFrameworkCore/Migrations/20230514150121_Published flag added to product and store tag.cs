using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class Publishedflagaddedtoproductandstoretag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Published",
                table: "StoreTags",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Published",
                table: "ProductTags",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Published",
                table: "StoreTags");

            migrationBuilder.DropColumn(
                name: "Published",
                table: "ProductTags");
        }
    }
}
