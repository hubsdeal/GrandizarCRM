using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetAllLeadsForExcelInput
    {
        public string Filter { get; set; }

        public string TitleFilter { get; set; }

        public string FirstNameFilter { get; set; }

        public string LastNameFilter { get; set; }

        public string EmailFilter { get; set; }

        public string PhoneFilter { get; set; }

        public string CompanyFilter { get; set; }

        public string JobTitleFilter { get; set; }

        public string IndustryFilter { get; set; }

        public int? MaxLeadScoreFilter { get; set; }
        public int? MinLeadScoreFilter { get; set; }

        public double? MaxExpectedSalesAmountFilter { get; set; }
        public double? MinExpectedSalesAmountFilter { get; set; }

        public DateTime? MaxCreatedDateFilter { get; set; }
        public DateTime? MinCreatedDateFilter { get; set; }

        public DateTime? MaxExpectedClosingDateFilter { get; set; }
        public DateTime? MinExpectedClosingDateFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

        public string BusinessNameFilter { get; set; }

        public string ProductNameFilter { get; set; }

        public string ProductCategoryNameFilter { get; set; }

        public string StoreNameFilter { get; set; }

        public string EmployeeNameFilter { get; set; }

        public string LeadSourceNameFilter { get; set; }

        public string LeadPipelineStageNameFilter { get; set; }

        public string HubNameFilter { get; set; }

    }
}