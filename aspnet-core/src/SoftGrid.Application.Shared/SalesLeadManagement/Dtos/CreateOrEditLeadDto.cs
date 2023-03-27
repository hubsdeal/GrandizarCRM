using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class CreateOrEditLeadDto : EntityDto<long?>
    {

        [Required]
        [StringLength(LeadConsts.MaxTitleLength, MinimumLength = LeadConsts.MinTitleLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(LeadConsts.MaxFirstNameLength, MinimumLength = LeadConsts.MinFirstNameLength)]
        public string FirstName { get; set; }

        [StringLength(LeadConsts.MaxLastNameLength, MinimumLength = LeadConsts.MinLastNameLength)]
        public string LastName { get; set; }

        [StringLength(LeadConsts.MaxEmailLength, MinimumLength = LeadConsts.MinEmailLength)]
        public string Email { get; set; }

        [StringLength(LeadConsts.MaxPhoneLength, MinimumLength = LeadConsts.MinPhoneLength)]
        public string Phone { get; set; }

        [StringLength(LeadConsts.MaxCompanyLength, MinimumLength = LeadConsts.MinCompanyLength)]
        public string Company { get; set; }

        [StringLength(LeadConsts.MaxJobTitleLength, MinimumLength = LeadConsts.MinJobTitleLength)]
        public string JobTitle { get; set; }

        [StringLength(LeadConsts.MaxIndustryLength, MinimumLength = LeadConsts.MinIndustryLength)]
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