using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.JobManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.JobManagement
{
    public interface IJobStatusTypesAppService : IApplicationService
    {
        Task<PagedResultDto<GetJobStatusTypeForViewDto>> GetAll(GetAllJobStatusTypesInput input);

        Task<GetJobStatusTypeForViewDto> GetJobStatusTypeForView(long id);

        Task<GetJobStatusTypeForEditOutput> GetJobStatusTypeForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditJobStatusTypeDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetJobStatusTypesToExcel(GetAllJobStatusTypesForExcelInput input);

    }
}