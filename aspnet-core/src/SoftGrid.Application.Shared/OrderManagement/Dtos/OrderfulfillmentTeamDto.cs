using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.OrderManagement.Dtos
{
    public class OrderfulfillmentTeamDto : EntityDto<long>
    {

        public long? OrderId { get; set; }

        public long? EmployeeId { get; set; }

        public long? ContactId { get; set; }

        public long? UserId { get; set; }

    }
}