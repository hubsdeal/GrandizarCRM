using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;

namespace SoftGrid.LookupData
{
    public interface ICitiesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCityForViewDto>> GetAll(GetAllCitiesInput input);

        Task<GetCityForViewDto> GetCityForView(long id);

        Task<GetCityForEditOutput> GetCityForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCityDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetCitiesToExcel(GetAllCitiesForExcelInput input);

        Task<List<CityCountryLookupTableDto>> GetAllCountryForTableDropdown();

        Task<List<CityStateLookupTableDto>> GetAllStateForTableDropdown();

        Task<List<CityCountyLookupTableDto>> GetAllCountyForTableDropdown();

    }
}