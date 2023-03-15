using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;

namespace SoftGrid.LookupData
{
    public interface IZipCodesAppService : IApplicationService
    {
        Task<PagedResultDto<GetZipCodeForViewDto>> GetAll(GetAllZipCodesInput input);

        Task<GetZipCodeForViewDto> GetZipCodeForView(long id);

        Task<GetZipCodeForEditOutput> GetZipCodeForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditZipCodeDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetZipCodesToExcel(GetAllZipCodesForExcelInput input);

        Task<List<ZipCodeCountryLookupTableDto>> GetAllCountryForTableDropdown();

        Task<List<ZipCodeStateLookupTableDto>> GetAllStateForTableDropdown();

        Task<List<ZipCodeCityLookupTableDto>> GetAllCityForTableDropdown();

        Task<List<ZipCodeCountyLookupTableDto>> GetAllCountyForTableDropdown();

    }
}