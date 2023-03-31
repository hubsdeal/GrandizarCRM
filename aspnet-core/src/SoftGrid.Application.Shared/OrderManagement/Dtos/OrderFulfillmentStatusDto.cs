using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.OrderManagement.Dtos
{
    public class OrderFulfillmentStatusDto : EntityDto<long>
    {
        public DateTime? EstimatedTime { get; set; }

        public DateTime? ActualTime { get; set; }

        public long? OrderStatusId { get; set; }

        public long? OrderId { get; set; }

        public long? EmployeeId { get; set; }

    }
}