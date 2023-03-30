using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Territory
{
    public interface IHubStoresAppService : IApplicationService
    {
        Task<PagedResultDto<GetHubStoreForViewDto>> GetAll(GetAllHubStoresInput input);

        Task<GetHubStoreForViewDto> GetHubStoreForView(long id);

        Task<GetHubStoreForEditOutput> GetHubStoreForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHubStoreDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetHubStoresToExcel(GetAllHubStoresForExcelInput input);

        Task<PagedResultDto<HubStoreHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HubStoreStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

    }
}