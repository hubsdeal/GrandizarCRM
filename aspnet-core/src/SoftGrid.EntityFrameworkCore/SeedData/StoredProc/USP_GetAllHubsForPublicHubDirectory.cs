namespace SoftGrid.SeedData.StoredProc;

public class USP_GetAllHubsForPublicHubDirectory
{
    public static string Up()
    {
        return @"
            
            SET ANSI_NULLS ON
            GO
            SET QUOTED_IDENTIFIER ON
            GO
            -- ============================================================================================================
            -- Author:		<Md. Zillur Rahman>
            -- Create date: <12 Mar, 2021>
            -- Description:	<This procedure is used to get all the hub list with filtering and pagination for 
            --				 public hub directory.>
            -- ============================================================================================================
            -- exec [dbo].[usp_GetAllHubsForPublicHubDirectory] @SkipCount=0,@MaxResultCount=10
            CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllHubsForPublicHubDirectory]
	            @Filter nvarchar(4000)='""',
	            @StoreIdFilter bigint=null,
	            @CountryIdFilter bigint=null,
	            @StateFilter nvarchar(4000)='""',
	            @CityFilter nvarchar(4000)='""',
	            @CountyFilter nvarchar(4000)='""',
	            @ZipCodeFilter nvarchar(4000)='""',
	            @SkipCount int,
	            @MaxResultCount int
            AS
            BEGIN
	            SET NOCOUNT ON;
		            SELECT(
		                SELECT COUNT(h.Id) 
			            from [dbo].[Hubs] h
			            WHERE h.IsDeleted=0 and h.Live=1
				             AND (@Filter = '""' OR Contains(h.Name,@Filter))
				             AND (@StateFilter= '""' OR h.StateId IN(select Id from States s where Contains(s.Name,@StateFilter) OR Contains(s.Ticker,@StateFilter)))
				             AND (@CityFilter= '""' OR h.CityId IN(select Id from Cities c where Contains(c.Name,@CityFilter)))
				             AND (@CountyFilter= '""' OR h.CountyId IN(select Id from Counties con where Contains(con.Name,@CountyFilter)))
				             AND (@ZipCodeFilter= '""' OR h.Id IN(select HubId from HubZipCodeMaps where ZipCodeId IN(Select Id from ZipCodes where Name=@ZipCodeFilter)))
				             AND (@StoreIdFilter IS NULL OR h.Id IN(select HubId from HubStores where StoreId=@StoreIdFilter))
				             AND (@CountryIdFilter IS NULL OR h.CountryId=@CountryIdFilter)
			            ) AS TotalCount,
			            (SELECT 
			            h.Id as Id,
			            h.Name as Name,
			            h.Url as Url,
			            --h.PictureId as PictureLink,
			            h.DisplaySequence as DisplaySequence

			            FROM [dbo].[Hubs] h
			            WHERE h.IsDeleted=0 and h.Live=1
				             AND (@Filter = '""' OR Contains(h.Name,@Filter))
				             AND (@StateFilter= '""' OR h.StateId IN(select Id from States s where Contains(s.Name,@StateFilter) OR Contains(s.Ticker,@StateFilter)))
				             AND (@CityFilter= '""' OR h.CityId IN(select Id from Cities c where Contains(c.Name,@CityFilter)))
				             AND (@CountyFilter= '""' OR h.CountyId IN(select Id from Counties con where Contains(con.Name,@CountyFilter)))
				             AND (@ZipCodeFilter= '""' OR h.Id IN(select HubId from HubZipCodeMaps where ZipCodeId IN(Select Id from ZipCodes where Name=@ZipCodeFilter)))
				             AND (@StoreIdFilter IS NULL OR h.Id IN(select HubId from HubStores where StoreId=@StoreIdFilter))
				             AND (@CountryIdFilter IS NULL OR h.CountryId=@CountryIdFilter)

			            ORDER BY h.DisplaySequence asc
			            OFFSET @SkipCount ROWS
			            FETCH NEXT @MaxResultCount ROWS ONLY
			            FOR JSON PATH)AS Hubs
		            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            END

            GO";
    }
}