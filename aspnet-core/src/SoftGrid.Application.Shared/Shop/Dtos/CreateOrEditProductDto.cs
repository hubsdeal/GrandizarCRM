using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductDto : EntityDto<long?>
    {

        [Required]
        [StringLength(ProductConsts.MaxNameLength, MinimumLength = ProductConsts.MinNameLength)]
        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        [StringLength(ProductConsts.MaxSkuLength, MinimumLength = ProductConsts.MinSkuLength)]
        public string Sku { get; set; }

        [StringLength(ProductConsts.MaxUrlLength, MinimumLength = ProductConsts.MinUrlLength)]
        public string Url { get; set; }

        [StringLength(ProductConsts.MaxSeoTitleLength, MinimumLength = ProductConsts.MinSeoTitleLength)]
        public string SeoTitle { get; set; }

        [StringLength(ProductConsts.MaxMetaKeywordsLength, MinimumLength = ProductConsts.MinMetaKeywordsLength)]
        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        public double? RegularPrice { get; set; }

        public double? SalePrice { get; set; }

        public double? PriceDiscountPercentage { get; set; }

        public bool CallForPrice { get; set; }

        public double? UnitPrice { get; set; }

        public int? MeasurementAmount { get; set; }

        public bool IsTaxExempt { get; set; }

        public int? StockQuantity { get; set; }

        public bool IsDisplayStockQuantity { get; set; }

        public bool IsPublished { get; set; }

        public bool IsPackageProduct { get; set; }

        public string InternalNotes { get; set; }

        public bool IsTemplate { get; set; }

        public double? PriceDiscountAmount { get; set; }

        public bool IsService { get; set; }

        public bool IsWholeSaleProduct { get; set; }

        [StringLength(ProductConsts.MaxProductManufacturerSkuLength, MinimumLength = ProductConsts.MinProductManufacturerSkuLength)]
        public string ProductManufacturerSku { get; set; }

        public bool ByOrderOnly { get; set; }

        public int? Score { get; set; }

        public long ProductCategoryId { get; set; }

        public long? MediaLibraryId { get; set; }

        public long? MeasurementUnitId { get; set; }

        public long? CurrencyId { get; set; }

        public long? RatingLikeId { get; set; }

    }
}