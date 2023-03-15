using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditCountyDto : EntityDto<long?>
    {

        [Required]
        [StringLength(CountyConsts.MaxNameLength, MinimumLength = CountyConsts.MinNameLength)]
        public string Name { get; set; }

        public long? CountryId { get; set; }

        public long? StateId { get; set; }

    }
}