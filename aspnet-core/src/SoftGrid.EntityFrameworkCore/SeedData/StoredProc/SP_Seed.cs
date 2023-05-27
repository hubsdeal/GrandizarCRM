using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftGrid.SeedData.StoredProc;

public class SP_Seed
{
    public static void Up(MigrationBuilder migrationBuilder)
    {
        USP_GetAllHubsForPublicHubDirectory.Up(migrationBuilder);
        USP_GetTopNearbyHubsByUserLocation.Up(migrationBuilder);
        USP_GetAllHubsForDropdown.Up(migrationBuilder);
        USP_GetAllStoreReviewsByStoreIdAndContactId.Up(migrationBuilder);

    }
}