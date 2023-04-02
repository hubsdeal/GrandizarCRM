using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData
{
    public interface IPaymentTypesAppService : IApplicationService
    {
        Task<PagedResultDto<GetPaymentTypeForViewDto>> GetAll(GetAllPaymentTypesInput input);

        Task<GetPaymentTypeForViewDto> GetPaymentTypeForView(long id);

        Task<GetPaymentTypeForEditOutput> GetPaymentTypeForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditPaymentTypeDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetPaymentTypesToExcel(GetAllPaymentTypesForExcelInput input);

    }
}