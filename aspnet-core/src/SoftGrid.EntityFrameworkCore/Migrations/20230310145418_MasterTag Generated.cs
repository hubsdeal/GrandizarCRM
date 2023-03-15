using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class MasterTagGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterTags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2500)", maxLength: 2500, nullable: true),
                    Synonyms = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    PictureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    MasterTagCategoryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterTags_MasterTagCategories_MasterTagCategoryId",
                        column: x => x.MasterTagCategoryId,
                        principalTable: "MasterTagCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterTags_MasterTagCategoryId",
                table: "MasterTags",
                column: "MasterTagCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterTags_TenantId",
                table: "MasterTags",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterTags");
        }
    }
}
