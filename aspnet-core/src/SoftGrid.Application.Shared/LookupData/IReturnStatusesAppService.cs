using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData
{
    public interface IReturnStatusesAppService : IApplicationService
    {
        Task<PagedResultDto<GetReturnStatusForViewDto>> GetAll(GetAllReturnStatusesInput input);

        Task<GetReturnStatusForViewDto> GetReturnStatusForView(long id);

        Task<GetReturnStatusForEditOutput> GetReturnStatusForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditReturnStatusDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetReturnStatusesToExcel(GetAllReturnStatusesForExcelInput input);

    }
}