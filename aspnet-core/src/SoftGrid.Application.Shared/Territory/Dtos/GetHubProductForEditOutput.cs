using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Territory.Dtos
{
    public class GetHubProductForEditOutput
    {
        public CreateOrEditHubProductDto HubProduct { get; set; }

        public string HubName { get; set; }

        public string ProductName { get; set; }

    }
}