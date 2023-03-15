using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.LookupData.Exporting;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.LookupData
{
    [AbpAuthorize(AppPermissions.Pages_ConnectChannels)]
    public class ConnectChannelsAppService : SoftGridAppServiceBase, IConnectChannelsAppService
    {
        private readonly IRepository<ConnectChannel, long> _connectChannelRepository;
        private readonly IConnectChannelsExcelExporter _connectChannelsExcelExporter;

        public ConnectChannelsAppService(IRepository<ConnectChannel, long> connectChannelRepository, IConnectChannelsExcelExporter connectChannelsExcelExporter)
        {
            _connectChannelRepository = connectChannelRepository;
            _connectChannelsExcelExporter = connectChannelsExcelExporter;

        }

        public async Task<PagedResultDto<GetConnectChannelForViewDto>> GetAll(GetAllConnectChannelsInput input)
        {

            var filteredConnectChannels = _connectChannelRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredConnectChannels = filteredConnectChannels
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var connectChannels = from o in pagedAndFilteredConnectChannels
                                  select new
                                  {

                                      o.Name,
                                      Id = o.Id
                                  };

            var totalCount = await filteredConnectChannels.CountAsync();

            var dbList = await connectChannels.ToListAsync();
            var results = new List<GetConnectChannelForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetConnectChannelForViewDto()
                {
                    ConnectChannel = new ConnectChannelDto
                    {

                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetConnectChannelForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetConnectChannelForViewDto> GetConnectChannelForView(long id)
        {
            var connectChannel = await _connectChannelRepository.GetAsync(id);

            var output = new GetConnectChannelForViewDto { ConnectChannel = ObjectMapper.Map<ConnectChannelDto>(connectChannel) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ConnectChannels_Edit)]
        public async Task<GetConnectChannelForEditOutput> GetConnectChannelForEdit(EntityDto<long> input)
        {
            var connectChannel = await _connectChannelRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetConnectChannelForEditOutput { ConnectChannel = ObjectMapper.Map<CreateOrEditConnectChannelDto>(connectChannel) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditConnectChannelDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_ConnectChannels_Create)]
        protected virtual async Task Create(CreateOrEditConnectChannelDto input)
        {
            var connectChannel = ObjectMapper.Map<ConnectChannel>(input);

            if (AbpSession.TenantId != null)
            {
                connectChannel.TenantId = (int?)AbpSession.TenantId;
            }

            await _connectChannelRepository.InsertAsync(connectChannel);

        }

        [AbpAuthorize(AppPermissions.Pages_ConnectChannels_Edit)]
        protected virtual async Task Update(CreateOrEditConnectChannelDto input)
        {
            var connectChannel = await _connectChannelRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, connectChannel);

        }

        [AbpAuthorize(AppPermissions.Pages_ConnectChannels_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _connectChannelRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetConnectChannelsToExcel(GetAllConnectChannelsForExcelInput input)
        {

            var filteredConnectChannels = _connectChannelRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredConnectChannels
                         select new GetConnectChannelForViewDto()
                         {
                             ConnectChannel = new ConnectChannelDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var connectChannelListDtos = await query.ToListAsync();

            return _connectChannelsExcelExporter.ExportToFile(connectChannelListDtos);
        }

    }
}