using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class TaskTeamAndItemadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskTeams",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartTime = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    EndTime = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    HourMinutes = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    EstimatedHour = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SubTaskTitle = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    TaskEventId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    ContactId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskTeams_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskTeams_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskTeams_TaskEvents_TaskEventId",
                        column: x => x.TaskEventId,
                        principalTable: "TaskEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskWorkItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    EstimatedHours = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ActualHours = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EndTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OpenOrClosed = table.Column<bool>(type: "bit", nullable: false),
                    CompletionPercentage = table.Column<int>(type: "int", nullable: true),
                    TaskEventId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskWorkItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskWorkItems_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskWorkItems_TaskEvents_TaskEventId",
                        column: x => x.TaskEventId,
                        principalTable: "TaskEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskTeams_ContactId",
                table: "TaskTeams",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTeams_EmployeeId",
                table: "TaskTeams",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTeams_TaskEventId",
                table: "TaskTeams",
                column: "TaskEventId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTeams_TenantId",
                table: "TaskTeams",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskWorkItems_EmployeeId",
                table: "TaskWorkItems",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskWorkItems_TaskEventId",
                table: "TaskWorkItems",
                column: "TaskEventId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskWorkItems_TenantId",
                table: "TaskWorkItems",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskTeams");

            migrationBuilder.DropTable(
                name: "TaskWorkItems");
        }
    }
}
