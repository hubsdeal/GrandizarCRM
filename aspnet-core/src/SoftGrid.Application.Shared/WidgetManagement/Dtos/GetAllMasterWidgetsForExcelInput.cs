using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class GetAllMasterWidgetsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string DesignCodeFilter { get; set; }

        public int? PublishFilter { get; set; }

        public int? MaxInternalDisplayNumberFilter { get; set; }
        public int? MinInternalDisplayNumberFilter { get; set; }

        public Guid? ThumbnailImageIdFilter { get; set; }

    }
}