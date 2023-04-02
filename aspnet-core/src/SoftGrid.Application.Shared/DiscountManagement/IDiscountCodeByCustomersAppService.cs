using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.DiscountManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.DiscountManagement
{
    public interface IDiscountCodeByCustomersAppService : IApplicationService
    {
        Task<PagedResultDto<GetDiscountCodeByCustomerForViewDto>> GetAll(GetAllDiscountCodeByCustomersInput input);

        Task<GetDiscountCodeByCustomerForViewDto> GetDiscountCodeByCustomerForView(long id);

        Task<GetDiscountCodeByCustomerForEditOutput> GetDiscountCodeByCustomerForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDiscountCodeByCustomerDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetDiscountCodeByCustomersToExcel(GetAllDiscountCodeByCustomersForExcelInput input);

        Task<PagedResultDto<DiscountCodeByCustomerDiscountCodeGeneratorLookupTableDto>> GetAllDiscountCodeGeneratorForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<DiscountCodeByCustomerContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

    }
}