using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductCategoryMapForEditOutput
    {
        public CreateOrEditProductCategoryMapDto ProductCategoryMap { get; set; }

        public string ProductName { get; set; }

        public string ProductCategoryName { get; set; }

    }
}