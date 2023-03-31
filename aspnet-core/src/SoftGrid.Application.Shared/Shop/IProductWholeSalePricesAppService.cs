using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductWholeSalePricesAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductWholeSalePriceForViewDto>> GetAll(GetAllProductWholeSalePricesInput input);

        Task<GetProductWholeSalePriceForViewDto> GetProductWholeSalePriceForView(long id);

        Task<GetProductWholeSalePriceForEditOutput> GetProductWholeSalePriceForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductWholeSalePriceDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductWholeSalePricesToExcel(GetAllProductWholeSalePricesForExcelInput input);

        Task<PagedResultDto<ProductWholeSalePriceProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductWholeSalePriceProductWholeSaleQuantityTypeLookupTableDto>> GetAllProductWholeSaleQuantityTypeForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductWholeSalePriceMeasurementUnitLookupTableDto>> GetAllMeasurementUnitForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductWholeSalePriceCurrencyLookupTableDto>> GetAllCurrencyForLookupTable(GetAllForLookupTableInput input);

    }
}