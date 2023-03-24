using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreSalesAlertsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreSalesAlertForViewDto>> GetAll(GetAllStoreSalesAlertsInput input);

        Task<GetStoreSalesAlertForViewDto> GetStoreSalesAlertForView(long id);

        Task<GetStoreSalesAlertForEditOutput> GetStoreSalesAlertForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreSalesAlertDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreSalesAlertsToExcel(GetAllStoreSalesAlertsForExcelInput input);

        Task<PagedResultDto<StoreSalesAlertStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

    }
}