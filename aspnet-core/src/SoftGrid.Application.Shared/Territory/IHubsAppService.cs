using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;

namespace SoftGrid.Territory
{
    public interface IHubsAppService : IApplicationService
    {
        Task<PagedResultDto<GetHubForViewDto>> GetAll(GetAllHubsInput input);

        Task<GetHubForViewDto> GetHubForView(long id);

        Task<GetHubForEditOutput> GetHubForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHubDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetHubsToExcel(GetAllHubsForExcelInput input);

        Task<List<HubCountryLookupTableDto>> GetAllCountryForTableDropdown();

        Task<List<HubStateLookupTableDto>> GetAllStateForTableDropdown();

        Task<List<HubCityLookupTableDto>> GetAllCityForTableDropdown();

        Task<List<HubCountyLookupTableDto>> GetAllCountyForTableDropdown();

        Task<List<HubHubTypeLookupTableDto>> GetAllHubTypeForTableDropdown();

        Task<List<HubCurrencyLookupTableDto>> GetAllCurrencyForTableDropdown();

    }
}