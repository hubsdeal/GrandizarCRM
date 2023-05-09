using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class ProductCatalogcreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string query = @"CREATE FULLTEXT CATALOG [ProductCatalog] WITH ACCENT_SENSITIVITY = ON";
            migrationBuilder.Sql(query,suppressTransaction:true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string query = @"DROP FULLTEXT CATALOG [ProductCatalog]";
            migrationBuilder.Sql(query);
        }
    }
}
