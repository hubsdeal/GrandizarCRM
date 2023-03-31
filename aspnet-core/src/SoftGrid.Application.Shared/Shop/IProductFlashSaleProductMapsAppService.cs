using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductFlashSaleProductMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductFlashSaleProductMapForViewDto>> GetAll(GetAllProductFlashSaleProductMapsInput input);

        Task<GetProductFlashSaleProductMapForViewDto> GetProductFlashSaleProductMapForView(long id);

        Task<GetProductFlashSaleProductMapForEditOutput> GetProductFlashSaleProductMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductFlashSaleProductMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductFlashSaleProductMapsToExcel(GetAllProductFlashSaleProductMapsForExcelInput input);

        Task<PagedResultDto<ProductFlashSaleProductMapProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductFlashSaleProductMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductFlashSaleProductMapMembershipTypeLookupTableDto>> GetAllMembershipTypeForLookupTable(GetAllForLookupTableInput input);

    }
}