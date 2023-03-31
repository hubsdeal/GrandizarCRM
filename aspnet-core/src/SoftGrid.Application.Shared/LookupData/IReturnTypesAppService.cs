using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData
{
    public interface IReturnTypesAppService : IApplicationService
    {
        Task<PagedResultDto<GetReturnTypeForViewDto>> GetAll(GetAllReturnTypesInput input);

        Task<GetReturnTypeForViewDto> GetReturnTypeForView(long id);

        Task<GetReturnTypeForEditOutput> GetReturnTypeForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditReturnTypeDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetReturnTypesToExcel(GetAllReturnTypesForExcelInput input);

    }
}