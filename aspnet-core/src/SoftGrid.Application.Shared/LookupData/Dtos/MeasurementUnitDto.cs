using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.LookupData.Dtos
{
    public class MeasurementUnitDto : EntityDto<long>
    {
        public string Name { get; set; }

    }
}