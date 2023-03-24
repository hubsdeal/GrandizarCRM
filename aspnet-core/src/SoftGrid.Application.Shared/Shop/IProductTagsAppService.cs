using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductTagsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductTagForViewDto>> GetAll(GetAllProductTagsInput input);

        Task<GetProductTagForViewDto> GetProductTagForView(long id);

        Task<GetProductTagForEditOutput> GetProductTagForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductTagDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductTagsToExcel(GetAllProductTagsForExcelInput input);

        Task<PagedResultDto<ProductTagProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductTagMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductTagMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input);

    }
}