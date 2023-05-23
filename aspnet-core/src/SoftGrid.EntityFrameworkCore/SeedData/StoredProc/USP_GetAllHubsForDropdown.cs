namespace SoftGrid.SeedData.StoredProc;

public class USP_GetAllHubsForDropdown
{
    public static string Up()
    {
        var sql = @"
            SET ANSI_NULLS ON
                    GO
                    SET QUOTED_IDENTIFIER ON
                    GO
                    -- ============================================================================================================
                    -- Author:		<Md. Zillur Rahman>
                    -- Create date: <12 Mar, 2021>
                    -- Description:	<This procedure is used to get all hubs for dropdown.>
                    -- Please set parameters default value with four double comma
                    -- ============================================================================================================
                    -- exec usp_GetAllHubsForDropdown
                    CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllHubsForDropdown]
                    @Filter nvarchar(4000)='""""',
                    @CityFilter nvarchar(4000)='""""',
                    @ZipCodeFilter nvarchar(4000)='""""'
                    AS
                    BEGIN
	                    SET NOCOUNT ON;
		                    SELECT(
			                    SELECT
				                    h.Id as Id,
				                    h.Name as DisplayName,
				                    h.Url as Url
				                    --h.PictureId as PictureId
			                    FROM dbo.Hubs h
			                    where h.IsDeleted=0 and h.Live=1
			                    AND(@Filter='""""' OR Contains(h.Name,@Filter))
			                    AND(@CityFilter='""""' OR h.Id in(Select HubId from HubZipCodeMaps where CityId in(Select Id from Cities where Contains(Name,@CityFilter))))
			                    AND(@ZipCodeFilter='""""' OR h.Id in(Select HubId from HubZipCodeMaps where ZipCodeId in(Select Id from ZipCodes where Contains(Name,@ZipCodeFilter))))
			                    ORDER BY h.DisplaySequence
			                    FOR JSON PATH)AS Hubs
		                    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                    END
                    GO";
        return sql;
    }
}