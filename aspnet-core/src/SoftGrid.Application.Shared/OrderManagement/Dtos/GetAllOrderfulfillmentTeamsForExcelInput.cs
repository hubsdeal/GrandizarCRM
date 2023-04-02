using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.OrderManagement.Dtos
{
    public class GetAllOrderfulfillmentTeamsForExcelInput
    {
        public string Filter { get; set; }

        public string OrderFullNameFilter { get; set; }

        public string EmployeeNameFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

        public string UserNameFilter { get; set; }

    }
}