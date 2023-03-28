using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Territory
{
    public interface IHubBusinessesAppService : IApplicationService
    {
        Task<PagedResultDto<GetHubBusinessForViewDto>> GetAll(GetAllHubBusinessesInput input);

        Task<GetHubBusinessForViewDto> GetHubBusinessForView(long id);

        Task<GetHubBusinessForEditOutput> GetHubBusinessForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHubBusinessDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetHubBusinessesToExcel(GetAllHubBusinessesForExcelInput input);

        Task<PagedResultDto<HubBusinessHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HubBusinessBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input);

    }
}