using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class TaskEventAndTagGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskEvents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<bool>(type: "bit", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EndTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Template = table.Column<bool>(type: "bit", nullable: false),
                    ActualTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstimatedTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HourAndMinutes = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaskStatusId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_TaskEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskEvents_TaskStatuses_TaskStatusId",
                        column: x => x.TaskStatusId,
                        principalTable: "TaskStatuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskTags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    CustomTag = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    TagValue = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Verfied = table.Column<bool>(type: "bit", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: true),
                    TaskEventId = table.Column<long>(type: "bigint", nullable: false),
                    MasterTagCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    MasterTagId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskTags_MasterTagCategories_MasterTagCategoryId",
                        column: x => x.MasterTagCategoryId,
                        principalTable: "MasterTagCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskTags_MasterTags_MasterTagId",
                        column: x => x.MasterTagId,
                        principalTable: "MasterTags",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskTags_TaskEvents_TaskEventId",
                        column: x => x.TaskEventId,
                        principalTable: "TaskEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskEvents_TaskStatusId",
                table: "TaskEvents",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskEvents_TenantId",
                table: "TaskEvents",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTags_MasterTagCategoryId",
                table: "TaskTags",
                column: "MasterTagCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTags_MasterTagId",
                table: "TaskTags",
                column: "MasterTagId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTags_TaskEventId",
                table: "TaskTags",
                column: "TaskEventId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTags_TenantId",
                table: "TaskTags",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskTags");

            migrationBuilder.DropTable(
                name: "TaskEvents");
        }
    }
}
