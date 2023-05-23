using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftGrid.SeedData.StoredProc;

public class SP_Seed
{
    public static void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(USP_GetAllHubsForPublicHubDirectory.Up());
        migrationBuilder.Sql(USP_GetTopNearbyHubsByUserLocation.Up());
        migrationBuilder.Sql(USP_GetAllHubsForDropdown.Up());
        migrationBuilder.Sql(USP_GetAllStoreReviewsByStoreIdAndContactId.Up());
    }
}