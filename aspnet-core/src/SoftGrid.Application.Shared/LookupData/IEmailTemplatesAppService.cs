using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData
{
    public interface IEmailTemplatesAppService : IApplicationService
    {
        Task<PagedResultDto<GetEmailTemplateForViewDto>> GetAll(GetAllEmailTemplatesInput input);

        Task<GetEmailTemplateForViewDto> GetEmailTemplateForView(long id);

        Task<GetEmailTemplateForEditOutput> GetEmailTemplateForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditEmailTemplateDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetEmailTemplatesToExcel(GetAllEmailTemplatesForExcelInput input);

    }
}