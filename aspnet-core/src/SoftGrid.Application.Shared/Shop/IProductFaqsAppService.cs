using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductFaqsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductFaqForViewDto>> GetAll(GetAllProductFaqsInput input);

        Task<GetProductFaqForViewDto> GetProductFaqForView(long id);

        Task<GetProductFaqForEditOutput> GetProductFaqForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductFaqDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductFaqsToExcel(GetAllProductFaqsForExcelInput input);

        Task<PagedResultDto<ProductFaqProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

    }
}