using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductTeamDto : EntityDto<long?>
    {

        public bool Primary { get; set; }

        public bool Active { get; set; }

        public long? EmployeeId { get; set; }

        public long ProductId { get; set; }

    }
}