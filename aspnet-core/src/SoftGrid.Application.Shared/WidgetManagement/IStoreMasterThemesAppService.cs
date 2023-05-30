using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.WidgetManagement
{
    public interface IStoreMasterThemesAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreMasterThemeForViewDto>> GetAll(GetAllStoreMasterThemesInput input);

        Task<GetStoreMasterThemeForViewDto> GetStoreMasterThemeForView(long id);

        Task<GetStoreMasterThemeForEditOutput> GetStoreMasterThemeForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreMasterThemeDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreMasterThemesToExcel(GetAllStoreMasterThemesForExcelInput input);

    }
}