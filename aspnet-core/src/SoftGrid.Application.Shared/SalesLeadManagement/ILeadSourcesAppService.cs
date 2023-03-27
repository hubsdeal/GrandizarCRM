using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.SalesLeadManagement
{
    public interface ILeadSourcesAppService : IApplicationService
    {
        Task<PagedResultDto<GetLeadSourceForViewDto>> GetAll(GetAllLeadSourcesInput input);

        Task<GetLeadSourceForViewDto> GetLeadSourceForView(long id);

        Task<GetLeadSourceForEditOutput> GetLeadSourceForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditLeadSourceDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetLeadSourcesToExcel(GetAllLeadSourcesForExcelInput input);

    }
}