using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using System.Collections.Generic;

namespace SoftGrid.LookupData
{
    public interface IStatesAppService : IApplicationService
    {
        Task<PagedResultDto<GetStateForViewDto>> GetAll(GetAllStatesInput input);

        Task<GetStateForViewDto> GetStateForView(long id);

        Task<GetStateForEditOutput> GetStateForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStateDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStatesToExcel(GetAllStatesForExcelInput input);

        Task<List<StateCountryLookupTableDto>> GetAllCountryForTableDropdown();

    }
}