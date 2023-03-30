using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductCategoryTeamsForExcelInput
    {
        public string Filter { get; set; }

        public int? PrimaryFilter { get; set; }

        public string ProductCategoryNameFilter { get; set; }

        public string EmployeeNameFilter { get; set; }

    }
}