using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductCategoryTeamForEditOutput
    {
        public CreateOrEditProductCategoryTeamDto ProductCategoryTeam { get; set; }

        public string ProductCategoryName { get; set; }

        public string EmployeeName { get; set; }

    }
}