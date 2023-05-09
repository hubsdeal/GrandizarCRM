using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductTeamForEditOutput
    {
        public CreateOrEditProductTeamDto ProductTeam { get; set; }

        public string EmployeeName { get; set; }

        public string ProductName { get; set; }

    }
}