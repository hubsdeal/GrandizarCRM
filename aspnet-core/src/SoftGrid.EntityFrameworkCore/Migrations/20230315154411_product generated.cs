using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class productgenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sku = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    SeoTitle = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    MetaKeywords = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegularPrice = table.Column<double>(type: "float", nullable: true),
                    SalePrice = table.Column<double>(type: "float", nullable: true),
                    PriceDiscountPercentage = table.Column<double>(type: "float", nullable: true),
                    CallForPrice = table.Column<bool>(type: "bit", nullable: false),
                    UnitPrice = table.Column<double>(type: "float", nullable: true),
                    MeasurementAmount = table.Column<int>(type: "int", nullable: true),
                    IsTaxExempt = table.Column<bool>(type: "bit", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: true),
                    IsDisplayStockQuantity = table.Column<bool>(type: "bit", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    IsPackageProduct = table.Column<bool>(type: "bit", nullable: false),
                    InternalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTemplate = table.Column<bool>(type: "bit", nullable: false),
                    PriceDiscountAmount = table.Column<double>(type: "float", nullable: true),
                    IsService = table.Column<bool>(type: "bit", nullable: false),
                    IsWholeSaleProduct = table.Column<bool>(type: "bit", nullable: false),
                    ProductManufacturerSku = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ByOrderOnly = table.Column<bool>(type: "bit", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: true),
                    ProductCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    MediaLibraryId = table.Column<long>(type: "bigint", nullable: true),
                    MeasurementUnitId = table.Column<long>(type: "bigint", nullable: true),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: true),
                    RatingLikeId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_MeasurementUnits_MeasurementUnitId",
                        column: x => x.MeasurementUnitId,
                        principalTable: "MeasurementUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_MediaLibraries_MediaLibraryId",
                        column: x => x.MediaLibraryId,
                        principalTable: "MediaLibraries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_RatingLikes_RatingLikeId",
                        column: x => x.RatingLikeId,
                        principalTable: "RatingLikes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CurrencyId",
                table: "Products",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_MeasurementUnitId",
                table: "Products",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_MediaLibraryId",
                table: "Products",
                column: "MediaLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCategoryId",
                table: "Products",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_RatingLikeId",
                table: "Products",
                column: "RatingLikeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_TenantId",
                table: "Products",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
