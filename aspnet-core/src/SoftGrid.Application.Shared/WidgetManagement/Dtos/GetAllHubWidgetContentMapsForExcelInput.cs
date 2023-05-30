using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class GetAllHubWidgetContentMapsForExcelInput
    {
        public string Filter { get; set; }

        public int? MaxDisplaySequenceFilter { get; set; }
        public int? MinDisplaySequenceFilter { get; set; }

        public string HubWidgetMapCustomNameFilter { get; set; }

        public string ContentTitleFilter { get; set; }

    }
}