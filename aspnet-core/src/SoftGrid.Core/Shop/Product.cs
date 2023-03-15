using SoftGrid.Shop;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("Products")]
    public class Product : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(ProductConsts.MaxNameLength, MinimumLength = ProductConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual string ShortDescription { get; set; }

        public virtual string Description { get; set; }

        [StringLength(ProductConsts.MaxSkuLength, MinimumLength = ProductConsts.MinSkuLength)]
        public virtual string Sku { get; set; }

        [StringLength(ProductConsts.MaxUrlLength, MinimumLength = ProductConsts.MinUrlLength)]
        public virtual string Url { get; set; }

        [StringLength(ProductConsts.MaxSeoTitleLength, MinimumLength = ProductConsts.MinSeoTitleLength)]
        public virtual string SeoTitle { get; set; }

        [StringLength(ProductConsts.MaxMetaKeywordsLength, MinimumLength = ProductConsts.MinMetaKeywordsLength)]
        public virtual string MetaKeywords { get; set; }

        public virtual string MetaDescription { get; set; }

        public virtual double? RegularPrice { get; set; }

        public virtual double? SalePrice { get; set; }

        public virtual double? PriceDiscountPercentage { get; set; }

        public virtual bool CallForPrice { get; set; }

        public virtual double? UnitPrice { get; set; }

        public virtual int? MeasurementAmount { get; set; }

        public virtual bool IsTaxExempt { get; set; }

        public virtual int? StockQuantity { get; set; }

        public virtual bool IsDisplayStockQuantity { get; set; }

        public virtual bool IsPublished { get; set; }

        public virtual bool IsPackageProduct { get; set; }

        public virtual string InternalNotes { get; set; }

        public virtual bool IsTemplate { get; set; }

        public virtual double? PriceDiscountAmount { get; set; }

        public virtual bool IsService { get; set; }

        public virtual bool IsWholeSaleProduct { get; set; }

        [StringLength(ProductConsts.MaxProductManufacturerSkuLength, MinimumLength = ProductConsts.MinProductManufacturerSkuLength)]
        public virtual string ProductManufacturerSku { get; set; }

        public virtual bool ByOrderOnly { get; set; }

        public virtual int? Score { get; set; }

        public virtual long ProductCategoryId { get; set; }

        [ForeignKey("ProductCategoryId")]
        public ProductCategory ProductCategoryFk { get; set; }

        public virtual long? MediaLibraryId { get; set; }

        [ForeignKey("MediaLibraryId")]
        public MediaLibrary MediaLibraryFk { get; set; }

        public virtual long? MeasurementUnitId { get; set; }

        [ForeignKey("MeasurementUnitId")]
        public MeasurementUnit MeasurementUnitFk { get; set; }

        public virtual long? CurrencyId { get; set; }

        [ForeignKey("CurrencyId")]
        public Currency CurrencyFk { get; set; }

        public virtual long? RatingLikeId { get; set; }

        [ForeignKey("RatingLikeId")]
        public RatingLike RatingLikeFk { get; set; }

    }
}