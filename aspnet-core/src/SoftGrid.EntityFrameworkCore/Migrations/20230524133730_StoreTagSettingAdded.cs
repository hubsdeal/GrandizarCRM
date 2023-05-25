using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class StoreTagSettingAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreTagSettingCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreTagSettingCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoreMasterTagSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    CustomTagTitle = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CustomTagChatQuestion = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    DisplayPublic = table.Column<bool>(type: "bit", nullable: false),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    StoreTagSettingCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    MasterTagCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreMasterTagSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreMasterTagSettings_MasterTagCategories_MasterTagCategoryId",
                        column: x => x.MasterTagCategoryId,
                        principalTable: "MasterTagCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoreMasterTagSettings_StoreTagSettingCategories_StoreTagSettingCategoryId",
                        column: x => x.StoreTagSettingCategoryId,
                        principalTable: "StoreTagSettingCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreMasterTagSettings_MasterTagCategoryId",
                table: "StoreMasterTagSettings",
                column: "MasterTagCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMasterTagSettings_StoreTagSettingCategoryId",
                table: "StoreMasterTagSettings",
                column: "StoreTagSettingCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreMasterTagSettings_TenantId",
                table: "StoreMasterTagSettings",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreTagSettingCategories_TenantId",
                table: "StoreTagSettingCategories",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreMasterTagSettings");

            migrationBuilder.DropTable(
                name: "StoreTagSettingCategories");
        }
    }
}
