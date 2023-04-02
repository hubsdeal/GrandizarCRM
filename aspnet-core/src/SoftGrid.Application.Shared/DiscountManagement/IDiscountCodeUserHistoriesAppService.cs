using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.DiscountManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.DiscountManagement
{
    public interface IDiscountCodeUserHistoriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetDiscountCodeUserHistoryForViewDto>> GetAll(GetAllDiscountCodeUserHistoriesInput input);

        Task<GetDiscountCodeUserHistoryForViewDto> GetDiscountCodeUserHistoryForView(long id);

        Task<GetDiscountCodeUserHistoryForEditOutput> GetDiscountCodeUserHistoryForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDiscountCodeUserHistoryDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetDiscountCodeUserHistoriesToExcel(GetAllDiscountCodeUserHistoriesForExcelInput input);

        Task<PagedResultDto<DiscountCodeUserHistoryDiscountCodeGeneratorLookupTableDto>> GetAllDiscountCodeGeneratorForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<DiscountCodeUserHistoryOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<DiscountCodeUserHistoryContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

    }
}