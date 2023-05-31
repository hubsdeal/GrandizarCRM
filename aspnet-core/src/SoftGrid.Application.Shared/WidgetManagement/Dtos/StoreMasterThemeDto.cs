using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class StoreMasterThemeDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ThemeCode { get; set; }

        public Guid ThumbnailImageId { get; set; }

    }
}