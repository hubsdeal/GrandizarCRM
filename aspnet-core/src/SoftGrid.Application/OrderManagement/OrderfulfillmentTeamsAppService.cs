using SoftGrid.OrderManagement;
using SoftGrid.CRM;
using SoftGrid.CRM;
using SoftGrid.Authorization.Users;

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
    [AbpAuthorize(AppPermissions.Pages_OrderfulfillmentTeams)]
    public class OrderfulfillmentTeamsAppService : SoftGridAppServiceBase, IOrderfulfillmentTeamsAppService
    {
        private readonly IRepository<OrderfulfillmentTeam, long> _orderfulfillmentTeamRepository;
        private readonly IOrderfulfillmentTeamsExcelExporter _orderfulfillmentTeamsExcelExporter;
        private readonly IRepository<Order, long> _lookup_orderRepository;
        private readonly IRepository<Employee, long> _lookup_employeeRepository;
        private readonly IRepository<Contact, long> _lookup_contactRepository;
        private readonly IRepository<User, long> _lookup_userRepository;

        public OrderfulfillmentTeamsAppService(IRepository<OrderfulfillmentTeam, long> orderfulfillmentTeamRepository, IOrderfulfillmentTeamsExcelExporter orderfulfillmentTeamsExcelExporter, IRepository<Order, long> lookup_orderRepository, IRepository<Employee, long> lookup_employeeRepository, IRepository<Contact, long> lookup_contactRepository, IRepository<User, long> lookup_userRepository)
        {
            _orderfulfillmentTeamRepository = orderfulfillmentTeamRepository;
            _orderfulfillmentTeamsExcelExporter = orderfulfillmentTeamsExcelExporter;
            _lookup_orderRepository = lookup_orderRepository;
            _lookup_employeeRepository = lookup_employeeRepository;
            _lookup_contactRepository = lookup_contactRepository;
            _lookup_userRepository = lookup_userRepository;

        }

        public async Task<PagedResultDto<GetOrderfulfillmentTeamForViewDto>> GetAll(GetAllOrderfulfillmentTeamsInput input)
        {

            var filteredOrderfulfillmentTeams = _orderfulfillmentTeamRepository.GetAll()
                        .Include(e => e.OrderFk)
                        .Include(e => e.EmployeeFk)
                        .Include(e => e.ContactFk)
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderFullNameFilter), e => e.OrderFk != null && e.OrderFk.FullName == input.OrderFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var pagedAndFilteredOrderfulfillmentTeams = filteredOrderfulfillmentTeams
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var orderfulfillmentTeams = from o in pagedAndFilteredOrderfulfillmentTeams
                                        join o1 in _lookup_orderRepository.GetAll() on o.OrderId equals o1.Id into j1
                                        from s1 in j1.DefaultIfEmpty()

                                        join o2 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o2.Id into j2
                                        from s2 in j2.DefaultIfEmpty()

                                        join o3 in _lookup_contactRepository.GetAll() on o.ContactId equals o3.Id into j3
                                        from s3 in j3.DefaultIfEmpty()

                                        join o4 in _lookup_userRepository.GetAll() on o.UserId equals o4.Id into j4
                                        from s4 in j4.DefaultIfEmpty()

                                        select new
                                        {

                                            Id = o.Id,
                                            OrderFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                                            EmployeeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                            ContactFullName = s3 == null || s3.FullName == null ? "" : s3.FullName.ToString(),
                                            UserName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                                        };

            var totalCount = await filteredOrderfulfillmentTeams.CountAsync();

            var dbList = await orderfulfillmentTeams.ToListAsync();
            var results = new List<GetOrderfulfillmentTeamForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOrderfulfillmentTeamForViewDto()
                {
                    OrderfulfillmentTeam = new OrderfulfillmentTeamDto
                    {

                        Id = o.Id,
                    },
                    OrderFullName = o.OrderFullName,
                    EmployeeName = o.EmployeeName,
                    ContactFullName = o.ContactFullName,
                    UserName = o.UserName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOrderfulfillmentTeamForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOrderfulfillmentTeamForViewDto> GetOrderfulfillmentTeamForView(long id)
        {
            var orderfulfillmentTeam = await _orderfulfillmentTeamRepository.GetAsync(id);

            var output = new GetOrderfulfillmentTeamForViewDto { OrderfulfillmentTeam = ObjectMapper.Map<OrderfulfillmentTeamDto>(orderfulfillmentTeam) };

            if (output.OrderfulfillmentTeam.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((long)output.OrderfulfillmentTeam.OrderId);
                output.OrderFullName = _lookupOrder?.FullName?.ToString();
            }

            if (output.OrderfulfillmentTeam.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.OrderfulfillmentTeam.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            if (output.OrderfulfillmentTeam.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.OrderfulfillmentTeam.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.OrderfulfillmentTeam.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.OrderfulfillmentTeam.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_OrderfulfillmentTeams_Edit)]
        public async Task<GetOrderfulfillmentTeamForEditOutput> GetOrderfulfillmentTeamForEdit(EntityDto<long> input)
        {
            var orderfulfillmentTeam = await _orderfulfillmentTeamRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOrderfulfillmentTeamForEditOutput { OrderfulfillmentTeam = ObjectMapper.Map<CreateOrEditOrderfulfillmentTeamDto>(orderfulfillmentTeam) };

            if (output.OrderfulfillmentTeam.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((long)output.OrderfulfillmentTeam.OrderId);
                output.OrderFullName = _lookupOrder?.FullName?.ToString();
            }

            if (output.OrderfulfillmentTeam.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.OrderfulfillmentTeam.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            if (output.OrderfulfillmentTeam.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.OrderfulfillmentTeam.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.OrderfulfillmentTeam.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.OrderfulfillmentTeam.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOrderfulfillmentTeamDto input)
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

        [AbpAuthorize(AppPermissions.Pages_OrderfulfillmentTeams_Create)]
        protected virtual async Task Create(CreateOrEditOrderfulfillmentTeamDto input)
        {
            var orderfulfillmentTeam = ObjectMapper.Map<OrderfulfillmentTeam>(input);

            if (AbpSession.TenantId != null)
            {
                orderfulfillmentTeam.TenantId = (int?)AbpSession.TenantId;
            }

            await _orderfulfillmentTeamRepository.InsertAsync(orderfulfillmentTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderfulfillmentTeams_Edit)]
        protected virtual async Task Update(CreateOrEditOrderfulfillmentTeamDto input)
        {
            var orderfulfillmentTeam = await _orderfulfillmentTeamRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, orderfulfillmentTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderfulfillmentTeams_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _orderfulfillmentTeamRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetOrderfulfillmentTeamsToExcel(GetAllOrderfulfillmentTeamsForExcelInput input)
        {

            var filteredOrderfulfillmentTeams = _orderfulfillmentTeamRepository.GetAll()
                        .Include(e => e.OrderFk)
                        .Include(e => e.EmployeeFk)
                        .Include(e => e.ContactFk)
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderFullNameFilter), e => e.OrderFk != null && e.OrderFk.FullName == input.OrderFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var query = (from o in filteredOrderfulfillmentTeams
                         join o1 in _lookup_orderRepository.GetAll() on o.OrderId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_contactRepository.GetAll() on o.ContactId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_userRepository.GetAll() on o.UserId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetOrderfulfillmentTeamForViewDto()
                         {
                             OrderfulfillmentTeam = new OrderfulfillmentTeamDto
                             {
                                 Id = o.Id
                             },
                             OrderFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                             EmployeeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             ContactFullName = s3 == null || s3.FullName == null ? "" : s3.FullName.ToString(),
                             UserName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                         });

            var orderfulfillmentTeamListDtos = await query.ToListAsync();

            return _orderfulfillmentTeamsExcelExporter.ExportToFile(orderfulfillmentTeamListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_OrderfulfillmentTeams)]
        public async Task<PagedResultDto<OrderfulfillmentTeamOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_orderRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var orderList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderfulfillmentTeamOrderLookupTableDto>();
            foreach (var order in orderList)
            {
                lookupTableDtoList.Add(new OrderfulfillmentTeamOrderLookupTableDto
                {
                    Id = order.Id,
                    DisplayName = order.FullName?.ToString()
                });
            }

            return new PagedResultDto<OrderfulfillmentTeamOrderLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_OrderfulfillmentTeams)]
        public async Task<PagedResultDto<OrderfulfillmentTeamEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_employeeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var employeeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderfulfillmentTeamEmployeeLookupTableDto>();
            foreach (var employee in employeeList)
            {
                lookupTableDtoList.Add(new OrderfulfillmentTeamEmployeeLookupTableDto
                {
                    Id = employee.Id,
                    DisplayName = employee.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderfulfillmentTeamEmployeeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_OrderfulfillmentTeams)]
        public async Task<PagedResultDto<OrderfulfillmentTeamContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderfulfillmentTeamContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new OrderfulfillmentTeamContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<OrderfulfillmentTeamContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_OrderfulfillmentTeams)]
        public async Task<PagedResultDto<OrderfulfillmentTeamUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderfulfillmentTeamUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new OrderfulfillmentTeamUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderfulfillmentTeamUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}