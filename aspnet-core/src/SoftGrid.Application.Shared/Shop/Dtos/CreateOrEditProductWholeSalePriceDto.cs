using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductWholeSalePriceDto : EntityDto<long?>
    {

        public double? Price { get; set; }

        public double? ExactQuantity { get; set; }

        public string PackageInfo { get; set; }

        public int? PackageQuantity { get; set; }

        [StringLength(ProductWholeSalePriceConsts.MaxWholeSaleSkuIdLength, MinimumLength = ProductWholeSalePriceConsts.MinWholeSaleSkuIdLength)]
        public string WholeSaleSkuId { get; set; }

        public long? ProductId { get; set; }

        public long? ProductWholeSaleQuantityTypeId { get; set; }

        public long? MeasurementUnitId { get; set; }

        public long? CurrencyId { get; set; }

    }
}