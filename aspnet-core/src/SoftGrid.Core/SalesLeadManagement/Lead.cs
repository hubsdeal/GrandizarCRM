using SoftGrid.CRM;
using SoftGrid.CRM;
using SoftGrid.Shop;
using SoftGrid.Shop;
using SoftGrid.Shop;
using SoftGrid.CRM;
using SoftGrid.SalesLeadManagement;
using SoftGrid.SalesLeadManagement;
using SoftGrid.Territory;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.SalesLeadManagement
{
    [Table("Leads")]
    public class Lead : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(LeadConsts.MaxTitleLength, MinimumLength = LeadConsts.MinTitleLength)]
        public virtual string Title { get; set; }

        [Required]
        [StringLength(LeadConsts.MaxFirstNameLength, MinimumLength = LeadConsts.MinFirstNameLength)]
        public virtual string FirstName { get; set; }

        [StringLength(LeadConsts.MaxLastNameLength, MinimumLength = LeadConsts.MinLastNameLength)]
        public virtual string LastName { get; set; }

        [StringLength(LeadConsts.MaxEmailLength, MinimumLength = LeadConsts.MinEmailLength)]
        public virtual string Email { get; set; }

        [StringLength(LeadConsts.MaxPhoneLength, MinimumLength = LeadConsts.MinPhoneLength)]
        public virtual string Phone { get; set; }

        [StringLength(LeadConsts.MaxCompanyLength, MinimumLength = LeadConsts.MinCompanyLength)]
        public virtual string Company { get; set; }

        [StringLength(LeadConsts.MaxJobTitleLength, MinimumLength = LeadConsts.MinJobTitleLength)]
        public virtual string JobTitle { get; set; }

        [StringLength(LeadConsts.MaxIndustryLength, MinimumLength = LeadConsts.MinIndustryLength)]
        public virtual string Industry { get; set; }

        public virtual int? LeadScore { get; set; }

        public virtual double? ExpectedSalesAmount { get; set; }

        public virtual DateTime? CreatedDate { get; set; }

        public virtual DateTime? ExpectedClosingDate { get; set; }

        public virtual long? ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact ContactFk { get; set; }

        public virtual long? BusinessId { get; set; }

        [ForeignKey("BusinessId")]
        public Business BusinessFk { get; set; }

        public virtual long? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

        public virtual long? ProductCategoryId { get; set; }

        [ForeignKey("ProductCategoryId")]
        public ProductCategory ProductCategoryFk { get; set; }

        public virtual long? StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

        public virtual long? EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee EmployeeFk { get; set; }

        public virtual long? LeadSourceId { get; set; }

        [ForeignKey("LeadSourceId")]
        public LeadSource LeadSourceFk { get; set; }

        public virtual long? LeadPipelineStageId { get; set; }

        [ForeignKey("LeadPipelineStageId")]
        public LeadPipelineStage LeadPipelineStageFk { get; set; }

        public virtual long? HubId { get; set; }

        [ForeignKey("HubId")]
        public Hub HubFk { get; set; }

    }
}