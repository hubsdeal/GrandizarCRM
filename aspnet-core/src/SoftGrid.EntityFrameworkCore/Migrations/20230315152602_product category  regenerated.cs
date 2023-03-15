using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class productcategoryregenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasParentCategory = table.Column<bool>(type: "bit", nullable: false),
                    ParentCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    MetaKeywords = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    ProductOrService = table.Column<bool>(type: "bit", nullable: false),
                    MediaLibraryId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategories_MediaLibraries_MediaLibraryId",
                        column: x => x.MediaLibraryId,
                        principalTable: "MediaLibraries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_MediaLibraryId",
                table: "ProductCategories",
                column: "MediaLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_TenantId",
                table: "ProductCategories",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCategories");
        }
    }
}
