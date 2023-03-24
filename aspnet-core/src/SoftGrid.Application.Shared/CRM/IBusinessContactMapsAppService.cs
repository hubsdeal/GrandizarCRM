using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CRM
{
    public interface IBusinessContactMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetBusinessContactMapForViewDto>> GetAll(GetAllBusinessContactMapsInput input);

        Task<GetBusinessContactMapForViewDto> GetBusinessContactMapForView(long id);

        Task<GetBusinessContactMapForEditOutput> GetBusinessContactMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditBusinessContactMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetBusinessContactMapsToExcel(GetAllBusinessContactMapsForExcelInput input);

        Task<PagedResultDto<BusinessContactMapBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<BusinessContactMapContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

    }
}