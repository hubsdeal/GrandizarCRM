using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllStoreSalesAlertsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string MessageFilter { get; set; }

        public int? CurrentFilter { get; set; }

        public DateTime? MaxStartDateFilter { get; set; }
        public DateTime? MinStartDateFilter { get; set; }

        public DateTime? MaxEndDateFilter { get; set; }
        public DateTime? MinEndDateFilter { get; set; }

        public string StoreNameFilter { get; set; }

    }
}