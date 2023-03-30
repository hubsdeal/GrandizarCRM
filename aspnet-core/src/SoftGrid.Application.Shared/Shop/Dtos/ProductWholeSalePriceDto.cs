using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductWholeSalePriceDto : EntityDto<long>
    {
        public double? Price { get; set; }

        public double? ExactQuantity { get; set; }

        public string PackageInfo { get; set; }

        public int? PackageQuantity { get; set; }

        public string WholeSaleSkuId { get; set; }

        public long? ProductId { get; set; }

        public long? ProductWholeSaleQuantityTypeId { get; set; }

        public long? MeasurementUnitId { get; set; }

        public long? CurrencyId { get; set; }

    }
}