using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData
{
    public interface IConnectChannelsAppService : IApplicationService
    {
        Task<PagedResultDto<GetConnectChannelForViewDto>> GetAll(GetAllConnectChannelsInput input);

        Task<GetConnectChannelForViewDto> GetConnectChannelForView(long id);

        Task<GetConnectChannelForEditOutput> GetConnectChannelForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditConnectChannelDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetConnectChannelsToExcel(GetAllConnectChannelsForExcelInput input);

    }
}