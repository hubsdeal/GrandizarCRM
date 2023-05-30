using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class GetHubWidgetMapForEditOutput
    {
        public CreateOrEditHubWidgetMapDto HubWidgetMap { get; set; }

        public string HubName { get; set; }

        public string MasterWidgetName { get; set; }

    }
}