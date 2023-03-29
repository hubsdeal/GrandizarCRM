using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.LookupData.Dtos
{
    public class SubscriptionTypeDto : EntityDto<long>
    {
        public string Name { get; set; }

        public int? NumberOfDays { get; set; }

    }
}