using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.LookupData.Dtos
{
    public class GetAllSubscriptionTypesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public int? MaxNumberOfDaysFilter { get; set; }
        public int? MinNumberOfDaysFilter { get; set; }

    }
}