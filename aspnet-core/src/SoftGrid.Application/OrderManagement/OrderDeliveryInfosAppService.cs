using SoftGrid.CRM;
using SoftGrid.OrderManagement;

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
    [AbpAuthorize(AppPermissions.Pages_OrderDeliveryInfos)]
    public class OrderDeliveryInfosAppService : SoftGridAppServiceBase, IOrderDeliveryInfosAppService
    {
        private readonly IRepository<OrderDeliveryInfo, long> _orderDeliveryInfoRepository;
        private readonly IOrderDeliveryInfosExcelExporter _orderDeliveryInfosExcelExporter;
        private readonly IRepository<Employee, long> _lookup_employeeRepository;
        private readonly IRepository<Order, long> _lookup_orderRepository;

        public OrderDeliveryInfosAppService(IRepository<OrderDeliveryInfo, long> orderDeliveryInfoRepository, IOrderDeliveryInfosExcelExporter orderDeliveryInfosExcelExporter, IRepository<Employee, long> lookup_employeeRepository, IRepository<Order, long> lookup_orderRepository)
        {
            _orderDeliveryInfoRepository = orderDeliveryInfoRepository;
            _orderDeliveryInfosExcelExporter = orderDeliveryInfosExcelExporter;
            _lookup_employeeRepository = lookup_employeeRepository;
            _lookup_orderRepository = lookup_orderRepository;

        }

        public async Task<PagedResultDto<GetOrderDeliveryInfoForViewDto>> GetAll(GetAllOrderDeliveryInfosInput input)
        {

            var filteredOrderDeliveryInfos = _orderDeliveryInfoRepository.GetAll()
                        .Include(e => e.EmployeeFk)
                        .Include(e => e.OrderFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TrackingNumber.Contains(input.Filter) || e.DeliveryProviderId.Contains(input.Filter) || e.DispatchTime.Contains(input.Filter) || e.DeliverToCustomerTime.Contains(input.Filter) || e.DeliveryNotes.Contains(input.Filter) || e.CustomerSignature.Contains(input.Filter) || e.CateringTime.Contains(input.Filter) || e.DeliveryTime.Contains(input.Filter) || e.DineInTime.Contains(input.Filter) || e.PickupTime.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TrackingNumberFilter), e => e.TrackingNumber.Contains(input.TrackingNumberFilter))
                        .WhereIf(input.MinTotalWeightFilter != null, e => e.TotalWeight >= input.MinTotalWeightFilter)
                        .WhereIf(input.MaxTotalWeightFilter != null, e => e.TotalWeight <= input.MaxTotalWeightFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DeliveryProviderIdFilter), e => e.DeliveryProviderId.Contains(input.DeliveryProviderIdFilter))
                        .WhereIf(input.MinDispatchDateFilter != null, e => e.DispatchDate >= input.MinDispatchDateFilter)
                        .WhereIf(input.MaxDispatchDateFilter != null, e => e.DispatchDate <= input.MaxDispatchDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DispatchTimeFilter), e => e.DispatchTime.Contains(input.DispatchTimeFilter))
                        .WhereIf(input.MinDeliverToCustomerDateFilter != null, e => e.DeliverToCustomerDate >= input.MinDeliverToCustomerDateFilter)
                        .WhereIf(input.MaxDeliverToCustomerDateFilter != null, e => e.DeliverToCustomerDate <= input.MaxDeliverToCustomerDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DeliverToCustomerTimeFilter), e => e.DeliverToCustomerTime.Contains(input.DeliverToCustomerTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DeliveryNotesFilter), e => e.DeliveryNotes.Contains(input.DeliveryNotesFilter))
                        .WhereIf(input.CustomerAcknowledgedFilter.HasValue && input.CustomerAcknowledgedFilter > -1, e => (input.CustomerAcknowledgedFilter == 1 && e.CustomerAcknowledged) || (input.CustomerAcknowledgedFilter == 0 && !e.CustomerAcknowledged))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerSignatureFilter), e => e.CustomerSignature.Contains(input.CustomerSignatureFilter))
                        .WhereIf(input.MinCateringDateFilter != null, e => e.CateringDate >= input.MinCateringDateFilter)
                        .WhereIf(input.MaxCateringDateFilter != null, e => e.CateringDate <= input.MaxCateringDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CateringTimeFilter), e => e.CateringTime.Contains(input.CateringTimeFilter))
                        .WhereIf(input.MinDeliveryDateFilter != null, e => e.DeliveryDate >= input.MinDeliveryDateFilter)
                        .WhereIf(input.MaxDeliveryDateFilter != null, e => e.DeliveryDate <= input.MaxDeliveryDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DeliveryTimeFilter), e => e.DeliveryTime.Contains(input.DeliveryTimeFilter))
                        .WhereIf(input.MinDineInDateFilter != null, e => e.DineInDate >= input.MinDineInDateFilter)
                        .WhereIf(input.MaxDineInDateFilter != null, e => e.DineInDate <= input.MaxDineInDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DineInTimeFilter), e => e.DineInTime.Contains(input.DineInTimeFilter))
                        .WhereIf(input.IncludedChildrenFilter.HasValue && input.IncludedChildrenFilter > -1, e => (input.IncludedChildrenFilter == 1 && e.IncludedChildren) || (input.IncludedChildrenFilter == 0 && !e.IncludedChildren))
                        .WhereIf(input.IsAsapFilter.HasValue && input.IsAsapFilter > -1, e => (input.IsAsapFilter == 1 && e.IsAsap) || (input.IsAsapFilter == 0 && !e.IsAsap))
                        .WhereIf(input.IsPickupCateringFilter.HasValue && input.IsPickupCateringFilter > -1, e => (input.IsPickupCateringFilter == 1 && e.IsPickupCatering) || (input.IsPickupCateringFilter == 0 && !e.IsPickupCatering))
                        .WhereIf(input.MinNumberOfGuestsFilter != null, e => e.NumberOfGuests >= input.MinNumberOfGuestsFilter)
                        .WhereIf(input.MaxNumberOfGuestsFilter != null, e => e.NumberOfGuests <= input.MaxNumberOfGuestsFilter)
                        .WhereIf(input.MinPickupDateFilter != null, e => e.PickupDate >= input.MinPickupDateFilter)
                        .WhereIf(input.MaxPickupDateFilter != null, e => e.PickupDate <= input.MaxPickupDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PickupTimeFilter), e => e.PickupTime.Contains(input.PickupTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderInvoiceNumberFilter), e => e.OrderFk != null && e.OrderFk.InvoiceNumber == input.OrderInvoiceNumberFilter);

            var pagedAndFilteredOrderDeliveryInfos = filteredOrderDeliveryInfos
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var orderDeliveryInfos = from o in pagedAndFilteredOrderDeliveryInfos
                                     join o1 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o1.Id into j1
                                     from s1 in j1.DefaultIfEmpty()

                                     join o2 in _lookup_orderRepository.GetAll() on o.OrderId equals o2.Id into j2
                                     from s2 in j2.DefaultIfEmpty()

                                     select new
                                     {

                                         o.TrackingNumber,
                                         o.TotalWeight,
                                         o.DeliveryProviderId,
                                         o.DispatchDate,
                                         o.DispatchTime,
                                         o.DeliverToCustomerDate,
                                         o.DeliverToCustomerTime,
                                         o.DeliveryNotes,
                                         o.CustomerAcknowledged,
                                         o.CustomerSignature,
                                         o.CateringDate,
                                         o.CateringTime,
                                         o.DeliveryDate,
                                         o.DeliveryTime,
                                         o.DineInDate,
                                         o.DineInTime,
                                         o.IncludedChildren,
                                         o.IsAsap,
                                         o.IsPickupCatering,
                                         o.NumberOfGuests,
                                         o.PickupDate,
                                         o.PickupTime,
                                         Id = o.Id,
                                         EmployeeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                         OrderInvoiceNumber = s2 == null || s2.InvoiceNumber == null ? "" : s2.InvoiceNumber.ToString()
                                     };

            var totalCount = await filteredOrderDeliveryInfos.CountAsync();

            var dbList = await orderDeliveryInfos.ToListAsync();
            var results = new List<GetOrderDeliveryInfoForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOrderDeliveryInfoForViewDto()
                {
                    OrderDeliveryInfo = new OrderDeliveryInfoDto
                    {

                        TrackingNumber = o.TrackingNumber,
                        TotalWeight = o.TotalWeight,
                        DeliveryProviderId = o.DeliveryProviderId,
                        DispatchDate = o.DispatchDate,
                        DispatchTime = o.DispatchTime,
                        DeliverToCustomerDate = o.DeliverToCustomerDate,
                        DeliverToCustomerTime = o.DeliverToCustomerTime,
                        DeliveryNotes = o.DeliveryNotes,
                        CustomerAcknowledged = o.CustomerAcknowledged,
                        CustomerSignature = o.CustomerSignature,
                        CateringDate = o.CateringDate,
                        CateringTime = o.CateringTime,
                        DeliveryDate = o.DeliveryDate,
                        DeliveryTime = o.DeliveryTime,
                        DineInDate = o.DineInDate,
                        DineInTime = o.DineInTime,
                        IncludedChildren = o.IncludedChildren,
                        IsAsap = o.IsAsap,
                        IsPickupCatering = o.IsPickupCatering,
                        NumberOfGuests = o.NumberOfGuests,
                        PickupDate = o.PickupDate,
                        PickupTime = o.PickupTime,
                        Id = o.Id,
                    },
                    EmployeeName = o.EmployeeName,
                    OrderInvoiceNumber = o.OrderInvoiceNumber
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOrderDeliveryInfoForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOrderDeliveryInfoForViewDto> GetOrderDeliveryInfoForView(long id)
        {
            var orderDeliveryInfo = await _orderDeliveryInfoRepository.GetAsync(id);

            var output = new GetOrderDeliveryInfoForViewDto { OrderDeliveryInfo = ObjectMapper.Map<OrderDeliveryInfoDto>(orderDeliveryInfo) };

            if (output.OrderDeliveryInfo.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.OrderDeliveryInfo.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            if (output.OrderDeliveryInfo.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((long)output.OrderDeliveryInfo.OrderId);
                output.OrderInvoiceNumber = _lookupOrder?.InvoiceNumber?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_OrderDeliveryInfos_Edit)]
        public async Task<GetOrderDeliveryInfoForEditOutput> GetOrderDeliveryInfoForEdit(EntityDto<long> input)
        {
            var orderDeliveryInfo = await _orderDeliveryInfoRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOrderDeliveryInfoForEditOutput { OrderDeliveryInfo = ObjectMapper.Map<CreateOrEditOrderDeliveryInfoDto>(orderDeliveryInfo) };

            if (output.OrderDeliveryInfo.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.OrderDeliveryInfo.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            if (output.OrderDeliveryInfo.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((long)output.OrderDeliveryInfo.OrderId);
                output.OrderInvoiceNumber = _lookupOrder?.InvoiceNumber?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOrderDeliveryInfoDto input)
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

        [AbpAuthorize(AppPermissions.Pages_OrderDeliveryInfos_Create)]
        protected virtual async Task Create(CreateOrEditOrderDeliveryInfoDto input)
        {
            var orderDeliveryInfo = ObjectMapper.Map<OrderDeliveryInfo>(input);

            if (AbpSession.TenantId != null)
            {
                orderDeliveryInfo.TenantId = (int?)AbpSession.TenantId;
            }

            await _orderDeliveryInfoRepository.InsertAsync(orderDeliveryInfo);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderDeliveryInfos_Edit)]
        protected virtual async Task Update(CreateOrEditOrderDeliveryInfoDto input)
        {
            var orderDeliveryInfo = await _orderDeliveryInfoRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, orderDeliveryInfo);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderDeliveryInfos_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _orderDeliveryInfoRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetOrderDeliveryInfosToExcel(GetAllOrderDeliveryInfosForExcelInput input)
        {

            var filteredOrderDeliveryInfos = _orderDeliveryInfoRepository.GetAll()
                        .Include(e => e.EmployeeFk)
                        .Include(e => e.OrderFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TrackingNumber.Contains(input.Filter) || e.DeliveryProviderId.Contains(input.Filter) || e.DispatchTime.Contains(input.Filter) || e.DeliverToCustomerTime.Contains(input.Filter) || e.DeliveryNotes.Contains(input.Filter) || e.CustomerSignature.Contains(input.Filter) || e.CateringTime.Contains(input.Filter) || e.DeliveryTime.Contains(input.Filter) || e.DineInTime.Contains(input.Filter) || e.PickupTime.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TrackingNumberFilter), e => e.TrackingNumber.Contains(input.TrackingNumberFilter))
                        .WhereIf(input.MinTotalWeightFilter != null, e => e.TotalWeight >= input.MinTotalWeightFilter)
                        .WhereIf(input.MaxTotalWeightFilter != null, e => e.TotalWeight <= input.MaxTotalWeightFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DeliveryProviderIdFilter), e => e.DeliveryProviderId.Contains(input.DeliveryProviderIdFilter))
                        .WhereIf(input.MinDispatchDateFilter != null, e => e.DispatchDate >= input.MinDispatchDateFilter)
                        .WhereIf(input.MaxDispatchDateFilter != null, e => e.DispatchDate <= input.MaxDispatchDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DispatchTimeFilter), e => e.DispatchTime.Contains(input.DispatchTimeFilter))
                        .WhereIf(input.MinDeliverToCustomerDateFilter != null, e => e.DeliverToCustomerDate >= input.MinDeliverToCustomerDateFilter)
                        .WhereIf(input.MaxDeliverToCustomerDateFilter != null, e => e.DeliverToCustomerDate <= input.MaxDeliverToCustomerDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DeliverToCustomerTimeFilter), e => e.DeliverToCustomerTime.Contains(input.DeliverToCustomerTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DeliveryNotesFilter), e => e.DeliveryNotes.Contains(input.DeliveryNotesFilter))
                        .WhereIf(input.CustomerAcknowledgedFilter.HasValue && input.CustomerAcknowledgedFilter > -1, e => (input.CustomerAcknowledgedFilter == 1 && e.CustomerAcknowledged) || (input.CustomerAcknowledgedFilter == 0 && !e.CustomerAcknowledged))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerSignatureFilter), e => e.CustomerSignature.Contains(input.CustomerSignatureFilter))
                        .WhereIf(input.MinCateringDateFilter != null, e => e.CateringDate >= input.MinCateringDateFilter)
                        .WhereIf(input.MaxCateringDateFilter != null, e => e.CateringDate <= input.MaxCateringDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CateringTimeFilter), e => e.CateringTime.Contains(input.CateringTimeFilter))
                        .WhereIf(input.MinDeliveryDateFilter != null, e => e.DeliveryDate >= input.MinDeliveryDateFilter)
                        .WhereIf(input.MaxDeliveryDateFilter != null, e => e.DeliveryDate <= input.MaxDeliveryDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DeliveryTimeFilter), e => e.DeliveryTime.Contains(input.DeliveryTimeFilter))
                        .WhereIf(input.MinDineInDateFilter != null, e => e.DineInDate >= input.MinDineInDateFilter)
                        .WhereIf(input.MaxDineInDateFilter != null, e => e.DineInDate <= input.MaxDineInDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DineInTimeFilter), e => e.DineInTime.Contains(input.DineInTimeFilter))
                        .WhereIf(input.IncludedChildrenFilter.HasValue && input.IncludedChildrenFilter > -1, e => (input.IncludedChildrenFilter == 1 && e.IncludedChildren) || (input.IncludedChildrenFilter == 0 && !e.IncludedChildren))
                        .WhereIf(input.IsAsapFilter.HasValue && input.IsAsapFilter > -1, e => (input.IsAsapFilter == 1 && e.IsAsap) || (input.IsAsapFilter == 0 && !e.IsAsap))
                        .WhereIf(input.IsPickupCateringFilter.HasValue && input.IsPickupCateringFilter > -1, e => (input.IsPickupCateringFilter == 1 && e.IsPickupCatering) || (input.IsPickupCateringFilter == 0 && !e.IsPickupCatering))
                        .WhereIf(input.MinNumberOfGuestsFilter != null, e => e.NumberOfGuests >= input.MinNumberOfGuestsFilter)
                        .WhereIf(input.MaxNumberOfGuestsFilter != null, e => e.NumberOfGuests <= input.MaxNumberOfGuestsFilter)
                        .WhereIf(input.MinPickupDateFilter != null, e => e.PickupDate >= input.MinPickupDateFilter)
                        .WhereIf(input.MaxPickupDateFilter != null, e => e.PickupDate <= input.MaxPickupDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PickupTimeFilter), e => e.PickupTime.Contains(input.PickupTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderInvoiceNumberFilter), e => e.OrderFk != null && e.OrderFk.InvoiceNumber == input.OrderInvoiceNumberFilter);

            var query = (from o in filteredOrderDeliveryInfos
                         join o1 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_orderRepository.GetAll() on o.OrderId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetOrderDeliveryInfoForViewDto()
                         {
                             OrderDeliveryInfo = new OrderDeliveryInfoDto
                             {
                                 TrackingNumber = o.TrackingNumber,
                                 TotalWeight = o.TotalWeight,
                                 DeliveryProviderId = o.DeliveryProviderId,
                                 DispatchDate = o.DispatchDate,
                                 DispatchTime = o.DispatchTime,
                                 DeliverToCustomerDate = o.DeliverToCustomerDate,
                                 DeliverToCustomerTime = o.DeliverToCustomerTime,
                                 DeliveryNotes = o.DeliveryNotes,
                                 CustomerAcknowledged = o.CustomerAcknowledged,
                                 CustomerSignature = o.CustomerSignature,
                                 CateringDate = o.CateringDate,
                                 CateringTime = o.CateringTime,
                                 DeliveryDate = o.DeliveryDate,
                                 DeliveryTime = o.DeliveryTime,
                                 DineInDate = o.DineInDate,
                                 DineInTime = o.DineInTime,
                                 IncludedChildren = o.IncludedChildren,
                                 IsAsap = o.IsAsap,
                                 IsPickupCatering = o.IsPickupCatering,
                                 NumberOfGuests = o.NumberOfGuests,
                                 PickupDate = o.PickupDate,
                                 PickupTime = o.PickupTime,
                                 Id = o.Id
                             },
                             EmployeeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             OrderInvoiceNumber = s2 == null || s2.InvoiceNumber == null ? "" : s2.InvoiceNumber.ToString()
                         });

            var orderDeliveryInfoListDtos = await query.ToListAsync();

            return _orderDeliveryInfosExcelExporter.ExportToFile(orderDeliveryInfoListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_OrderDeliveryInfos)]
        public async Task<PagedResultDto<OrderDeliveryInfoEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_employeeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var employeeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderDeliveryInfoEmployeeLookupTableDto>();
            foreach (var employee in employeeList)
            {
                lookupTableDtoList.Add(new OrderDeliveryInfoEmployeeLookupTableDto
                {
                    Id = employee.Id,
                    DisplayName = employee.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderDeliveryInfoEmployeeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_OrderDeliveryInfos)]
        public async Task<PagedResultDto<OrderDeliveryInfoOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_orderRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.InvoiceNumber != null && e.InvoiceNumber.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var orderList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderDeliveryInfoOrderLookupTableDto>();
            foreach (var order in orderList)
            {
                lookupTableDtoList.Add(new OrderDeliveryInfoOrderLookupTableDto
                {
                    Id = order.Id,
                    DisplayName = order.InvoiceNumber?.ToString()
                });
            }

            return new PagedResultDto<OrderDeliveryInfoOrderLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}