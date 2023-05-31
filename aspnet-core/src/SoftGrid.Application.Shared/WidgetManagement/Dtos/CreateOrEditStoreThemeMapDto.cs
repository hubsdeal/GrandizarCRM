using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class CreateOrEditStoreThemeMapDto : EntityDto<long?>
    {

        public bool Active { get; set; }

        public long StoreMasterThemeId { get; set; }

        public long StoreId { get; set; }

    }
}