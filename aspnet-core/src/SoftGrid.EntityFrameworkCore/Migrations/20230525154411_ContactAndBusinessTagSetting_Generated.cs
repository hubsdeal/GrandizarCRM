using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class ContactAndBusinessTagSettingGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessMasterTagSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    DisplayPublic = table.Column<bool>(type: "bit", nullable: false),
                    CustomTagTitle = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CustomTagChatQuestion = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    AnswerTypeId = table.Column<int>(type: "int", nullable: false),
                    MasterTagCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    BusinessTypeId = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessMasterTagSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessMasterTagSettings_MasterTagCategories_MasterTagCategoryId",
                        column: x => x.MasterTagCategoryId,
                        principalTable: "MasterTagCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BusinessMasterTagSettings_MasterTags_BusinessTypeId",
                        column: x => x.BusinessTypeId,
                        principalTable: "MasterTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactMasterTagSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    DisplayPublic = table.Column<bool>(type: "bit", nullable: false),
                    CustomTagTitle = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CustomTagChatQuestion = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    AnswerTypeId = table.Column<int>(type: "int", nullable: false),
                    ContactTypeId = table.Column<long>(type: "bigint", nullable: false),
                    MasterTagCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMasterTagSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactMasterTagSettings_MasterTagCategories_MasterTagCategoryId",
                        column: x => x.MasterTagCategoryId,
                        principalTable: "MasterTagCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContactMasterTagSettings_MasterTags_ContactTypeId",
                        column: x => x.ContactTypeId,
                        principalTable: "MasterTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessMasterTagSettings_BusinessTypeId",
                table: "BusinessMasterTagSettings",
                column: "BusinessTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessMasterTagSettings_MasterTagCategoryId",
                table: "BusinessMasterTagSettings",
                column: "MasterTagCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessMasterTagSettings_TenantId",
                table: "BusinessMasterTagSettings",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMasterTagSettings_ContactTypeId",
                table: "ContactMasterTagSettings",
                column: "ContactTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMasterTagSettings_MasterTagCategoryId",
                table: "ContactMasterTagSettings",
                column: "MasterTagCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMasterTagSettings_TenantId",
                table: "ContactMasterTagSettings",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessMasterTagSettings");

            migrationBuilder.DropTable(
                name: "ContactMasterTagSettings");
        }
    }
}
