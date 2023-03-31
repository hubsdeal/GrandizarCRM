using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.OrderManagement.Dtos
{
    public class CreateOrEditOrderFulfillmentStatusDto : EntityDto<long?>
    {

        public DateTime? EstimatedTime { get; set; }

        public DateTime? ActualTime { get; set; }

        public long? OrderStatusId { get; set; }

        public long? OrderId { get; set; }

        public long? EmployeeId { get; set; }

    }
}