using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreContactMapsAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreContactMapForViewDto>> GetAll(GetAllStoreContactMapsInput input);

        Task<GetStoreContactMapForViewDto> GetStoreContactMapForView(long id);

        Task<GetStoreContactMapForEditOutput> GetStoreContactMapForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreContactMapDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreContactMapsToExcel(GetAllStoreContactMapsForExcelInput input);

        Task<PagedResultDto<StoreContactMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<StoreContactMapContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

    }
}