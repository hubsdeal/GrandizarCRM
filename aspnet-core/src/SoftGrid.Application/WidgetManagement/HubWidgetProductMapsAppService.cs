using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;

using Microsoft.EntityFrameworkCore;

using SoftGrid.Authorization;
using SoftGrid.Dto;
using SoftGrid.Shop;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.WidgetManagement.Exporting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace SoftGrid.WidgetManagement
{
    [AbpAuthorize(AppPermissions.Pages_HubWidgetProductMaps)]
    public class HubWidgetProductMapsAppService : SoftGridAppServiceBase, IHubWidgetProductMapsAppService
    {
        private readonly IRepository<HubWidgetProductMap, long> _hubWidgetProductMapRepository;
        private readonly IHubWidgetProductMapsExcelExporter _hubWidgetProductMapsExcelExporter;
        private readonly IRepository<HubWidgetMap, long> _lookup_hubWidgetMapRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;

        public HubWidgetProductMapsAppService(IRepository<HubWidgetProductMap, long> hubWidgetProductMapRepository, IHubWidgetProductMapsExcelExporter hubWidgetProductMapsExcelExporter, IRepository<HubWidgetMap, long> lookup_hubWidgetMapRepository, IRepository<Product, long> lookup_productRepository)
        {
            _hubWidgetProductMapRepository = hubWidgetProductMapRepository;
            _hubWidgetProductMapsExcelExporter = hubWidgetProductMapsExcelExporter;
            _lookup_hubWidgetMapRepository = lookup_hubWidgetMapRepository;
            _lookup_productRepository = lookup_productRepository;

        }

        public async Task<PagedResultDto<GetHubWidgetProductMapForViewDto>> GetAll(GetAllHubWidgetProductMapsInput input)
        {

            var filteredHubWidgetProductMaps = _hubWidgetProductMapRepository.GetAll()
                        .Include(e => e.HubWidgetMapFk)
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubWidgetMapCustomNameFilter), e => e.HubWidgetMapFk != null && e.HubWidgetMapFk.CustomName == input.HubWidgetMapCustomNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var pagedAndFilteredHubWidgetProductMaps = filteredHubWidgetProductMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var hubWidgetProductMaps = from o in pagedAndFilteredHubWidgetProductMaps
                                       join o1 in _lookup_hubWidgetMapRepository.GetAll() on o.HubWidgetMapId equals o1.Id into j1
                                       from s1 in j1.DefaultIfEmpty()

                                       join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                                       from s2 in j2.DefaultIfEmpty()

                                       select new
                                       {

                                           o.DisplaySequence,
                                           Id = o.Id,
                                           HubWidgetMapCustomName = s1 == null || s1.CustomName == null ? "" : s1.CustomName.ToString(),
                                           ProductName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                       };

            var totalCount = await filteredHubWidgetProductMaps.CountAsync();

            var dbList = await hubWidgetProductMaps.ToListAsync();
            var results = new List<GetHubWidgetProductMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetHubWidgetProductMapForViewDto()
                {
                    HubWidgetProductMap = new HubWidgetProductMapDto
                    {

                        DisplaySequence = o.DisplaySequence,
                        Id = o.Id,
                    },
                    HubWidgetMapCustomName = o.HubWidgetMapCustomName,
                    ProductName = o.ProductName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetHubWidgetProductMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetHubWidgetProductMapForViewDto> GetHubWidgetProductMapForView(long id)
        {
            var hubWidgetProductMap = await _hubWidgetProductMapRepository.GetAsync(id);

            var output = new GetHubWidgetProductMapForViewDto { HubWidgetProductMap = ObjectMapper.Map<HubWidgetProductMapDto>(hubWidgetProductMap) };

            if (output.HubWidgetProductMap.HubWidgetMapId != null)
            {
                var _lookupHubWidgetMap = await _lookup_hubWidgetMapRepository.FirstOrDefaultAsync((long)output.HubWidgetProductMap.HubWidgetMapId);
                output.HubWidgetMapCustomName = _lookupHubWidgetMap?.CustomName?.ToString();
            }

            if (output.HubWidgetProductMap.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.HubWidgetProductMap.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_HubWidgetProductMaps_Edit)]
        public async Task<GetHubWidgetProductMapForEditOutput> GetHubWidgetProductMapForEdit(EntityDto<long> input)
        {
            var hubWidgetProductMap = await _hubWidgetProductMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHubWidgetProductMapForEditOutput { HubWidgetProductMap = ObjectMapper.Map<CreateOrEditHubWidgetProductMapDto>(hubWidgetProductMap) };

            if (output.HubWidgetProductMap.HubWidgetMapId != null)
            {
                var _lookupHubWidgetMap = await _lookup_hubWidgetMapRepository.FirstOrDefaultAsync((long)output.HubWidgetProductMap.HubWidgetMapId);
                output.HubWidgetMapCustomName = _lookupHubWidgetMap?.CustomName?.ToString();
            }

            if (output.HubWidgetProductMap.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.HubWidgetProductMap.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHubWidgetProductMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_HubWidgetProductMaps_Create)]
        protected virtual async Task Create(CreateOrEditHubWidgetProductMapDto input)
        {
            var hubWidgetProductMap = ObjectMapper.Map<HubWidgetProductMap>(input);

            if (AbpSession.TenantId != null)
            {
                hubWidgetProductMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _hubWidgetProductMapRepository.InsertAsync(hubWidgetProductMap);

        }

        [AbpAuthorize(AppPermissions.Pages_HubWidgetProductMaps_Edit)]
        protected virtual async Task Update(CreateOrEditHubWidgetProductMapDto input)
        {
            var hubWidgetProductMap = await _hubWidgetProductMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, hubWidgetProductMap);

        }

        [AbpAuthorize(AppPermissions.Pages_HubWidgetProductMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _hubWidgetProductMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetHubWidgetProductMapsToExcel(GetAllHubWidgetProductMapsForExcelInput input)
        {

            var filteredHubWidgetProductMaps = _hubWidgetProductMapRepository.GetAll()
                        .Include(e => e.HubWidgetMapFk)
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubWidgetMapCustomNameFilter), e => e.HubWidgetMapFk != null && e.HubWidgetMapFk.CustomName == input.HubWidgetMapCustomNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var query = (from o in filteredHubWidgetProductMaps
                         join o1 in _lookup_hubWidgetMapRepository.GetAll() on o.HubWidgetMapId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetHubWidgetProductMapForViewDto()
                         {
                             HubWidgetProductMap = new HubWidgetProductMapDto
                             {
                                 DisplaySequence = o.DisplaySequence,
                                 Id = o.Id
                             },
                             HubWidgetMapCustomName = s1 == null || s1.CustomName == null ? "" : s1.CustomName.ToString(),
                             ProductName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var hubWidgetProductMapListDtos = await query.ToListAsync();

            return _hubWidgetProductMapsExcelExporter.ExportToFile(hubWidgetProductMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_HubWidgetProductMaps)]
        public async Task<PagedResultDto<HubWidgetProductMapHubWidgetMapLookupTableDto>> GetAllHubWidgetMapForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_hubWidgetMapRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.CustomName != null && e.CustomName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var hubWidgetMapList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubWidgetProductMapHubWidgetMapLookupTableDto>();
            foreach (var hubWidgetMap in hubWidgetMapList)
            {
                lookupTableDtoList.Add(new HubWidgetProductMapHubWidgetMapLookupTableDto
                {
                    Id = hubWidgetMap.Id,
                    DisplayName = hubWidgetMap.CustomName?.ToString()
                });
            }

            return new PagedResultDto<HubWidgetProductMapHubWidgetMapLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HubWidgetProductMaps)]
        public async Task<PagedResultDto<HubWidgetProductMapProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubWidgetProductMapProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new HubWidgetProductMapProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<HubWidgetProductMapProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }



        [AbpAllowAnonymous]
        public async Task<dynamic> GetHubWidgetStoreProductsByHubId(long hubId)
        {
            try
            {
                var dataList = await _hubWidgetProductMapRepository.GetAll()
                .Include(c => c.HubWidgetMapFk).ThenInclude(c => c.MasterWidgetFk)
                .Include(c => c.ProductFk).ThenInclude(c => c.StoreFk)
                .Include(c => c.ProductFk).ThenInclude(c => c.ContactFk)
                .Include(c => c.ProductFk).ThenInclude(c => c.ProductCategoryFk)
                .Include(c => c.ProductFk).ThenInclude(c => c.MediaLibraryFk)
                .Include(c => c.ProductFk).ThenInclude(c => c.RatingLikeFk)
                .Where(c => c.HubWidgetMapFk.HubId == hubId).ToListAsync();

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
                    widget.Products = dataList.Where(c => c.HubWidgetMapFk?.MasterWidgetFk?.Id == widget.Id).Select(c => c.ProductFk).Select(c => new HwsProductJsonViewDto
                    {
                        Id = c?.Id,
                        WidgetId = widget.Id,
                        HubId = widget?.HubId,
                        StoreId = c?.StoreId,
                        Name = c?.Name,
                        Description = c?.Description,
                        TenantId = c?.TenantId,
                        Score = c?.Score,
                        RatingLikeId = c?.RatingLikeId,
                        RatingLikeScore = c?.RatingLikeFk?.Score,
                        RatingLikeName = c?.RatingLikeFk?.Name,

                        ContactId = c?.ContactId,


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