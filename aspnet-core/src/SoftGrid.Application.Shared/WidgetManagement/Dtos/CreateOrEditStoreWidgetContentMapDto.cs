using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class CreateOrEditStoreWidgetContentMapDto : EntityDto<long?>
    {

        public int? DisplaySequence { get; set; }

        public long StoreWidgetMapId { get; set; }

        public long ContentId { get; set; }

    }
}