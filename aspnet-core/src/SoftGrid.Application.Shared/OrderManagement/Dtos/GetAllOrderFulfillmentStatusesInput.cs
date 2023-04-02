using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.OrderManagement.Dtos
{
    public class GetAllOrderFulfillmentStatusesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public DateTime? MaxEstimatedTimeFilter { get; set; }
        public DateTime? MinEstimatedTimeFilter { get; set; }

        public DateTime? MaxActualTimeFilter { get; set; }
        public DateTime? MinActualTimeFilter { get; set; }

        public string OrderStatusNameFilter { get; set; }

        public string OrderInvoiceNumberFilter { get; set; }

        public string EmployeeNameFilter { get; set; }

    }
}