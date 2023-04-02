using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.OrderManagement.Dtos
{
    public class GetAllOrderTeamsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string OrderInvoiceNumberFilter { get; set; }

        public string EmployeeNameFilter { get; set; }

    }
}