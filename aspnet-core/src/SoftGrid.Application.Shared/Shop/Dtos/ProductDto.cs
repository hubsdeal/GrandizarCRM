using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public string Sku { get; set; }

        public string Url { get; set; }

        public string SeoTitle { get; set; }

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

        public string ProductManufacturerSku { get; set; }

        public bool ByOrderOnly { get; set; }

        public int? Score { get; set; }

        public long ProductCategoryId { get; set; }

        public long? MediaLibraryId { get; set; }

        public long? MeasurementUnitId { get; set; }

        public long? CurrencyId { get; set; }

        public long? RatingLikeId { get; set; }

        public long? ContactId { get; set; }

        public long? StoreId { get; set; }

        public Guid? PictureId { get; set; }

        public string Picture { get; set; }

    }
}