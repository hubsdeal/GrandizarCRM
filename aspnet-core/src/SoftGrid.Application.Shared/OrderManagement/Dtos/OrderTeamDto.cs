using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.OrderManagement.Dtos
{
    public class OrderTeamDto : EntityDto<long>
    {

        public long? OrderId { get; set; }

        public long? EmployeeId { get; set; }

    }
}