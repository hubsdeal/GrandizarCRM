using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.OrderManagement.Dtos
{
    public class GetAllOrderStatusesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? MaxSequenceNoFilter { get; set; }
        public int? MinSequenceNoFilter { get; set; }

        public string ColorCodeFilter { get; set; }

        public string MessageFilter { get; set; }

        public int? DeliveryOrPickupFilter { get; set; }

        public string RoleNameFilter { get; set; }

    }
}