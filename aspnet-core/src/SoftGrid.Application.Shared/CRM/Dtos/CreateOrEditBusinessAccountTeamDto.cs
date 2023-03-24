using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class CreateOrEditBusinessAccountTeamDto : EntityDto<long?>
    {

        public bool Primary { get; set; }

        public long BusinessId { get; set; }

        public long EmployeeId { get; set; }

    }
}