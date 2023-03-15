using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData
{
    public interface ICountriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCountryForViewDto>> GetAll(GetAllCountriesInput input);

        Task<GetCountryForViewDto> GetCountryForView(long id);

        Task<GetCountryForEditOutput> GetCountryForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCountryDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetCountriesToExcel(GetAllCountriesForExcelInput input);

    }
}