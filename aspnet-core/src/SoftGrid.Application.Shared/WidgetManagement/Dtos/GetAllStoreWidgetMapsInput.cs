using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class GetAllStoreWidgetMapsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxDisplaySequenceFilter { get; set; }
        public int? MinDisplaySequenceFilter { get; set; }

        public int? WidgetTypeIdFilter { get; set; }

        public string CustomNameFilter { get; set; }

        public string MasterWidgetNameFilter { get; set; }

        public string StoreNameFilter { get; set; }

    }
}