using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class StoreThemeMapDto : EntityDto<long>
    {
        public bool Active { get; set; }

        public long StoreMasterThemeId { get; set; }

        public long StoreId { get; set; }

    }
}