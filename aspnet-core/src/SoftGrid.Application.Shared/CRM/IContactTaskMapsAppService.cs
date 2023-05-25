using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CRM
{
    public interface IContactTaskMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetContactTaskMapForViewDto>> GetAll(GetAllContactTaskMapsInput input);

        Task<GetContactTaskMapForViewDto> GetContactTaskMapForView(long id);

        Task<GetContactTaskMapForEditOutput> GetContactTaskMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditContactTaskMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetContactTaskMapsToExcel(GetAllContactTaskMapsForExcelInput input);

        Task<PagedResultDto<ContactTaskMapContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ContactTaskMapTaskEventLookupTableDto>> GetAllTaskEventForLookupTable(GetAllForLookupTableInput input);

    }
}