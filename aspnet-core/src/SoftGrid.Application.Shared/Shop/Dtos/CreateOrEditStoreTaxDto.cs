using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditStoreTaxDto : EntityDto<long?>
    {

        [StringLength(StoreTaxConsts.MaxTaxNameLength, MinimumLength = StoreTaxConsts.MinTaxNameLength)]
        public string TaxName { get; set; }

        public bool PercentageOrAmount { get; set; }

        public double? TaxRatePercentage { get; set; }

        public double? TaxAmount { get; set; }

        public long StoreId { get; set; }

    }
}