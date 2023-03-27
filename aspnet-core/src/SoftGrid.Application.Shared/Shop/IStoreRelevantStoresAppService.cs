using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreRelevantStoresAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreRelevantStoreForViewDto>> GetAll(GetAllStoreRelevantStoresInput input);

        Task<GetStoreRelevantStoreForViewDto> GetStoreRelevantStoreForView(long id);

        Task<GetStoreRelevantStoreForEditOutput> GetStoreRelevantStoreForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreRelevantStoreDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreRelevantStoresToExcel(GetAllStoreRelevantStoresForExcelInput input);

        Task<PagedResultDto<StoreRelevantStoreStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

    }
}