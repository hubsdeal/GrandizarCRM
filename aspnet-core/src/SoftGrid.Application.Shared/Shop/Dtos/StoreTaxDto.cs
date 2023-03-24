using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreTaxDto : EntityDto<long>
    {
        public string TaxName { get; set; }

        public bool PercentageOrAmount { get; set; }

        public double? TaxRatePercentage { get; set; }

        public double? TaxAmount { get; set; }

        public long StoreId { get; set; }

    }
}