using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.SalesLeadManagement
{
    public interface ILeadPipelineStagesAppService : IApplicationService
    {
        Task<PagedResultDto<GetLeadPipelineStageForViewDto>> GetAll(GetAllLeadPipelineStagesInput input);

        Task<GetLeadPipelineStageForViewDto> GetLeadPipelineStageForView(long id);

        Task<GetLeadPipelineStageForEditOutput> GetLeadPipelineStageForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditLeadPipelineStageDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetLeadPipelineStagesToExcel(GetAllLeadPipelineStagesForExcelInput input);

    }
}