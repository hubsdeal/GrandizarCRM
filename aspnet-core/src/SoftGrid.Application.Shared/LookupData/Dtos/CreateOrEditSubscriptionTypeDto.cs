using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditSubscriptionTypeDto : EntityDto<long?>
    {

        [Required]
        [StringLength(SubscriptionTypeConsts.MaxNameLength, MinimumLength = SubscriptionTypeConsts.MinNameLength)]
        public string Name { get; set; }

        public int? NumberOfDays { get; set; }

    }
}