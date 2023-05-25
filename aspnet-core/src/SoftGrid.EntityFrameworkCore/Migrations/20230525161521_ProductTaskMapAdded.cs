using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class ProductTaskMapAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactTaskMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ContactId = table.Column<long>(type: "bigint", nullable: false),
                    TaskEventId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactTaskMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactTaskMaps_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactTaskMaps_TaskEvents_TaskEventId",
                        column: x => x.TaskEventId,
                        principalTable: "TaskEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTaskMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    TaskEventId = table.Column<long>(type: "bigint", nullable: false),
                    ProductCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTaskMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTaskMaps_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductTaskMaps_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductTaskMaps_TaskEvents_TaskEventId",
                        column: x => x.TaskEventId,
                        principalTable: "TaskEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactTaskMaps_ContactId",
                table: "ContactTaskMaps",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactTaskMaps_TaskEventId",
                table: "ContactTaskMaps",
                column: "TaskEventId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactTaskMaps_TenantId",
                table: "ContactTaskMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTaskMaps_ProductCategoryId",
                table: "ProductTaskMaps",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTaskMaps_ProductId",
                table: "ProductTaskMaps",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTaskMaps_TaskEventId",
                table: "ProductTaskMaps",
                column: "TaskEventId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTaskMaps_TenantId",
                table: "ProductTaskMaps",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactTaskMaps");

            migrationBuilder.DropTable(
                name: "ProductTaskMaps");
        }
    }
}
