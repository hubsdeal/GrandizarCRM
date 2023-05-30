using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class GetStoreWidgetContentMapForEditOutput
    {
        public CreateOrEditStoreWidgetContentMapDto StoreWidgetContentMap { get; set; }

        public string StoreWidgetMapCustomName { get; set; }

        public string ContentTitle { get; set; }

    }
}