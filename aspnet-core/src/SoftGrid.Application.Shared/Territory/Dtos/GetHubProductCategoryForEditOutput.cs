using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Territory.Dtos
{
    public class GetHubProductCategoryForEditOutput
    {
        public CreateOrEditHubProductCategoryDto HubProductCategory { get; set; }

        public string HubName { get; set; }

        public string ProductCategoryName { get; set; }

    }
}