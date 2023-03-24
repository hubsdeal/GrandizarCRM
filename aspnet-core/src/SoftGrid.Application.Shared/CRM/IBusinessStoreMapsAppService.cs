using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CRM
{
    public interface IBusinessStoreMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetBusinessStoreMapForViewDto>> GetAll(GetAllBusinessStoreMapsInput input);

        Task<GetBusinessStoreMapForViewDto> GetBusinessStoreMapForView(long id);

        Task<GetBusinessStoreMapForEditOutput> GetBusinessStoreMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditBusinessStoreMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetBusinessStoreMapsToExcel(GetAllBusinessStoreMapsForExcelInput input);

        Task<PagedResultDto<BusinessStoreMapBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<BusinessStoreMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

    }
}