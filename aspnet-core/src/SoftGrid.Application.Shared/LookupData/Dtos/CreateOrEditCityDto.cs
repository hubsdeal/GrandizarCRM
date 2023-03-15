using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditCityDto : EntityDto<long?>
    {

        [Required]
        [StringLength(CityConsts.MaxNameLength, MinimumLength = CityConsts.MinNameLength)]
        public string Name { get; set; }

        public long? CountryId { get; set; }

        public long? StateId { get; set; }

        public long? CountyId { get; set; }

    }
}