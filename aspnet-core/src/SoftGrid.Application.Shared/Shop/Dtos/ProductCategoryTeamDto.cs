using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductCategoryTeamDto : EntityDto<long>
    {
        public bool Primary { get; set; }

        public long ProductCategoryId { get; set; }

        public long EmployeeId { get; set; }

    }
}