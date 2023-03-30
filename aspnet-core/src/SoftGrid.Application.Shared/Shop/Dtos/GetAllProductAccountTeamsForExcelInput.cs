using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductAccountTeamsForExcelInput
    {
        public string Filter { get; set; }

        public int? PrimaryFilter { get; set; }

        public int? ActiveFilter { get; set; }

        public DateTime? MaxRemoveDateFilter { get; set; }
        public DateTime? MinRemoveDateFilter { get; set; }

        public DateTime? MaxAssignDateFilter { get; set; }
        public DateTime? MinAssignDateFilter { get; set; }

        public string EmployeeNameFilter { get; set; }

        public string ProductNameFilter { get; set; }

    }
}