using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class OrderandDiscountclassgenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerWallets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    WalletOpeningDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BalanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BalanceAmount = table.Column<double>(type: "float", nullable: true),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_CustomerWallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerWallets_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerWallets_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerWallets_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DiscountCodeGenerators",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CouponCode = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PercentageOrFixedAmount = table.Column<bool>(type: "bit", nullable: false),
                    DiscountPercentage = table.Column<double>(type: "float", nullable: true),
                    DiscountAmount = table.Column<double>(type: "float", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdminNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    StartTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EndTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCodeGenerators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderProductVariants",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    OrderProductInfoId = table.Column<long>(type: "bigint", nullable: true),
                    ProductVariantCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    ProductVariantId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProductVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderProductVariants_ProductVariantCategories_ProductVariantCategoryId",
                        column: x => x.ProductVariantCategoryId,
                        principalTable: "ProductVariantCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderProductVariants_ProductVariants_ProductVariantId",
                        column: x => x.ProductVariantId,
                        principalTable: "ProductVariants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderSalesChannels",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    LinkName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApiLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSalesChannels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SequenceNo = table.Column<int>(type: "int", nullable: true),
                    ColorCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryOrPickup = table.Column<bool>(type: "bit", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderStatuses_AbpRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AbpRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PaymentTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscountCodeByCustomers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DiscountCodeGeneratorId = table.Column<long>(type: "bigint", nullable: true),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCodeByCustomers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountCodeByCustomers_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DiscountCodeByCustomers_DiscountCodeGenerators_DiscountCodeGeneratorId",
                        column: x => x.DiscountCodeGeneratorId,
                        principalTable: "DiscountCodeGenerators",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DiscountCodeMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DiscountCodeGeneratorId = table.Column<long>(type: "bigint", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    MembershipTypeId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCodeMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountCodeMaps_DiscountCodeGenerators_DiscountCodeGeneratorId",
                        column: x => x.DiscountCodeGeneratorId,
                        principalTable: "DiscountCodeGenerators",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DiscountCodeMaps_MembershipTypes_MembershipTypeId",
                        column: x => x.MembershipTypeId,
                        principalTable: "MembershipTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DiscountCodeMaps_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DiscountCodeMaps_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    DeliveryOrPickup = table.Column<bool>(type: "bit", nullable: false),
                    PaymentCompleted = table.Column<bool>(type: "bit", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    City = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryFee = table.Column<double>(type: "float", nullable: true),
                    SubTotalExcludedTax = table.Column<double>(type: "float", nullable: true),
                    TotalDiscountAmount = table.Column<double>(type: "float", nullable: true),
                    TotalTaxAmount = table.Column<double>(type: "float", nullable: true),
                    TotalAmount = table.Column<double>(type: "float", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DiscountAmountByCode = table.Column<double>(type: "float", nullable: true),
                    GratuityAmount = table.Column<double>(type: "float", nullable: true),
                    GratuityPercentage = table.Column<double>(type: "float", nullable: true),
                    ServiceCharge = table.Column<double>(type: "float", nullable: true),
                    StateId = table.Column<long>(type: "bigint", nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    OrderStatusId = table.Column<long>(type: "bigint", nullable: true),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
                    OrderSalesChannelId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_OrderSalesChannels_OrderSalesChannelId",
                        column: x => x.OrderSalesChannelId,
                        principalTable: "OrderSalesChannels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_OrderStatuses_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "OrderStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DiscountCodeUserHistories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiscountCodeGeneratorId = table.Column<long>(type: "bigint", nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: true),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCodeUserHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountCodeUserHistories_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DiscountCodeUserHistories_DiscountCodeGenerators_DiscountCodeGeneratorId",
                        column: x => x.DiscountCodeGeneratorId,
                        principalTable: "DiscountCodeGenerators",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DiscountCodeUserHistories_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderDeliveryInfos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TrackingNumber = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    TotalWeight = table.Column<double>(type: "float", nullable: true),
                    DeliveryProviderId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    DispatchDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DispatchTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeliverToCustomerDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliverToCustomerTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeliveryNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerAcknowledged = table.Column<bool>(type: "bit", nullable: false),
                    CustomerSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CateringDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CateringTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DineInDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DineInTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IncludedChildren = table.Column<bool>(type: "bit", nullable: false),
                    IsAsap = table.Column<bool>(type: "bit", nullable: false),
                    IsPickupCatering = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfGuests = table.Column<int>(type: "int", nullable: true),
                    PickupDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PickupTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDeliveryInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDeliveryInfos_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderDeliveryInfos_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderFulfillmentStatuses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    EstimatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderStatusId = table.Column<long>(type: "bigint", nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_OrderFulfillmentStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderFulfillmentStatuses_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderFulfillmentStatuses_OrderStatuses_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "OrderStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderFulfillmentStatuses_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderfulfillmentTeams",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderfulfillmentTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderfulfillmentTeams_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderfulfillmentTeams_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderfulfillmentTeams_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderfulfillmentTeams_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderPaymentInfos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    PaymentSplit = table.Column<bool>(type: "bit", nullable: false),
                    DueAmount = table.Column<double>(type: "float", nullable: true),
                    PaySplitAmount = table.Column<double>(type: "float", nullable: true),
                    BillingAddress = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    BillingCity = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    BillingState = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    BillingZipCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SaveCreditCardNumber = table.Column<bool>(type: "bit", nullable: false),
                    MaskedCreditCardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CardNumber = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CardCvv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CardExpirationMonth = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CardExpirationYear = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    AuthorizationTransactionNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorizationTransactionCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthrorizationTransactionResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerIpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerDeviceInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaidTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: true),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentTypeId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPaymentInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderPaymentInfos_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderPaymentInfos_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderPaymentInfos_PaymentTypes_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderProductInfos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    UnitPrice = table.Column<double>(type: "float", nullable: true),
                    ByProductDiscountAmount = table.Column<double>(type: "float", nullable: true),
                    ByProductDiscountPercentage = table.Column<double>(type: "float", nullable: true),
                    ByProductTaxAmount = table.Column<double>(type: "float", nullable: true),
                    ByProductTotalAmount = table.Column<double>(type: "float", nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    MeasurementUnitId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProductInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderProductInfos_MeasurementUnits_MeasurementUnitId",
                        column: x => x.MeasurementUnitId,
                        principalTable: "MeasurementUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderProductInfos_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderProductInfos_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderProductInfos_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderTeams",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTeams_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderTeams_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerWallets_ContactId",
                table: "CustomerWallets",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerWallets_CurrencyId",
                table: "CustomerWallets",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerWallets_TenantId",
                table: "CustomerWallets",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerWallets_UserId",
                table: "CustomerWallets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodeByCustomers_ContactId",
                table: "DiscountCodeByCustomers",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodeByCustomers_DiscountCodeGeneratorId",
                table: "DiscountCodeByCustomers",
                column: "DiscountCodeGeneratorId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodeByCustomers_TenantId",
                table: "DiscountCodeByCustomers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodeGenerators_TenantId",
                table: "DiscountCodeGenerators",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodeMaps_DiscountCodeGeneratorId",
                table: "DiscountCodeMaps",
                column: "DiscountCodeGeneratorId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodeMaps_MembershipTypeId",
                table: "DiscountCodeMaps",
                column: "MembershipTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodeMaps_ProductId",
                table: "DiscountCodeMaps",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodeMaps_StoreId",
                table: "DiscountCodeMaps",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodeMaps_TenantId",
                table: "DiscountCodeMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodeUserHistories_ContactId",
                table: "DiscountCodeUserHistories",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodeUserHistories_DiscountCodeGeneratorId",
                table: "DiscountCodeUserHistories",
                column: "DiscountCodeGeneratorId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodeUserHistories_OrderId",
                table: "DiscountCodeUserHistories",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodeUserHistories_TenantId",
                table: "DiscountCodeUserHistories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDeliveryInfos_EmployeeId",
                table: "OrderDeliveryInfos",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDeliveryInfos_OrderId",
                table: "OrderDeliveryInfos",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDeliveryInfos_TenantId",
                table: "OrderDeliveryInfos",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderFulfillmentStatuses_EmployeeId",
                table: "OrderFulfillmentStatuses",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderFulfillmentStatuses_OrderId",
                table: "OrderFulfillmentStatuses",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderFulfillmentStatuses_OrderStatusId",
                table: "OrderFulfillmentStatuses",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderFulfillmentStatuses_TenantId",
                table: "OrderFulfillmentStatuses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderfulfillmentTeams_ContactId",
                table: "OrderfulfillmentTeams",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderfulfillmentTeams_EmployeeId",
                table: "OrderfulfillmentTeams",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderfulfillmentTeams_OrderId",
                table: "OrderfulfillmentTeams",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderfulfillmentTeams_TenantId",
                table: "OrderfulfillmentTeams",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderfulfillmentTeams_UserId",
                table: "OrderfulfillmentTeams",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPaymentInfos_CurrencyId",
                table: "OrderPaymentInfos",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPaymentInfos_OrderId",
                table: "OrderPaymentInfos",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPaymentInfos_PaymentTypeId",
                table: "OrderPaymentInfos",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPaymentInfos_TenantId",
                table: "OrderPaymentInfos",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProductInfos_MeasurementUnitId",
                table: "OrderProductInfos",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProductInfos_OrderId",
                table: "OrderProductInfos",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProductInfos_ProductId",
                table: "OrderProductInfos",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProductInfos_StoreId",
                table: "OrderProductInfos",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProductInfos_TenantId",
                table: "OrderProductInfos",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProductVariants_ProductVariantCategoryId",
                table: "OrderProductVariants",
                column: "ProductVariantCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProductVariants_ProductVariantId",
                table: "OrderProductVariants",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProductVariants_TenantId",
                table: "OrderProductVariants",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ContactId",
                table: "Orders",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CountryId",
                table: "Orders",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CurrencyId",
                table: "Orders",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderSalesChannelId",
                table: "Orders",
                column: "OrderSalesChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderStatusId",
                table: "Orders",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StateId",
                table: "Orders",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StoreId",
                table: "Orders",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TenantId",
                table: "Orders",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSalesChannels_TenantId",
                table: "OrderSalesChannels",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatuses_RoleId",
                table: "OrderStatuses",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatuses_TenantId",
                table: "OrderStatuses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTeams_EmployeeId",
                table: "OrderTeams",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTeams_OrderId",
                table: "OrderTeams",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTeams_TenantId",
                table: "OrderTeams",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTypes_TenantId",
                table: "PaymentTypes",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerWallets");

            migrationBuilder.DropTable(
                name: "DiscountCodeByCustomers");

            migrationBuilder.DropTable(
                name: "DiscountCodeMaps");

            migrationBuilder.DropTable(
                name: "DiscountCodeUserHistories");

            migrationBuilder.DropTable(
                name: "OrderDeliveryInfos");

            migrationBuilder.DropTable(
                name: "OrderFulfillmentStatuses");

            migrationBuilder.DropTable(
                name: "OrderfulfillmentTeams");

            migrationBuilder.DropTable(
                name: "OrderPaymentInfos");

            migrationBuilder.DropTable(
                name: "OrderProductInfos");

            migrationBuilder.DropTable(
                name: "OrderProductVariants");

            migrationBuilder.DropTable(
                name: "OrderTeams");

            migrationBuilder.DropTable(
                name: "DiscountCodeGenerators");

            migrationBuilder.DropTable(
                name: "PaymentTypes");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "OrderSalesChannels");

            migrationBuilder.DropTable(
                name: "OrderStatuses");
        }
    }
}
