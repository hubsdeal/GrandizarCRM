using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Territory.Dtos
{
    public class GetHubSalesProjectionForEditOutput
    {
        public CreateOrEditHubSalesProjectionDto HubSalesProjection { get; set; }

        public string HubName { get; set; }

        public string ProductCategoryName { get; set; }

        public string StoreName { get; set; }

        public string CurrencyName { get; set; }

    }
}