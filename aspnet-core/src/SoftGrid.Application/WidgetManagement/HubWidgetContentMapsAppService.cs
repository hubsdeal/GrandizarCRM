using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;

using Microsoft.EntityFrameworkCore;

using SoftGrid.Authorization;
using SoftGrid.CMS;
using SoftGrid.CMS.Enums;
using SoftGrid.Dto;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.WidgetManagement.Exporting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

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



        [AbpAllowAnonymous]
        public async Task<dynamic> GetHubWidgetContentsByHubId(long hubId, int? contentTypeId = null)
        {
            try
            {
                var query = _hubWidgetContentMapRepository.GetAll()
                .Include(c => c.HubWidgetMapFk).ThenInclude(c => c.MasterWidgetFk)
                .Include(c => c.ContentFk).ThenInclude(c => c.BannerMediaLibraryFk)
                .Where(c => c.HubWidgetMapFk.HubId == hubId);

                List<HubWidgetContentMap> dataList = null;
                if (contentTypeId > 0) dataList = await query.Where(c => c.ContentFk.ContentTypeId == (ContentType)contentTypeId).ToListAsync();
                else dataList = await query.ToListAsync();

                var widgets = dataList.Where(c => c.HubWidgetMapFk.MasterWidgetFk.Publish).Distinct()
                    .Select(c => new HwsMapWidgetJsonViewDto
                    {
                        Id = c.HubWidgetMapFk?.MasterWidgetFk?.Id,
                        Name = c.HubWidgetMapFk?.MasterWidgetFk?.Name,
                        Description = c.HubWidgetMapFk?.MasterWidgetFk?.Description,
                        DesignCode = c.HubWidgetMapFk?.MasterWidgetFk?.DesignCode,
                        InternalDisplayNumber = c.HubWidgetMapFk?.MasterWidgetFk?.InternalDisplayNumber,
                        ThumbnailImageId = c.HubWidgetMapFk?.MasterWidgetFk?.ThumbnailImageId,
                        TenantId = c.HubWidgetMapFk?.MasterWidgetFk?.TenantId,
                        Publish = c.HubWidgetMapFk?.MasterWidgetFk?.Publish ?? false,
                        HubId = c.HubWidgetMapFk?.HubId,
                    }).ToList();


                foreach (var widget in widgets)
                {
                    widget.Contents = dataList.Where(c => c.HubWidgetMapFk?.MasterWidgetFk?.Id == widget.Id).Select(c => new HwsContentJsonViewDto
                    {
                        Id = c?.Id,
                        WidgetId = widget.Id,
                        HubId = widget?.HubId,
                        Title = c?.ContentFk?.Title,
                        Body = c?.ContentFk?.Body,
                        TenantId = c?.TenantId,
                        ContentTypeId = (c?.ContentFk?.ContentTypeId ?? 0) > 0 ? (int)(c?.ContentFk?.ContentTypeId ?? 0) : 0,
                        PublishTime = c?.ContentFk?.PublishTime,
                        Published = c?.ContentFk?.Published,
                        PublishedDate = c?.ContentFk?.PublishedDate,
                        SeoDescription = c?.ContentFk?.SeoDescription,
                        SeoKeywords = c?.ContentFk?.SeoKeywords,
                        SeoUrl = c?.ContentFk?.SeoUrl,
                        BannerMediaLibraryId = c?.ContentFk?.BannerMediaLibraryId,
                        BannerMediaLibraryName = c?.ContentFk?.BannerMediaLibraryFk?.Name,
                        BannerMediaLibraryAltTag = c?.ContentFk?.BannerMediaLibraryFk?.AltTag,
                        BannerMediaLibraryBinaryObjectId = c?.ContentFk?.BannerMediaLibraryFk?.BinaryObjectId,
                        BannerMediaLibraryBinaryVirtualPath = c?.ContentFk?.BannerMediaLibraryFk?.VirtualPath,

                    }).ToList();

                }

                return widgets;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }



        }
    }
}