using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class CreateOrEditHubWidgetContentMapDto : EntityDto<long?>
    {

        public int? DisplaySequence { get; set; }

        public long HubWidgetMapId { get; set; }

        public long ContentId { get; set; }

    }
}