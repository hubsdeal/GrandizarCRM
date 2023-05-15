using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.CMS.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CMS
{
    public interface IContentsAppService : IApplicationService
    {
        Task<PagedResultDto<GetContentForViewDto>> GetAll(GetAllContentsInput input);

        Task<GetContentForViewDto> GetContentForView(long id);

        Task<GetContentForEditOutput> GetContentForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditContentDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetContentsToExcel(GetAllContentsForExcelInput input);

        Task<PagedResultDto<ContentMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input);

    }
}