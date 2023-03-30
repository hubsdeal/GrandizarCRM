using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductOwnerPublicContactInfosAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductOwnerPublicContactInfoForViewDto>> GetAll(GetAllProductOwnerPublicContactInfosInput input);

        Task<GetProductOwnerPublicContactInfoForViewDto> GetProductOwnerPublicContactInfoForView(long id);

        Task<GetProductOwnerPublicContactInfoForEditOutput> GetProductOwnerPublicContactInfoForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductOwnerPublicContactInfoDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductOwnerPublicContactInfosToExcel(GetAllProductOwnerPublicContactInfosForExcelInput input);

        Task<PagedResultDto<ProductOwnerPublicContactInfoContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductOwnerPublicContactInfoProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductOwnerPublicContactInfoStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductOwnerPublicContactInfoUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

    }
}