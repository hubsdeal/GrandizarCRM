using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductCustomerQueriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductCustomerQueryForViewDto>> GetAll(GetAllProductCustomerQueriesInput input);

        Task<GetProductCustomerQueryForViewDto> GetProductCustomerQueryForView(long id);

        Task<GetProductCustomerQueryForEditOutput> GetProductCustomerQueryForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductCustomerQueryDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductCustomerQueriesToExcel(GetAllProductCustomerQueriesForExcelInput input);

        Task<PagedResultDto<ProductCustomerQueryProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductCustomerQueryContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductCustomerQueryEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input);

    }
}