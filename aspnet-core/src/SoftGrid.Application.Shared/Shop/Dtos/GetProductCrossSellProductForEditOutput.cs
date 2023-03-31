using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductCrossSellProductForEditOutput
    {
        public CreateOrEditProductCrossSellProductDto ProductCrossSellProduct { get; set; }

        public string ProductName { get; set; }

    }
}