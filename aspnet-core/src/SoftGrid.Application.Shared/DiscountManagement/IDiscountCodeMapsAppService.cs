using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.DiscountManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.DiscountManagement
{
    public interface IDiscountCodeMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDiscountCodeMapForViewDto>> GetAll(GetAllDiscountCodeMapsInput input);

        Task<GetDiscountCodeMapForViewDto> GetDiscountCodeMapForView(long id);

        Task<GetDiscountCodeMapForEditOutput> GetDiscountCodeMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDiscountCodeMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetDiscountCodeMapsToExcel(GetAllDiscountCodeMapsForExcelInput input);

        Task<PagedResultDto<DiscountCodeMapDiscountCodeGeneratorLookupTableDto>> GetAllDiscountCodeGeneratorForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<DiscountCodeMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<DiscountCodeMapProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<DiscountCodeMapMembershipTypeLookupTableDto>> GetAllMembershipTypeForLookupTable(GetAllForLookupTableInput input);

    }
}