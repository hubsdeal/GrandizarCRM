using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class CompanyTagGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessTags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    CustomTag = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    TagValue = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Verified = table.Column<bool>(type: "bit", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: true),
                    BusinessId = table.Column<long>(type: "bigint", nullable: false),
                    MasterTagCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    MasterTagId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessTags_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessTags_MasterTagCategories_MasterTagCategoryId",
                        column: x => x.MasterTagCategoryId,
                        principalTable: "MasterTagCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BusinessTags_MasterTags_MasterTagId",
                        column: x => x.MasterTagId,
                        principalTable: "MasterTags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessTags_BusinessId",
                table: "BusinessTags",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessTags_MasterTagCategoryId",
                table: "BusinessTags",
                column: "MasterTagCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessTags_MasterTagId",
                table: "BusinessTags",
                column: "MasterTagId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessTags_TenantId",
                table: "BusinessTags",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessTags");
        }
    }
}
