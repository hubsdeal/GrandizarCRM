using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData
{
    public interface IMembershipTypesAppService : IApplicationService
    {
        Task<PagedResultDto<GetMembershipTypeForViewDto>> GetAll(GetAllMembershipTypesInput input);

        Task<GetMembershipTypeForViewDto> GetMembershipTypeForView(long id);

        Task<GetMembershipTypeForEditOutput> GetMembershipTypeForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditMembershipTypeDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetMembershipTypesToExcel(GetAllMembershipTypesForExcelInput input);

    }
}