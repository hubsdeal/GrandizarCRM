using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreOwnerTeamsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreOwnerTeamForViewDto>> GetAll(GetAllStoreOwnerTeamsInput input);

        Task<GetStoreOwnerTeamForViewDto> GetStoreOwnerTeamForView(long id);

        Task<GetStoreOwnerTeamForEditOutput> GetStoreOwnerTeamForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreOwnerTeamDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreOwnerTeamsToExcel(GetAllStoreOwnerTeamsForExcelInput input);

        Task<PagedResultDto<StoreOwnerTeamStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreOwnerTeamUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

    }
}