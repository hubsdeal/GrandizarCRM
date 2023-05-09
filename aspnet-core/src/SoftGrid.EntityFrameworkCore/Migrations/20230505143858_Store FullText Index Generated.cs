using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class StoreFullTextIndexGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string query = @"CREATE FULLTEXT INDEX ON [dbo].[Stores](
[Address] LANGUAGE 'English', 
[City] LANGUAGE 'English', 
[Description] LANGUAGE 'English', 
[Email] LANGUAGE 'English', 
[Facebook] LANGUAGE 'English', 
[Fax] LANGUAGE 'English', 
[FullAddress] LANGUAGE 'English', 
[Instagram] LANGUAGE 'English', 
[LegalName] LANGUAGE 'English', 
[LinkedIn] LANGUAGE 'English', 
[MetaDescription] LANGUAGE 'English', 
[MetaTag] LANGUAGE 'English', 
[Mobile] LANGUAGE 'English', 
[Name] LANGUAGE 'English', 
[Phone] LANGUAGE 'English', 
[StoreUrl] LANGUAGE 'English', 
[Website] LANGUAGE 'English', 
[YearOfEstablishment] LANGUAGE 'English', 
[Youtube] LANGUAGE 'English', 
[ZipCode] LANGUAGE 'English')
KEY INDEX [PK_Stores]ON ([StoreCatalog], FILEGROUP [PRIMARY])
WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)";
            migrationBuilder.Sql(query,suppressTransaction:true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string query = @"Drop FULLTEXT INDEX ON [dbo].[Stores]";
            migrationBuilder.Sql(query);
        }
    }
}
