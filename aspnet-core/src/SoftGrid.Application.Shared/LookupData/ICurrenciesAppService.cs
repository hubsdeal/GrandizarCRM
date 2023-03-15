using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData
{
    public interface ICurrenciesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCurrencyForViewDto>> GetAll(GetAllCurrenciesInput input);

        Task<GetCurrencyForViewDto> GetCurrencyForView(long id);

        Task<GetCurrencyForEditOutput> GetCurrencyForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCurrencyDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetCurrenciesToExcel(GetAllCurrenciesForExcelInput input);

    }
}