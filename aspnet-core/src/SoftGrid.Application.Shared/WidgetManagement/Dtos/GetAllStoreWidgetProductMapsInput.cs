using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class GetAllStoreWidgetProductMapsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxDisplaySequenceFilter { get; set; }
        public int? MinDisplaySequenceFilter { get; set; }

        public string StoreWidgetMapCustomNameFilter { get; set; }

        public string ProductNameFilter { get; set; }

    }
}