using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductWholeSalePriceForEditOutput
    {
        public CreateOrEditProductWholeSalePriceDto ProductWholeSalePrice { get; set; }

        public string ProductName { get; set; }

        public string ProductWholeSaleQuantityTypeName { get; set; }

        public string MeasurementUnitName { get; set; }

        public string CurrencyName { get; set; }

    }
}