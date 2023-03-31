using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductAccountTeamDto : EntityDto<long?>
    {

        public bool Primary { get; set; }

        public bool Active { get; set; }

        public DateTime? RemoveDate { get; set; }

        public DateTime? AssignDate { get; set; }

        public long EmployeeId { get; set; }

        public long ProductId { get; set; }

    }
}