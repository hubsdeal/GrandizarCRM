using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class ProductCategoryFullTextIndexcreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string query = @"CREATE FULLTEXT INDEX ON [dbo].[ProductCategories](
[Description] LANGUAGE 'English', 
[MetaKeywords] LANGUAGE 'English', 
[MetaTitle] LANGUAGE 'English', 
[Name] LANGUAGE 'English', 
[Url] LANGUAGE 'English')
KEY INDEX [PK_ProductCategories]ON ([ProductCatalog], FILEGROUP [PRIMARY])
WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)";
            migrationBuilder.Sql(query,suppressTransaction:true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string query = @"DROP FULLTEXT INDEX ON [dbo].[ProductCategories]";
            migrationBuilder.Sql(query);
        }
    }
}
