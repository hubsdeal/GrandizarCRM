using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.TaskManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.TaskManagement
{
    public interface IStoreTaskMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreTaskMapForViewDto>> GetAll(GetAllStoreTaskMapsInput input);

        Task<GetStoreTaskMapForViewDto> GetStoreTaskMapForView(long id);

        Task<GetStoreTaskMapForEditOutput> GetStoreTaskMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreTaskMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreTaskMapsToExcel(GetAllStoreTaskMapsForExcelInput input);

        Task<PagedResultDto<StoreTaskMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreTaskMapTaskEventLookupTableDto>> GetAllTaskEventForLookupTable(GetAllForLookupTableInput input);

    }
}