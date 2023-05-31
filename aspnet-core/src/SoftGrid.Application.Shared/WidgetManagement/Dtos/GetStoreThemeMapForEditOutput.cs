using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class GetStoreThemeMapForEditOutput
    {
        public CreateOrEditStoreThemeMapDto StoreThemeMap { get; set; }

        public string StoreMasterThemeName { get; set; }

        public string StoreName { get; set; }

    }
}