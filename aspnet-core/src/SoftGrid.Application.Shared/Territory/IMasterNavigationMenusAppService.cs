using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Territory
{
    public interface IMasterNavigationMenusAppService : IApplicationService
    {
        Task<PagedResultDto<GetMasterNavigationMenuForViewDto>> GetAll(GetAllMasterNavigationMenusInput input);

        Task<GetMasterNavigationMenuForViewDto> GetMasterNavigationMenuForView(long id);

        Task<GetMasterNavigationMenuForEditOutput> GetMasterNavigationMenuForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditMasterNavigationMenuDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetMasterNavigationMenusToExcel(GetAllMasterNavigationMenusForExcelInput input);

    }
}