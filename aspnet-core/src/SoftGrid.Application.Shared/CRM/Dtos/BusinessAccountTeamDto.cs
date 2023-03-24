using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.CRM.Dtos
{
    public class BusinessAccountTeamDto : EntityDto<long>
    {
        public bool Primary { get; set; }

        public long BusinessId { get; set; }

        public long EmployeeId { get; set; }

    }
}