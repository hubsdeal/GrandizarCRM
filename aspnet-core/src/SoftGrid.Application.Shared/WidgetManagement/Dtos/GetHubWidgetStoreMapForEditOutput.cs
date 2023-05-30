using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class GetHubWidgetStoreMapForEditOutput
    {
        public CreateOrEditHubWidgetStoreMapDto HubWidgetStoreMap { get; set; }

        public string HubWidgetMapCustomName { get; set; }

        public string StoreName { get; set; }

    }
}