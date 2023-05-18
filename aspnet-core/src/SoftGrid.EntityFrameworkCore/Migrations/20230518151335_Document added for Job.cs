using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class DocumentaddedforJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobDocuments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    DocumentTitle = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    FileBinaryObjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobId = table.Column<long>(type: "bigint", nullable: false),
                    DocumentTypeId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobDocuments_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobDocuments_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobDocuments_DocumentTypeId",
                table: "JobDocuments",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_JobDocuments_JobId",
                table: "JobDocuments",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobDocuments_TenantId",
                table: "JobDocuments",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobDocuments");
        }
    }
}
