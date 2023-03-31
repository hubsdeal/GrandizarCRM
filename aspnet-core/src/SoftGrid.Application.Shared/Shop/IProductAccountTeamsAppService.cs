using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductAccountTeamsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductAccountTeamForViewDto>> GetAll(GetAllProductAccountTeamsInput input);

        Task<GetProductAccountTeamForViewDto> GetProductAccountTeamForView(long id);

        Task<GetProductAccountTeamForEditOutput> GetProductAccountTeamForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductAccountTeamDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductAccountTeamsToExcel(GetAllProductAccountTeamsForExcelInput input);

        Task<PagedResultDto<ProductAccountTeamEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductAccountTeamProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

    }
}