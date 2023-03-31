using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.DiscountManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.DiscountManagement
{
    public interface IDiscountCodeGeneratorsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDiscountCodeGeneratorForViewDto>> GetAll(GetAllDiscountCodeGeneratorsInput input);

        Task<GetDiscountCodeGeneratorForViewDto> GetDiscountCodeGeneratorForView(long id);

        Task<GetDiscountCodeGeneratorForEditOutput> GetDiscountCodeGeneratorForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditDiscountCodeGeneratorDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetDiscountCodeGeneratorsToExcel(GetAllDiscountCodeGeneratorsForExcelInput input);

    }
}