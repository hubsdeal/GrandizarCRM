using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Territory
{
    public interface IHubProductsAppService : IApplicationService
    {
        Task<PagedResultDto<GetHubProductForViewDto>> GetAll(GetAllHubProductsInput input);

        Task<GetHubProductForViewDto> GetHubProductForView(long id);

        Task<GetHubProductForEditOutput> GetHubProductForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHubProductDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetHubProductsToExcel(GetAllHubProductsForExcelInput input);

        Task<PagedResultDto<HubProductHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HubProductProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

    }
}