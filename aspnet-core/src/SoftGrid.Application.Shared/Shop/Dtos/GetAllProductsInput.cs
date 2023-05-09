using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string ShortDescriptionFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string SkuFilter { get; set; }

        public string UrlFilter { get; set; }

        public string SeoTitleFilter { get; set; }

        public string MetaKeywordsFilter { get; set; }

        public string MetaDescriptionFilter { get; set; }

        public double? MaxRegularPriceFilter { get; set; }
        public double? MinRegularPriceFilter { get; set; }

        public double? MaxSalePriceFilter { get; set; }
        public double? MinSalePriceFilter { get; set; }

        public double? MaxPriceDiscountPercentageFilter { get; set; }
        public double? MinPriceDiscountPercentageFilter { get; set; }

        public int? CallForPriceFilter { get; set; }

        public double? MaxUnitPriceFilter { get; set; }
        public double? MinUnitPriceFilter { get; set; }

        public int? MaxMeasurementAmountFilter { get; set; }
        public int? MinMeasurementAmountFilter { get; set; }

        public int? IsTaxExemptFilter { get; set; }

        public int? MaxStockQuantityFilter { get; set; }
        public int? MinStockQuantityFilter { get; set; }

        public int? IsDisplayStockQuantityFilter { get; set; }

        public int? IsPublishedFilter { get; set; }

        public int? IsPackageProductFilter { get; set; }

        public string InternalNotesFilter { get; set; }

        public int? IsTemplateFilter { get; set; }

        public double? MaxPriceDiscountAmountFilter { get; set; }
        public double? MinPriceDiscountAmountFilter { get; set; }

        public int? IsServiceFilter { get; set; }

        public int? IsWholeSaleProductFilter { get; set; }

        public string ProductManufacturerSkuFilter { get; set; }

        public int? ByOrderOnlyFilter { get; set; }

        public int? MaxScoreFilter { get; set; }
        public int? MinScoreFilter { get; set; }

        public string ProductCategoryNameFilter { get; set; }

        public string MediaLibraryNameFilter { get; set; }

        public string MeasurementUnitNameFilter { get; set; }

        public string CurrencyNameFilter { get; set; }

        public string RatingLikeNameFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

        public string StoreNameFilter { get; set; }

    }

    public class GetAllProductsInputForSp : GetAllProductsInput
    {
        public decimal? MinPriceFilter { get; set; }
        public decimal? MaxPriceFilter { get; set; }

        public long? ProductCategoryIdFilter { get; set; }
        public long? CurrencyIdFilter { get; set; }
        public long? MeasurementUnitIdFilter { get; set; }
        public long? RatingLikeIdFilter { get; set; }

        public int? IsTemplateFilter { get; set; }
        public long? EmployeeIdFilter { get; set; }
        public long? BusinessIdFilter { get; set; }
        public long? StoreIdFilter { get; set; }
        public string ProductTagNameFilter { get; set; }
    }
}