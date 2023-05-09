using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductTeamsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductTeamForViewDto>> GetAll(GetAllProductTeamsInput input);

        Task<GetProductTeamForViewDto> GetProductTeamForView(long id);

        Task<GetProductTeamForEditOutput> GetProductTeamForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductTeamDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductTeamsToExcel(GetAllProductTeamsForExcelInput input);

        Task<PagedResultDto<ProductTeamEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductTeamProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

    }
}