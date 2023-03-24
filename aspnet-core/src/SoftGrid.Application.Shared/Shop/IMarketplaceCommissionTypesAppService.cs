using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IMarketplaceCommissionTypesAppService : IApplicationService
    {
        Task<PagedResultDto<GetMarketplaceCommissionTypeForViewDto>> GetAll(GetAllMarketplaceCommissionTypesInput input);

        Task<GetMarketplaceCommissionTypeForViewDto> GetMarketplaceCommissionTypeForView(long id);

        Task<GetMarketplaceCommissionTypeForEditOutput> GetMarketplaceCommissionTypeForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditMarketplaceCommissionTypeDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetMarketplaceCommissionTypesToExcel(GetAllMarketplaceCommissionTypesForExcelInput input);

    }
}