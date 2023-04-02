using SoftGrid.OrderManagement;
using SoftGrid.Shop;
using SoftGrid.Shop;
using SoftGrid.LookupData;

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
    [AbpAuthorize(AppPermissions.Pages_OrderProductInfos)]
    public class OrderProductInfosAppService : SoftGridAppServiceBase, IOrderProductInfosAppService
    {
        private readonly IRepository<OrderProductInfo, long> _orderProductInfoRepository;
        private readonly IOrderProductInfosExcelExporter _orderProductInfosExcelExporter;
        private readonly IRepository<Order, long> _lookup_orderRepository;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<MeasurementUnit, long> _lookup_measurementUnitRepository;

        public OrderProductInfosAppService(IRepository<OrderProductInfo, long> orderProductInfoRepository, IOrderProductInfosExcelExporter orderProductInfosExcelExporter, IRepository<Order, long> lookup_orderRepository, IRepository<Store, long> lookup_storeRepository, IRepository<Product, long> lookup_productRepository, IRepository<MeasurementUnit, long> lookup_measurementUnitRepository)
        {
            _orderProductInfoRepository = orderProductInfoRepository;
            _orderProductInfosExcelExporter = orderProductInfosExcelExporter;
            _lookup_orderRepository = lookup_orderRepository;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_productRepository = lookup_productRepository;
            _lookup_measurementUnitRepository = lookup_measurementUnitRepository;

        }

        public async Task<PagedResultDto<GetOrderProductInfoForViewDto>> GetAll(GetAllOrderProductInfosInput input)
        {

            var filteredOrderProductInfos = _orderProductInfoRepository.GetAll()
                        .Include(e => e.OrderFk)
                        .Include(e => e.StoreFk)
                        .Include(e => e.ProductFk)
                        .Include(e => e.MeasurementUnitFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinQuantityFilter != null, e => e.Quantity >= input.MinQuantityFilter)
                        .WhereIf(input.MaxQuantityFilter != null, e => e.Quantity <= input.MaxQuantityFilter)
                        .WhereIf(input.MinUnitPriceFilter != null, e => e.UnitPrice >= input.MinUnitPriceFilter)
                        .WhereIf(input.MaxUnitPriceFilter != null, e => e.UnitPrice <= input.MaxUnitPriceFilter)
                        .WhereIf(input.MinByProductDiscountAmountFilter != null, e => e.ByProductDiscountAmount >= input.MinByProductDiscountAmountFilter)
                        .WhereIf(input.MaxByProductDiscountAmountFilter != null, e => e.ByProductDiscountAmount <= input.MaxByProductDiscountAmountFilter)
                        .WhereIf(input.MinByProductDiscountPercentageFilter != null, e => e.ByProductDiscountPercentage >= input.MinByProductDiscountPercentageFilter)
                        .WhereIf(input.MaxByProductDiscountPercentageFilter != null, e => e.ByProductDiscountPercentage <= input.MaxByProductDiscountPercentageFilter)
                        .WhereIf(input.MinByProductTaxAmountFilter != null, e => e.ByProductTaxAmount >= input.MinByProductTaxAmountFilter)
                        .WhereIf(input.MaxByProductTaxAmountFilter != null, e => e.ByProductTaxAmount <= input.MaxByProductTaxAmountFilter)
                        .WhereIf(input.MinByProductTotalAmountFilter != null, e => e.ByProductTotalAmount >= input.MinByProductTotalAmountFilter)
                        .WhereIf(input.MaxByProductTotalAmountFilter != null, e => e.ByProductTotalAmount <= input.MaxByProductTotalAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderInvoiceNumberFilter), e => e.OrderFk != null && e.OrderFk.InvoiceNumber == input.OrderInvoiceNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MeasurementUnitNameFilter), e => e.MeasurementUnitFk != null && e.MeasurementUnitFk.Name == input.MeasurementUnitNameFilter);

            var pagedAndFilteredOrderProductInfos = filteredOrderProductInfos
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var orderProductInfos = from o in pagedAndFilteredOrderProductInfos
                                    join o1 in _lookup_orderRepository.GetAll() on o.OrderId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    join o2 in _lookup_storeRepository.GetAll() on o.StoreId equals o2.Id into j2
                                    from s2 in j2.DefaultIfEmpty()

                                    join o3 in _lookup_productRepository.GetAll() on o.ProductId equals o3.Id into j3
                                    from s3 in j3.DefaultIfEmpty()

                                    join o4 in _lookup_measurementUnitRepository.GetAll() on o.MeasurementUnitId equals o4.Id into j4
                                    from s4 in j4.DefaultIfEmpty()

                                    select new
                                    {

                                        o.Quantity,
                                        o.UnitPrice,
                                        o.ByProductDiscountAmount,
                                        o.ByProductDiscountPercentage,
                                        o.ByProductTaxAmount,
                                        o.ByProductTotalAmount,
                                        Id = o.Id,
                                        OrderInvoiceNumber = s1 == null || s1.InvoiceNumber == null ? "" : s1.InvoiceNumber.ToString(),
                                        StoreName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                        ProductName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                                        MeasurementUnitName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                                    };

            var totalCount = await filteredOrderProductInfos.CountAsync();

            var dbList = await orderProductInfos.ToListAsync();
            var results = new List<GetOrderProductInfoForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOrderProductInfoForViewDto()
                {
                    OrderProductInfo = new OrderProductInfoDto
                    {

                        Quantity = o.Quantity,
                        UnitPrice = o.UnitPrice,
                        ByProductDiscountAmount = o.ByProductDiscountAmount,
                        ByProductDiscountPercentage = o.ByProductDiscountPercentage,
                        ByProductTaxAmount = o.ByProductTaxAmount,
                        ByProductTotalAmount = o.ByProductTotalAmount,
                        Id = o.Id,
                    },
                    OrderInvoiceNumber = o.OrderInvoiceNumber,
                    StoreName = o.StoreName,
                    ProductName = o.ProductName,
                    MeasurementUnitName = o.MeasurementUnitName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOrderProductInfoForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOrderProductInfoForViewDto> GetOrderProductInfoForView(long id)
        {
            var orderProductInfo = await _orderProductInfoRepository.GetAsync(id);

            var output = new GetOrderProductInfoForViewDto { OrderProductInfo = ObjectMapper.Map<OrderProductInfoDto>(orderProductInfo) };

            if (output.OrderProductInfo.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((long)output.OrderProductInfo.OrderId);
                output.OrderInvoiceNumber = _lookupOrder?.InvoiceNumber?.ToString();
            }

            if (output.OrderProductInfo.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.OrderProductInfo.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.OrderProductInfo.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.OrderProductInfo.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.OrderProductInfo.MeasurementUnitId != null)
            {
                var _lookupMeasurementUnit = await _lookup_measurementUnitRepository.FirstOrDefaultAsync((long)output.OrderProductInfo.MeasurementUnitId);
                output.MeasurementUnitName = _lookupMeasurementUnit?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_OrderProductInfos_Edit)]
        public async Task<GetOrderProductInfoForEditOutput> GetOrderProductInfoForEdit(EntityDto<long> input)
        {
            var orderProductInfo = await _orderProductInfoRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOrderProductInfoForEditOutput { OrderProductInfo = ObjectMapper.Map<CreateOrEditOrderProductInfoDto>(orderProductInfo) };

            if (output.OrderProductInfo.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((long)output.OrderProductInfo.OrderId);
                output.OrderInvoiceNumber = _lookupOrder?.InvoiceNumber?.ToString();
            }

            if (output.OrderProductInfo.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.OrderProductInfo.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.OrderProductInfo.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.OrderProductInfo.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.OrderProductInfo.MeasurementUnitId != null)
            {
                var _lookupMeasurementUnit = await _lookup_measurementUnitRepository.FirstOrDefaultAsync((long)output.OrderProductInfo.MeasurementUnitId);
                output.MeasurementUnitName = _lookupMeasurementUnit?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOrderProductInfoDto input)
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

        [AbpAuthorize(AppPermissions.Pages_OrderProductInfos_Create)]
        protected virtual async Task Create(CreateOrEditOrderProductInfoDto input)
        {
            var orderProductInfo = ObjectMapper.Map<OrderProductInfo>(input);

            if (AbpSession.TenantId != null)
            {
                orderProductInfo.TenantId = (int?)AbpSession.TenantId;
            }

            await _orderProductInfoRepository.InsertAsync(orderProductInfo);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderProductInfos_Edit)]
        protected virtual async Task Update(CreateOrEditOrderProductInfoDto input)
        {
            var orderProductInfo = await _orderProductInfoRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, orderProductInfo);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderProductInfos_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _orderProductInfoRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetOrderProductInfosToExcel(GetAllOrderProductInfosForExcelInput input)
        {

            var filteredOrderProductInfos = _orderProductInfoRepository.GetAll()
                        .Include(e => e.OrderFk)
                        .Include(e => e.StoreFk)
                        .Include(e => e.ProductFk)
                        .Include(e => e.MeasurementUnitFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinQuantityFilter != null, e => e.Quantity >= input.MinQuantityFilter)
                        .WhereIf(input.MaxQuantityFilter != null, e => e.Quantity <= input.MaxQuantityFilter)
                        .WhereIf(input.MinUnitPriceFilter != null, e => e.UnitPrice >= input.MinUnitPriceFilter)
                        .WhereIf(input.MaxUnitPriceFilter != null, e => e.UnitPrice <= input.MaxUnitPriceFilter)
                        .WhereIf(input.MinByProductDiscountAmountFilter != null, e => e.ByProductDiscountAmount >= input.MinByProductDiscountAmountFilter)
                        .WhereIf(input.MaxByProductDiscountAmountFilter != null, e => e.ByProductDiscountAmount <= input.MaxByProductDiscountAmountFilter)
                        .WhereIf(input.MinByProductDiscountPercentageFilter != null, e => e.ByProductDiscountPercentage >= input.MinByProductDiscountPercentageFilter)
                        .WhereIf(input.MaxByProductDiscountPercentageFilter != null, e => e.ByProductDiscountPercentage <= input.MaxByProductDiscountPercentageFilter)
                        .WhereIf(input.MinByProductTaxAmountFilter != null, e => e.ByProductTaxAmount >= input.MinByProductTaxAmountFilter)
                        .WhereIf(input.MaxByProductTaxAmountFilter != null, e => e.ByProductTaxAmount <= input.MaxByProductTaxAmountFilter)
                        .WhereIf(input.MinByProductTotalAmountFilter != null, e => e.ByProductTotalAmount >= input.MinByProductTotalAmountFilter)
                        .WhereIf(input.MaxByProductTotalAmountFilter != null, e => e.ByProductTotalAmount <= input.MaxByProductTotalAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderInvoiceNumberFilter), e => e.OrderFk != null && e.OrderFk.InvoiceNumber == input.OrderInvoiceNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MeasurementUnitNameFilter), e => e.MeasurementUnitFk != null && e.MeasurementUnitFk.Name == input.MeasurementUnitNameFilter);

            var query = (from o in filteredOrderProductInfos
                         join o1 in _lookup_orderRepository.GetAll() on o.OrderId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_storeRepository.GetAll() on o.StoreId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_productRepository.GetAll() on o.ProductId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_measurementUnitRepository.GetAll() on o.MeasurementUnitId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetOrderProductInfoForViewDto()
                         {
                             OrderProductInfo = new OrderProductInfoDto
                             {
                                 Quantity = o.Quantity,
                                 UnitPrice = o.UnitPrice,
                                 ByProductDiscountAmount = o.ByProductDiscountAmount,
                                 ByProductDiscountPercentage = o.ByProductDiscountPercentage,
                                 ByProductTaxAmount = o.ByProductTaxAmount,
                                 ByProductTotalAmount = o.ByProductTotalAmount,
                                 Id = o.Id
                             },
                             OrderInvoiceNumber = s1 == null || s1.InvoiceNumber == null ? "" : s1.InvoiceNumber.ToString(),
                             StoreName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             ProductName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             MeasurementUnitName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                         });

            var orderProductInfoListDtos = await query.ToListAsync();

            return _orderProductInfosExcelExporter.ExportToFile(orderProductInfoListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_OrderProductInfos)]
        public async Task<PagedResultDto<OrderProductInfoOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_orderRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.InvoiceNumber != null && e.InvoiceNumber.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var orderList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderProductInfoOrderLookupTableDto>();
            foreach (var order in orderList)
            {
                lookupTableDtoList.Add(new OrderProductInfoOrderLookupTableDto
                {
                    Id = order.Id,
                    DisplayName = order.InvoiceNumber?.ToString()
                });
            }

            return new PagedResultDto<OrderProductInfoOrderLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_OrderProductInfos)]
        public async Task<PagedResultDto<OrderProductInfoStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderProductInfoStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new OrderProductInfoStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderProductInfoStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_OrderProductInfos)]
        public async Task<PagedResultDto<OrderProductInfoProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderProductInfoProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new OrderProductInfoProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderProductInfoProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_OrderProductInfos)]
        public async Task<PagedResultDto<OrderProductInfoMeasurementUnitLookupTableDto>> GetAllMeasurementUnitForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_measurementUnitRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var measurementUnitList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderProductInfoMeasurementUnitLookupTableDto>();
            foreach (var measurementUnit in measurementUnitList)
            {
                lookupTableDtoList.Add(new OrderProductInfoMeasurementUnitLookupTableDto
                {
                    Id = measurementUnit.Id,
                    DisplayName = measurementUnit.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderProductInfoMeasurementUnitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}