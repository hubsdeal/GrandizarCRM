using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class StoreTagGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreTags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    CustomTag = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    TagValue = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Verified = table.Column<bool>(type: "bit", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    MasterTagCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    MasterTagId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreTags_MasterTagCategories_MasterTagCategoryId",
                        column: x => x.MasterTagCategoryId,
                        principalTable: "MasterTagCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreTags_MasterTags_MasterTagId",
                        column: x => x.MasterTagId,
                        principalTable: "MasterTags",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreTags_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreTags_MasterTagCategoryId",
                table: "StoreTags",
                column: "MasterTagCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTags_MasterTagId",
                table: "StoreTags",
                column: "MasterTagId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTags_StoreId",
                table: "StoreTags",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTags_TenantId",
                table: "StoreTags",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreTags");
        }
    }
}
