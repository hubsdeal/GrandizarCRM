using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductReturnInfosAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductReturnInfoForViewDto>> GetAll(GetAllProductReturnInfosInput input);

        Task<GetProductReturnInfoForViewDto> GetProductReturnInfoForView(long id);

        Task<GetProductReturnInfoForEditOutput> GetProductReturnInfoForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductReturnInfoDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductReturnInfosToExcel(GetAllProductReturnInfosForExcelInput input);

        Task<PagedResultDto<ProductReturnInfoProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductReturnInfoReturnTypeLookupTableDto>> GetAllReturnTypeForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductReturnInfoReturnStatusLookupTableDto>> GetAllReturnStatusForLookupTable(GetAllForLookupTableInput input);

    }
}