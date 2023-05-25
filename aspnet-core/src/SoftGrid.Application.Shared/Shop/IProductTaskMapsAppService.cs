using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductTaskMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductTaskMapForViewDto>> GetAll(GetAllProductTaskMapsInput input);

        Task<GetProductTaskMapForViewDto> GetProductTaskMapForView(long id);

        Task<GetProductTaskMapForEditOutput> GetProductTaskMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductTaskMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductTaskMapsToExcel(GetAllProductTaskMapsForExcelInput input);

        Task<PagedResultDto<ProductTaskMapProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductTaskMapTaskEventLookupTableDto>> GetAllTaskEventForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductTaskMapProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input);

    }
}