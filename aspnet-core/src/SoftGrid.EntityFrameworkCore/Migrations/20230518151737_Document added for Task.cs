using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class DocumentaddedforTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskDocuments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DocumentTitle = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    FileBinaryObjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskEventId = table.Column<long>(type: "bigint", nullable: false),
                    DocumentTypeId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskDocuments_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskDocuments_TaskEvents_TaskEventId",
                        column: x => x.TaskEventId,
                        principalTable: "TaskEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskDocuments_DocumentTypeId",
                table: "TaskDocuments",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskDocuments_TaskEventId",
                table: "TaskDocuments",
                column: "TaskEventId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskDocuments_TenantId",
                table: "TaskDocuments",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskDocuments");
        }
    }
}
