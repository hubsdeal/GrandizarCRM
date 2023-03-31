using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductByVariantsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public double? MaxPriceFilter { get; set; }
        public double? MinPriceFilter { get; set; }

        public int? MaxDisplaySequenceFilter { get; set; }
        public int? MinDisplaySequenceFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string ProductNameFilter { get; set; }

        public string ProductVariantNameFilter { get; set; }

        public string ProductVariantCategoryNameFilter { get; set; }

        public string MediaLibraryNameFilter { get; set; }

    }
}