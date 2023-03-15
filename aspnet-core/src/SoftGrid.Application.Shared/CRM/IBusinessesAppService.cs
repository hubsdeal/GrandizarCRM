using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;

namespace SoftGrid.CRM
{
    public interface IBusinessesAppService : IApplicationService
    {
        Task<PagedResultDto<GetBusinessForViewDto>> GetAll(GetAllBusinessesInput input);

        Task<GetBusinessForViewDto> GetBusinessForView(long id);

        Task<GetBusinessForEditOutput> GetBusinessForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditBusinessDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetBusinessesToExcel(GetAllBusinessesForExcelInput input);

        Task<List<BusinessCountryLookupTableDto>> GetAllCountryForTableDropdown();

        Task<List<BusinessStateLookupTableDto>> GetAllStateForTableDropdown();

        Task<List<BusinessCityLookupTableDto>> GetAllCityForTableDropdown();

        Task<PagedResultDto<BusinessMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input);

    }
}