using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.SalesLeadManagement.Dtos
{
    public class GetLeadReferralRewardForEditOutput
    {
        public CreateOrEditLeadReferralRewardDto LeadReferralReward { get; set; }

        public string LeadTitle { get; set; }

        public string ContactFullName { get; set; }

    }
}