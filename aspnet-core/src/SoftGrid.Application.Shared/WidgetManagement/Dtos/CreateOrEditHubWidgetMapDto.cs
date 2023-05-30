using SoftGrid.WidgetManagement.Enums;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class CreateOrEditHubWidgetMapDto : EntityDto<long?>
    {

        [StringLength(HubWidgetMapConsts.MaxCustomNameLength, MinimumLength = HubWidgetMapConsts.MinCustomNameLength)]
        public string CustomName { get; set; }

        public int? DisplaySequence { get; set; }

        public WidgetType WidgetTypeId { get; set; }

        public long HubId { get; set; }

        public long MasterWidgetId { get; set; }

    }
}