using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Territory
{
    public interface IHubProductCategoriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetHubProductCategoryForViewDto>> GetAll(GetAllHubProductCategoriesInput input);

        Task<GetHubProductCategoryForViewDto> GetHubProductCategoryForView(long id);

        Task<GetHubProductCategoryForEditOutput> GetHubProductCategoryForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditHubProductCategoryDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetHubProductCategoriesToExcel(GetAllHubProductCategoriesForExcelInput input);

        Task<PagedResultDto<HubProductCategoryHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<HubProductCategoryProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input);

    }
}