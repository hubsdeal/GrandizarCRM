using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductMediasAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductMediaForViewDto>> GetAll(GetAllProductMediasInput input);

        Task<GetProductMediaForViewDto> GetProductMediaForView(long id);

        Task<GetProductMediaForEditOutput> GetProductMediaForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductMediaDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductMediasToExcel(GetAllProductMediasForExcelInput input);

        Task<PagedResultDto<ProductMediaProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductMediaMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input);

    }
}