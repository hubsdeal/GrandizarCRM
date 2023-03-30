using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Territory.Dtos
{
    public class HubAccountTeamDto : EntityDto<long>
    {
        public bool PrimaryManager { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public long HubId { get; set; }

        public long? EmployeeId { get; set; }

        public long? UserId { get; set; }

    }
}