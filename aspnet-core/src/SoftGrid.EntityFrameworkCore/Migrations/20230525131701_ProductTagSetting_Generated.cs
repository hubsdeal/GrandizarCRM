using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class ProductTagSettingGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductMasterTagSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    CustomTagTitle = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CustomTagChatQuestion = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    DisplayPublic = table.Column<bool>(type: "bit", nullable: false),
                    AnswerTypeId = table.Column<int>(type: "int", nullable: false),
                    ProductCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    MasterTagCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMasterTagSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductMasterTagSettings_MasterTagCategories_MasterTagCategoryId",
                        column: x => x.MasterTagCategoryId,
                        principalTable: "MasterTagCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductMasterTagSettings_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductMasterTagSettings_MasterTagCategoryId",
                table: "ProductMasterTagSettings",
                column: "MasterTagCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMasterTagSettings_ProductCategoryId",
                table: "ProductMasterTagSettings",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMasterTagSettings_TenantId",
                table: "ProductMasterTagSettings",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductMasterTagSettings");
        }
    }
}
