using SoftGrid.OrderManagement;
using SoftGrid.OrderManagement;
using SoftGrid.CRM;

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
    [AbpAuthorize(AppPermissions.Pages_OrderFulfillmentStatuses)]
    public class OrderFulfillmentStatusesAppService : SoftGridAppServiceBase, IOrderFulfillmentStatusesAppService
    {
        private readonly IRepository<OrderFulfillmentStatus, long> _orderFulfillmentStatusRepository;
        private readonly IOrderFulfillmentStatusesExcelExporter _orderFulfillmentStatusesExcelExporter;
        private readonly IRepository<OrderStatus, long> _lookup_orderStatusRepository;
        private readonly IRepository<Order, long> _lookup_orderRepository;
        private readonly IRepository<Employee, long> _lookup_employeeRepository;

        public OrderFulfillmentStatusesAppService(IRepository<OrderFulfillmentStatus, long> orderFulfillmentStatusRepository, IOrderFulfillmentStatusesExcelExporter orderFulfillmentStatusesExcelExporter, IRepository<OrderStatus, long> lookup_orderStatusRepository, IRepository<Order, long> lookup_orderRepository, IRepository<Employee, long> lookup_employeeRepository)
        {
            _orderFulfillmentStatusRepository = orderFulfillmentStatusRepository;
            _orderFulfillmentStatusesExcelExporter = orderFulfillmentStatusesExcelExporter;
            _lookup_orderStatusRepository = lookup_orderStatusRepository;
            _lookup_orderRepository = lookup_orderRepository;
            _lookup_employeeRepository = lookup_employeeRepository;

        }

        public async Task<PagedResultDto<GetOrderFulfillmentStatusForViewDto>> GetAll(GetAllOrderFulfillmentStatusesInput input)
        {

            var filteredOrderFulfillmentStatuses = _orderFulfillmentStatusRepository.GetAll()
                        .Include(e => e.OrderStatusFk)
                        .Include(e => e.OrderFk)
                        .Include(e => e.EmployeeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinEstimatedTimeFilter != null, e => e.EstimatedTime >= input.MinEstimatedTimeFilter)
                        .WhereIf(input.MaxEstimatedTimeFilter != null, e => e.EstimatedTime <= input.MaxEstimatedTimeFilter)
                        .WhereIf(input.MinActualTimeFilter != null, e => e.ActualTime >= input.MinActualTimeFilter)
                        .WhereIf(input.MaxActualTimeFilter != null, e => e.ActualTime <= input.MaxActualTimeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderStatusNameFilter), e => e.OrderStatusFk != null && e.OrderStatusFk.Name == input.OrderStatusNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderInvoiceNumberFilter), e => e.OrderFk != null && e.OrderFk.InvoiceNumber == input.OrderInvoiceNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter);

            var pagedAndFilteredOrderFulfillmentStatuses = filteredOrderFulfillmentStatuses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var orderFulfillmentStatuses = from o in pagedAndFilteredOrderFulfillmentStatuses
                                           join o1 in _lookup_orderStatusRepository.GetAll() on o.OrderStatusId equals o1.Id into j1
                                           from s1 in j1.DefaultIfEmpty()

                                           join o2 in _lookup_orderRepository.GetAll() on o.OrderId equals o2.Id into j2
                                           from s2 in j2.DefaultIfEmpty()

                                           join o3 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o3.Id into j3
                                           from s3 in j3.DefaultIfEmpty()

                                           select new
                                           {

                                               o.EstimatedTime,
                                               o.ActualTime,
                                               Id = o.Id,
                                               OrderStatusName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                               OrderInvoiceNumber = s2 == null || s2.InvoiceNumber == null ? "" : s2.InvoiceNumber.ToString(),
                                               EmployeeName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                                           };

            var totalCount = await filteredOrderFulfillmentStatuses.CountAsync();

            var dbList = await orderFulfillmentStatuses.ToListAsync();
            var results = new List<GetOrderFulfillmentStatusForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOrderFulfillmentStatusForViewDto()
                {
                    OrderFulfillmentStatus = new OrderFulfillmentStatusDto
                    {

                        EstimatedTime = o.EstimatedTime,
                        ActualTime = o.ActualTime,
                        Id = o.Id,
                    },
                    OrderStatusName = o.OrderStatusName,
                    OrderInvoiceNumber = o.OrderInvoiceNumber,
                    EmployeeName = o.EmployeeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOrderFulfillmentStatusForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOrderFulfillmentStatusForViewDto> GetOrderFulfillmentStatusForView(long id)
        {
            var orderFulfillmentStatus = await _orderFulfillmentStatusRepository.GetAsync(id);

            var output = new GetOrderFulfillmentStatusForViewDto { OrderFulfillmentStatus = ObjectMapper.Map<OrderFulfillmentStatusDto>(orderFulfillmentStatus) };

            if (output.OrderFulfillmentStatus.OrderStatusId != null)
            {
                var _lookupOrderStatus = await _lookup_orderStatusRepository.FirstOrDefaultAsync((long)output.OrderFulfillmentStatus.OrderStatusId);
                output.OrderStatusName = _lookupOrderStatus?.Name?.ToString();
            }

            if (output.OrderFulfillmentStatus.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((long)output.OrderFulfillmentStatus.OrderId);
                output.OrderInvoiceNumber = _lookupOrder?.InvoiceNumber?.ToString();
            }

            if (output.OrderFulfillmentStatus.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.OrderFulfillmentStatus.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_OrderFulfillmentStatuses_Edit)]
        public async Task<GetOrderFulfillmentStatusForEditOutput> GetOrderFulfillmentStatusForEdit(EntityDto<long> input)
        {
            var orderFulfillmentStatus = await _orderFulfillmentStatusRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOrderFulfillmentStatusForEditOutput { OrderFulfillmentStatus = ObjectMapper.Map<CreateOrEditOrderFulfillmentStatusDto>(orderFulfillmentStatus) };

            if (output.OrderFulfillmentStatus.OrderStatusId != null)
            {
                var _lookupOrderStatus = await _lookup_orderStatusRepository.FirstOrDefaultAsync((long)output.OrderFulfillmentStatus.OrderStatusId);
                output.OrderStatusName = _lookupOrderStatus?.Name?.ToString();
            }

            if (output.OrderFulfillmentStatus.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((long)output.OrderFulfillmentStatus.OrderId);
                output.OrderInvoiceNumber = _lookupOrder?.InvoiceNumber?.ToString();
            }

            if (output.OrderFulfillmentStatus.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.OrderFulfillmentStatus.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOrderFulfillmentStatusDto input)
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

        [AbpAuthorize(AppPermissions.Pages_OrderFulfillmentStatuses_Create)]
        protected virtual async Task Create(CreateOrEditOrderFulfillmentStatusDto input)
        {
            var orderFulfillmentStatus = ObjectMapper.Map<OrderFulfillmentStatus>(input);

            if (AbpSession.TenantId != null)
            {
                orderFulfillmentStatus.TenantId = (int?)AbpSession.TenantId;
            }

            await _orderFulfillmentStatusRepository.InsertAsync(orderFulfillmentStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderFulfillmentStatuses_Edit)]
        protected virtual async Task Update(CreateOrEditOrderFulfillmentStatusDto input)
        {
            var orderFulfillmentStatus = await _orderFulfillmentStatusRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, orderFulfillmentStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderFulfillmentStatuses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _orderFulfillmentStatusRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetOrderFulfillmentStatusesToExcel(GetAllOrderFulfillmentStatusesForExcelInput input)
        {

            var filteredOrderFulfillmentStatuses = _orderFulfillmentStatusRepository.GetAll()
                        .Include(e => e.OrderStatusFk)
                        .Include(e => e.OrderFk)
                        .Include(e => e.EmployeeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinEstimatedTimeFilter != null, e => e.EstimatedTime >= input.MinEstimatedTimeFilter)
                        .WhereIf(input.MaxEstimatedTimeFilter != null, e => e.EstimatedTime <= input.MaxEstimatedTimeFilter)
                        .WhereIf(input.MinActualTimeFilter != null, e => e.ActualTime >= input.MinActualTimeFilter)
                        .WhereIf(input.MaxActualTimeFilter != null, e => e.ActualTime <= input.MaxActualTimeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderStatusNameFilter), e => e.OrderStatusFk != null && e.OrderStatusFk.Name == input.OrderStatusNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderInvoiceNumberFilter), e => e.OrderFk != null && e.OrderFk.InvoiceNumber == input.OrderInvoiceNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter);

            var query = (from o in filteredOrderFulfillmentStatuses
                         join o1 in _lookup_orderStatusRepository.GetAll() on o.OrderStatusId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_orderRepository.GetAll() on o.OrderId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetOrderFulfillmentStatusForViewDto()
                         {
                             OrderFulfillmentStatus = new OrderFulfillmentStatusDto
                             {
                                 EstimatedTime = o.EstimatedTime,
                                 ActualTime = o.ActualTime,
                                 Id = o.Id
                             },
                             OrderStatusName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             OrderInvoiceNumber = s2 == null || s2.InvoiceNumber == null ? "" : s2.InvoiceNumber.ToString(),
                             EmployeeName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var orderFulfillmentStatusListDtos = await query.ToListAsync();

            return _orderFulfillmentStatusesExcelExporter.ExportToFile(orderFulfillmentStatusListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_OrderFulfillmentStatuses)]
        public async Task<PagedResultDto<OrderFulfillmentStatusOrderStatusLookupTableDto>> GetAllOrderStatusForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_orderStatusRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var orderStatusList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderFulfillmentStatusOrderStatusLookupTableDto>();
            foreach (var orderStatus in orderStatusList)
            {
                lookupTableDtoList.Add(new OrderFulfillmentStatusOrderStatusLookupTableDto
                {
                    Id = orderStatus.Id,
                    DisplayName = orderStatus.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderFulfillmentStatusOrderStatusLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_OrderFulfillmentStatuses)]
        public async Task<PagedResultDto<OrderFulfillmentStatusOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_orderRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.InvoiceNumber != null && e.InvoiceNumber.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var orderList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderFulfillmentStatusOrderLookupTableDto>();
            foreach (var order in orderList)
            {
                lookupTableDtoList.Add(new OrderFulfillmentStatusOrderLookupTableDto
                {
                    Id = order.Id,
                    DisplayName = order.InvoiceNumber?.ToString()
                });
            }

            return new PagedResultDto<OrderFulfillmentStatusOrderLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_OrderFulfillmentStatuses)]
        public async Task<PagedResultDto<OrderFulfillmentStatusEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_employeeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var employeeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderFulfillmentStatusEmployeeLookupTableDto>();
            foreach (var employee in employeeList)
            {
                lookupTableDtoList.Add(new OrderFulfillmentStatusEmployeeLookupTableDto
                {
                    Id = employee.Id,
                    DisplayName = employee.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderFulfillmentStatusEmployeeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}