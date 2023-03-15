using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditStateDto : EntityDto<long?>
    {

        [Required]
        [StringLength(StateConsts.MaxNameLength, MinimumLength = StateConsts.MinNameLength)]
        public string Name { get; set; }

        [StringLength(StateConsts.MaxTickerLength, MinimumLength = StateConsts.MinTickerLength)]
        public string Ticker { get; set; }

        public long CountryId { get; set; }

    }
}