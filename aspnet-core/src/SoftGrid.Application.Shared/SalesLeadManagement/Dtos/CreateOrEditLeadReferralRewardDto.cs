using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class CreateOrEditLeadReferralRewardDto : EntityDto<long?>
    {

        [StringLength(LeadReferralRewardConsts.MaxFirstNameLength, MinimumLength = LeadReferralRewardConsts.MinFirstNameLength)]
        public string FirstName { get; set; }

        [StringLength(LeadReferralRewardConsts.MaxLastNameLength, MinimumLength = LeadReferralRewardConsts.MinLastNameLength)]
        public string LastName { get; set; }

        [StringLength(LeadReferralRewardConsts.MaxPhoneLength, MinimumLength = LeadReferralRewardConsts.MinPhoneLength)]
        public string Phone { get; set; }

        [StringLength(LeadReferralRewardConsts.MaxEmailLength, MinimumLength = LeadReferralRewardConsts.MinEmailLength)]
        public string Email { get; set; }

        [StringLength(LeadReferralRewardConsts.MaxRewardTypeLength, MinimumLength = LeadReferralRewardConsts.MinRewardTypeLength)]
        public string RewardType { get; set; }

        public double? RewardAmount { get; set; }

        public bool RewardStatus { get; set; }

        public long LeadId { get; set; }

        public long? ContactId { get; set; }

    }
}