using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.OrderManagement
{
    public interface IOrderProductVariantsAppService : IApplicationService
    {
        Task<PagedResultDto<GetOrderProductVariantForViewDto>> GetAll(GetAllOrderProductVariantsInput input);

        Task<GetOrderProductVariantForViewDto> GetOrderProductVariantForView(long id);

        Task<GetOrderProductVariantForEditOutput> GetOrderProductVariantForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditOrderProductVariantDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetOrderProductVariantsToExcel(GetAllOrderProductVariantsForExcelInput input);

        Task<PagedResultDto<OrderProductVariantProductVariantCategoryLookupTableDto>> GetAllProductVariantCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderProductVariantProductVariantLookupTableDto>> GetAllProductVariantForLookupTable(GetAllForLookupTableInput input);

    }
}