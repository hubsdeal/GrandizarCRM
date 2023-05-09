using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class ProductFullTextIndexcreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string query = @"CREATE FULLTEXT INDEX ON [dbo].[Products](
[Description] LANGUAGE 'English', 
[InternalNotes] LANGUAGE 'English', 
[MetaDescription] LANGUAGE 'English', 
[MetaKeywords] LANGUAGE 'English', 
[Name] LANGUAGE 'English', 
[ProductManufacturerSku] LANGUAGE 'English', 
[SeoTitle] LANGUAGE 'English', 
[ShortDescription] LANGUAGE 'English', 
[Sku] LANGUAGE 'English', 
[Url] LANGUAGE 'English')
KEY INDEX [PK_Products]ON ([ProductCatalog], FILEGROUP [PRIMARY])
WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)";
            migrationBuilder.Sql(query,suppressTransaction:true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string query = @"DROP FULLTEXT INDEX ON [dbo].[Products]";
            migrationBuilder.Sql(query);
        }
    }
}
