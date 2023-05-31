using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class GetStoreWidgetProductMapForEditOutput
    {
        public CreateOrEditStoreWidgetProductMapDto StoreWidgetProductMap { get; set; }

        public string StoreWidgetMapCustomName { get; set; }

        public string ProductName { get; set; }

    }
}