using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.TaskManagement.Dtos
{
    public class GetTaskTeamForEditOutput
    {
        public CreateOrEditTaskTeamDto TaskTeam { get; set; }

        public string TaskEventName { get; set; }

        public string EmployeeName { get; set; }

        public string ContactFullName { get; set; }

    }
}