using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CRM
{
    public interface IBusinessUsersAppService : IApplicationService
    {
        Task<PagedResultDto<GetBusinessUserForViewDto>> GetAll(GetAllBusinessUsersInput input);

        Task<GetBusinessUserForViewDto> GetBusinessUserForView(long id);

        Task<GetBusinessUserForEditOutput> GetBusinessUserForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditBusinessUserDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetBusinessUsersToExcel(GetAllBusinessUsersForExcelInput input);

        Task<PagedResultDto<BusinessUserBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<BusinessUserUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

    }
}