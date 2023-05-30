using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class GetAllStoreMasterThemesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string ThemeCodeFilter { get; set; }

        public Guid? ThumbnailImageIdFilter { get; set; }

    }
}