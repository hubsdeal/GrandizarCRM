using SoftGrid.Shop;
using SoftGrid.TaskManagement;
using SoftGrid.Shop;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.Shop.Exporting;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.Shop
{
    [AbpAuthorize(AppPermissions.Pages_ProductTaskMaps)]
    public class ProductTaskMapsAppService : SoftGridAppServiceBase, IProductTaskMapsAppService
    {
        private readonly IRepository<ProductTaskMap, long> _productTaskMapRepository;
        private readonly IProductTaskMapsExcelExporter _productTaskMapsExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<TaskEvent, long> _lookup_taskEventRepository;
        private readonly IRepository<ProductCategory, long> _lookup_productCategoryRepository;

        public ProductTaskMapsAppService(IRepository<ProductTaskMap, long> productTaskMapRepository, IProductTaskMapsExcelExporter productTaskMapsExcelExporter, IRepository<Product, long> lookup_productRepository, IRepository<TaskEvent, long> lookup_taskEventRepository, IRepository<ProductCategory, long> lookup_productCategoryRepository)
        {
            _productTaskMapRepository = productTaskMapRepository;
            _productTaskMapsExcelExporter = productTaskMapsExcelExporter;
            _lookup_productRepository = lookup_productRepository;
            _lookup_taskEventRepository = lookup_taskEventRepository;
            _lookup_productCategoryRepository = lookup_productCategoryRepository;

        }

        public async Task<PagedResultDto<GetProductTaskMapForViewDto>> GetAll(GetAllProductTaskMapsInput input)
        {

            var filteredProductTaskMaps = _productTaskMapRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.TaskEventFk)
                        .Include(e => e.ProductCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskEventNameFilter), e => e.TaskEventFk != null && e.TaskEventFk.Name == input.TaskEventNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter);

            var pagedAndFilteredProductTaskMaps = filteredProductTaskMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productTaskMaps = from o in pagedAndFilteredProductTaskMaps
                                  join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                  from s1 in j1.DefaultIfEmpty()

                                  join o2 in _lookup_taskEventRepository.GetAll() on o.TaskEventId equals o2.Id into j2
                                  from s2 in j2.DefaultIfEmpty()

                                  join o3 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o3.Id into j3
                                  from s3 in j3.DefaultIfEmpty()

                                  select new
                                  {

                                      Id = o.Id,
                                      ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                      TaskEventName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                      ProductCategoryName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                                  };

            var totalCount = await filteredProductTaskMaps.CountAsync();

            var dbList = await productTaskMaps.ToListAsync();
            var results = new List<GetProductTaskMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductTaskMapForViewDto()
                {
                    ProductTaskMap = new ProductTaskMapDto
                    {

                        Id = o.Id,
                    },
                    ProductName = o.ProductName,
                    TaskEventName = o.TaskEventName,
                    ProductCategoryName = o.ProductCategoryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductTaskMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductTaskMapForViewDto> GetProductTaskMapForView(long id)
        {
            var productTaskMap = await _productTaskMapRepository.GetAsync(id);

            var output = new GetProductTaskMapForViewDto { ProductTaskMap = ObjectMapper.Map<ProductTaskMapDto>(productTaskMap) };

            if (output.ProductTaskMap.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductTaskMap.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductTaskMap.TaskEventId != null)
            {
                var _lookupTaskEvent = await _lookup_taskEventRepository.FirstOrDefaultAsync((long)output.ProductTaskMap.TaskEventId);
                output.TaskEventName = _lookupTaskEvent?.Name?.ToString();
            }

            if (output.ProductTaskMap.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.ProductTaskMap.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductTaskMaps_Edit)]
        public async Task<GetProductTaskMapForEditOutput> GetProductTaskMapForEdit(EntityDto<long> input)
        {
            var productTaskMap = await _productTaskMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductTaskMapForEditOutput { ProductTaskMap = ObjectMapper.Map<CreateOrEditProductTaskMapDto>(productTaskMap) };

            if (output.ProductTaskMap.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductTaskMap.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductTaskMap.TaskEventId != null)
            {
                var _lookupTaskEvent = await _lookup_taskEventRepository.FirstOrDefaultAsync((long)output.ProductTaskMap.TaskEventId);
                output.TaskEventName = _lookupTaskEvent?.Name?.ToString();
            }

            if (output.ProductTaskMap.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.ProductTaskMap.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductTaskMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductTaskMaps_Create)]
        protected virtual async Task Create(CreateOrEditProductTaskMapDto input)
        {
            var productTaskMap = ObjectMapper.Map<ProductTaskMap>(input);

            if (AbpSession.TenantId != null)
            {
                productTaskMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _productTaskMapRepository.InsertAsync(productTaskMap);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductTaskMaps_Edit)]
        protected virtual async Task Update(CreateOrEditProductTaskMapDto input)
        {
            var productTaskMap = await _productTaskMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productTaskMap);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductTaskMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productTaskMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductTaskMapsToExcel(GetAllProductTaskMapsForExcelInput input)
        {

            var filteredProductTaskMaps = _productTaskMapRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.TaskEventFk)
                        .Include(e => e.ProductCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskEventNameFilter), e => e.TaskEventFk != null && e.TaskEventFk.Name == input.TaskEventNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter);

            var query = (from o in filteredProductTaskMaps
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_taskEventRepository.GetAll() on o.TaskEventId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetProductTaskMapForViewDto()
                         {
                             ProductTaskMap = new ProductTaskMapDto
                             {
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             TaskEventName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             ProductCategoryName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var productTaskMapListDtos = await query.ToListAsync();

            return _productTaskMapsExcelExporter.ExportToFile(productTaskMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductTaskMaps)]
        public async Task<PagedResultDto<ProductTaskMapProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductTaskMapProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductTaskMapProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductTaskMapProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductTaskMaps)]
        public async Task<PagedResultDto<ProductTaskMapTaskEventLookupTableDto>> GetAllTaskEventForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_taskEventRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var taskEventList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductTaskMapTaskEventLookupTableDto>();
            foreach (var taskEvent in taskEventList)
            {
                lookupTableDtoList.Add(new ProductTaskMapTaskEventLookupTableDto
                {
                    Id = taskEvent.Id,
                    DisplayName = taskEvent.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductTaskMapTaskEventLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductTaskMaps)]
        public async Task<PagedResultDto<ProductTaskMapProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductTaskMapProductCategoryLookupTableDto>();
            foreach (var productCategory in productCategoryList)
            {
                lookupTableDtoList.Add(new ProductTaskMapProductCategoryLookupTableDto
                {
                    Id = productCategory.Id,
                    DisplayName = productCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductTaskMapProductCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}