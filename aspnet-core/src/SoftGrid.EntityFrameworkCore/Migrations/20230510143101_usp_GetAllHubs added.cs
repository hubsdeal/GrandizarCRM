using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class uspGetAllHubsadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string query = @"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================================================================================
-- Author:		<Md. Zillur Rahman>
-- Create date: <10 May, 2023>
-- Description:	<This procedure is used to get all the hub list with filtering and pagination for 
--				 internal admin.>
-- ============================================================================================================
-- exec [dbo].[usp_GetAllHubs] @SkipCount=0,@MaxResultCount=10
Create PROCEDURE [dbo].[usp_GetAllHubs]
	@Filter nvarchar(4000)='""""',
	@Name nvarchar(4000)='""""',
	@Live bit=null,
	@Owner bit=null,
	@CountryId bigint=null,
	@StateId bigint=null,
	@City nvarchar(256)='""""',
	@HubTypeId bigint=null,
	@SkipCount int,
	@MaxResultCount int
AS
BEGIN
	SET NOCOUNT ON;
		SELECT(
		    SELECT COUNT(h.Id) 
			from [dbo].[Hubs] h
			LEFT JOIN [dbo].[HubTypes] ht on h.HubTypeId=ht.Id
			LEFT JOIN [dbo].[Countries] con on h.CountryId=con.Id
			LEFT JOIN [dbo].[States] s on h.StateId=s.Id
			LEFT JOIN [dbo].[Counties] cu on h.CountyId=cu.Id
			LEFT JOIN [dbo].[Cities] ci on h.CityId=ci.Id
			
			--Begin Condition For Filtering

			WHERE h.IsDeleted=0
				 AND (@Filter = '""""' OR Contains(h.Name,@Filter)
									   OR Contains(h.Phone,@Filter)
									   OR Contains(h.OfficeFullAddress,@Filter)
									   OR Contains(ht.Name,@Filter)
									   OR Contains(con.Name,@Filter)
									   OR Contains(s.Name,@Filter)
									   OR Contains(cu.Name,@Filter)
									   OR Contains(ci.Name,@Filter))
				 AND (@Name='""""' OR Contains(h.Name,@Name))
				 AND (@Live IS NULL OR h.Live=@Live)
				 AND (@Owner IS NULL OR h.PartnerOrOwned=@Owner)
				 AND (@CountryId IS NULL OR h.CountryId=@CountryId)
				 AND (@StateId IS NULL OR h.StateId=@StateId)
				 AND (@City='""""' OR Contains(ci.Name,@City))
				 --End Condition For filtering
			) AS TotalCount,

			(SELECT 
			h.*,
			ml.BinaryObjectId
			from [dbo].[Hubs] h
			LEFT JOIN [dbo].[HubTypes] ht on h.HubTypeId=ht.Id
			LEFT JOIN [dbo].[Countries] con on h.CountryId=con.Id
			LEFT JOIN [dbo].[States] s on h.StateId=s.Id
			LEFT JOIN [dbo].[Counties] cu on h.CountyId=cu.Id
			LEFT JOIN [dbo].[Cities] ci on h.CityId=ci.Id
			LEFT JOIN [dbo].[MediaLibraries] ml on h.PictureMediaLibraryId=ml.Id
			
			--Begin Condition For Filtering

			WHERE h.IsDeleted=0
				 AND (@Filter = '""""' OR Contains(h.Name,@Filter)
									   OR Contains(h.Phone,@Filter)
									   OR Contains(h.OfficeFullAddress,@Filter)
									   OR Contains(ht.Name,@Filter)
									   OR Contains(con.Name,@Filter)
									   OR Contains(s.Name,@Filter)
									   OR Contains(cu.Name,@Filter)
									   OR Contains(ci.Name,@Filter))
				 AND (@Name='""""' OR Contains(h.Name,@Name))
				 AND (@Live IS NULL OR h.Live=@Live)
				 AND (@Owner IS NULL OR h.PartnerOrOwned=@Owner)
				 AND (@CountryId IS NULL OR h.CountryId=@CountryId)
				 AND (@StateId IS NULL OR h.StateId=@StateId)
				 AND (@City='""""' OR Contains(ci.Name,@City))
				 --End Condition For filtering

			ORDER BY h.DisplaySequence asc
			OFFSET @SkipCount ROWS
			FETCH NEXT @MaxResultCount ROWS ONLY
			FOR JSON PATH)AS Products
		FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
END";
            migrationBuilder.Sql(query);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			string query = @"DROP PROCEDURE [dbo].[usp_GetAllHubs]";
			migrationBuilder.Sql(query);
        }
    }
}
