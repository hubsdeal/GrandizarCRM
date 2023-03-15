using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductCategoriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductCategoryForViewDto>> GetAll(GetAllProductCategoriesInput input);

        Task<GetProductCategoryForViewDto> GetProductCategoryForView(long id);

        Task<GetProductCategoryForEditOutput> GetProductCategoryForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductCategoryDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductCategoriesToExcel(GetAllProductCategoriesForExcelInput input);

        Task<PagedResultDto<ProductCategoryMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input);

    }
}