namespace SoftGrid.SeedData.StoredProc;

public class USP_GetAllStoreReviewsByStoreIdAndContactId
{
    public static string Up()
    {
        var sql = @"
            SET ANSI_NULLS ON
            GO
            SET QUOTED_IDENTIFIER ON
            GO
            -- ==================================================================================================
            -- Author:		<Md. Zillur Rahman>
            -- Create date: <27 August, 2020>
            -- Description:	<This procedure is used to get all the store reviews by Store id or ContactId with pagination 
            --				 and feedback count >
            -- ==================================================================================================
            -- exec usp_GetAllStoreReviewsByStoreIdAndContactId @SkipCount=0,@MaxResultCount=10
            CREATE OR ALTER PROCEDURE [dbo].[usp_GetAllStoreReviewsByStoreIdAndContactId]
	            @Filter nvarchar(4000)='""',
	            @StoreId bigint=null,
	            @ContactId bigint=null,
	            @IsPublished bit=null,
	            @SkipCount int,
	            @MaxResultCount int
            AS
            BEGIN
	            SET NOCOUNT ON;
	            SELECT(
		            SELECT COUNT(sr.Id)
		            FROM [dbo].[StoreReviews] sr
		            where 
                    --sr.IsDeleted=0
		            --AND 
                    (@StoreId IS NULL OR sr.StoreId=@StoreId)
		            AND (@ContactId IS NULL OR sr.ContactId=@ContactId)
		            AND (@IsPublished IS NULL OR sr.IsPublish=@IsPublished)
		            AND (@Filter = '""'     OR Contains(sr.ReviewInfo, @Filter)))AS TotalCount, 
		            (SELECT
		            sr.Id as Id,
		            sr.ReviewInfo as ReviewInfo,
		            sr.PostDate as PostDate,
		            sr.PostTime as PostTime,
		            sr.IsPublish as IsPublish,
		            rl.Id as RatingLikeId,
		            rl.Name as RatingLikeName,
		            rl.Score as RatingScore,
		            c.Id as ContactId,
		            c.FullName as ContactName,
		            s.Id as StoreId,
		            s.Name as StoreName,
		            (select Count(srf.Id) from [dbo].[StoreReviewFeedbacks] srf where srf.StoreReviewId=sr.Id) as NumberOfFeedbacks
		            FROM [dbo].[StoreReviews] sr
		            LEFT JOIN [dbo].[Contacts] c ON c.Id=sr.ContactId
		            LEFT JOIN [dbo].[Stores] s ON s.Id=sr.StoreId
		            LEFT JOIN [dbo].[RatingLikes] rl ON rl.Id =sr.RatingLikeId
		            where 
                    --sr.IsDeleted=0
		            --AND 
                    (@StoreId IS NULL OR sr.StoreId=@StoreId)
		            AND (@ContactId IS NULL OR sr.ContactId=@ContactId)
		            AND (@IsPublished IS NULL OR sr.IsPublish=@IsPublished)
		            AND (@Filter = '""'     OR Contains(sr.ReviewInfo, @Filter))
		            ORDER BY sr.Id DESC
		            OFFSET @SkipCount ROWS	
		            FETCH NEXT @MaxResultCount ROWS ONLY
		            FOR JSON PATH)AS StoreReviews
	            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
            END

            GO


            ";
        return sql;
    }
}