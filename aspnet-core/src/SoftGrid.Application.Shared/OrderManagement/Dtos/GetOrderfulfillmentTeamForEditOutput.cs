using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.OrderManagement.Dtos
{
    public class GetOrderfulfillmentTeamForEditOutput
    {
        public CreateOrEditOrderfulfillmentTeamDto OrderfulfillmentTeam { get; set; }

        public string OrderFullName { get; set; }

        public string EmployeeName { get; set; }

        public string ContactFullName { get; set; }

        public string UserName { get; set; }

    }
}