using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductSubscriptionMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductSubscriptionMapForViewDto>> GetAll(GetAllProductSubscriptionMapsInput input);

        Task<GetProductSubscriptionMapForViewDto> GetProductSubscriptionMapForView(long id);

        Task<GetProductSubscriptionMapForEditOutput> GetProductSubscriptionMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductSubscriptionMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductSubscriptionMapsToExcel(GetAllProductSubscriptionMapsForExcelInput input);

        Task<PagedResultDto<ProductSubscriptionMapProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductSubscriptionMapSubscriptionTypeLookupTableDto>> GetAllSubscriptionTypeForLookupTable(GetAllForLookupTableInput input);

    }
}