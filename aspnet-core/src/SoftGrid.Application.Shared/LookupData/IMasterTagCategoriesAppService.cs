using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData
{
    public interface IMasterTagCategoriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetMasterTagCategoryForViewDto>> GetAll(GetAllMasterTagCategoriesInput input);

        Task<GetMasterTagCategoryForViewDto> GetMasterTagCategoryForView(long id);

        Task<GetMasterTagCategoryForEditOutput> GetMasterTagCategoryForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditMasterTagCategoryDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetMasterTagCategoriesToExcel(GetAllMasterTagCategoriesForExcelInput input);

    }
}