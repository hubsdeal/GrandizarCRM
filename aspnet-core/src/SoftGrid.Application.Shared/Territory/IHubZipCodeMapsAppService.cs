using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Territory
{
    public interface IHubZipCodeMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetHubZipCodeMapForViewDto>> GetAll(GetAllHubZipCodeMapsInput input);

        Task<GetHubZipCodeMapForViewDto> GetHubZipCodeMapForView(long id);

        Task<GetHubZipCodeMapForEditOutput> GetHubZipCodeMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHubZipCodeMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetHubZipCodeMapsToExcel(GetAllHubZipCodeMapsForExcelInput input);

        Task<PagedResultDto<HubZipCodeMapHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HubZipCodeMapCityLookupTableDto>> GetAllCityForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HubZipCodeMapZipCodeLookupTableDto>> GetAllZipCodeForLookupTable(GetAllForLookupTableInput input);

    }
}