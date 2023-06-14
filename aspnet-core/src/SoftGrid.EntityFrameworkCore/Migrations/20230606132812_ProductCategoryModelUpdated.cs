using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class ProductCategoryModelUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductCategoryId",
                table: "ProductCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_ProductCategoryId",
                table: "ProductCategories",
                column: "ProductCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_ProductCategories_ProductCategoryId",
                table: "ProductCategories",
                column: "ProductCategoryId",
                principalTable: "ProductCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_ProductCategories_ProductCategoryId",
                table: "ProductCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_ProductCategoryId",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "ProductCategoryId",
                table: "ProductCategories");
        }
    }
}
