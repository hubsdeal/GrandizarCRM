using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class CreateOrEditStoreMasterThemeDto : EntityDto<long?>
    {

        [StringLength(StoreMasterThemeConsts.MaxNameLength, MinimumLength = StoreMasterThemeConsts.MinNameLength)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string ThemeCode { get; set; }

        public Guid ThumbnailImageId { get; set; }

    }
}