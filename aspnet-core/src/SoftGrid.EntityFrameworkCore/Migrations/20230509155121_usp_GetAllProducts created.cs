using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftGrid.Migrations
{
    /// <inheritdoc />
    public partial class uspGetAllProductscreated : Migration
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
-- Create date: <09 May, 2023>
-- Description:	<This procedure is used to get all the product list with filtering and pagination for 
--				 internal admin.>
-- ============================================================================================================
-- exec [dbo].[usp_GetAllProducts] @SkipCount=0,@MaxResultCount=10
Create PROCEDURE [dbo].[usp_GetAllProducts]
	@Filter nvarchar(4000)='""""',
	@Name nvarchar(4000)='""""',
	@Sku nvarchar(4000)='""""',
	@SeoTitle nvarchar(4000)='""""',
	@MetaKeyWords nvarchar(4000)='""""',
	@MinPrice float=null,
	@MaxPrice float=null,
	@MinSalesPrice float=null,
	@MaxSalesPrice float=null,
	@MinDiscountPercentage int=null,
	@MaxDiscountPercentage int=null,
	@IsCallForPrice bit=null,
	@IsTaxExempt bit=null,
	@MinStockQuantity int=null,
	@MaxStockQuantity int=null,
	@IsDisplayStockQuantity bit=null,
	@IsPublished bit=null,
	@IsPackageProduct bit=null,
	@MinMeasureAmount int=null,
	@MaxMeasureAmount int=null,
	@ProductCategoryId bigint=null,
	@CurrencyId bigint=null,
	@MeasurementUnitId bigint=null,
	@RatingLikeId bigint=null,
	@IsTemplate bit=null,
	@EmployeeId bigint=null,
	@BusinessId bigint=null,
	@StoreId bigint=null,
	@ProductTagNameFilter nvarchar(4000)='""""',
	@SkipCount int,
	@MaxResultCount int
AS
BEGIN
	SET NOCOUNT ON;
		SELECT(
		    SELECT COUNT(p.Id) 
			from [dbo].[Products] p
			LEFT JOIN [dbo].[ProductCategories] pc ON pc.Id=p.ProductCategoryId
			LEFT JOIN [dbo].[MediaLibraries] ml ON ml.Id=p.MediaLibraryId
			LEFT JOIN [dbo].[MeasurementUnits] mu ON mu.Id=p.MeasurementUnitId
			LEFT JOIN [dbo].[Currencies] c ON c.Id=p.CurrencyId
			LEFT JOIN [dbo].[RatingLikes] rl ON rl.Id=p.RatingLikeId
			--LEFT JOIN [dbo].[AppBinaryObjects] abo ON abo.Id=ml.BinaryObjectId
			
			--Begin Condition For Filtering

			WHERE p.IsDeleted=0
				 AND (@Filter = '""""' OR Contains(p.Name,@Filter)
									   OR Contains(p.Sku,@Filter)
									   OR Contains(p.SeoTitle,@Filter)
									   OR Contains(p.MetaKeywords,@Filter)
									   OR Contains(pc.Name,@Filter)
									   OR Contains(mu.Name,@Filter)
									   OR Contains(c.Name,@Filter))
				 AND (@Name='""""' OR Contains(p.Name,@Name))
				 AND (@Sku='""""' OR Contains(p.Sku,@Sku))
				 AND (@SeoTitle='""""' OR Contains(p.SeoTitle,@SeoTitle))
				 AND (@MetaKeyWords='""""' OR Contains(p.MetaKeywords,@MetaKeyWords))
				 AND (@MinPrice IS NULL OR p.RegularPrice>=@MinPrice)
				 AND (@MaxPrice IS NULL OR p.RegularPrice<=@MaxPrice)
				 AND (@MinSalesPrice IS NULL OR p.SalePrice>=@MinSalesPrice)
				 AND (@MaxSalesPrice IS NULL OR p.SalePrice<=@MaxSalesPrice)
				 AND (@MinDiscountPercentage IS NULL OR p.PriceDiscountPercentage>=@MinDiscountPercentage)
				 AND (@MaxDiscountPercentage IS NULL OR p.PriceDiscountPercentage<=@MaxDiscountPercentage)
				 AND (@IsCallForPrice IS NULL OR p.CallForPrice=@IsCallForPrice)
				 AND (@IsTaxExempt IS NULL OR p.IsTaxExempt=@IsTaxExempt)
				 AND (@MinStockQuantity IS NULL OR p.StockQuantity>=@MinStockQuantity)
				 AND (@MaxStockQuantity IS NULL OR p.StockQuantity<=@MaxStockQuantity)
				 AND (@IsDisplayStockQuantity IS NULL OR p.IsDisplayStockQuantity=@IsDisplayStockQuantity)
				 AND (@IsPackageProduct IS NULL OR p.IsPackageProduct=@IsPackageProduct)
				 AND (@IsPublished IS NULL OR p.IsPublished=@IsPublished)
				 AND (@MinMeasureAmount IS NULL OR p.MeasurementAmount>=@MinMeasureAmount)
				 AND (@MaxMeasureAmount IS NULL OR p.MeasurementAmount<=@MaxMeasureAmount)
				 AND (@ProductCategoryId IS NULL OR p.ProductCategoryId=@ProductCategoryId)
				 AND (@CurrencyId IS NULL OR p.CurrencyId=@CurrencyId)
				 AND (@MeasurementUnitId IS NULL OR p.MeasurementUnitId=@MeasurementUnitId)
				 AND (@RatingLikeId IS NULL OR p.RatingLikeId=@RatingLikeId)
				 AND (@IsTemplate IS NULL OR p.IsTemplate=@IsTemplate)
				 AND (@EmployeeId IS NULL OR p.Id IN(select pt.ProductId from ProductTeams pt where pt.EmployeeId=@EmployeeId))
				 AND (@BusinessId IS NULL OR p.Id IN(select cp.ProductId from BusinessProductMaps cp where cp.BusinessId=@BusinessId))
				 AND (@StoreId IS NULL OR p.Id IN(select spm.ProductId from StoreProductMaps spm where spm.StoreId=@StoreId))
				 AND (@ProductTagNameFilter='""""' OR p.Id IN(Select pt.ProductId from dbo.ProductTags pt LEFT JOIN MasterTags mt on mt.Id=pt.MasterTagId where Contains(mt.Name,@ProductTagNameFilter))
												 OR p.Id IN(Select pt.ProductId from dbo.ProductTags pt where Contains(pt.CustomTag,@ProductTagNameFilter)))
				 --End Condition For filtering
			) AS TotalCount,

			(SELECT COUNT(p.Id) 
			from [dbo].[Products] p
			LEFT JOIN [dbo].[ProductCategories] pc ON pc.Id=p.ProductCategoryId
			LEFT JOIN [dbo].[MediaLibraries] ml ON ml.Id=p.MediaLibraryId
			LEFT JOIN [dbo].[MeasurementUnits] mu ON mu.Id=p.MeasurementUnitId
			LEFT JOIN [dbo].[Currencies] c ON c.Id=p.CurrencyId
			LEFT JOIN [dbo].[RatingLikes] rl ON rl.Id=p.RatingLikeId
			WHERE p.IsDeleted=0 AND p.IsPublished=1
				 AND (@IsTemplate IS NULL OR p.IsTemplate=@IsTemplate)
				 AND (@Filter = '""""' OR Contains(p.Name,@Filter)
									   OR Contains(p.Sku,@Filter)
									   OR Contains(p.SeoTitle,@Filter)
									   OR Contains(p.MetaKeywords,@Filter)
									   OR Contains(pc.Name,@Filter)
									   OR Contains(mu.Name,@Filter)
									   OR Contains(c.Name,@Filter))
				 AND (@Name='""""' OR Contains(p.Name,@Name))
				 AND (@Sku='""""' OR Contains(p.Sku,@Sku))
				 AND (@SeoTitle='""""' OR Contains(p.SeoTitle,@SeoTitle))
				 AND (@MetaKeyWords='""""' OR Contains(p.MetaKeywords,@MetaKeyWords))
				 AND (@MinPrice IS NULL OR p.RegularPrice>=@MinPrice)
				 AND (@MaxPrice IS NULL OR p.RegularPrice<=@MaxPrice)
				 AND (@MinSalesPrice IS NULL OR p.SalePrice>=@MinSalesPrice)
				 AND (@MaxSalesPrice IS NULL OR p.SalePrice<=@MaxSalesPrice)
				 AND (@MinDiscountPercentage IS NULL OR p.PriceDiscountPercentage>=@MinDiscountPercentage)
				 AND (@MaxDiscountPercentage IS NULL OR p.PriceDiscountPercentage<=@MaxDiscountPercentage)
				 AND (@IsCallForPrice IS NULL OR p.CallForPrice=@IsCallForPrice)
				 AND (@IsTaxExempt IS NULL OR p.IsTaxExempt=@IsTaxExempt)
				 AND (@MinStockQuantity IS NULL OR p.StockQuantity>=@MinStockQuantity)
				 AND (@MaxStockQuantity IS NULL OR p.StockQuantity<=@MaxStockQuantity)
				 AND (@IsDisplayStockQuantity IS NULL OR p.IsDisplayStockQuantity=@IsDisplayStockQuantity)
				 AND (@IsPackageProduct IS NULL OR p.IsPackageProduct=@IsPackageProduct)
				 AND (@MinMeasureAmount IS NULL OR p.MeasurementAmount>=@MinMeasureAmount)
				 AND (@MaxMeasureAmount IS NULL OR p.MeasurementAmount<=@MaxMeasureAmount)
				 AND (@ProductCategoryId IS NULL OR p.ProductCategoryId=@ProductCategoryId)
				 AND (@CurrencyId IS NULL OR p.CurrencyId=@CurrencyId)
				 AND (@MeasurementUnitId IS NULL OR p.MeasurementUnitId=@MeasurementUnitId)
				 AND (@RatingLikeId IS NULL OR p.RatingLikeId=@RatingLikeId)
				 AND (@EmployeeId IS NULL OR p.Id IN(select pt.ProductId from ProductTeams pt where pt.EmployeeId=@EmployeeId))
				 AND (@BusinessId IS NULL OR p.Id IN(select cp.ProductId from BusinessProductMaps cp where cp.BusinessId=@BusinessId))
				 AND (@StoreId IS NULL OR p.Id IN(select spm.ProductId from StoreProductMaps spm where spm.StoreId=@StoreId))
				 AND (@ProductTagNameFilter='""""' OR p.Id IN(Select pt.ProductId from dbo.ProductTags pt LEFT JOIN MasterTags mt on mt.Id=pt.MasterTagId where Contains(mt.Name,@ProductTagNameFilter))
												 OR p.Id IN(Select pt.ProductId from dbo.ProductTags pt where Contains(pt.CustomTag,@ProductTagNameFilter)))
			) AS Published,

			(SELECT COUNT(p.Id) 
			from [dbo].[Products] p
			LEFT JOIN [dbo].[ProductCategories] pc ON pc.Id=p.ProductCategoryId
			LEFT JOIN [dbo].[MediaLibraries] ml ON ml.Id=p.MediaLibraryId
			LEFT JOIN [dbo].[MeasurementUnits] mu ON mu.Id=p.MeasurementUnitId
			LEFT JOIN [dbo].[Currencies] c ON c.Id=p.CurrencyId
			LEFT JOIN [dbo].[RatingLikes] rl ON rl.Id=p.RatingLikeId
			WHERE p.IsDeleted=0 AND p.IsPublished=0
				 AND (@IsTemplate IS NULL OR p.IsTemplate=@IsTemplate)
				 AND (@Filter = '""""' OR Contains(p.Name,@Filter)
									   OR Contains(p.Sku,@Filter)
									   OR Contains(p.SeoTitle,@Filter)
									   OR Contains(p.MetaKeywords,@Filter)
									   OR Contains(pc.Name,@Filter)
									   OR Contains(mu.Name,@Filter)
									   OR Contains(c.Name,@Filter))
				 AND (@Name='""""' OR Contains(p.Name,@Name))
				 AND (@Sku='""""' OR Contains(p.Sku,@Sku))
				 AND (@SeoTitle='""""' OR Contains(p.SeoTitle,@SeoTitle))
				 AND (@MetaKeyWords='""""' OR Contains(p.MetaKeywords,@MetaKeyWords))
				 AND (@MinPrice IS NULL OR p.RegularPrice>=@MinPrice)
				 AND (@MaxPrice IS NULL OR p.RegularPrice<=@MaxPrice)
				 AND (@MinSalesPrice IS NULL OR p.SalePrice>=@MinSalesPrice)
				 AND (@MaxSalesPrice IS NULL OR p.SalePrice<=@MaxSalesPrice)
				 AND (@MinDiscountPercentage IS NULL OR p.PriceDiscountPercentage>=@MinDiscountPercentage)
				 AND (@MaxDiscountPercentage IS NULL OR p.PriceDiscountPercentage<=@MaxDiscountPercentage)
				 AND (@IsCallForPrice IS NULL OR p.CallForPrice=@IsCallForPrice)
				 AND (@IsTaxExempt IS NULL OR p.IsTaxExempt=@IsTaxExempt)
				 AND (@MinStockQuantity IS NULL OR p.StockQuantity>=@MinStockQuantity)
				 AND (@MaxStockQuantity IS NULL OR p.StockQuantity<=@MaxStockQuantity)
				 AND (@IsDisplayStockQuantity IS NULL OR p.IsDisplayStockQuantity=@IsDisplayStockQuantity)
				 AND (@IsPackageProduct IS NULL OR p.IsPackageProduct=@IsPackageProduct)
				 AND (@MinMeasureAmount IS NULL OR p.MeasurementAmount>=@MinMeasureAmount)
				 AND (@MaxMeasureAmount IS NULL OR p.MeasurementAmount<=@MaxMeasureAmount)
				 AND (@ProductCategoryId IS NULL OR p.ProductCategoryId=@ProductCategoryId)
				 AND (@CurrencyId IS NULL OR p.CurrencyId=@CurrencyId)
				 AND (@MeasurementUnitId IS NULL OR p.MeasurementUnitId=@MeasurementUnitId)
				 AND (@RatingLikeId IS NULL OR p.RatingLikeId=@RatingLikeId)
				 AND (@EmployeeId IS NULL OR p.Id IN(select pt.ProductId from ProductTeams pt where pt.EmployeeId=@EmployeeId))
				 AND (@BusinessId IS NULL OR p.Id IN(select cp.ProductId from BusinessProductMaps cp where cp.BusinessId=@BusinessId))
				 AND (@StoreId IS NULL OR p.Id IN(select spm.ProductId from StoreProductMaps spm where spm.StoreId=@StoreId))
				 AND (@ProductTagNameFilter='""""' OR p.Id IN(Select pt.ProductId from dbo.ProductTags pt LEFT JOIN MasterTags mt on mt.Id=pt.MasterTagId where Contains(mt.Name,@ProductTagNameFilter))
												 OR p.Id IN(Select pt.ProductId from dbo.ProductTags pt where Contains(pt.CustomTag,@ProductTagNameFilter)))
			) AS UnPublished,

			(SELECT 
			abo.Id AS PictureId,
			p.Id as Id,
			p.Name AS Name,
			(select Name from dbo.Stores where Id=(Select Top 1 spm.StoreId from StoreProductMaps spm where spm.ProductId=p.Id order by spm.Id desc))as StoreName,
			pc.Name AS CategoryName,
			p.Sku AS Sku,
			p.RegularPrice AS RegularPrice,
			p.SalePrice AS SalesPrice,
			p.UnitPrice AS UnitPrice,
			p.PriceDiscountPercentage AS PriceDiscountPercentage,
			p.CallForPrice AS CallForPrice,
			c.Name AS CurrencyName,
			c.Ticker AS CurrencyTicker,
			c.Icon AS CurrencyIcon,
			p.MeasurementAmount AS MeasureAmount,
			mu.Name AS MeasurementUnitName,
			p.StockQuantity AS StockQuantity,
			p.IsDisplayStockQuantity AS IsDisplayStockQuantity,
			p.IsPublished AS IsPublished,
			p.IsPackageProduct AS IsPackageProduct
			FROM [dbo].[Products] p
			LEFT JOIN [dbo].[ProductCategories] pc ON pc.Id=p.ProductCategoryId
			LEFT JOIN [dbo].[MediaLibraries] ml ON ml.Id=p.MediaLibraryId
			LEFT JOIN [dbo].[MeasurementUnits] mu ON mu.Id=p.MeasurementUnitId
			LEFT JOIN [dbo].[Currencies] c ON c.Id=p.CurrencyId
			LEFT JOIN [dbo].[RatingLikes] rl ON rl.Id=p.RatingLikeId
			LEFT JOIN [dbo].[AppBinaryObjects] abo ON abo.Id=ml.BinaryObjectId
			
			--Begin Condition For Filtering

			WHERE p.IsDeleted=0
				 AND (@Filter = '""""' OR Contains(p.Name,@Filter)
									   OR Contains(p.Sku,@Filter)
									   OR Contains(p.SeoTitle,@Filter)
									   OR Contains(p.MetaKeywords,@Filter)
									   OR Contains(pc.Name,@Filter)
									   OR Contains(mu.Name,@Filter)
									   OR Contains(c.Name,@Filter))
				 AND (@Name='""""' OR Contains(p.Name,@Name))
				 AND (@Sku='""""' OR Contains(p.Sku,@Sku))
				 AND (@SeoTitle='""""' OR Contains(p.SeoTitle,@SeoTitle))
				 AND (@MetaKeyWords='""""' OR Contains(p.MetaKeywords,@MetaKeyWords))
				 AND (@MinPrice IS NULL OR p.RegularPrice>=@MinPrice)
				 AND (@MaxPrice IS NULL OR p.RegularPrice<=@MaxPrice)
				 AND (@MinSalesPrice IS NULL OR p.SalePrice>=@MinSalesPrice)
				 AND (@MaxSalesPrice IS NULL OR p.SalePrice<=@MaxSalesPrice)
				 AND (@MinDiscountPercentage IS NULL OR p.PriceDiscountPercentage>=@MinDiscountPercentage)
				 AND (@MaxDiscountPercentage IS NULL OR p.PriceDiscountPercentage<=@MaxDiscountPercentage)
				 AND (@IsCallForPrice IS NULL OR p.CallForPrice=@IsCallForPrice)
				 AND (@IsTaxExempt IS NULL OR p.IsTaxExempt=@IsTaxExempt)
				 AND (@MinStockQuantity IS NULL OR p.StockQuantity>=@MinStockQuantity)
				 AND (@MaxStockQuantity IS NULL OR p.StockQuantity<=@MaxStockQuantity)
				 AND (@IsDisplayStockQuantity IS NULL OR p.IsDisplayStockQuantity=@IsDisplayStockQuantity)
				 AND (@IsPackageProduct IS NULL OR p.IsPackageProduct=@IsPackageProduct)
				 AND (@IsPublished IS NULL OR p.IsPublished=@IsPublished)
				 AND (@MinMeasureAmount IS NULL OR p.MeasurementAmount>=@MinMeasureAmount)
				 AND (@MaxMeasureAmount IS NULL OR p.MeasurementAmount<=@MaxMeasureAmount)
				 AND (@ProductCategoryId IS NULL OR p.ProductCategoryId=@ProductCategoryId)
				 AND (@CurrencyId IS NULL OR p.CurrencyId=@CurrencyId)
				 AND (@MeasurementUnitId IS NULL OR p.MeasurementUnitId=@MeasurementUnitId)
				 AND (@RatingLikeId IS NULL OR p.RatingLikeId=@RatingLikeId)
				 AND (@IsTemplate IS NULL OR p.IsTemplate=@IsTemplate)
				 AND (@EmployeeId IS NULL OR p.Id IN(select pt.ProductId from ProductTeams pt where pt.EmployeeId=@EmployeeId))
				 AND (@BusinessId IS NULL OR p.Id IN(select cp.ProductId from BusinessProductMaps cp where cp.BusinessId=@BusinessId))
				 AND (@StoreId IS NULL OR p.Id IN(select spm.ProductId from StoreProductMaps spm where spm.StoreId=@StoreId))
				 AND (@ProductTagNameFilter='""""' OR p.Id IN(Select pt.ProductId from dbo.ProductTags pt LEFT JOIN MasterTags mt on mt.Id=pt.MasterTagId where Contains(mt.Name,@ProductTagNameFilter))
												 OR p.Id IN(Select pt.ProductId from dbo.ProductTags pt where Contains(pt.CustomTag,@ProductTagNameFilter)))
				 --End Condition For filtering

			ORDER BY ml.Id desc
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
            string query = @"DROP PROCEDURE [dbo].[usp_GetAllProducts]";
            migrationBuilder.Sql(query);
        }
    }
}
