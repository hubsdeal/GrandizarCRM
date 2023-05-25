using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreReservationSettingsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreReservationSettingForViewDto>> GetAll(GetAllStoreReservationSettingsInput input);

        Task<GetStoreReservationSettingForViewDto> GetStoreReservationSettingForView(long id);

        Task<GetStoreReservationSettingForEditOutput> GetStoreReservationSettingForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreReservationSettingDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreReservationSettingsToExcel(GetAllStoreReservationSettingsForExcelInput input);

        Task<PagedResultDto<StoreReservationSettingStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

    }
}