using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreAccountTeamsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreAccountTeamForViewDto>> GetAll(GetAllStoreAccountTeamsInput input);

        Task<GetStoreAccountTeamForViewDto> GetStoreAccountTeamForView(long id);

        Task<GetStoreAccountTeamForEditOutput> GetStoreAccountTeamForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreAccountTeamDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreAccountTeamsToExcel(GetAllStoreAccountTeamsForExcelInput input);

        Task<PagedResultDto<StoreAccountTeamStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreAccountTeamEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input);

    }
}