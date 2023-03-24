using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class StoreMapTablesGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeTags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    CustomTag = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    TagValue = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Verified = table.Column<bool>(type: "bit", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    MasterTagCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    MasterTagId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeTags_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeTags_MasterTagCategories_MasterTagCategoryId",
                        column: x => x.MasterTagCategoryId,
                        principalTable: "MasterTagCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeTags_MasterTags_MasterTagId",
                        column: x => x.MasterTagId,
                        principalTable: "MasterTags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MarketplaceCommissionTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Percentage = table.Column<double>(type: "float", nullable: true),
                    FixedAmount = table.Column<double>(type: "float", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketplaceCommissionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoreAccountTeams",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Primary = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    OrderEmailNotification = table.Column<bool>(type: "bit", nullable: false),
                    OrderSmsNotification = table.Column<bool>(type: "bit", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreAccountTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreAccountTeams_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreAccountTeams_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreBankAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    AccountNo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    RoutingNo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    BankAddress = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreBankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreBankAccounts_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreBusinessCustomerMaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    PaidCustomer = table.Column<bool>(type: "bit", nullable: false),
                    LifeTimeSalesAmount = table.Column<double>(type: "float", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    BusinessId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreBusinessCustomerMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreBusinessCustomerMaps_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreBusinessCustomerMaps_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreBusinessHours",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    NowOpenOrClosed = table.Column<bool>(type: "bit", nullable: false),
                    IsOpen24Hours = table.Column<bool>(type: "bit", nullable: false),
                    MondayStartTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MondayEndTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TuesdayStartTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TuesdayEndTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WednesdayStartTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WednesdayEndTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ThursdayStartTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ThursdayEndTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FridayStartTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FridayEndTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SaturdayStartTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SaturdayEndTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SundayStartTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SundayEndTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsAcceptOnlyBusinessHour = table.Column<bool>(type: "bit", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    MasterTagCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    MasterTagId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_StoreBusinessHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreBusinessHours_MasterTagCategories_MasterTagCategoryId",
                        column: x => x.MasterTagCategoryId,
                        principalTable: "MasterTagCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreBusinessHours_MasterTags_MasterTagId",
                        column: x => x.MasterTagId,
                        principalTable: "MasterTags",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreBusinessHours_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreContactMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    PaidCustomer = table.Column<bool>(type: "bit", nullable: false),
                    LifeTimeSalesAmount = table.Column<double>(type: "float", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    ContactId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreContactMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreContactMaps_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreContactMaps_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreLocations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    LocationName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    FullAddress = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: true),
                    StateId = table.Column<long>(type: "bigint", nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreLocations_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreLocations_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreLocations_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreLocations_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreMedias",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    MediaLibraryId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_StoreMedias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreMedias_MediaLibraries_MediaLibraryId",
                        column: x => x.MediaLibraryId,
                        principalTable: "MediaLibraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreMedias_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreNotes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreNotes_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreOwnerTeams",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Primary = table.Column<bool>(type: "bit", nullable: false),
                    OrderEmailNotification = table.Column<bool>(type: "bit", nullable: false),
                    OrderSmsNotification = table.Column<bool>(type: "bit", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreOwnerTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreOwnerTeams_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreOwnerTeams_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreProductCategoryMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    ProductCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreProductCategoryMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreProductCategoryMaps_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreProductCategoryMaps_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreProductMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Published = table.Column<bool>(type: "bit", nullable: false),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreProductMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreProductMaps_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreProductMaps_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreSalesAlerts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    Current = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreSalesAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreSalesAlerts_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreTaxes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TaxName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    PercentageOrAmount = table.Column<bool>(type: "bit", nullable: false),
                    TaxRatePercentage = table.Column<double>(type: "float", nullable: true),
                    TaxAmount = table.Column<double>(type: "float", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreTaxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreTaxes_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreMarketplaceCommissionSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Percentage = table.Column<double>(type: "float", nullable: true),
                    FixedAmount = table.Column<double>(type: "float", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    MarketplaceCommissionTypeId = table.Column<long>(type: "bigint", nullable: true),
                    ProductCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreMarketplaceCommissionSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreMarketplaceCommissionSettings_MarketplaceCommissionTypes_MarketplaceCommissionTypeId",
                        column: x => x.MarketplaceCommissionTypeId,
                        principalTable: "MarketplaceCommissionTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreMarketplaceCommissionSettings_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreMarketplaceCommissionSettings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreMarketplaceCommissionSettings_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTags_EmployeeId",
                table: "EmployeeTags",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTags_MasterTagCategoryId",
                table: "EmployeeTags",
                column: "MasterTagCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTags_MasterTagId",
                table: "EmployeeTags",
                column: "MasterTagId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTags_TenantId",
                table: "EmployeeTags",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketplaceCommissionTypes_TenantId",
                table: "MarketplaceCommissionTypes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreAccountTeams_EmployeeId",
                table: "StoreAccountTeams",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreAccountTeams_StoreId",
                table: "StoreAccountTeams",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreAccountTeams_TenantId",
                table: "StoreAccountTeams",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreBankAccounts_StoreId",
                table: "StoreBankAccounts",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreBankAccounts_TenantId",
                table: "StoreBankAccounts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreBusinessCustomerMaps_BusinessId",
                table: "StoreBusinessCustomerMaps",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreBusinessCustomerMaps_StoreId",
                table: "StoreBusinessCustomerMaps",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreBusinessCustomerMaps_TenantId",
                table: "StoreBusinessCustomerMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreBusinessHours_MasterTagCategoryId",
                table: "StoreBusinessHours",
                column: "MasterTagCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreBusinessHours_MasterTagId",
                table: "StoreBusinessHours",
                column: "MasterTagId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreBusinessHours_StoreId",
                table: "StoreBusinessHours",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreBusinessHours_TenantId",
                table: "StoreBusinessHours",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreContactMaps_ContactId",
                table: "StoreContactMaps",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreContactMaps_StoreId",
                table: "StoreContactMaps",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreContactMaps_TenantId",
                table: "StoreContactMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreLocations_CityId",
                table: "StoreLocations",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreLocations_CountryId",
                table: "StoreLocations",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreLocations_StateId",
                table: "StoreLocations",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreLocations_StoreId",
                table: "StoreLocations",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreLocations_TenantId",
                table: "StoreLocations",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMarketplaceCommissionSettings_MarketplaceCommissionTypeId",
                table: "StoreMarketplaceCommissionSettings",
                column: "MarketplaceCommissionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMarketplaceCommissionSettings_ProductCategoryId",
                table: "StoreMarketplaceCommissionSettings",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMarketplaceCommissionSettings_ProductId",
                table: "StoreMarketplaceCommissionSettings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMarketplaceCommissionSettings_StoreId",
                table: "StoreMarketplaceCommissionSettings",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMarketplaceCommissionSettings_TenantId",
                table: "StoreMarketplaceCommissionSettings",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMedias_MediaLibraryId",
                table: "StoreMedias",
                column: "MediaLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMedias_StoreId",
                table: "StoreMedias",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMedias_TenantId",
                table: "StoreMedias",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreNotes_StoreId",
                table: "StoreNotes",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreNotes_TenantId",
                table: "StoreNotes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreOwnerTeams_StoreId",
                table: "StoreOwnerTeams",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreOwnerTeams_TenantId",
                table: "StoreOwnerTeams",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreOwnerTeams_UserId",
                table: "StoreOwnerTeams",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProductCategoryMaps_ProductCategoryId",
                table: "StoreProductCategoryMaps",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProductCategoryMaps_StoreId",
                table: "StoreProductCategoryMaps",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProductCategoryMaps_TenantId",
                table: "StoreProductCategoryMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProductMaps_ProductId",
                table: "StoreProductMaps",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProductMaps_StoreId",
                table: "StoreProductMaps",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreProductMaps_TenantId",
                table: "StoreProductMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreSalesAlerts_StoreId",
                table: "StoreSalesAlerts",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreSalesAlerts_TenantId",
                table: "StoreSalesAlerts",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTaxes_StoreId",
                table: "StoreTaxes",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTaxes_TenantId",
                table: "StoreTaxes",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeTags");

            migrationBuilder.DropTable(
                name: "StoreAccountTeams");

            migrationBuilder.DropTable(
                name: "StoreBankAccounts");

            migrationBuilder.DropTable(
                name: "StoreBusinessCustomerMaps");

            migrationBuilder.DropTable(
                name: "StoreBusinessHours");

            migrationBuilder.DropTable(
                name: "StoreContactMaps");

            migrationBuilder.DropTable(
                name: "StoreLocations");

            migrationBuilder.DropTable(
                name: "StoreMarketplaceCommissionSettings");

            migrationBuilder.DropTable(
                name: "StoreMedias");

            migrationBuilder.DropTable(
                name: "StoreNotes");

            migrationBuilder.DropTable(
                name: "StoreOwnerTeams");

            migrationBuilder.DropTable(
                name: "StoreProductCategoryMaps");

            migrationBuilder.DropTable(
                name: "StoreProductMaps");

            migrationBuilder.DropTable(
                name: "StoreSalesAlerts");

            migrationBuilder.DropTable(
                name: "StoreTaxes");

            migrationBuilder.DropTable(
                name: "MarketplaceCommissionTypes");
        }
    }
}
