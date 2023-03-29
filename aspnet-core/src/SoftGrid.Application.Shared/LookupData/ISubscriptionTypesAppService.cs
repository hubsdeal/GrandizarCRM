using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData
{
    public interface ISubscriptionTypesAppService : IApplicationService
    {
        Task<PagedResultDto<GetSubscriptionTypeForViewDto>> GetAll(GetAllSubscriptionTypesInput input);

        Task<GetSubscriptionTypeForViewDto> GetSubscriptionTypeForView(long id);

        Task<GetSubscriptionTypeForEditOutput> GetSubscriptionTypeForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditSubscriptionTypeDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetSubscriptionTypesToExcel(GetAllSubscriptionTypesForExcelInput input);

    }
}