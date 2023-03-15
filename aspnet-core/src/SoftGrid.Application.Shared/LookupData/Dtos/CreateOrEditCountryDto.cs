using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditCountryDto : EntityDto<long?>
    {

        [Required]
        [StringLength(CountryConsts.MaxNameLength, MinimumLength = CountryConsts.MinNameLength)]
        public string Name { get; set; }

        [StringLength(CountryConsts.MaxTickerLength, MinimumLength = CountryConsts.MinTickerLength)]
        public string Ticker { get; set; }

        [StringLength(CountryConsts.MaxFlagIconLength, MinimumLength = CountryConsts.MinFlagIconLength)]
        public string FlagIcon { get; set; }

        [StringLength(CountryConsts.MaxPhoneCodeLength, MinimumLength = CountryConsts.MinPhoneCodeLength)]
        public string PhoneCode { get; set; }

    }
}