using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class JobTagSettingGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobMasterTagSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    DisplayPublic = table.Column<bool>(type: "bit", nullable: false),
                    AnswerTypeId = table.Column<int>(type: "int", nullable: false),
                    CustomTagTitle = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CustomTagChatQuestion = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    JobCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    MasterTagCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobMasterTagSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobMasterTagSettings_MasterTagCategories_MasterTagCategoryId",
                        column: x => x.MasterTagCategoryId,
                        principalTable: "MasterTagCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobMasterTagSettings_MasterTags_JobCategoryId",
                        column: x => x.JobCategoryId,
                        principalTable: "MasterTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobMasterTagSettings_JobCategoryId",
                table: "JobMasterTagSettings",
                column: "JobCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_JobMasterTagSettings_MasterTagCategoryId",
                table: "JobMasterTagSettings",
                column: "MasterTagCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_JobMasterTagSettings_TenantId",
                table: "JobMasterTagSettings",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobMasterTagSettings");
        }
    }
}
