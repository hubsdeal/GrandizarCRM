using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class HubandHubMaptableGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HubAccountTeams",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    PrimaryManager = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HubId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubAccountTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HubAccountTeams_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HubAccountTeams_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HubAccountTeams_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HubBusinesses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    DisplayScore = table.Column<int>(type: "int", nullable: true),
                    HubId = table.Column<long>(type: "bigint", nullable: false),
                    BusinessId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubBusinesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HubBusinesses_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HubBusinesses_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HubContacts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DisplayScore = table.Column<int>(type: "int", nullable: true),
                    HubId = table.Column<long>(type: "bigint", nullable: false),
                    ContactId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HubContacts_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HubContacts_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HubProductCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    DisplayScore = table.Column<int>(type: "int", nullable: true),
                    HubId = table.Column<long>(type: "bigint", nullable: false),
                    ProductCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubProductCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HubProductCategories_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HubProductCategories_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HubProducts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    DisplayScore = table.Column<int>(type: "int", nullable: true),
                    HubId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HubProducts_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HubProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HubSalesProjections",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DurationTypeId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpectedSalesAmount = table.Column<double>(type: "float", nullable: true),
                    ActualSalesAmount = table.Column<double>(type: "float", nullable: true),
                    HubId = table.Column<long>(type: "bigint", nullable: false),
                    ProductCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubSalesProjections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HubSalesProjections_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HubSalesProjections_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HubSalesProjections_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HubSalesProjections_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HubStores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    HubId = table.Column<long>(type: "bigint", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubStores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HubStores_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HubStores_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HubZipCodeMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    CityName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HubId = table.Column<long>(type: "bigint", nullable: false),
                    CityId = table.Column<long>(type: "bigint", nullable: true),
                    ZipCodeId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubZipCodeMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HubZipCodeMaps_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HubZipCodeMaps_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HubZipCodeMaps_ZipCodes_ZipCodeId",
                        column: x => x.ZipCodeId,
                        principalTable: "ZipCodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MasterNavigationMenus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    HasParentMenu = table.Column<bool>(type: "bit", nullable: false),
                    ParentMenuId = table.Column<long>(type: "bigint", nullable: true),
                    IconLink = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MediaLink = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterNavigationMenus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HubNavigationMenus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    CustomName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    NavigationLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HubId = table.Column<long>(type: "bigint", nullable: false),
                    MasterNavigationMenuId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HubNavigationMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HubNavigationMenus_Hubs_HubId",
                        column: x => x.HubId,
                        principalTable: "Hubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HubNavigationMenus_MasterNavigationMenus_MasterNavigationMenuId",
                        column: x => x.MasterNavigationMenuId,
                        principalTable: "MasterNavigationMenus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HubAccountTeams_EmployeeId",
                table: "HubAccountTeams",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HubAccountTeams_HubId",
                table: "HubAccountTeams",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_HubAccountTeams_TenantId",
                table: "HubAccountTeams",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HubAccountTeams_UserId",
                table: "HubAccountTeams",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HubBusinesses_BusinessId",
                table: "HubBusinesses",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_HubBusinesses_HubId",
                table: "HubBusinesses",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_HubBusinesses_TenantId",
                table: "HubBusinesses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HubContacts_ContactId",
                table: "HubContacts",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_HubContacts_HubId",
                table: "HubContacts",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_HubContacts_TenantId",
                table: "HubContacts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HubNavigationMenus_HubId",
                table: "HubNavigationMenus",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_HubNavigationMenus_MasterNavigationMenuId",
                table: "HubNavigationMenus",
                column: "MasterNavigationMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_HubNavigationMenus_TenantId",
                table: "HubNavigationMenus",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HubProductCategories_HubId",
                table: "HubProductCategories",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_HubProductCategories_ProductCategoryId",
                table: "HubProductCategories",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_HubProductCategories_TenantId",
                table: "HubProductCategories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HubProducts_HubId",
                table: "HubProducts",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_HubProducts_ProductId",
                table: "HubProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_HubProducts_TenantId",
                table: "HubProducts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HubSalesProjections_CurrencyId",
                table: "HubSalesProjections",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_HubSalesProjections_HubId",
                table: "HubSalesProjections",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_HubSalesProjections_ProductCategoryId",
                table: "HubSalesProjections",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_HubSalesProjections_StoreId",
                table: "HubSalesProjections",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_HubSalesProjections_TenantId",
                table: "HubSalesProjections",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HubStores_HubId",
                table: "HubStores",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_HubStores_StoreId",
                table: "HubStores",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_HubStores_TenantId",
                table: "HubStores",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HubZipCodeMaps_CityId",
                table: "HubZipCodeMaps",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_HubZipCodeMaps_HubId",
                table: "HubZipCodeMaps",
                column: "HubId");

            migrationBuilder.CreateIndex(
                name: "IX_HubZipCodeMaps_TenantId",
                table: "HubZipCodeMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_HubZipCodeMaps_ZipCodeId",
                table: "HubZipCodeMaps",
                column: "ZipCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterNavigationMenus_TenantId",
                table: "MasterNavigationMenus",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HubAccountTeams");

            migrationBuilder.DropTable(
                name: "HubBusinesses");

            migrationBuilder.DropTable(
                name: "HubContacts");

            migrationBuilder.DropTable(
                name: "HubNavigationMenus");

            migrationBuilder.DropTable(
                name: "HubProductCategories");

            migrationBuilder.DropTable(
                name: "HubProducts");

            migrationBuilder.DropTable(
                name: "HubSalesProjections");

            migrationBuilder.DropTable(
                name: "HubStores");

            migrationBuilder.DropTable(
                name: "HubZipCodeMaps");

            migrationBuilder.DropTable(
                name: "MasterNavigationMenus");
        }
    }
}
