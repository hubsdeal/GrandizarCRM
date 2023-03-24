using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CRM
{
    public interface IBusinessJobMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetBusinessJobMapForViewDto>> GetAll(GetAllBusinessJobMapsInput input);

        Task<GetBusinessJobMapForViewDto> GetBusinessJobMapForView(long id);

        Task<GetBusinessJobMapForEditOutput> GetBusinessJobMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditBusinessJobMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetBusinessJobMapsToExcel(GetAllBusinessJobMapsForExcelInput input);

        Task<PagedResultDto<BusinessJobMapBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<BusinessJobMapJobLookupTableDto>> GetAllJobForLookupTable(GetAllForLookupTableInput input);

    }
}