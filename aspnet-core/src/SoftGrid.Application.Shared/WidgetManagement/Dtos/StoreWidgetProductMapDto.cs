using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class StoreWidgetProductMapDto : EntityDto<long>
    {
        public int? DisplaySequence { get; set; }

        public long StoreWidgetMapId { get; set; }

        public long ProductId { get; set; }

    }
}