using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData
{
    public interface ISocialMediasAppService : IApplicationService
    {
        Task<PagedResultDto<GetSocialMediaForViewDto>> GetAll(GetAllSocialMediasInput input);

        Task<GetSocialMediaForViewDto> GetSocialMediaForView(long id);

        Task<GetSocialMediaForEditOutput> GetSocialMediaForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditSocialMediaDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetSocialMediasToExcel(GetAllSocialMediasForExcelInput input);

    }
}