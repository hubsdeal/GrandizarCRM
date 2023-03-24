using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreTaxesAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreTaxForViewDto>> GetAll(GetAllStoreTaxesInput input);

        Task<GetStoreTaxForViewDto> GetStoreTaxForView(long id);

        Task<GetStoreTaxForEditOutput> GetStoreTaxForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreTaxDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreTaxesToExcel(GetAllStoreTaxesForExcelInput input);

        Task<PagedResultDto<StoreTaxStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

    }
}