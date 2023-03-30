using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductAndGiftCardMapForEditOutput
    {
        public CreateOrEditProductAndGiftCardMapDto ProductAndGiftCardMap { get; set; }

        public string ProductName { get; set; }

        public string CurrencyName { get; set; }

    }
}