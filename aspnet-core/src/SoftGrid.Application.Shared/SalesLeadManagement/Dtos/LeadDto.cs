using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class LeadDto : EntityDto<long>
    {
        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Company { get; set; }

        public string JobTitle { get; set; }

        public string Industry { get; set; }

        public int? LeadScore { get; set; }

        public double? ExpectedSalesAmount { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ExpectedClosingDate { get; set; }

        public long? ContactId { get; set; }

        public long? BusinessId { get; set; }

        public long? ProductId { get; set; }

        public long? ProductCategoryId { get; set; }

        public long? StoreId { get; set; }

        public long? EmployeeId { get; set; }

        public long? LeadSourceId { get; set; }

        public long? LeadPipelineStageId { get; set; }

        public long? HubId { get; set; }

    }
}