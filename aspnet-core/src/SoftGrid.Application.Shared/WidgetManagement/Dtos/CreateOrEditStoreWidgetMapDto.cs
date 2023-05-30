using SoftGrid.WidgetManagement.Enums;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class CreateOrEditStoreWidgetMapDto : EntityDto<long?>
    {

        public int? DisplaySequence { get; set; }

        public WidgetType WidgetTypeId { get; set; }

        [StringLength(StoreWidgetMapConsts.MaxCustomNameLength, MinimumLength = StoreWidgetMapConsts.MinCustomNameLength)]
        public string CustomName { get; set; }

        public long MasterWidgetId { get; set; }

        public long StoreId { get; set; }

    }
}