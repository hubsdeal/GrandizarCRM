using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductTeamDto : EntityDto<long>
    {
        public bool Primary { get; set; }

        public bool Active { get; set; }

        public long? EmployeeId { get; set; }

        public long ProductId { get; set; }

    }
}