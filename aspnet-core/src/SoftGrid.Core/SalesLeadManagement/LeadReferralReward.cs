using SoftGrid.SalesLeadManagement;
using SoftGrid.CRM;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.SalesLeadManagement
{
    [Table("LeadReferralRewards")]
    public class LeadReferralReward : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [StringLength(LeadReferralRewardConsts.MaxFirstNameLength, MinimumLength = LeadReferralRewardConsts.MinFirstNameLength)]
        public virtual string FirstName { get; set; }

        [StringLength(LeadReferralRewardConsts.MaxLastNameLength, MinimumLength = LeadReferralRewardConsts.MinLastNameLength)]
        public virtual string LastName { get; set; }

        [StringLength(LeadReferralRewardConsts.MaxPhoneLength, MinimumLength = LeadReferralRewardConsts.MinPhoneLength)]
        public virtual string Phone { get; set; }

        [StringLength(LeadReferralRewardConsts.MaxEmailLength, MinimumLength = LeadReferralRewardConsts.MinEmailLength)]
        public virtual string Email { get; set; }

        [StringLength(LeadReferralRewardConsts.MaxRewardTypeLength, MinimumLength = LeadReferralRewardConsts.MinRewardTypeLength)]
        public virtual string RewardType { get; set; }

        public virtual double? RewardAmount { get; set; }

        public virtual bool RewardStatus { get; set; }

        public virtual long LeadId { get; set; }

        [ForeignKey("LeadId")]
        public Lead LeadFk { get; set; }

        public virtual long? ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact ContactFk { get; set; }

    }
}