using Microsoft.EntityFrameworkCore.Migrations;

using SoftGrid.SeedData.StoredProc;


namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class SPTestUspGetAllHubsForDropdown : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            SP_Seed.Up(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
