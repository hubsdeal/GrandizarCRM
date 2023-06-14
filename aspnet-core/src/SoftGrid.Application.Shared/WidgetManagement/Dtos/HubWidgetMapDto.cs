using Abp.Application.Services.Dto;

using SoftGrid.WidgetManagement.Enums;

using System;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class HubWidgetMapDto : EntityDto<long>
    {
        public string CustomName { get; set; }

        public int? DisplaySequence { get; set; }

        public WidgetType WidgetTypeId { get; set; }

        public long HubId { get; set; }

        public long MasterWidgetId { get; set; }
        public string MasterWidgetName { get; set; }
        public string MasterWidgetDescription { get; set; }
        public string MasterWidgetDesignCode { get; set; }
        public int? MasterWidgetInternalDisplayNumber { get; set; }
        public Guid? MasterWidgetThumbnailImageId { get; set; }
    }
}