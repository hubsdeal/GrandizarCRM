using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class hubsfulltextindexadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string query = @"CREATE FULLTEXT INDEX ON [dbo].[Hubs](
[Description] LANGUAGE 'English', 
[Name] LANGUAGE 'English', 
[OfficeFullAddress] LANGUAGE 'English', 
[Phone] LANGUAGE 'English', 
[Url] LANGUAGE 'English', 
[YearlyRevenue] LANGUAGE 'English')
KEY INDEX [PK_Hubs]ON ([HubCatalog], FILEGROUP [PRIMARY])
WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)";
            migrationBuilder.Sql(query, suppressTransaction: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string query = @"DROP FULLTEXT INDEX ON [dbo].[Hubs]";
            migrationBuilder.Sql(query, suppressTransaction: true);
        }
    }
}
