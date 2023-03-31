using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class ProductMapTableGenerated300323 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "MembershipTypes");

            migrationBuilder.CreateTable(
                name: "ProductAccountTeams",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Primary = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    RemoveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAccountTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAccountTeams_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAccountTeams_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAndGiftCardMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    PurchaseAmount = table.Column<double>(type: "float", nullable: true),
                    GiftAmount = table.Column<double>(type: "float", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAndGiftCardMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAndGiftCardMaps_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductAndGiftCardMaps_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductByVariants",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    ProductVariantId = table.Column<long>(type: "bigint", nullable: true),
                    ProductVariantCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    MediaLibraryId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductByVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductByVariants_MediaLibraries_MediaLibraryId",
                        column: x => x.MediaLibraryId,
                        principalTable: "MediaLibraries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductByVariants_ProductVariantCategories_ProductVariantCategoryId",
                        column: x => x.ProductVariantCategoryId,
                        principalTable: "ProductVariantCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductByVariants_ProductVariants_ProductVariantId",
                        column: x => x.ProductVariantId,
                        principalTable: "ProductVariants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductByVariants_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductCategoryVariantMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ProductCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    ProductVariantCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategoryVariantMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategoryVariantMaps_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategoryVariantMaps_ProductVariantCategories_ProductVariantCategoryId",
                        column: x => x.ProductVariantCategoryId,
                        principalTable: "ProductVariantCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCrossSellProducts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    CrossProductId = table.Column<long>(type: "bigint", nullable: false),
                    CrossSellScore = table.Column<int>(type: "int", nullable: true),
                    PrimaryProductId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCrossSellProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCrossSellProducts_Products_PrimaryProductId",
                        column: x => x.PrimaryProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductFlashSaleProductMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    FlashSalePrice = table.Column<double>(type: "float", nullable: true),
                    DiscountPercentage = table.Column<double>(type: "float", nullable: true),
                    DiscountAmount = table.Column<double>(type: "float", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
                    MembershipTypeId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFlashSaleProductMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductFlashSaleProductMaps_MembershipTypes_MembershipTypeId",
                        column: x => x.MembershipTypeId,
                        principalTable: "MembershipTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductFlashSaleProductMaps_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductFlashSaleProductMaps_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductOwnerPublicContactInfos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ShortBio = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    Publish = table.Column<bool>(type: "bit", nullable: false),
                    PhotoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOwnerPublicContactInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductOwnerPublicContactInfos_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductOwnerPublicContactInfos_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductOwnerPublicContactInfos_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductOwnerPublicContactInfos_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductReviews",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ReviewInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Publish = table.Column<bool>(type: "bit", nullable: false),
                    PostTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
                    RatingLikeId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductReviews_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductReviews_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductReviews_RatingLikes_RatingLikeId",
                        column: x => x.RatingLikeId,
                        principalTable: "RatingLikes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductReviews_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductUpsellRelatedProducts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    RelatedProductId = table.Column<long>(type: "bigint", nullable: false),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    PrimaryProductId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductUpsellRelatedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductUpsellRelatedProducts_Products_PrimaryProductId",
                        column: x => x.PrimaryProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductWholeSaleQuantityTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    MinQty = table.Column<int>(type: "int", nullable: true),
                    MaxQty = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductWholeSaleQuantityTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReturnStatuses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReturnTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductReviewFeedbacks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ReplyText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    ProductReviewId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_ProductReviewFeedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductReviewFeedbacks_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductReviewFeedbacks_ProductReviews_ProductReviewId",
                        column: x => x.ProductReviewId,
                        principalTable: "ProductReviews",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductReviewFeedbacks_RatingLikes_RatingLikeId",
                        column: x => x.RatingLikeId,
                        principalTable: "RatingLikes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductWholeSalePrices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    ExactQuantity = table.Column<double>(type: "float", nullable: true),
                    PackageInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackageQuantity = table.Column<int>(type: "int", nullable: true),
                    WholeSaleSkuId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    ProductWholeSaleQuantityTypeId = table.Column<long>(type: "bigint", nullable: true),
                    MeasurementUnitId = table.Column<long>(type: "bigint", nullable: true),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductWholeSalePrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductWholeSalePrices_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductWholeSalePrices_MeasurementUnits_MeasurementUnitId",
                        column: x => x.MeasurementUnitId,
                        principalTable: "MeasurementUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductWholeSalePrices_ProductWholeSaleQuantityTypes_ProductWholeSaleQuantityTypeId",
                        column: x => x.ProductWholeSaleQuantityTypeId,
                        principalTable: "ProductWholeSaleQuantityTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductWholeSalePrices_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductReturnInfos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    CustomerNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ReturnTypeId = table.Column<long>(type: "bigint", nullable: true),
                    ReturnStatusId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_ProductReturnInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductReturnInfos_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductReturnInfos_ReturnStatuses_ReturnStatusId",
                        column: x => x.ReturnStatusId,
                        principalTable: "ReturnStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductReturnInfos_ReturnTypes_ReturnTypeId",
                        column: x => x.ReturnTypeId,
                        principalTable: "ReturnTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAccountTeams_EmployeeId",
                table: "ProductAccountTeams",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAccountTeams_ProductId",
                table: "ProductAccountTeams",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAccountTeams_TenantId",
                table: "ProductAccountTeams",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAndGiftCardMaps_CurrencyId",
                table: "ProductAndGiftCardMaps",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAndGiftCardMaps_ProductId",
                table: "ProductAndGiftCardMaps",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAndGiftCardMaps_TenantId",
                table: "ProductAndGiftCardMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductByVariants_MediaLibraryId",
                table: "ProductByVariants",
                column: "MediaLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductByVariants_ProductId",
                table: "ProductByVariants",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductByVariants_ProductVariantCategoryId",
                table: "ProductByVariants",
                column: "ProductVariantCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductByVariants_ProductVariantId",
                table: "ProductByVariants",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductByVariants_TenantId",
                table: "ProductByVariants",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryVariantMaps_ProductCategoryId",
                table: "ProductCategoryVariantMaps",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryVariantMaps_ProductVariantCategoryId",
                table: "ProductCategoryVariantMaps",
                column: "ProductVariantCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryVariantMaps_TenantId",
                table: "ProductCategoryVariantMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCrossSellProducts_PrimaryProductId",
                table: "ProductCrossSellProducts",
                column: "PrimaryProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCrossSellProducts_TenantId",
                table: "ProductCrossSellProducts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFlashSaleProductMaps_MembershipTypeId",
                table: "ProductFlashSaleProductMaps",
                column: "MembershipTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFlashSaleProductMaps_ProductId",
                table: "ProductFlashSaleProductMaps",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFlashSaleProductMaps_StoreId",
                table: "ProductFlashSaleProductMaps",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFlashSaleProductMaps_TenantId",
                table: "ProductFlashSaleProductMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOwnerPublicContactInfos_ContactId",
                table: "ProductOwnerPublicContactInfos",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOwnerPublicContactInfos_ProductId",
                table: "ProductOwnerPublicContactInfos",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOwnerPublicContactInfos_StoreId",
                table: "ProductOwnerPublicContactInfos",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOwnerPublicContactInfos_TenantId",
                table: "ProductOwnerPublicContactInfos",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOwnerPublicContactInfos_UserId",
                table: "ProductOwnerPublicContactInfos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReturnInfos_ProductId",
                table: "ProductReturnInfos",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReturnInfos_ReturnStatusId",
                table: "ProductReturnInfos",
                column: "ReturnStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReturnInfos_ReturnTypeId",
                table: "ProductReturnInfos",
                column: "ReturnTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReturnInfos_TenantId",
                table: "ProductReturnInfos",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviewFeedbacks_ContactId",
                table: "ProductReviewFeedbacks",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviewFeedbacks_ProductReviewId",
                table: "ProductReviewFeedbacks",
                column: "ProductReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviewFeedbacks_RatingLikeId",
                table: "ProductReviewFeedbacks",
                column: "RatingLikeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviewFeedbacks_TenantId",
                table: "ProductReviewFeedbacks",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_ContactId",
                table: "ProductReviews",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_ProductId",
                table: "ProductReviews",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_RatingLikeId",
                table: "ProductReviews",
                column: "RatingLikeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_StoreId",
                table: "ProductReviews",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_TenantId",
                table: "ProductReviews",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductUpsellRelatedProducts_PrimaryProductId",
                table: "ProductUpsellRelatedProducts",
                column: "PrimaryProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductUpsellRelatedProducts_TenantId",
                table: "ProductUpsellRelatedProducts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWholeSalePrices_CurrencyId",
                table: "ProductWholeSalePrices",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWholeSalePrices_MeasurementUnitId",
                table: "ProductWholeSalePrices",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWholeSalePrices_ProductId",
                table: "ProductWholeSalePrices",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWholeSalePrices_ProductWholeSaleQuantityTypeId",
                table: "ProductWholeSalePrices",
                column: "ProductWholeSaleQuantityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWholeSalePrices_TenantId",
                table: "ProductWholeSalePrices",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWholeSaleQuantityTypes_TenantId",
                table: "ProductWholeSaleQuantityTypes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnStatuses_TenantId",
                table: "ReturnStatuses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnTypes_TenantId",
                table: "ReturnTypes",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductAccountTeams");

            migrationBuilder.DropTable(
                name: "ProductAndGiftCardMaps");

            migrationBuilder.DropTable(
                name: "ProductByVariants");

            migrationBuilder.DropTable(
                name: "ProductCategoryVariantMaps");

            migrationBuilder.DropTable(
                name: "ProductCrossSellProducts");

            migrationBuilder.DropTable(
                name: "ProductFlashSaleProductMaps");

            migrationBuilder.DropTable(
                name: "ProductOwnerPublicContactInfos");

            migrationBuilder.DropTable(
                name: "ProductReturnInfos");

            migrationBuilder.DropTable(
                name: "ProductReviewFeedbacks");

            migrationBuilder.DropTable(
                name: "ProductUpsellRelatedProducts");

            migrationBuilder.DropTable(
                name: "ProductWholeSalePrices");

            migrationBuilder.DropTable(
                name: "ReturnStatuses");

            migrationBuilder.DropTable(
                name: "ReturnTypes");

            migrationBuilder.DropTable(
                name: "ProductReviews");

            migrationBuilder.DropTable(
                name: "ProductWholeSaleQuantityTypes");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MembershipTypes",
                type: "nvarchar(1500)",
                maxLength: 1500,
                nullable: true);
        }
    }
}
