using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.WidgetManagement
{
    public interface IMasterWidgetsAppService : IApplicationService
    {
        Task<PagedResultDto<GetMasterWidgetForViewDto>> GetAll(GetAllMasterWidgetsInput input);

        Task<GetMasterWidgetForViewDto> GetMasterWidgetForView(long id);

        Task<GetMasterWidgetForEditOutput> GetMasterWidgetForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditMasterWidgetDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetMasterWidgetsToExcel(GetAllMasterWidgetsForExcelInput input);

    }
}