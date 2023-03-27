using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class StoreRelevantStoreGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreRelevantStores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    RelevantStoreId = table.Column<long>(type: "bigint", nullable: false),
                    PrimaryStoreId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreRelevantStores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreRelevantStores_Stores_PrimaryStoreId",
                        column: x => x.PrimaryStoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreRelevantStores_PrimaryStoreId",
                table: "StoreRelevantStores",
                column: "PrimaryStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreRelevantStores_TenantId",
                table: "StoreRelevantStores",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreRelevantStores");
        }
    }
}
