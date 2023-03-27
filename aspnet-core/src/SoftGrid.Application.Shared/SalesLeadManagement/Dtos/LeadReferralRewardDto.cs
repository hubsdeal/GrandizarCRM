using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class LeadReferralRewardDto : EntityDto<long>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string RewardType { get; set; }

        public double? RewardAmount { get; set; }

        public bool RewardStatus { get; set; }

        public long LeadId { get; set; }

        public long? ContactId { get; set; }

    }
}