using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreBankAccountsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreBankAccountForViewDto>> GetAll(GetAllStoreBankAccountsInput input);

        Task<GetStoreBankAccountForViewDto> GetStoreBankAccountForView(long id);

        Task<GetStoreBankAccountForEditOutput> GetStoreBankAccountForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreBankAccountDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreBankAccountsToExcel(GetAllStoreBankAccountsForExcelInput input);

        Task<PagedResultDto<StoreBankAccountStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

    }
}