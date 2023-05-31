using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class GetHubWidgetContentMapForEditOutput
    {
        public CreateOrEditHubWidgetContentMapDto HubWidgetContentMap { get; set; }

        public string HubWidgetMapCustomName { get; set; }

        public string ContentTitle { get; set; }

    }
}