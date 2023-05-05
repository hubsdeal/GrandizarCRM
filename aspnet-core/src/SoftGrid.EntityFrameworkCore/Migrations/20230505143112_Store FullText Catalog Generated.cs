using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class StoreFullTextCatalogGenerated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string query = @"CREATE FULLTEXT CATALOG [StoreCatalog] WITH ACCENT_SENSITIVITY = ON";
            migrationBuilder.Sql(query, suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string query = @"DROP FULLTEXT CATALOG [StoreCatalog] WITH ACCENT_SENSITIVITY = ON";
            migrationBuilder.Sql(query);
        }
    }
}
