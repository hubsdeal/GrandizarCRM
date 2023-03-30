using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class ProductMapTableGenerated290323 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductCategoryMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ProductCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategoryMaps", x => x.Id);
                    //table.ForeignKey(
                    //    name: "FK_ProductCategoryMaps_ProductCategories_ProductCategoryId",
                    //    column: x => x.ProductCategoryId,
                    //    principalTable: "ProductCategories",
                    //    principalColumn: "Id",
                    //    onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategoryMaps_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategoryTeams",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Primary = table.Column<bool>(type: "bit", nullable: false),
                    ProductCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategoryTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategoryTeams_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategoryTeams_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCustomerQueries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Question = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AnswerTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Publish = table.Column<bool>(type: "bit", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCustomerQueries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCustomerQueries_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductCustomerQueries_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductCustomerQueries_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductFaqs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Question = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Template = table.Column<bool>(type: "bit", nullable: false),
                    Publish = table.Column<bool>(type: "bit", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFaqs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductFaqs_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductMedias",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    MediaLibraryId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMedias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductMedias_MediaLibraries_MediaLibraryId",
                        column: x => x.MediaLibraryId,
                        principalTable: "MediaLibraries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductMedias_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductNotes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductNotes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPackages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    PackageProductId = table.Column<long>(type: "bigint", nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    PrimaryProductId = table.Column<long>(type: "bigint", nullable: false),
                    MediaLibraryId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPackages_MediaLibraries_MediaLibraryId",
                        column: x => x.MediaLibraryId,
                        principalTable: "MediaLibraries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductPackages_Products_PrimaryProductId",
                        column: x => x.PrimaryProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariantCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariantCategories_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SocialMedias",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMedias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    NumberOfDays = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariants",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProductVariantCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariants_ProductVariantCategories_ProductVariantCategoryId",
                        column: x => x.ProductVariantCategoryId,
                        principalTable: "ProductVariantCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductCustomerStats",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Clicked = table.Column<bool>(type: "bit", nullable: false),
                    WishedOrFavorite = table.Column<bool>(type: "bit", nullable: false),
                    Purchased = table.Column<bool>(type: "bit", nullable: false),
                    PurchasedQuantity = table.Column<int>(type: "int", nullable: true),
                    Shared = table.Column<bool>(type: "bit", nullable: false),
                    PageLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppOrWeb = table.Column<bool>(type: "bit", nullable: false),
                    QuitFromLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
                    HubId = table.Column<long>(type: "bigint", nullable: true),
                    SocialMediaId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCustomerStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCustomerStats_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductCustomerStats_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductCustomerStats_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductCustomerStats_SocialMedias_SocialMediaId",
                        column: x => x.SocialMediaId,
                        principalTable: "SocialMedias",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductCustomerStats_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductSubscriptionMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DiscountPercentage = table.Column<double>(type: "float", nullable: true),
                    DiscountAmount = table.Column<double>(type: "float", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    SubscriptionTypeId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSubscriptionMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSubscriptionMaps_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductSubscriptionMaps_SubscriptionTypes_SubscriptionTypeId",
                        column: x => x.SubscriptionTypeId,
                        principalTable: "SubscriptionTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryMaps_ProductCategoryId",
                table: "ProductCategoryMaps",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryMaps_ProductId",
                table: "ProductCategoryMaps",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryMaps_TenantId",
                table: "ProductCategoryMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryTeams_EmployeeId",
                table: "ProductCategoryTeams",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryTeams_ProductCategoryId",
                table: "ProductCategoryTeams",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategoryTeams_TenantId",
                table: "ProductCategoryTeams",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCustomerQueries_ContactId",
                table: "ProductCustomerQueries",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCustomerQueries_EmployeeId",
                table: "ProductCustomerQueries",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCustomerQueries_ProductId",
                table: "ProductCustomerQueries",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCustomerQueries_TenantId",
                table: "ProductCustomerQueries",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCustomerStats_ContactId",
                table: "ProductCustomerStats",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCustomerStats_HubId",
                table: "ProductCustomerStats",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCustomerStats_ProductId",
                table: "ProductCustomerStats",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCustomerStats_SocialMediaId",
                table: "ProductCustomerStats",
                column: "SocialMediaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCustomerStats_StoreId",
                table: "ProductCustomerStats",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCustomerStats_TenantId",
                table: "ProductCustomerStats",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFaqs_ProductId",
                table: "ProductFaqs",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFaqs_TenantId",
                table: "ProductFaqs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMedias_MediaLibraryId",
                table: "ProductMedias",
                column: "MediaLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMedias_ProductId",
                table: "ProductMedias",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMedias_TenantId",
                table: "ProductMedias",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductNotes_ProductId",
                table: "ProductNotes",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductNotes_TenantId",
                table: "ProductNotes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPackages_MediaLibraryId",
                table: "ProductPackages",
                column: "MediaLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPackages_PrimaryProductId",
                table: "ProductPackages",
                column: "PrimaryProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPackages_TenantId",
                table: "ProductPackages",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubscriptionMaps_ProductId",
                table: "ProductSubscriptionMaps",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubscriptionMaps_SubscriptionTypeId",
                table: "ProductSubscriptionMaps",
                column: "SubscriptionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubscriptionMaps_TenantId",
                table: "ProductSubscriptionMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantCategories_StoreId",
                table: "ProductVariantCategories",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantCategories_TenantId",
                table: "ProductVariantCategories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ProductVariantCategoryId",
                table: "ProductVariants",
                column: "ProductVariantCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_TenantId",
                table: "ProductVariants",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialMedias_TenantId",
                table: "SocialMedias",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionTypes_TenantId",
                table: "SubscriptionTypes",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCategoryMaps");

            migrationBuilder.DropTable(
                name: "ProductCategoryTeams");

            migrationBuilder.DropTable(
                name: "ProductCustomerQueries");

            migrationBuilder.DropTable(
                name: "ProductCustomerStats");

            migrationBuilder.DropTable(
                name: "ProductFaqs");

            migrationBuilder.DropTable(
                name: "ProductMedias");

            migrationBuilder.DropTable(
                name: "ProductNotes");

            migrationBuilder.DropTable(
                name: "ProductPackages");

            migrationBuilder.DropTable(
                name: "ProductSubscriptionMaps");

            migrationBuilder.DropTable(
                name: "ProductVariants");

            migrationBuilder.DropTable(
                name: "SocialMedias");

            migrationBuilder.DropTable(
                name: "SubscriptionTypes");

            migrationBuilder.DropTable(
                name: "ProductVariantCategories");
        }
    }
}
