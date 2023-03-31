using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.DiscountManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.DiscountManagement
{
    public interface ICustomerWalletsAppService : IApplicationService
    {
        Task<PagedResultDto<GetCustomerWalletForViewDto>> GetAll(GetAllCustomerWalletsInput input);

        Task<GetCustomerWalletForViewDto> GetCustomerWalletForView(long id);

        Task<GetCustomerWalletForEditOutput> GetCustomerWalletForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCustomerWalletDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetCustomerWalletsToExcel(GetAllCustomerWalletsForExcelInput input);

        Task<PagedResultDto<CustomerWalletContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<CustomerWalletUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<CustomerWalletCurrencyLookupTableDto>> GetAllCurrencyForLookupTable(GetAllForLookupTableInput input);

    }
}