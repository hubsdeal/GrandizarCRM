using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductPackagesAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductPackageForViewDto>> GetAll(GetAllProductPackagesInput input);

        Task<GetProductPackageForViewDto> GetProductPackageForView(long id);

        Task<GetProductPackageForEditOutput> GetProductPackageForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductPackageDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductPackagesToExcel(GetAllProductPackagesForExcelInput input);

        Task<PagedResultDto<ProductPackageProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductPackageMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input);

    }
}