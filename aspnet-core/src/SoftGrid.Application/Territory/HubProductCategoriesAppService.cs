using SoftGrid.Territory;
using SoftGrid.Shop;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.Territory.Exporting;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.Territory
{
    [AbpAuthorize(AppPermissions.Pages_HubProductCategories)]
    public class HubProductCategoriesAppService : SoftGridAppServiceBase, IHubProductCategoriesAppService
    {
        private readonly IRepository<HubProductCategory, long> _hubProductCategoryRepository;
        private readonly IHubProductCategoriesExcelExporter _hubProductCategoriesExcelExporter;
        private readonly IRepository<Hub, long> _lookup_hubRepository;
        private readonly IRepository<ProductCategory, long> _lookup_productCategoryRepository;

        public HubProductCategoriesAppService(IRepository<HubProductCategory, long> hubProductCategoryRepository, IHubProductCategoriesExcelExporter hubProductCategoriesExcelExporter, IRepository<Hub, long> lookup_hubRepository, IRepository<ProductCategory, long> lookup_productCategoryRepository)
        {
            _hubProductCategoryRepository = hubProductCategoryRepository;
            _hubProductCategoriesExcelExporter = hubProductCategoriesExcelExporter;
            _lookup_hubRepository = lookup_hubRepository;
            _lookup_productCategoryRepository = lookup_productCategoryRepository;

        }

        public async Task<PagedResultDto<GetHubProductCategoryForViewDto>> GetAll(GetAllHubProductCategoriesInput input)
        {

            var filteredHubProductCategories = _hubProductCategoryRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.ProductCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published))
                        .WhereIf(input.MinDisplayScoreFilter != null, e => e.DisplayScore >= input.MinDisplayScoreFilter)
                        .WhereIf(input.MaxDisplayScoreFilter != null, e => e.DisplayScore <= input.MaxDisplayScoreFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter);

            var pagedAndFilteredHubProductCategories = filteredHubProductCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var hubProductCategories = from o in pagedAndFilteredHubProductCategories
                                       join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                                       from s1 in j1.DefaultIfEmpty()

                                       join o2 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o2.Id into j2
                                       from s2 in j2.DefaultIfEmpty()

                                       select new
                                       {

                                           o.Published,
                                           o.DisplayScore,
                                           Id = o.Id,
                                           HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                           ProductCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                       };

            var totalCount = await filteredHubProductCategories.CountAsync();

            var dbList = await hubProductCategories.ToListAsync();
            var results = new List<GetHubProductCategoryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetHubProductCategoryForViewDto()
                {
                    HubProductCategory = new HubProductCategoryDto
                    {

                        Published = o.Published,
                        DisplayScore = o.DisplayScore,
                        Id = o.Id,
                    },
                    HubName = o.HubName,
                    ProductCategoryName = o.ProductCategoryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetHubProductCategoryForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetHubProductCategoryForViewDto> GetHubProductCategoryForView(long id)
        {
            var hubProductCategory = await _hubProductCategoryRepository.GetAsync(id);

            var output = new GetHubProductCategoryForViewDto { HubProductCategory = ObjectMapper.Map<HubProductCategoryDto>(hubProductCategory) };

            if (output.HubProductCategory.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.HubProductCategory.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.HubProductCategory.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.HubProductCategory.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_HubProductCategories_Edit)]
        public async Task<GetHubProductCategoryForEditOutput> GetHubProductCategoryForEdit(EntityDto<long> input)
        {
            var hubProductCategory = await _hubProductCategoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHubProductCategoryForEditOutput { HubProductCategory = ObjectMapper.Map<CreateOrEditHubProductCategoryDto>(hubProductCategory) };

            if (output.HubProductCategory.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.HubProductCategory.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.HubProductCategory.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.HubProductCategory.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHubProductCategoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_HubProductCategories_Create)]
        protected virtual async Task Create(CreateOrEditHubProductCategoryDto input)
        {
            var hubProductCategory = ObjectMapper.Map<HubProductCategory>(input);

            if (AbpSession.TenantId != null)
            {
                hubProductCategory.TenantId = (int?)AbpSession.TenantId;
            }

            await _hubProductCategoryRepository.InsertAsync(hubProductCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_HubProductCategories_Edit)]
        protected virtual async Task Update(CreateOrEditHubProductCategoryDto input)
        {
            var hubProductCategory = await _hubProductCategoryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, hubProductCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_HubProductCategories_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _hubProductCategoryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetHubProductCategoriesToExcel(GetAllHubProductCategoriesForExcelInput input)
        {

            var filteredHubProductCategories = _hubProductCategoryRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.ProductCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published))
                        .WhereIf(input.MinDisplayScoreFilter != null, e => e.DisplayScore >= input.MinDisplayScoreFilter)
                        .WhereIf(input.MaxDisplayScoreFilter != null, e => e.DisplayScore <= input.MaxDisplayScoreFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter);

            var query = (from o in filteredHubProductCategories
                         join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetHubProductCategoryForViewDto()
                         {
                             HubProductCategory = new HubProductCategoryDto
                             {
                                 Published = o.Published,
                                 DisplayScore = o.DisplayScore,
                                 Id = o.Id
                             },
                             HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ProductCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var hubProductCategoryListDtos = await query.ToListAsync();

            return _hubProductCategoriesExcelExporter.ExportToFile(hubProductCategoryListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_HubProductCategories)]
        public async Task<PagedResultDto<HubProductCategoryHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_hubRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var hubList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubProductCategoryHubLookupTableDto>();
            foreach (var hub in hubList)
            {
                lookupTableDtoList.Add(new HubProductCategoryHubLookupTableDto
                {
                    Id = hub.Id,
                    DisplayName = hub.Name?.ToString()
                });
            }

            return new PagedResultDto<HubProductCategoryHubLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HubProductCategories)]
        public async Task<PagedResultDto<HubProductCategoryProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubProductCategoryProductCategoryLookupTableDto>();
            foreach (var productCategory in productCategoryList)
            {
                lookupTableDtoList.Add(new HubProductCategoryProductCategoryLookupTableDto
                {
                    Id = productCategory.Id,
                    DisplayName = productCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<HubProductCategoryProductCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}