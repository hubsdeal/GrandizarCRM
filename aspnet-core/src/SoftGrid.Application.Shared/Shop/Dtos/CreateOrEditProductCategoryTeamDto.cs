using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductCategoryTeamDto : EntityDto<long?>
    {

        public bool Primary { get; set; }

        public long ProductCategoryId { get; set; }

        public long EmployeeId { get; set; }

    }
}