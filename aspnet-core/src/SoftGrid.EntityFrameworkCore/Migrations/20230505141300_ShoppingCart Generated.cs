using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class ShoppingCartGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    UnitPrice = table.Column<double>(type: "float", nullable: true),
                    TotalAmount = table.Column<double>(type: "float", nullable: true),
                    UnitTotalPrice = table.Column<double>(type: "float", nullable: true),
                    UnitDiscountAmount = table.Column<double>(type: "float", nullable: true),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_ContactId",
                table: "ShoppingCarts",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_CurrencyId",
                table: "ShoppingCarts",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_OrderId",
                table: "ShoppingCarts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_ProductId",
                table: "ShoppingCarts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_StoreId",
                table: "ShoppingCarts",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_TenantId",
                table: "ShoppingCarts",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingCarts");
        }
    }
}
