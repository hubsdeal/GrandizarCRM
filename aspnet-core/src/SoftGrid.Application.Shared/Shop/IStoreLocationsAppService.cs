using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreLocationsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreLocationForViewDto>> GetAll(GetAllStoreLocationsInput input);

        Task<GetStoreLocationForViewDto> GetStoreLocationForView(long id);

        Task<GetStoreLocationForEditOutput> GetStoreLocationForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreLocationDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreLocationsToExcel(GetAllStoreLocationsForExcelInput input);

        Task<PagedResultDto<StoreLocationCityLookupTableDto>> GetAllCityForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreLocationStateLookupTableDto>> GetAllStateForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreLocationCountryLookupTableDto>> GetAllCountryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreLocationStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

    }
}