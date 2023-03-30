using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductCustomerStatForEditOutput
    {
        public CreateOrEditProductCustomerStatDto ProductCustomerStat { get; set; }

        public string ProductName { get; set; }

        public string ContactFullName { get; set; }

        public string StoreName { get; set; }

        public string HubName { get; set; }

        public string SocialMediaName { get; set; }

    }
}