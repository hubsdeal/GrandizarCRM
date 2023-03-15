using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditMeasurementUnitDto : EntityDto<long?>
    {

        [Required]
        [StringLength(MeasurementUnitConsts.MaxNameLength, MinimumLength = MeasurementUnitConsts.MinNameLength)]
        public string Name { get; set; }

    }
}