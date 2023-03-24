using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreBusinessCustomerMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreBusinessCustomerMapForViewDto>> GetAll(GetAllStoreBusinessCustomerMapsInput input);

        Task<GetStoreBusinessCustomerMapForViewDto> GetStoreBusinessCustomerMapForView(int id);

        Task<GetStoreBusinessCustomerMapForEditOutput> GetStoreBusinessCustomerMapForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditStoreBusinessCustomerMapDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetStoreBusinessCustomerMapsToExcel(GetAllStoreBusinessCustomerMapsForExcelInput input);

        Task<PagedResultDto<StoreBusinessCustomerMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreBusinessCustomerMapBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input);

    }
}