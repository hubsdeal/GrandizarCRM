using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.OrderManagement.Dtos
{
    public class GetOrderTeamForEditOutput
    {
        public CreateOrEditOrderTeamDto OrderTeam { get; set; }

        public string OrderInvoiceNumber { get; set; }

        public string EmployeeName { get; set; }

    }
}