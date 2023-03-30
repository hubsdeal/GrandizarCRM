using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Territory.Dtos
{
    public class GetHubAccountTeamForEditOutput
    {
        public CreateOrEditHubAccountTeamDto HubAccountTeam { get; set; }

        public string HubName { get; set; }

        public string EmployeeName { get; set; }

        public string UserName { get; set; }

    }
}