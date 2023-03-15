using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData
{
    public interface ISmsTemplatesAppService : IApplicationService
    {
        Task<PagedResultDto<GetSmsTemplateForViewDto>> GetAll(GetAllSmsTemplatesInput input);

        Task<GetSmsTemplateForViewDto> GetSmsTemplateForView(long id);

        Task<GetSmsTemplateForEditOutput> GetSmsTemplateForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditSmsTemplateDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetSmsTemplatesToExcel(GetAllSmsTemplatesForExcelInput input);

    }
}