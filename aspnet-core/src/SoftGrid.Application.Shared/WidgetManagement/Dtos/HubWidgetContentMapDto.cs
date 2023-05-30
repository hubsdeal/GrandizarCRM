using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class HubWidgetContentMapDto : EntityDto<long>
    {
        public int? DisplaySequence { get; set; }

        public long HubWidgetMapId { get; set; }

        public long ContentId { get; set; }

    }
}