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
    [AbpAuthorize(AppPermissions.Pages_OrderTeams)]
    public class OrderTeamsAppService : SoftGridAppServiceBase, IOrderTeamsAppService
    {
        private readonly IRepository<OrderTeam, long> _orderTeamRepository;
        private readonly IOrderTeamsExcelExporter _orderTeamsExcelExporter;
        private readonly IRepository<Order, long> _lookup_orderRepository;
        private readonly IRepository<Employee, long> _lookup_employeeRepository;

        public OrderTeamsAppService(IRepository<OrderTeam, long> orderTeamRepository, IOrderTeamsExcelExporter orderTeamsExcelExporter, IRepository<Order, long> lookup_orderRepository, IRepository<Employee, long> lookup_employeeRepository)
        {
            _orderTeamRepository = orderTeamRepository;
            _orderTeamsExcelExporter = orderTeamsExcelExporter;
            _lookup_orderRepository = lookup_orderRepository;
            _lookup_employeeRepository = lookup_employeeRepository;

        }

        public async Task<PagedResultDto<GetOrderTeamForViewDto>> GetAll(GetAllOrderTeamsInput input)
        {

            var filteredOrderTeams = _orderTeamRepository.GetAll()
                        .Include(e => e.OrderFk)
                        .Include(e => e.EmployeeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderInvoiceNumberFilter), e => e.OrderFk != null && e.OrderFk.InvoiceNumber == input.OrderInvoiceNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter);

            var pagedAndFilteredOrderTeams = filteredOrderTeams
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var orderTeams = from o in pagedAndFilteredOrderTeams
                             join o1 in _lookup_orderRepository.GetAll() on o.OrderId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             join o2 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o2.Id into j2
                             from s2 in j2.DefaultIfEmpty()

                             select new
                             {

                                 Id = o.Id,
                                 OrderInvoiceNumber = s1 == null || s1.InvoiceNumber == null ? "" : s1.InvoiceNumber.ToString(),
                                 EmployeeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                             };

            var totalCount = await filteredOrderTeams.CountAsync();

            var dbList = await orderTeams.ToListAsync();
            var results = new List<GetOrderTeamForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOrderTeamForViewDto()
                {
                    OrderTeam = new OrderTeamDto
                    {

                        Id = o.Id,
                    },
                    OrderInvoiceNumber = o.OrderInvoiceNumber,
                    EmployeeName = o.EmployeeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOrderTeamForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOrderTeamForViewDto> GetOrderTeamForView(long id)
        {
            var orderTeam = await _orderTeamRepository.GetAsync(id);

            var output = new GetOrderTeamForViewDto { OrderTeam = ObjectMapper.Map<OrderTeamDto>(orderTeam) };

            if (output.OrderTeam.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((long)output.OrderTeam.OrderId);
                output.OrderInvoiceNumber = _lookupOrder?.InvoiceNumber?.ToString();
            }

            if (output.OrderTeam.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.OrderTeam.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_OrderTeams_Edit)]
        public async Task<GetOrderTeamForEditOutput> GetOrderTeamForEdit(EntityDto<long> input)
        {
            var orderTeam = await _orderTeamRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOrderTeamForEditOutput { OrderTeam = ObjectMapper.Map<CreateOrEditOrderTeamDto>(orderTeam) };

            if (output.OrderTeam.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((long)output.OrderTeam.OrderId);
                output.OrderInvoiceNumber = _lookupOrder?.InvoiceNumber?.ToString();
            }

            if (output.OrderTeam.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.OrderTeam.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOrderTeamDto input)
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

        [AbpAuthorize(AppPermissions.Pages_OrderTeams_Create)]
        protected virtual async Task Create(CreateOrEditOrderTeamDto input)
        {
            var orderTeam = ObjectMapper.Map<OrderTeam>(input);

            if (AbpSession.TenantId != null)
            {
                orderTeam.TenantId = (int?)AbpSession.TenantId;
            }

            await _orderTeamRepository.InsertAsync(orderTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderTeams_Edit)]
        protected virtual async Task Update(CreateOrEditOrderTeamDto input)
        {
            var orderTeam = await _orderTeamRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, orderTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderTeams_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _orderTeamRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetOrderTeamsToExcel(GetAllOrderTeamsForExcelInput input)
        {

            var filteredOrderTeams = _orderTeamRepository.GetAll()
                        .Include(e => e.OrderFk)
                        .Include(e => e.EmployeeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderInvoiceNumberFilter), e => e.OrderFk != null && e.OrderFk.InvoiceNumber == input.OrderInvoiceNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter);

            var query = (from o in filteredOrderTeams
                         join o1 in _lookup_orderRepository.GetAll() on o.OrderId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetOrderTeamForViewDto()
                         {
                             OrderTeam = new OrderTeamDto
                             {
                                 Id = o.Id
                             },
                             OrderInvoiceNumber = s1 == null || s1.InvoiceNumber == null ? "" : s1.InvoiceNumber.ToString(),
                             EmployeeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var orderTeamListDtos = await query.ToListAsync();

            return _orderTeamsExcelExporter.ExportToFile(orderTeamListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_OrderTeams)]
        public async Task<PagedResultDto<OrderTeamOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_orderRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.InvoiceNumber != null && e.InvoiceNumber.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var orderList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderTeamOrderLookupTableDto>();
            foreach (var order in orderList)
            {
                lookupTableDtoList.Add(new OrderTeamOrderLookupTableDto
                {
                    Id = order.Id,
                    DisplayName = order.InvoiceNumber?.ToString()
                });
            }

            return new PagedResultDto<OrderTeamOrderLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_OrderTeams)]
        public async Task<PagedResultDto<OrderTeamEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_employeeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var employeeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderTeamEmployeeLookupTableDto>();
            foreach (var employee in employeeList)
            {
                lookupTableDtoList.Add(new OrderTeamEmployeeLookupTableDto
                {
                    Id = employee.Id,
                    DisplayName = employee.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderTeamEmployeeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}