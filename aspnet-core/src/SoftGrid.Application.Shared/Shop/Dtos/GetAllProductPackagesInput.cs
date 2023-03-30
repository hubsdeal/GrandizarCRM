using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductPackagesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public long? MaxPackageProductIdFilter { get; set; }
        public long? MinPackageProductIdFilter { get; set; }

        public int? MaxDisplaySequenceFilter { get; set; }
        public int? MinDisplaySequenceFilter { get; set; }

        public double? MaxPriceFilter { get; set; }
        public double? MinPriceFilter { get; set; }

        public int? MaxQuantityFilter { get; set; }
        public int? MinQuantityFilter { get; set; }

        public string ProductNameFilter { get; set; }

        public string MediaLibraryNameFilter { get; set; }

    }
}