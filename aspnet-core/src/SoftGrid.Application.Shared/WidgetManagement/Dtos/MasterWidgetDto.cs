using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class MasterWidgetDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string DesignCode { get; set; }

        public bool Publish { get; set; }

        public int? InternalDisplayNumber { get; set; }

        public Guid ThumbnailImageId { get; set; }

    }
}