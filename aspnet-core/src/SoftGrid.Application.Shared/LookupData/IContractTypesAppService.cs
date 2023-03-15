using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData
{
    public interface IContractTypesAppService : IApplicationService
    {
        Task<PagedResultDto<GetContractTypeForViewDto>> GetAll(GetAllContractTypesInput input);

        Task<GetContractTypeForViewDto> GetContractTypeForView(long id);

        Task<GetContractTypeForEditOutput> GetContractTypeForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditContractTypeDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetContractTypesToExcel(GetAllContractTypesForExcelInput input);

    }
}