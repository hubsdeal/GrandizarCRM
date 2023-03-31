using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductUpsellRelatedProductForEditOutput
    {
        public CreateOrEditProductUpsellRelatedProductDto ProductUpsellRelatedProduct { get; set; }

        public string ProductName { get; set; }

    }
}