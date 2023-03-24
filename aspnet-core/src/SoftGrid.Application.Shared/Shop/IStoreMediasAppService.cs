using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreMediasAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreMediaForViewDto>> GetAll(GetAllStoreMediasInput input);

        Task<GetStoreMediaForViewDto> GetStoreMediaForView(long id);

        Task<GetStoreMediaForEditOutput> GetStoreMediaForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreMediaDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreMediasToExcel(GetAllStoreMediasForExcelInput input);

        Task<PagedResultDto<StoreMediaStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreMediaMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input);

    }
}