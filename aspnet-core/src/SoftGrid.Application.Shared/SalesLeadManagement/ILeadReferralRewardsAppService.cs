using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.SalesLeadManagement
{
    public interface ILeadReferralRewardsAppService : IApplicationService
    {
        Task<PagedResultDto<GetLeadReferralRewardForViewDto>> GetAll(GetAllLeadReferralRewardsInput input);

        Task<GetLeadReferralRewardForViewDto> GetLeadReferralRewardForView(long id);

        Task<GetLeadReferralRewardForEditOutput> GetLeadReferralRewardForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditLeadReferralRewardDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetLeadReferralRewardsToExcel(GetAllLeadReferralRewardsForExcelInput input);

        Task<PagedResultDto<LeadReferralRewardLeadLookupTableDto>> GetAllLeadForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<LeadReferralRewardContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

    }
}