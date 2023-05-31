using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class GetAllStoreThemeMapsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? ActiveFilter { get; set; }

        public string StoreMasterThemeNameFilter { get; set; }

        public string StoreNameFilter { get; set; }

    }
}