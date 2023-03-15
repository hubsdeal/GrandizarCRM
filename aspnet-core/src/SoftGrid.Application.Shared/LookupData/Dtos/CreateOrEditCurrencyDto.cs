using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditCurrencyDto : EntityDto<long?>
    {

        [Required]
        [StringLength(CurrencyConsts.MaxNameLength, MinimumLength = CurrencyConsts.MinNameLength)]
        public string Name { get; set; }

        [StringLength(CurrencyConsts.MaxTickerLength, MinimumLength = CurrencyConsts.MinTickerLength)]
        public string Ticker { get; set; }

        [StringLength(CurrencyConsts.MaxIconLength, MinimumLength = CurrencyConsts.MinIconLength)]
        public string Icon { get; set; }

    }
}