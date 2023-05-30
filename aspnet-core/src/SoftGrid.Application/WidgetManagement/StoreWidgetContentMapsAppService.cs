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
    [AbpAuthorize(AppPermissions.Pages_StoreWidgetContentMaps)]
    public class StoreWidgetContentMapsAppService : SoftGridAppServiceBase, IStoreWidgetContentMapsAppService
    {
        private readonly IRepository<StoreWidgetContentMap, long> _storeWidgetContentMapRepository;
        private readonly IStoreWidgetContentMapsExcelExporter _storeWidgetContentMapsExcelExporter;
        private readonly IRepository<StoreWidgetMap, long> _lookup_storeWidgetMapRepository;
        private readonly IRepository<Content, long> _lookup_contentRepository;

        public StoreWidgetContentMapsAppService(IRepository<StoreWidgetContentMap, long> storeWidgetContentMapRepository, IStoreWidgetContentMapsExcelExporter storeWidgetContentMapsExcelExporter, IRepository<StoreWidgetMap, long> lookup_storeWidgetMapRepository, IRepository<Content, long> lookup_contentRepository)
        {
            _storeWidgetContentMapRepository = storeWidgetContentMapRepository;
            _storeWidgetContentMapsExcelExporter = storeWidgetContentMapsExcelExporter;
            _lookup_storeWidgetMapRepository = lookup_storeWidgetMapRepository;
            _lookup_contentRepository = lookup_contentRepository;

        }

        public async Task<PagedResultDto<GetStoreWidgetContentMapForViewDto>> GetAll(GetAllStoreWidgetContentMapsInput input)
        {

            var filteredStoreWidgetContentMaps = _storeWidgetContentMapRepository.GetAll()
                        .Include(e => e.StoreWidgetMapFk)
                        .Include(e => e.ContentFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreWidgetMapCustomNameFilter), e => e.StoreWidgetMapFk != null && e.StoreWidgetMapFk.CustomName == input.StoreWidgetMapCustomNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContentTitleFilter), e => e.ContentFk != null && e.ContentFk.Title == input.ContentTitleFilter);

            var pagedAndFilteredStoreWidgetContentMaps = filteredStoreWidgetContentMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeWidgetContentMaps = from o in pagedAndFilteredStoreWidgetContentMaps
                                         join o1 in _lookup_storeWidgetMapRepository.GetAll() on o.StoreWidgetMapId equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()

                                         join o2 in _lookup_contentRepository.GetAll() on o.ContentId equals o2.Id into j2
                                         from s2 in j2.DefaultIfEmpty()

                                         select new
                                         {

                                             o.DisplaySequence,
                                             Id = o.Id,
                                             StoreWidgetMapCustomName = s1 == null || s1.CustomName == null ? "" : s1.CustomName.ToString(),
                                             ContentTitle = s2 == null || s2.Title == null ? "" : s2.Title.ToString()
                                         };

            var totalCount = await filteredStoreWidgetContentMaps.CountAsync();

            var dbList = await storeWidgetContentMaps.ToListAsync();
            var results = new List<GetStoreWidgetContentMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreWidgetContentMapForViewDto()
                {
                    StoreWidgetContentMap = new StoreWidgetContentMapDto
                    {

                        DisplaySequence = o.DisplaySequence,
                        Id = o.Id,
                    },
                    StoreWidgetMapCustomName = o.StoreWidgetMapCustomName,
                    ContentTitle = o.ContentTitle
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreWidgetContentMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreWidgetContentMapForViewDto> GetStoreWidgetContentMapForView(long id)
        {
            var storeWidgetContentMap = await _storeWidgetContentMapRepository.GetAsync(id);

            var output = new GetStoreWidgetContentMapForViewDto { StoreWidgetContentMap = ObjectMapper.Map<StoreWidgetContentMapDto>(storeWidgetContentMap) };

            if (output.StoreWidgetContentMap.StoreWidgetMapId != null)
            {
                var _lookupStoreWidgetMap = await _lookup_storeWidgetMapRepository.FirstOrDefaultAsync((long)output.StoreWidgetContentMap.StoreWidgetMapId);
                output.StoreWidgetMapCustomName = _lookupStoreWidgetMap?.CustomName?.ToString();
            }

            if (output.StoreWidgetContentMap.ContentId != null)
            {
                var _lookupContent = await _lookup_contentRepository.FirstOrDefaultAsync((long)output.StoreWidgetContentMap.ContentId);
                output.ContentTitle = _lookupContent?.Title?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreWidgetContentMaps_Edit)]
        public async Task<GetStoreWidgetContentMapForEditOutput> GetStoreWidgetContentMapForEdit(EntityDto<long> input)
        {
            var storeWidgetContentMap = await _storeWidgetContentMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreWidgetContentMapForEditOutput { StoreWidgetContentMap = ObjectMapper.Map<CreateOrEditStoreWidgetContentMapDto>(storeWidgetContentMap) };

            if (output.StoreWidgetContentMap.StoreWidgetMapId != null)
            {
                var _lookupStoreWidgetMap = await _lookup_storeWidgetMapRepository.FirstOrDefaultAsync((long)output.StoreWidgetContentMap.StoreWidgetMapId);
                output.StoreWidgetMapCustomName = _lookupStoreWidgetMap?.CustomName?.ToString();
            }

            if (output.StoreWidgetContentMap.ContentId != null)
            {
                var _lookupContent = await _lookup_contentRepository.FirstOrDefaultAsync((long)output.StoreWidgetContentMap.ContentId);
                output.ContentTitle = _lookupContent?.Title?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreWidgetContentMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreWidgetContentMaps_Create)]
        protected virtual async Task Create(CreateOrEditStoreWidgetContentMapDto input)
        {
            var storeWidgetContentMap = ObjectMapper.Map<StoreWidgetContentMap>(input);

            if (AbpSession.TenantId != null)
            {
                storeWidgetContentMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeWidgetContentMapRepository.InsertAsync(storeWidgetContentMap);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreWidgetContentMaps_Edit)]
        protected virtual async Task Update(CreateOrEditStoreWidgetContentMapDto input)
        {
            var storeWidgetContentMap = await _storeWidgetContentMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeWidgetContentMap);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreWidgetContentMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeWidgetContentMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreWidgetContentMapsToExcel(GetAllStoreWidgetContentMapsForExcelInput input)
        {

            var filteredStoreWidgetContentMaps = _storeWidgetContentMapRepository.GetAll()
                        .Include(e => e.StoreWidgetMapFk)
                        .Include(e => e.ContentFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreWidgetMapCustomNameFilter), e => e.StoreWidgetMapFk != null && e.StoreWidgetMapFk.CustomName == input.StoreWidgetMapCustomNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContentTitleFilter), e => e.ContentFk != null && e.ContentFk.Title == input.ContentTitleFilter);

            var query = (from o in filteredStoreWidgetContentMaps
                         join o1 in _lookup_storeWidgetMapRepository.GetAll() on o.StoreWidgetMapId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_contentRepository.GetAll() on o.ContentId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetStoreWidgetContentMapForViewDto()
                         {
                             StoreWidgetContentMap = new StoreWidgetContentMapDto
                             {
                                 DisplaySequence = o.DisplaySequence,
                                 Id = o.Id
                             },
                             StoreWidgetMapCustomName = s1 == null || s1.CustomName == null ? "" : s1.CustomName.ToString(),
                             ContentTitle = s2 == null || s2.Title == null ? "" : s2.Title.ToString()
                         });

            var storeWidgetContentMapListDtos = await query.ToListAsync();

            return _storeWidgetContentMapsExcelExporter.ExportToFile(storeWidgetContentMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreWidgetContentMaps)]
        public async Task<PagedResultDto<StoreWidgetContentMapStoreWidgetMapLookupTableDto>> GetAllStoreWidgetMapForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeWidgetMapRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.CustomName != null && e.CustomName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeWidgetMapList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreWidgetContentMapStoreWidgetMapLookupTableDto>();
            foreach (var storeWidgetMap in storeWidgetMapList)
            {
                lookupTableDtoList.Add(new StoreWidgetContentMapStoreWidgetMapLookupTableDto
                {
                    Id = storeWidgetMap.Id,
                    DisplayName = storeWidgetMap.CustomName?.ToString()
                });
            }

            return new PagedResultDto<StoreWidgetContentMapStoreWidgetMapLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreWidgetContentMaps)]
        public async Task<PagedResultDto<StoreWidgetContentMapContentLookupTableDto>> GetAllContentForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contentRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Title != null && e.Title.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contentList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreWidgetContentMapContentLookupTableDto>();
            foreach (var content in contentList)
            {
                lookupTableDtoList.Add(new StoreWidgetContentMapContentLookupTableDto
                {
                    Id = content.Id,
                    DisplayName = content.Title?.ToString()
                });
            }

            return new PagedResultDto<StoreWidgetContentMapContentLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}