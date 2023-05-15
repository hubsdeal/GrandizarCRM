using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class Countriesfulltextindexadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string query = @"CREATE FULLTEXT INDEX ON [dbo].[Countries](
[FlagIcon] LANGUAGE 'English', 
[Name] LANGUAGE 'English', 
[PhoneCode] LANGUAGE 'English', 
[Ticker] LANGUAGE 'English')
KEY INDEX [PK_Countries]ON ([MasterDataCatalog], FILEGROUP [PRIMARY])
WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)";
            migrationBuilder.Sql(query, suppressTransaction: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string query = @"DROP FULLTEXT INDEX ON [dbo].[Countries]";
            migrationBuilder.Sql(query);
        }
    }
}
