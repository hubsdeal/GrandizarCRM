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
    [AbpAuthorize(AppPermissions.Pages_HubProducts)]
    public class HubProductsAppService : SoftGridAppServiceBase, IHubProductsAppService
    {
        private readonly IRepository<HubProduct, long> _hubProductRepository;
        private readonly IHubProductsExcelExporter _hubProductsExcelExporter;
        private readonly IRepository<Hub, long> _lookup_hubRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;

        public HubProductsAppService(IRepository<HubProduct, long> hubProductRepository, IHubProductsExcelExporter hubProductsExcelExporter, IRepository<Hub, long> lookup_hubRepository, IRepository<Product, long> lookup_productRepository)
        {
            _hubProductRepository = hubProductRepository;
            _hubProductsExcelExporter = hubProductsExcelExporter;
            _lookup_hubRepository = lookup_hubRepository;
            _lookup_productRepository = lookup_productRepository;

        }

        public async Task<PagedResultDto<GetHubProductForViewDto>> GetAll(GetAllHubProductsInput input)
        {

            var filteredHubProducts = _hubProductRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published))
                        .WhereIf(input.MinDisplayScoreFilter != null, e => e.DisplayScore >= input.MinDisplayScoreFilter)
                        .WhereIf(input.MaxDisplayScoreFilter != null, e => e.DisplayScore <= input.MaxDisplayScoreFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var pagedAndFilteredHubProducts = filteredHubProducts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var hubProducts = from o in pagedAndFilteredHubProducts
                              join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                              from s1 in j1.DefaultIfEmpty()

                              join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                              from s2 in j2.DefaultIfEmpty()

                              select new
                              {

                                  o.Published,
                                  o.DisplayScore,
                                  Id = o.Id,
                                  HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                  ProductName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                              };

            var totalCount = await filteredHubProducts.CountAsync();

            var dbList = await hubProducts.ToListAsync();
            var results = new List<GetHubProductForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetHubProductForViewDto()
                {
                    HubProduct = new HubProductDto
                    {

                        Published = o.Published,
                        DisplayScore = o.DisplayScore,
                        Id = o.Id,
                    },
                    HubName = o.HubName,
                    ProductName = o.ProductName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetHubProductForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetHubProductForViewDto> GetHubProductForView(long id)
        {
            var hubProduct = await _hubProductRepository.GetAsync(id);

            var output = new GetHubProductForViewDto { HubProduct = ObjectMapper.Map<HubProductDto>(hubProduct) };

            if (output.HubProduct.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.HubProduct.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.HubProduct.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.HubProduct.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_HubProducts_Edit)]
        public async Task<GetHubProductForEditOutput> GetHubProductForEdit(EntityDto<long> input)
        {
            var hubProduct = await _hubProductRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHubProductForEditOutput { HubProduct = ObjectMapper.Map<CreateOrEditHubProductDto>(hubProduct) };

            if (output.HubProduct.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.HubProduct.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.HubProduct.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.HubProduct.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHubProductDto input)
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

        [AbpAuthorize(AppPermissions.Pages_HubProducts_Create)]
        protected virtual async Task Create(CreateOrEditHubProductDto input)
        {
            var hubProduct = ObjectMapper.Map<HubProduct>(input);

            if (AbpSession.TenantId != null)
            {
                hubProduct.TenantId = (int?)AbpSession.TenantId;
            }

            await _hubProductRepository.InsertAsync(hubProduct);

        }

        [AbpAuthorize(AppPermissions.Pages_HubProducts_Edit)]
        protected virtual async Task Update(CreateOrEditHubProductDto input)
        {
            var hubProduct = await _hubProductRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, hubProduct);

        }

        [AbpAuthorize(AppPermissions.Pages_HubProducts_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _hubProductRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetHubProductsToExcel(GetAllHubProductsForExcelInput input)
        {

            var filteredHubProducts = _hubProductRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published))
                        .WhereIf(input.MinDisplayScoreFilter != null, e => e.DisplayScore >= input.MinDisplayScoreFilter)
                        .WhereIf(input.MaxDisplayScoreFilter != null, e => e.DisplayScore <= input.MaxDisplayScoreFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var query = (from o in filteredHubProducts
                         join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetHubProductForViewDto()
                         {
                             HubProduct = new HubProductDto
                             {
                                 Published = o.Published,
                                 DisplayScore = o.DisplayScore,
                                 Id = o.Id
                             },
                             HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ProductName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var hubProductListDtos = await query.ToListAsync();

            return _hubProductsExcelExporter.ExportToFile(hubProductListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_HubProducts)]
        public async Task<PagedResultDto<HubProductHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_hubRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var hubList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubProductHubLookupTableDto>();
            foreach (var hub in hubList)
            {
                lookupTableDtoList.Add(new HubProductHubLookupTableDto
                {
                    Id = hub.Id,
                    DisplayName = hub.Name?.ToString()
                });
            }

            return new PagedResultDto<HubProductHubLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HubProducts)]
        public async Task<PagedResultDto<HubProductProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubProductProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new HubProductProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<HubProductProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}