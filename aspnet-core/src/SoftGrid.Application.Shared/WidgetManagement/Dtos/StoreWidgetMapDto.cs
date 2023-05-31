using SoftGrid.WidgetManagement.Enums;

using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class StoreWidgetMapDto : EntityDto<long>
    {
        public int? DisplaySequence { get; set; }

        public WidgetType WidgetTypeId { get; set; }

        public string CustomName { get; set; }

        public long MasterWidgetId { get; set; }

        public long StoreId { get; set; }

    }
}