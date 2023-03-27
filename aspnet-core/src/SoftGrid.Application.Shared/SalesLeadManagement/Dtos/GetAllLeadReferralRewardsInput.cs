using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetAllLeadReferralRewardsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string FirstNameFilter { get; set; }

        public string LastNameFilter { get; set; }

        public string PhoneFilter { get; set; }

        public string EmailFilter { get; set; }

        public string RewardTypeFilter { get; set; }

        public double? MaxRewardAmountFilter { get; set; }
        public double? MinRewardAmountFilter { get; set; }

        public int? RewardStatusFilter { get; set; }

        public string LeadTitleFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

    }
}