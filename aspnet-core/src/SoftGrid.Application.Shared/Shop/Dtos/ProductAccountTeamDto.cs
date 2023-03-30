using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductAccountTeamDto : EntityDto<long>
    {
        public bool Primary { get; set; }

        public bool Active { get; set; }

        public DateTime? RemoveDate { get; set; }

        public DateTime? AssignDate { get; set; }

        public long EmployeeId { get; set; }

        public long ProductId { get; set; }

    }
}