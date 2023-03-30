using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductCategoryVariantMapsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string ProductCategoryNameFilter { get; set; }

        public string ProductVariantCategoryNameFilter { get; set; }

    }
}