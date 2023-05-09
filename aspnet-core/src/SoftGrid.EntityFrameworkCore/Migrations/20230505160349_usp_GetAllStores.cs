using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class uspGetAllStores : Migration
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
-- Create date: <05 May, 2023>
-- Description:	<This procedure is used to get all the store list with filtering and pagination for 
--				 internal admin.>
-- ============================================================================================================
-- exec [dbo].[usp_GetAllStores] @SkipCount=0,@MaxResultCount=10,@IsFavoriteOnly=1,@EmployeeIdFilter=30007
Create PROCEDURE [dbo].[usp_GetAllStores]
	@Filter nvarchar(4000)='""""',
	@Name nvarchar(4000)='""""',
	@Phone nvarchar(4000)='""""',
	@Email nvarchar(4000)='""""',
	@Mobile nvarchar(4000)='""""',
	@Address nvarchar(4000)='""""',
	@City nvarchar(4000)='""""',
	@StateId bigint=null,
	@CountryId bigint=null,
	@IsPublished bit=null,
	@OnlineStore bit=null,
	@Verified bit=null,
	@EmployeeIdFilter bigint=null,
	@IsFavoriteOnly bit=null,
	@ContactIdFilter bigint=null,
	@ZipCodeFilter nvarchar(50)=null,
	@MasterTagCategoryIdFilter bigint=null,
	@MasterTagIdFilter bigint=null,
	@SkipCount int,
	@MaxResultCount int
AS
BEGIN
	SET NOCOUNT ON;
	declare @StorePrimaryCategoryId bigint=37;
		SELECT(
		    SELECT COUNT(s.Id) 
			from [dbo].[Stores] s
			LEFT JOIN [dbo].[States] st ON s.StateId=st.Id
			LEFT JOIN [dbo].[Countries] con ON s.CountryId=con.Id
			
			--Begin Condition For Filtering

			WHERE s.IsDeleted=0
				 AND (@Filter = '""""' OR Contains(s.Name,@Filter)
									   OR Contains(s.Phone,@Filter)
									   OR Contains(s.Mobile,@Filter)
									   OR Contains(s.Email,@Filter)
									   OR Contains(s.Address,@Filter)
									   OR s.Id IN(select st.StoreId from StoreTags st
									   LEFT JOIN MasterTags mt on st.MasterTagId=mt.Id
									   where st.MasterTagCategoryId=@StorePrimaryCategoryId AND mt.Name like '%'+@Filter+'%'))
				 AND (@Name='""""' OR Contains(s.Name,@Name))
				 AND (@Phone='""""' OR Contains(s.Phone,@Phone))
				 AND (@Mobile='""""' OR Contains(s.Mobile,@Mobile))
				 AND (@Email='""""' OR Contains(s.Email,@Email))
				 AND (@Address='""""' OR Contains(s.Address,@Address))
				 AND (@City='""""' OR Contains(s.City,@City))
				 AND (@StateId IS NULL OR s.StateId=@StateId)
				 AND (@CountryId IS NULL OR s.CountryId=@CountryId)
				 AND (@IsPublished IS NULL OR s.IsPublished=@IsPublished)
				 AND (@OnlineStore IS NULL OR s.IsLocalOrOnlineStore=@OnlineStore)
				 AND (@Verified IS NULL OR s.IsVerified=@Verified)
				 AND (@EmployeeIdFilter IS NULL OR s.Id IN(Select StoreId from StoreAccountTeams where EmployeeId=@EmployeeIdFilter))
				 AND (@ZipCodeFilter IS NULL OR s.ZipCode=@ZipCodeFilter OR s.Id IN(select StoreId from StoreZipCodeMaps where ZipCode=@ZipCodeFilter))
				 AND (@MasterTagCategoryIdFilter IS NULL OR s.Id IN (select st.StoreId from StoreTags st where st.MasterTagCategoryId=@MasterTagCategoryIdFilter))
				 AND (@MasterTagIdFilter IS NULL OR s.Id IN (select st.StoreId from StoreTags st where st.MasterTagId=@MasterTagIdFilter))
				 --End Condition For filtering
			) AS TotalCount,

			(
			SELECT COUNT(s.Id) 
			from [dbo].[Stores] s
			LEFT JOIN [dbo].[States] st ON s.StateId=st.Id
			LEFT JOIN [dbo].[Countries] con ON s.CountryId=con.Id
			
			--Begin Condition For Filtering

			WHERE s.IsDeleted=0 AND s.IsPublished=1
				 AND (@Filter = '""""' OR Contains(s.Name,@Filter)
									   OR Contains(s.Phone,@Filter)
									   OR Contains(s.Mobile,@Filter)
									   OR Contains(s.Email,@Filter)
									   OR Contains(s.Address,@Filter)
									   OR s.Id IN(select st.StoreId from StoreTags st
									   LEFT JOIN MasterTags mt on st.MasterTagId=mt.Id
									   where st.MasterTagCategoryId=@StorePrimaryCategoryId AND mt.Name like '%'+@Filter+'%'))
				 AND (@Name='""""' OR Contains(s.Name,@Name))
				 AND (@Phone='""""' OR Contains(s.Phone,@Phone))
				 AND (@Mobile='""""' OR Contains(s.Mobile,@Mobile))
				 AND (@Email='""""' OR Contains(s.Email,@Email))
				 AND (@Address='""""' OR Contains(s.Address,@Address))
				 AND (@City='""""' OR Contains(s.City,@City))
				 AND (@StateId IS NULL OR s.StateId=@StateId)
				 AND (@CountryId IS NULL OR s.CountryId=@CountryId)
				 AND (@OnlineStore IS NULL OR s.IsLocalOrOnlineStore=@OnlineStore)
				 AND (@Verified IS NULL OR s.IsVerified=@Verified)
				 AND (@EmployeeIdFilter IS NULL OR s.Id IN(Select StoreId from StoreAccountTeams where EmployeeId=@EmployeeIdFilter))
				 AND (@ZipCodeFilter IS NULL OR s.ZipCode=@ZipCodeFilter OR s.Id IN(select StoreId from StoreZipCodeMaps where ZipCode=@ZipCodeFilter))
				 AND (@MasterTagCategoryIdFilter IS NULL OR s.Id IN (select st.StoreId from StoreTags st where st.MasterTagCategoryId=@MasterTagCategoryIdFilter))
				 AND (@MasterTagIdFilter IS NULL OR s.Id IN (select st.StoreId from StoreTags st where st.MasterTagId=@MasterTagIdFilter))
			) AS Published,

			(
			SELECT COUNT(s.Id) 
			from [dbo].[Stores] s
			LEFT JOIN [dbo].[States] st ON s.StateId=st.Id
			LEFT JOIN [dbo].[Countries] con ON s.CountryId=con.Id
			
			--Begin Condition For Filtering

			WHERE s.IsDeleted=0 AND s.IsPublished=0
				 AND (@Filter = '""""' OR Contains(s.Name,@Filter)
									   OR Contains(s.Phone,@Filter)
									   OR Contains(s.Mobile,@Filter)
									   OR Contains(s.Email,@Filter)
									   OR Contains(s.Address,@Filter)
									   OR s.Id IN(select st.StoreId from StoreTags st
									   LEFT JOIN MasterTags mt on st.MasterTagId=mt.Id
									   where st.MasterTagCategoryId=@StorePrimaryCategoryId AND mt.Name like '%'+@Filter+'%'))
				 AND (@Name='""""' OR Contains(s.Name,@Name))
				 AND (@Phone='""""' OR Contains(s.Phone,@Phone))
				 AND (@Mobile='""""' OR Contains(s.Mobile,@Mobile))
				 AND (@Email='""""' OR Contains(s.Email,@Email))
				 AND (@Address='""""' OR Contains(s.Address,@Address))
				 AND (@City='""""' OR Contains(s.City,@City))
				 AND (@StateId IS NULL OR s.StateId=@StateId)
				 AND (@CountryId IS NULL OR s.CountryId=@CountryId)
				 AND (@OnlineStore IS NULL OR s.IsLocalOrOnlineStore=@OnlineStore)
				 AND (@Verified IS NULL OR s.IsVerified=@Verified)
				 AND (@EmployeeIdFilter IS NULL OR s.Id IN(Select StoreId from StoreAccountTeams where EmployeeId=@EmployeeIdFilter))
				 AND (@ZipCodeFilter IS NULL OR s.ZipCode=@ZipCodeFilter OR s.Id IN(select StoreId from StoreZipCodeMaps where ZipCode=@ZipCodeFilter))
				 AND (@MasterTagCategoryIdFilter IS NULL OR s.Id IN (select st.StoreId from StoreTags st where st.MasterTagCategoryId=@MasterTagCategoryIdFilter))
				 AND (@MasterTagIdFilter IS NULL OR s.Id IN (select st.StoreId from StoreTags st where st.MasterTagId=@MasterTagIdFilter))
			) AS UnPublished,

			(
			SELECT COUNT(s.Id) 
			from [dbo].[Stores] s
			LEFT JOIN [dbo].[States] st ON s.StateId=st.Id
			LEFT JOIN [dbo].[Countries] con ON s.CountryId=con.Id
			
			--Begin Condition For Filtering

			WHERE s.IsDeleted=0 AND s.IsVerified=1
				 AND (@Filter = '""""' OR Contains(s.Name,@Filter)
									   OR Contains(s.Phone,@Filter)
									   OR Contains(s.Mobile,@Filter)
									   OR Contains(s.Email,@Filter)
									   OR Contains(s.Address,@Filter)
									   OR s.Id IN(select st.StoreId from StoreTags st
									   LEFT JOIN MasterTags mt on st.MasterTagId=mt.Id
									   where st.MasterTagCategoryId=@StorePrimaryCategoryId AND mt.Name like '%'+@Filter+'%'))
				 AND (@Name='""""' OR Contains(s.Name,@Name))
				 AND (@Phone='""""' OR Contains(s.Phone,@Phone))
				 AND (@Mobile='""""' OR Contains(s.Mobile,@Mobile))
				 AND (@Email='""""' OR Contains(s.Email,@Email))
				 AND (@Address='""""' OR Contains(s.Address,@Address))
				 AND (@City='""""' OR Contains(s.City,@City))
				 AND (@StateId IS NULL OR s.StateId=@StateId)
				 AND (@CountryId IS NULL OR s.CountryId=@CountryId)
				 AND (@IsPublished IS NULL OR s.IsPublished=@IsPublished)
				 AND (@OnlineStore IS NULL OR s.IsLocalOrOnlineStore=@OnlineStore)
				 AND (@EmployeeIdFilter IS NULL OR s.Id IN(Select StoreId from StoreAccountTeams where EmployeeId=@EmployeeIdFilter))
				 AND (@ZipCodeFilter IS NULL OR s.ZipCode=@ZipCodeFilter OR s.Id IN(select StoreId from StoreZipCodeMaps where ZipCode=@ZipCodeFilter))
				 AND (@MasterTagCategoryIdFilter IS NULL OR s.Id IN (select st.StoreId from StoreTags st where st.MasterTagCategoryId=@MasterTagCategoryIdFilter))
				 AND (@MasterTagIdFilter IS NULL OR s.Id IN (select st.StoreId from StoreTags st where st.MasterTagId=@MasterTagIdFilter))
			) AS Verified,
			(SELECT COUNT(s.Id)
			from [dbo].[Stores] s
			LEFT JOIN [dbo].[States] st ON s.StateId=st.Id
			LEFT JOIN [dbo].[Countries] con ON s.CountryId=con.Id
			OUTER APPLY
			(
				SELECT TOP 1 *
				FROM dbo.WishLists wl
				WHERE wl.StoreId = s.Id
				AND wl.ProductId IS NULL
				AND (@ContactIdFilter IS NULL OR wl.ContactId=@ContactIdFilter)
			) m
			WHERE s.IsDeleted=0
				 AND (@Filter = '""""' OR Contains(s.Name,@Filter)
									   OR Contains(s.Phone,@Filter)
									   OR Contains(s.Mobile,@Filter)
									   OR Contains(s.Email,@Filter)
									   OR Contains(s.Address,@Filter)
									   OR s.Id IN(select st.StoreId from StoreTags st
									   LEFT JOIN MasterTags mt on st.MasterTagId=mt.Id
									   where st.MasterTagCategoryId=@StorePrimaryCategoryId AND mt.Name like '%'+@Filter+'%'))
				 AND (@Name='""""' OR Contains(s.Name,@Name))
				 AND (@Phone='""""' OR Contains(s.Phone,@Phone))
				 AND (@Mobile='""""' OR Contains(s.Mobile,@Mobile))
				 AND (@Email='""""' OR Contains(s.Email,@Email))
				 AND (@Address='""""' OR Contains(s.Address,@Address))
				 AND (@City='""""' OR Contains(s.City,@City))
				 AND (@StateId IS NULL OR s.StateId=@StateId)
				 AND (@CountryId IS NULL OR s.CountryId=@CountryId)
				 AND (@IsPublished IS NULL OR s.IsPublished=@IsPublished)
				 AND (@OnlineStore IS NULL OR s.IsLocalOrOnlineStore=@OnlineStore)
				 AND (@Verified IS NULL OR s.IsVerified=@Verified)
				 AND (@EmployeeIdFilter IS NULL OR s.Id IN(Select StoreId from StoreAccountTeams where EmployeeId=@EmployeeIdFilter))
				 AND (@ContactIdFilter IS NULL OR @IsFavoriteOnly IS NULL OR @IsFavoriteOnly=0 OR s.Id IN(Select m.StoreId where m.ContactId=@ContactIdFilter))
				 AND (@ZipCodeFilter IS NULL OR s.ZipCode=@ZipCodeFilter OR s.Id IN(select StoreId from StoreZipCodeMaps where ZipCode=@ZipCodeFilter))
				 AND (@MasterTagCategoryIdFilter IS NULL OR s.Id IN (select st.StoreId from StoreTags st where st.MasterTagCategoryId=@MasterTagCategoryIdFilter))
				 AND (@MasterTagIdFilter IS NULL OR s.Id IN (select st.StoreId from StoreTags st where st.MasterTagId=@MasterTagIdFilter))
			)as Favorite,
			(
			SELECT 
			s.Id as Id,
			s.Name as Name,
			s.Mobile as Mobile,
			s.Phone as Phone,
			s.Email as Email,
			ml.BinaryObjectId as StoreLogoLink,
			s.Address as Address,
			s.City as City,
			st.Name as StateName,
			con.Name as CountryName,
			s.DisplaySequence as DisplaySequence,
			s.ZipCode as ZipCode,
			s.IsPublished as IsPublished,
            s.IsLocalOrOnlineStore as IsLocalOrOnlineStore,
            s.IsVerified as IsVerified,
			(select Count(Id) from StoreProductMaps spm where spm.StoreId=s.Id) as NumberOfProducts,
			(select Name from MasterTags mt where mt.Id=(select Top 1 st.MasterTagId from StoreTags st where st.MasterTagCategoryId=@StorePrimaryCategoryId AND st.StoreId=s.Id))as PrimaryCategoryName,
			m.Id as WishListId
			from [dbo].[Stores] s
			LEFT JOIN [dbo].[States] st ON s.StateId=st.Id
			LEFT JOIN [dbo].[Countries] con ON s.CountryId=con.Id
			LEFT JOIN [dbo].[MediaLibraries] ml ON s.LogoMediaLibraryId=ml.Id
			OUTER APPLY
			(
				SELECT TOP 1 *
				FROM dbo.WishLists wl
				WHERE wl.StoreId = s.Id
				AND wl.ProductId IS NULL
				AND (@ContactIdFilter IS NULL OR wl.ContactId=@ContactIdFilter)
			) m
			WHERE s.IsDeleted=0
				 AND (@Filter = '""""' OR Contains(s.Name,@Filter)
									   OR Contains(s.Phone,@Filter)
									   OR Contains(s.Mobile,@Filter)
									   OR Contains(s.Email,@Filter)
									   OR Contains(s.Address,@Filter)
									   OR s.Id IN(select st.StoreId from StoreTags st
									   LEFT JOIN MasterTags mt on st.MasterTagId=mt.Id
									   where st.MasterTagCategoryId=@StorePrimaryCategoryId AND mt.Name like '%'+@Filter+'%'))
				 AND (@Name='""""' OR Contains(s.Name,@Name))
				 AND (@Phone='""""' OR Contains(s.Phone,@Phone))
				 AND (@Mobile='""""' OR Contains(s.Mobile,@Mobile))
				 AND (@Email='""""' OR Contains(s.Email,@Email))
				 AND (@Address='""""' OR Contains(s.Address,@Address))
				 AND (@City='""""' OR Contains(s.City,@City))
				 AND (@StateId IS NULL OR s.StateId=@StateId)
				 AND (@CountryId IS NULL OR s.CountryId=@CountryId)
				 AND (@IsPublished IS NULL OR s.IsPublished=@IsPublished)
				 AND (@OnlineStore IS NULL OR s.IsLocalOrOnlineStore=@OnlineStore)
				 AND (@Verified IS NULL OR s.IsVerified=@Verified)
				 AND (@EmployeeIdFilter IS NULL OR s.Id IN(Select StoreId from StoreAccountTeams where EmployeeId=@EmployeeIdFilter))
				 AND (@ContactIdFilter IS NULL OR @IsFavoriteOnly IS NULL OR @IsFavoriteOnly=0 OR s.Id IN(Select m.StoreId where m.ContactId=@ContactIdFilter))
				 AND (@ZipCodeFilter IS NULL OR s.ZipCode=@ZipCodeFilter OR s.Id IN(select StoreId from StoreZipCodeMaps where ZipCode=@ZipCodeFilter))
				 AND (@MasterTagCategoryIdFilter IS NULL OR s.Id IN (select st.StoreId from StoreTags st where st.MasterTagCategoryId=@MasterTagCategoryIdFilter))
				 AND (@MasterTagIdFilter IS NULL OR s.Id IN (select st.StoreId from StoreTags st where st.MasterTagId=@MasterTagIdFilter))
				 --End Condition For filtering
			ORDER BY m.Id desc
			OFFSET @SkipCount ROWS
			FETCH NEXT @MaxResultCount ROWS ONLY
			FOR JSON PATH)AS Stores
		FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
END";
            migrationBuilder.Sql(query);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string query = @"DROP PROCEDURE [dbo].[usp_GetAllStores]";
            migrationBuilder.Sql(query);
        }
    }
}
