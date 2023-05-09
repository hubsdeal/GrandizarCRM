using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class MasterTagFullTextIndexcreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string query = @"CREATE FULLTEXT INDEX ON [dbo].[MasterTags](
[Description] LANGUAGE 'English', 
[Name] LANGUAGE 'English', 
[Synonyms] LANGUAGE 'English')
KEY INDEX [PK_MasterTags]ON ([StoreCatalog], FILEGROUP [PRIMARY])
WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)";
            migrationBuilder.Sql(query,suppressTransaction:true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string query = @"DROP FULLTEXT INDEX ON [dbo].[MasterTags]";
            migrationBuilder.Sql(query);
        }
    }
}
