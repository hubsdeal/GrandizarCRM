using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class GetStoreWidgetMapForEditOutput
    {
        public CreateOrEditStoreWidgetMapDto StoreWidgetMap { get; set; }

        public string MasterWidgetName { get; set; }

        public string StoreName { get; set; }

    }
}