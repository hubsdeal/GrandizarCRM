using SoftGrid.WidgetManagement.Enums;

using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class HubWidgetMapDto : EntityDto<long>
    {
        public string CustomName { get; set; }

        public int? DisplaySequence { get; set; }

        public WidgetType WidgetTypeId { get; set; }

        public long HubId { get; set; }

        public long MasterWidgetId { get; set; }

    }
}