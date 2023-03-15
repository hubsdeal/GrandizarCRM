using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData
{
    public interface IRatingLikesAppService : IApplicationService
    {
        Task<PagedResultDto<GetRatingLikeForViewDto>> GetAll(GetAllRatingLikesInput input);

        Task<GetRatingLikeForViewDto> GetRatingLikeForView(long id);

        Task<GetRatingLikeForEditOutput> GetRatingLikeForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditRatingLikeDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetRatingLikesToExcel(GetAllRatingLikesForExcelInput input);

    }
}