using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreZipCodeMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreZipCodeMapForViewDto>> GetAll(GetAllStoreZipCodeMapsInput input);

        Task<GetStoreZipCodeMapForViewDto> GetStoreZipCodeMapForView(long id);

        Task<GetStoreZipCodeMapForEditOutput> GetStoreZipCodeMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreZipCodeMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreZipCodeMapsToExcel(GetAllStoreZipCodeMapsForExcelInput input);

        Task<PagedResultDto<StoreZipCodeMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreZipCodeMapZipCodeLookupTableDto>> GetAllZipCodeForLookupTable(GetAllForLookupTableInput input);

    }
}