using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductAccountTeamForEditOutput
    {
        public CreateOrEditProductAccountTeamDto ProductAccountTeam { get; set; }

        public string EmployeeName { get; set; }

        public string ProductName { get; set; }

    }
}