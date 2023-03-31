using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.OrderManagement.Dtos
{
    public class CreateOrEditOrderTeamDto : EntityDto<long?>
    {

        public long? OrderId { get; set; }

        public long? EmployeeId { get; set; }

    }
}