using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CRM
{
    public interface IBusinessProductMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetBusinessProductMapForViewDto>> GetAll(GetAllBusinessProductMapsInput input);

        Task<GetBusinessProductMapForViewDto> GetBusinessProductMapForView(long id);

        Task<GetBusinessProductMapForEditOutput> GetBusinessProductMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditBusinessProductMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetBusinessProductMapsToExcel(GetAllBusinessProductMapsForExcelInput input);

        Task<PagedResultDto<BusinessProductMapBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<BusinessProductMapProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

    }
}