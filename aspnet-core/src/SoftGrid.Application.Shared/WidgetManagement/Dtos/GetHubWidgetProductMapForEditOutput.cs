using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class GetHubWidgetProductMapForEditOutput
    {
        public CreateOrEditHubWidgetProductMapDto HubWidgetProductMap { get; set; }

        public string HubWidgetMapCustomName { get; set; }

        public string ProductName { get; set; }

    }
}