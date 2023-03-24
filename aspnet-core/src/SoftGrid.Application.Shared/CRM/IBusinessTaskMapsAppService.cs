using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CRM
{
    public interface IBusinessTaskMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetBusinessTaskMapForViewDto>> GetAll(GetAllBusinessTaskMapsInput input);

        Task<GetBusinessTaskMapForViewDto> GetBusinessTaskMapForView(long id);

        Task<GetBusinessTaskMapForEditOutput> GetBusinessTaskMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditBusinessTaskMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetBusinessTaskMapsToExcel(GetAllBusinessTaskMapsForExcelInput input);

        Task<PagedResultDto<BusinessTaskMapBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<BusinessTaskMapTaskEventLookupTableDto>> GetAllTaskEventForLookupTable(GetAllForLookupTableInput input);

    }
}