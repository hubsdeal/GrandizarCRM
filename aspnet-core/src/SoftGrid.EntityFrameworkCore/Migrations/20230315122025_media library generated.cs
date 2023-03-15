using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class medialibrarygenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MediaLibraries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Size = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    FileExtension = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Dimension = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    VideoLink = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    SeoTag = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    AltTag = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    VirtualPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BinaryObjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MasterTagCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    MasterTagId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaLibraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaLibraries_MasterTagCategories_MasterTagCategoryId",
                        column: x => x.MasterTagCategoryId,
                        principalTable: "MasterTagCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MediaLibraries_MasterTags_MasterTagId",
                        column: x => x.MasterTagId,
                        principalTable: "MasterTags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaLibraries_MasterTagCategoryId",
                table: "MediaLibraries",
                column: "MasterTagCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaLibraries_MasterTagId",
                table: "MediaLibraries",
                column: "MasterTagId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaLibraries_TenantId",
                table: "MediaLibraries",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaLibraries");
        }
    }
}
