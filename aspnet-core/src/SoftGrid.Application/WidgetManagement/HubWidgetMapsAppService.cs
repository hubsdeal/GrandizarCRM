using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;

using Microsoft.EntityFrameworkCore;

using SoftGrid.Authorization;
using SoftGrid.Dto;
using SoftGrid.Territory;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.WidgetManagement.Enums;
using SoftGrid.WidgetManagement.Exporting;

using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace SoftGrid.WidgetManagement
{
    [AbpAuthorize(AppPermissions.Pages_HubWidgetMaps)]
    public class HubWidgetMapsAppService : SoftGridAppServiceBase, IHubWidgetMapsAppService
    {
        private readonly IRepository<HubWidgetMap, long> _hubWidgetMapRepository;
        private readonly IHubWidgetMapsExcelExporter _hubWidgetMapsExcelExporter;
        private readonly IRepository<Hub, long> _lookup_hubRepository;
        private readonly IRepository<MasterWidget, long> _lookup_masterWidgetRepository;

        public HubWidgetMapsAppService(IRepository<HubWidgetMap, long> hubWidgetMapRepository, IHubWidgetMapsExcelExporter hubWidgetMapsExcelExporter, IRepository<Hub, long> lookup_hubRepository, IRepository<MasterWidget, long> lookup_masterWidgetRepository)
        {
            _hubWidgetMapRepository = hubWidgetMapRepository;
            _hubWidgetMapsExcelExporter = hubWidgetMapsExcelExporter;
            _lookup_hubRepository = lookup_hubRepository;
            _lookup_masterWidgetRepository = lookup_masterWidgetRepository;

        }

        public async Task<PagedResultDto<GetHubWidgetMapForViewDto>> GetAll(GetAllHubWidgetMapsInput input)
        {
            var widgetTypeIdFilter = input.WidgetTypeIdFilter.HasValue
                        ? (WidgetType)input.WidgetTypeIdFilter
                        : default;

            var filteredHubWidgetMaps = _hubWidgetMapRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.MasterWidgetFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomNameFilter), e => e.CustomName.Contains(input.CustomNameFilter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(input.WidgetTypeIdFilter.HasValue && input.WidgetTypeIdFilter > -1, e => e.WidgetTypeId == widgetTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterWidgetNameFilter), e => e.MasterWidgetFk != null && e.MasterWidgetFk.Name == input.MasterWidgetNameFilter);

            var pagedAndFilteredHubWidgetMaps = filteredHubWidgetMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var hubWidgetMaps = from o in pagedAndFilteredHubWidgetMaps
                                join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                join o2 in _lookup_masterWidgetRepository.GetAll() on o.MasterWidgetId equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()

                                select new
                                {

                                    o.CustomName,
                                    o.DisplaySequence,
                                    o.WidgetTypeId,
                                    Id = o.Id,
                                    HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                    MasterWidgetName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                };

            var totalCount = await filteredHubWidgetMaps.CountAsync();

            var dbList = await hubWidgetMaps.ToListAsync();
            var results = new List<GetHubWidgetMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetHubWidgetMapForViewDto()
                {
                    HubWidgetMap = new HubWidgetMapDto
                    {

                        CustomName = o.CustomName,
                        DisplaySequence = o.DisplaySequence,
                        WidgetTypeId = o.WidgetTypeId,
                        Id = o.Id,
                    },
                    HubName = o.HubName,
                    MasterWidgetName = o.MasterWidgetName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetHubWidgetMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetHubWidgetMapForViewDto> GetHubWidgetMapForView(long id)
        {
            var hubWidgetMap = await _hubWidgetMapRepository.GetAsync(id);

            var output = new GetHubWidgetMapForViewDto { HubWidgetMap = ObjectMapper.Map<HubWidgetMapDto>(hubWidgetMap) };

            if (output.HubWidgetMap.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.HubWidgetMap.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.HubWidgetMap.MasterWidgetId != null)
            {
                var _lookupMasterWidget = await _lookup_masterWidgetRepository.FirstOrDefaultAsync((long)output.HubWidgetMap.MasterWidgetId);
                output.MasterWidgetName = _lookupMasterWidget?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_HubWidgetMaps_Edit)]
        public async Task<GetHubWidgetMapForEditOutput> GetHubWidgetMapForEdit(EntityDto<long> input)
        {
            var hubWidgetMap = await _hubWidgetMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHubWidgetMapForEditOutput { HubWidgetMap = ObjectMapper.Map<CreateOrEditHubWidgetMapDto>(hubWidgetMap) };

            if (output.HubWidgetMap.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.HubWidgetMap.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.HubWidgetMap.MasterWidgetId != null)
            {
                var _lookupMasterWidget = await _lookup_masterWidgetRepository.FirstOrDefaultAsync((long)output.HubWidgetMap.MasterWidgetId);
                output.MasterWidgetName = _lookupMasterWidget?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHubWidgetMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_HubWidgetMaps_Create)]
        protected virtual async Task Create(CreateOrEditHubWidgetMapDto input)
        {
            var hubWidgetMap = ObjectMapper.Map<HubWidgetMap>(input);

            if (AbpSession.TenantId != null)
            {
                hubWidgetMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _hubWidgetMapRepository.InsertAsync(hubWidgetMap);

        }

        [AbpAuthorize(AppPermissions.Pages_HubWidgetMaps_Edit)]
        protected virtual async Task Update(CreateOrEditHubWidgetMapDto input)
        {
            var hubWidgetMap = await _hubWidgetMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, hubWidgetMap);

        }

        [AbpAuthorize(AppPermissions.Pages_HubWidgetMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _hubWidgetMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetHubWidgetMapsToExcel(GetAllHubWidgetMapsForExcelInput input)
        {
            var widgetTypeIdFilter = input.WidgetTypeIdFilter.HasValue
                        ? (WidgetType)input.WidgetTypeIdFilter
                        : default;

            var filteredHubWidgetMaps = _hubWidgetMapRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.MasterWidgetFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomNameFilter), e => e.CustomName.Contains(input.CustomNameFilter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(input.WidgetTypeIdFilter.HasValue && input.WidgetTypeIdFilter > -1, e => e.WidgetTypeId == widgetTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterWidgetNameFilter), e => e.MasterWidgetFk != null && e.MasterWidgetFk.Name == input.MasterWidgetNameFilter);

            var query = (from o in filteredHubWidgetMaps
                         join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_masterWidgetRepository.GetAll() on o.MasterWidgetId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetHubWidgetMapForViewDto()
                         {
                             HubWidgetMap = new HubWidgetMapDto
                             {
                                 CustomName = o.CustomName,
                                 DisplaySequence = o.DisplaySequence,
                                 WidgetTypeId = o.WidgetTypeId,
                                 Id = o.Id
                             },
                             HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MasterWidgetName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var hubWidgetMapListDtos = await query.ToListAsync();

            return _hubWidgetMapsExcelExporter.ExportToFile(hubWidgetMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_HubWidgetMaps)]
        public async Task<PagedResultDto<HubWidgetMapHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_hubRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var hubList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubWidgetMapHubLookupTableDto>();
            foreach (var hub in hubList)
            {
                lookupTableDtoList.Add(new HubWidgetMapHubLookupTableDto
                {
                    Id = hub.Id,
                    DisplayName = hub.Name?.ToString()
                });
            }

            return new PagedResultDto<HubWidgetMapHubLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HubWidgetMaps)]
        public async Task<PagedResultDto<HubWidgetMapMasterWidgetLookupTableDto>> GetAllMasterWidgetForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterWidgetRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterWidgetList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubWidgetMapMasterWidgetLookupTableDto>();
            foreach (var masterWidget in masterWidgetList)
            {
                lookupTableDtoList.Add(new HubWidgetMapMasterWidgetLookupTableDto
                {
                    Id = masterWidget.Id,
                    DisplayName = masterWidget.Name?.ToString()
                });
            }

            return new PagedResultDto<HubWidgetMapMasterWidgetLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }



        [AbpAllowAnonymous]
        public async Task<List<HubWidgetMapDto>> GetHubWidgetsByHubId(int hubId)
        {
            var hubWidgetMaps = await _hubWidgetMapRepository.GetAll().Include(c => c.MasterWidgetFk).Include(c => c.HubFk).Where(c => c.HubId == hubId).ToListAsync();

            var dataList = ObjectMapper.Map<List<HubWidgetMapDto>>(hubWidgetMaps);
            foreach (var hubWidgetMap in hubWidgetMaps)
            {
                var dto = dataList.FirstOrDefault(c => c.Id == hubWidgetMap.Id);
                if (dto == null) continue;
                if (hubWidgetMap.MasterWidgetFk is { Publish: true })
                {
                    dto.MasterWidgetName = hubWidgetMap.MasterWidgetFk?.Name;
                    dto.MasterWidgetDescription = hubWidgetMap.MasterWidgetFk?.Description;
                    dto.MasterWidgetDesignCode = hubWidgetMap.MasterWidgetFk?.DesignCode;
                    dto.MasterWidgetInternalDisplayNumber = hubWidgetMap.MasterWidgetFk?.InternalDisplayNumber;
                    dto.MasterWidgetThumbnailImageId = hubWidgetMap.MasterWidgetFk?.ThumbnailImageId;
                }

            }

            return dataList;
        }

    }
}