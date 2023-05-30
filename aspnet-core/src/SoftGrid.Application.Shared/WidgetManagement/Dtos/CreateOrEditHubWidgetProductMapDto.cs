using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class CreateOrEditHubWidgetProductMapDto : EntityDto<long?>
    {

        public int? DisplaySequence { get; set; }

        public long HubWidgetMapId { get; set; }

        public long ProductId { get; set; }

    }
}