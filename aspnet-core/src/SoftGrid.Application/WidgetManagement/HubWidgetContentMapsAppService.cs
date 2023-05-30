using SoftGrid.WidgetManagement;
using SoftGrid.CMS;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.WidgetManagement.Exporting;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.WidgetManagement
{
    [AbpAuthorize(AppPermissions.Pages_HubWidgetContentMaps)]
    public class HubWidgetContentMapsAppService : SoftGridAppServiceBase, IHubWidgetContentMapsAppService
    {
        private readonly IRepository<HubWidgetContentMap, long> _hubWidgetContentMapRepository;
        private readonly IHubWidgetContentMapsExcelExporter _hubWidgetContentMapsExcelExporter;
        private readonly IRepository<HubWidgetMap, long> _lookup_hubWidgetMapRepository;
        private readonly IRepository<Content, long> _lookup_contentRepository;

        public HubWidgetContentMapsAppService(IRepository<HubWidgetContentMap, long> hubWidgetContentMapRepository, IHubWidgetContentMapsExcelExporter hubWidgetContentMapsExcelExporter, IRepository<HubWidgetMap, long> lookup_hubWidgetMapRepository, IRepository<Content, long> lookup_contentRepository)
        {
            _hubWidgetContentMapRepository = hubWidgetContentMapRepository;
            _hubWidgetContentMapsExcelExporter = hubWidgetContentMapsExcelExporter;
            _lookup_hubWidgetMapRepository = lookup_hubWidgetMapRepository;
            _lookup_contentRepository = lookup_contentRepository;

        }

        public async Task<PagedResultDto<GetHubWidgetContentMapForViewDto>> GetAll(GetAllHubWidgetContentMapsInput input)
        {

            var filteredHubWidgetContentMaps = _hubWidgetContentMapRepository.GetAll()
                        .Include(e => e.HubWidgetMapFk)
                        .Include(e => e.ContentFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubWidgetMapCustomNameFilter), e => e.HubWidgetMapFk != null && e.HubWidgetMapFk.CustomName == input.HubWidgetMapCustomNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContentTitleFilter), e => e.ContentFk != null && e.ContentFk.Title == input.ContentTitleFilter);

            var pagedAndFilteredHubWidgetContentMaps = filteredHubWidgetContentMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var hubWidgetContentMaps = from o in pagedAndFilteredHubWidgetContentMaps
                                       join o1 in _lookup_hubWidgetMapRepository.GetAll() on o.HubWidgetMapId equals o1.Id into j1
                                       from s1 in j1.DefaultIfEmpty()

                                       join o2 in _lookup_contentRepository.GetAll() on o.ContentId equals o2.Id into j2
                                       from s2 in j2.DefaultIfEmpty()

                                       select new
                                       {

                                           o.DisplaySequence,
                                           Id = o.Id,
                                           HubWidgetMapCustomName = s1 == null || s1.CustomName == null ? "" : s1.CustomName.ToString(),
                                           ContentTitle = s2 == null || s2.Title == null ? "" : s2.Title.ToString()
                                       };

            var totalCount = await filteredHubWidgetContentMaps.CountAsync();

            var dbList = await hubWidgetContentMaps.ToListAsync();
            var results = new List<GetHubWidgetContentMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetHubWidgetContentMapForViewDto()
                {
                    HubWidgetContentMap = new HubWidgetContentMapDto
                    {

                        DisplaySequence = o.DisplaySequence,
                        Id = o.Id,
                    },
                    HubWidgetMapCustomName = o.HubWidgetMapCustomName,
                    ContentTitle = o.ContentTitle
                };

                results.Add(res);
            }

            return new PagedResultDto<GetHubWidgetContentMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetHubWidgetContentMapForViewDto> GetHubWidgetContentMapForView(long id)
        {
            var hubWidgetContentMap = await _hubWidgetContentMapRepository.GetAsync(id);

            var output = new GetHubWidgetContentMapForViewDto { HubWidgetContentMap = ObjectMapper.Map<HubWidgetContentMapDto>(hubWidgetContentMap) };

            if (output.HubWidgetContentMap.HubWidgetMapId != null)
            {
                var _lookupHubWidgetMap = await _lookup_hubWidgetMapRepository.FirstOrDefaultAsync((long)output.HubWidgetContentMap.HubWidgetMapId);
                output.HubWidgetMapCustomName = _lookupHubWidgetMap?.CustomName?.ToString();
            }

            if (output.HubWidgetContentMap.ContentId != null)
            {
                var _lookupContent = await _lookup_contentRepository.FirstOrDefaultAsync((long)output.HubWidgetContentMap.ContentId);
                output.ContentTitle = _lookupContent?.Title?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_HubWidgetContentMaps_Edit)]
        public async Task<GetHubWidgetContentMapForEditOutput> GetHubWidgetContentMapForEdit(EntityDto<long> input)
        {
            var hubWidgetContentMap = await _hubWidgetContentMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHubWidgetContentMapForEditOutput { HubWidgetContentMap = ObjectMapper.Map<CreateOrEditHubWidgetContentMapDto>(hubWidgetContentMap) };

            if (output.HubWidgetContentMap.HubWidgetMapId != null)
            {
                var _lookupHubWidgetMap = await _lookup_hubWidgetMapRepository.FirstOrDefaultAsync((long)output.HubWidgetContentMap.HubWidgetMapId);
                output.HubWidgetMapCustomName = _lookupHubWidgetMap?.CustomName?.ToString();
            }

            if (output.HubWidgetContentMap.ContentId != null)
            {
                var _lookupContent = await _lookup_contentRepository.FirstOrDefaultAsync((long)output.HubWidgetContentMap.ContentId);
                output.ContentTitle = _lookupContent?.Title?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHubWidgetContentMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_HubWidgetContentMaps_Create)]
        protected virtual async Task Create(CreateOrEditHubWidgetContentMapDto input)
        {
            var hubWidgetContentMap = ObjectMapper.Map<HubWidgetContentMap>(input);

            if (AbpSession.TenantId != null)
            {
                hubWidgetContentMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _hubWidgetContentMapRepository.InsertAsync(hubWidgetContentMap);

        }

        [AbpAuthorize(AppPermissions.Pages_HubWidgetContentMaps_Edit)]
        protected virtual async Task Update(CreateOrEditHubWidgetContentMapDto input)
        {
            var hubWidgetContentMap = await _hubWidgetContentMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, hubWidgetContentMap);

        }

        [AbpAuthorize(AppPermissions.Pages_HubWidgetContentMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _hubWidgetContentMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetHubWidgetContentMapsToExcel(GetAllHubWidgetContentMapsForExcelInput input)
        {

            var filteredHubWidgetContentMaps = _hubWidgetContentMapRepository.GetAll()
                        .Include(e => e.HubWidgetMapFk)
                        .Include(e => e.ContentFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubWidgetMapCustomNameFilter), e => e.HubWidgetMapFk != null && e.HubWidgetMapFk.CustomName == input.HubWidgetMapCustomNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContentTitleFilter), e => e.ContentFk != null && e.ContentFk.Title == input.ContentTitleFilter);

            var query = (from o in filteredHubWidgetContentMaps
                         join o1 in _lookup_hubWidgetMapRepository.GetAll() on o.HubWidgetMapId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_contentRepository.GetAll() on o.ContentId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetHubWidgetContentMapForViewDto()
                         {
                             HubWidgetContentMap = new HubWidgetContentMapDto
                             {
                                 DisplaySequence = o.DisplaySequence,
                                 Id = o.Id
                             },
                             HubWidgetMapCustomName = s1 == null || s1.CustomName == null ? "" : s1.CustomName.ToString(),
                             ContentTitle = s2 == null || s2.Title == null ? "" : s2.Title.ToString()
                         });

            var hubWidgetContentMapListDtos = await query.ToListAsync();

            return _hubWidgetContentMapsExcelExporter.ExportToFile(hubWidgetContentMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_HubWidgetContentMaps)]
        public async Task<PagedResultDto<HubWidgetContentMapHubWidgetMapLookupTableDto>> GetAllHubWidgetMapForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_hubWidgetMapRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.CustomName != null && e.CustomName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var hubWidgetMapList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubWidgetContentMapHubWidgetMapLookupTableDto>();
            foreach (var hubWidgetMap in hubWidgetMapList)
            {
                lookupTableDtoList.Add(new HubWidgetContentMapHubWidgetMapLookupTableDto
                {
                    Id = hubWidgetMap.Id,
                    DisplayName = hubWidgetMap.CustomName?.ToString()
                });
            }

            return new PagedResultDto<HubWidgetContentMapHubWidgetMapLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HubWidgetContentMaps)]
        public async Task<PagedResultDto<HubWidgetContentMapContentLookupTableDto>> GetAllContentForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contentRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Title != null && e.Title.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contentList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubWidgetContentMapContentLookupTableDto>();
            foreach (var content in contentList)
            {
                lookupTableDtoList.Add(new HubWidgetContentMapContentLookupTableDto
                {
                    Id = content.Id,
                    DisplayName = content.Title?.ToString()
                });
            }

            return new PagedResultDto<HubWidgetContentMapContentLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}