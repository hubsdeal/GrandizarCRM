using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class StoreTagSettingCategoryIdaddedinstore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StoreTagSettingCategoryId",
                table: "Stores",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stores_StoreTagSettingCategoryId",
                table: "Stores",
                column: "StoreTagSettingCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_StoreTagSettingCategories_StoreTagSettingCategoryId",
                table: "Stores",
                column: "StoreTagSettingCategoryId",
                principalTable: "StoreTagSettingCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_StoreTagSettingCategories_StoreTagSettingCategoryId",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Stores_StoreTagSettingCategoryId",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "StoreTagSettingCategoryId",
                table: "Stores");
        }
    }
}
