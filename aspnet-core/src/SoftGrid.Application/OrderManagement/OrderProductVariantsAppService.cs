using SoftGrid.Shop;
using SoftGrid.Shop;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.OrderManagement.Exporting;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.OrderManagement
{
    [AbpAuthorize(AppPermissions.Pages_OrderProductVariants)]
    public class OrderProductVariantsAppService : SoftGridAppServiceBase, IOrderProductVariantsAppService
    {
        private readonly IRepository<OrderProductVariant, long> _orderProductVariantRepository;
        private readonly IOrderProductVariantsExcelExporter _orderProductVariantsExcelExporter;
        private readonly IRepository<ProductVariantCategory, long> _lookup_productVariantCategoryRepository;
        private readonly IRepository<ProductVariant, long> _lookup_productVariantRepository;

        public OrderProductVariantsAppService(IRepository<OrderProductVariant, long> orderProductVariantRepository, IOrderProductVariantsExcelExporter orderProductVariantsExcelExporter, IRepository<ProductVariantCategory, long> lookup_productVariantCategoryRepository, IRepository<ProductVariant, long> lookup_productVariantRepository)
        {
            _orderProductVariantRepository = orderProductVariantRepository;
            _orderProductVariantsExcelExporter = orderProductVariantsExcelExporter;
            _lookup_productVariantCategoryRepository = lookup_productVariantCategoryRepository;
            _lookup_productVariantRepository = lookup_productVariantRepository;

        }

        public async Task<PagedResultDto<GetOrderProductVariantForViewDto>> GetAll(GetAllOrderProductVariantsInput input)
        {

            var filteredOrderProductVariants = _orderProductVariantRepository.GetAll()
                        .Include(e => e.ProductVariantCategoryFk)
                        .Include(e => e.ProductVariantFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(input.MinOrderProductInfoIdFilter != null, e => e.OrderProductInfoId >= input.MinOrderProductInfoIdFilter)
                        .WhereIf(input.MaxOrderProductInfoIdFilter != null, e => e.OrderProductInfoId <= input.MaxOrderProductInfoIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductVariantCategoryNameFilter), e => e.ProductVariantCategoryFk != null && e.ProductVariantCategoryFk.Name == input.ProductVariantCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductVariantNameFilter), e => e.ProductVariantFk != null && e.ProductVariantFk.Name == input.ProductVariantNameFilter);

            var pagedAndFilteredOrderProductVariants = filteredOrderProductVariants
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var orderProductVariants = from o in pagedAndFilteredOrderProductVariants
                                       join o1 in _lookup_productVariantCategoryRepository.GetAll() on o.ProductVariantCategoryId equals o1.Id into j1
                                       from s1 in j1.DefaultIfEmpty()

                                       join o2 in _lookup_productVariantRepository.GetAll() on o.ProductVariantId equals o2.Id into j2
                                       from s2 in j2.DefaultIfEmpty()

                                       select new
                                       {

                                           o.Price,
                                           o.OrderProductInfoId,
                                           Id = o.Id,
                                           ProductVariantCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                           ProductVariantName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                       };

            var totalCount = await filteredOrderProductVariants.CountAsync();

            var dbList = await orderProductVariants.ToListAsync();
            var results = new List<GetOrderProductVariantForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOrderProductVariantForViewDto()
                {
                    OrderProductVariant = new OrderProductVariantDto
                    {

                        Price = o.Price,
                        OrderProductInfoId = o.OrderProductInfoId,
                        Id = o.Id,
                    },
                    ProductVariantCategoryName = o.ProductVariantCategoryName,
                    ProductVariantName = o.ProductVariantName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOrderProductVariantForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOrderProductVariantForViewDto> GetOrderProductVariantForView(long id)
        {
            var orderProductVariant = await _orderProductVariantRepository.GetAsync(id);

            var output = new GetOrderProductVariantForViewDto { OrderProductVariant = ObjectMapper.Map<OrderProductVariantDto>(orderProductVariant) };

            if (output.OrderProductVariant.ProductVariantCategoryId != null)
            {
                var _lookupProductVariantCategory = await _lookup_productVariantCategoryRepository.FirstOrDefaultAsync((long)output.OrderProductVariant.ProductVariantCategoryId);
                output.ProductVariantCategoryName = _lookupProductVariantCategory?.Name?.ToString();
            }

            if (output.OrderProductVariant.ProductVariantId != null)
            {
                var _lookupProductVariant = await _lookup_productVariantRepository.FirstOrDefaultAsync((long)output.OrderProductVariant.ProductVariantId);
                output.ProductVariantName = _lookupProductVariant?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_OrderProductVariants_Edit)]
        public async Task<GetOrderProductVariantForEditOutput> GetOrderProductVariantForEdit(EntityDto<long> input)
        {
            var orderProductVariant = await _orderProductVariantRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOrderProductVariantForEditOutput { OrderProductVariant = ObjectMapper.Map<CreateOrEditOrderProductVariantDto>(orderProductVariant) };

            if (output.OrderProductVariant.ProductVariantCategoryId != null)
            {
                var _lookupProductVariantCategory = await _lookup_productVariantCategoryRepository.FirstOrDefaultAsync((long)output.OrderProductVariant.ProductVariantCategoryId);
                output.ProductVariantCategoryName = _lookupProductVariantCategory?.Name?.ToString();
            }

            if (output.OrderProductVariant.ProductVariantId != null)
            {
                var _lookupProductVariant = await _lookup_productVariantRepository.FirstOrDefaultAsync((long)output.OrderProductVariant.ProductVariantId);
                output.ProductVariantName = _lookupProductVariant?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOrderProductVariantDto input)
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

        [AbpAuthorize(AppPermissions.Pages_OrderProductVariants_Create)]
        protected virtual async Task Create(CreateOrEditOrderProductVariantDto input)
        {
            var orderProductVariant = ObjectMapper.Map<OrderProductVariant>(input);

            if (AbpSession.TenantId != null)
            {
                orderProductVariant.TenantId = (int?)AbpSession.TenantId;
            }

            await _orderProductVariantRepository.InsertAsync(orderProductVariant);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderProductVariants_Edit)]
        protected virtual async Task Update(CreateOrEditOrderProductVariantDto input)
        {
            var orderProductVariant = await _orderProductVariantRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, orderProductVariant);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderProductVariants_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _orderProductVariantRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetOrderProductVariantsToExcel(GetAllOrderProductVariantsForExcelInput input)
        {

            var filteredOrderProductVariants = _orderProductVariantRepository.GetAll()
                        .Include(e => e.ProductVariantCategoryFk)
                        .Include(e => e.ProductVariantFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(input.MinOrderProductInfoIdFilter != null, e => e.OrderProductInfoId >= input.MinOrderProductInfoIdFilter)
                        .WhereIf(input.MaxOrderProductInfoIdFilter != null, e => e.OrderProductInfoId <= input.MaxOrderProductInfoIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductVariantCategoryNameFilter), e => e.ProductVariantCategoryFk != null && e.ProductVariantCategoryFk.Name == input.ProductVariantCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductVariantNameFilter), e => e.ProductVariantFk != null && e.ProductVariantFk.Name == input.ProductVariantNameFilter);

            var query = (from o in filteredOrderProductVariants
                         join o1 in _lookup_productVariantCategoryRepository.GetAll() on o.ProductVariantCategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productVariantRepository.GetAll() on o.ProductVariantId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetOrderProductVariantForViewDto()
                         {
                             OrderProductVariant = new OrderProductVariantDto
                             {
                                 Price = o.Price,
                                 OrderProductInfoId = o.OrderProductInfoId,
                                 Id = o.Id
                             },
                             ProductVariantCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ProductVariantName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var orderProductVariantListDtos = await query.ToListAsync();

            return _orderProductVariantsExcelExporter.ExportToFile(orderProductVariantListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_OrderProductVariants)]
        public async Task<PagedResultDto<OrderProductVariantProductVariantCategoryLookupTableDto>> GetAllProductVariantCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productVariantCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productVariantCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderProductVariantProductVariantCategoryLookupTableDto>();
            foreach (var productVariantCategory in productVariantCategoryList)
            {
                lookupTableDtoList.Add(new OrderProductVariantProductVariantCategoryLookupTableDto
                {
                    Id = productVariantCategory.Id,
                    DisplayName = productVariantCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderProductVariantProductVariantCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_OrderProductVariants)]
        public async Task<PagedResultDto<OrderProductVariantProductVariantLookupTableDto>> GetAllProductVariantForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productVariantRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productVariantList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderProductVariantProductVariantLookupTableDto>();
            foreach (var productVariant in productVariantList)
            {
                lookupTableDtoList.Add(new OrderProductVariantProductVariantLookupTableDto
                {
                    Id = productVariant.Id,
                    DisplayName = productVariant.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderProductVariantProductVariantLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}