using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class GetBusinessAccountTeamForEditOutput
    {
        public CreateOrEditBusinessAccountTeamDto BusinessAccountTeam { get; set; }

        public string BusinessName { get; set; }

        public string EmployeeName { get; set; }

    }
}