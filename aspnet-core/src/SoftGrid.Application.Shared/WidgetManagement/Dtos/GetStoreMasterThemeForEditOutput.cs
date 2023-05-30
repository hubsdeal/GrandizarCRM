using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class GetStoreMasterThemeForEditOutput
    {
        public CreateOrEditStoreMasterThemeDto StoreMasterTheme { get; set; }

    }
}