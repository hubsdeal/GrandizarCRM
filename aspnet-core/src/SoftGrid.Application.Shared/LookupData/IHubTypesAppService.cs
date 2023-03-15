using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData
{
    public interface IHubTypesAppService : IApplicationService
    {
        Task<PagedResultDto<GetHubTypeForViewDto>> GetAll(GetAllHubTypesInput input);

        Task<GetHubTypeForViewDto> GetHubTypeForView(long id);

        Task<GetHubTypeForEditOutput> GetHubTypeForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHubTypeDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetHubTypesToExcel(GetAllHubTypesForExcelInput input);

    }
}