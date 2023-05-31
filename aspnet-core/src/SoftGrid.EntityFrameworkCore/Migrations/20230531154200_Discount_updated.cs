using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class Discountupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductCategoryId",
                table: "DiscountCodeMaps",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OpenForAll",
                table: "DiscountCodeGenerators",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RedeemCount",
                table: "DiscountCodeGenerators",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesTag",
                table: "DiscountCodeGenerators",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodeMaps_ProductCategoryId",
                table: "DiscountCodeMaps",
                column: "ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCodeMaps_ProductCategories_ProductCategoryId",
                table: "DiscountCodeMaps",
                column: "ProductCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCodeMaps_ProductCategories_ProductCategoryId",
                table: "DiscountCodeMaps");

            migrationBuilder.DropIndex(
                name: "IX_DiscountCodeMaps_ProductCategoryId",
                table: "DiscountCodeMaps");

            migrationBuilder.DropColumn(
                name: "ProductCategoryId",
                table: "DiscountCodeMaps");

            migrationBuilder.DropColumn(
                name: "OpenForAll",
                table: "DiscountCodeGenerators");

            migrationBuilder.DropColumn(
                name: "RedeemCount",
                table: "DiscountCodeGenerators");

            migrationBuilder.DropColumn(
                name: "SalesTag",
                table: "DiscountCodeGenerators");
        }
    }
}
