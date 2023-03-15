using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using System.Collections.Generic;
using System.Collections.Generic;

namespace SoftGrid.LookupData
{
    public interface ICountiesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCountyForViewDto>> GetAll(GetAllCountiesInput input);

        Task<GetCountyForViewDto> GetCountyForView(long id);

        Task<GetCountyForEditOutput> GetCountyForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCountyDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetCountiesToExcel(GetAllCountiesForExcelInput input);

        Task<List<CountyCountryLookupTableDto>> GetAllCountryForTableDropdown();

        Task<List<CountyStateLookupTableDto>> GetAllStateForTableDropdown();

    }
}