using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftGrid.SeedData.StoredProc;

public class USP_GetTopNearbyHubsByUserLocation
{
    public static void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
            SET ANSI_NULLS ON
            GO
            SET QUOTED_IDENTIFIER ON
            GO
            -- ============================================================================================================
            -- Author:		<Md. Zillur Rahman>
            -- Create date: <24 Nov, 2020>
            -- Description:	<This procedure is used to compare the distance with hubs lat long to user lat long and return by nearest distancer order>
            -- ============================================================================================================
            -- exec usp_GetTopNearbyHubsByUserLocation @Latitude=23.83593722784342, @Longitude=90.41724203757542
            CREATE OR ALTER PROCEDURE [dbo].[usp_GetTopNearbyHubsByUserLocation]
	            @Latitude decimal=null,
	            @Longitude decimal=null
            AS
            BEGIN
	            SET NOCOUNT ON;
		            SELECT(
			            SELECT Top 3 
				            h.Id,
				            h.Name,
				            h.Url,
				            h.Latitude, 
				            h.Longitude,
				            --abo.Id as PictureId,
				            SQRT(POWER(69.1 * (Latitude - @Latitude), 2) +POWER(69.1 * (@Longitude - Longitude) * COS(Latitude / 57.3), 2)) AS distance
			            FROM dbo.Hubs h
			            --LEFT JOIN dbo.AppBinaryObjects abo on h.PictureId=abo.Id
			            where h.Latitude IS NOT NULL AND h.Longitude IS NOT NULL AND h.Live=1
			            ORDER BY distance
			            FOR JSON PATH)AS NearestHubs
		            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            END
            GO");
    }
}