using SoftGrid.Shop;
using SoftGrid.CRM;

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
    [AbpAuthorize(AppPermissions.Pages_StoreAccountTeams)]
    public class StoreAccountTeamsAppService : SoftGridAppServiceBase, IStoreAccountTeamsAppService
    {
        private readonly IRepository<StoreAccountTeam, long> _storeAccountTeamRepository;
        private readonly IStoreAccountTeamsExcelExporter _storeAccountTeamsExcelExporter;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<Employee, long> _lookup_employeeRepository;

        public StoreAccountTeamsAppService(IRepository<StoreAccountTeam, long> storeAccountTeamRepository, IStoreAccountTeamsExcelExporter storeAccountTeamsExcelExporter, IRepository<Store, long> lookup_storeRepository, IRepository<Employee, long> lookup_employeeRepository)
        {
            _storeAccountTeamRepository = storeAccountTeamRepository;
            _storeAccountTeamsExcelExporter = storeAccountTeamsExcelExporter;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_employeeRepository = lookup_employeeRepository;

        }

        public async Task<PagedResultDto<GetStoreAccountTeamForViewDto>> GetAll(GetAllStoreAccountTeamsInput input)
        {

            var filteredStoreAccountTeams = _storeAccountTeamRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.EmployeeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PrimaryFilter.HasValue && input.PrimaryFilter > -1, e => (input.PrimaryFilter == 1 && e.Primary) || (input.PrimaryFilter == 0 && !e.Primary))
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(input.OrderEmailNotificationFilter.HasValue && input.OrderEmailNotificationFilter > -1, e => (input.OrderEmailNotificationFilter == 1 && e.OrderEmailNotification) || (input.OrderEmailNotificationFilter == 0 && !e.OrderEmailNotification))
                        .WhereIf(input.OrderSmsNotificationFilter.HasValue && input.OrderSmsNotificationFilter > -1, e => (input.OrderSmsNotificationFilter == 1 && e.OrderSmsNotification) || (input.OrderSmsNotificationFilter == 0 && !e.OrderSmsNotification))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter);

            var pagedAndFilteredStoreAccountTeams = filteredStoreAccountTeams
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeAccountTeams = from o in pagedAndFilteredStoreAccountTeams
                                    join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    join o2 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o2.Id into j2
                                    from s2 in j2.DefaultIfEmpty()

                                    select new
                                    {

                                        o.Primary,
                                        o.Active,
                                        o.OrderEmailNotification,
                                        o.OrderSmsNotification,
                                        Id = o.Id,
                                        StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                        EmployeeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                    };

            var totalCount = await filteredStoreAccountTeams.CountAsync();

            var dbList = await storeAccountTeams.ToListAsync();
            var results = new List<GetStoreAccountTeamForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreAccountTeamForViewDto()
                {
                    StoreAccountTeam = new StoreAccountTeamDto
                    {

                        Primary = o.Primary,
                        Active = o.Active,
                        OrderEmailNotification = o.OrderEmailNotification,
                        OrderSmsNotification = o.OrderSmsNotification,
                        Id = o.Id,
                    },
                    StoreName = o.StoreName,
                    EmployeeName = o.EmployeeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreAccountTeamForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreAccountTeamForViewDto> GetStoreAccountTeamForView(long id)
        {
            var storeAccountTeam = await _storeAccountTeamRepository.GetAsync(id);

            var output = new GetStoreAccountTeamForViewDto { StoreAccountTeam = ObjectMapper.Map<StoreAccountTeamDto>(storeAccountTeam) };

            if (output.StoreAccountTeam.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreAccountTeam.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreAccountTeam.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.StoreAccountTeam.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreAccountTeams_Edit)]
        public async Task<GetStoreAccountTeamForEditOutput> GetStoreAccountTeamForEdit(EntityDto<long> input)
        {
            var storeAccountTeam = await _storeAccountTeamRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreAccountTeamForEditOutput { StoreAccountTeam = ObjectMapper.Map<CreateOrEditStoreAccountTeamDto>(storeAccountTeam) };

            if (output.StoreAccountTeam.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreAccountTeam.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreAccountTeam.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.StoreAccountTeam.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreAccountTeamDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreAccountTeams_Create)]
        protected virtual async Task Create(CreateOrEditStoreAccountTeamDto input)
        {
            var storeAccountTeam = ObjectMapper.Map<StoreAccountTeam>(input);

            if (AbpSession.TenantId != null)
            {
                storeAccountTeam.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeAccountTeamRepository.InsertAsync(storeAccountTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreAccountTeams_Edit)]
        protected virtual async Task Update(CreateOrEditStoreAccountTeamDto input)
        {
            var storeAccountTeam = await _storeAccountTeamRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeAccountTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreAccountTeams_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeAccountTeamRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreAccountTeamsToExcel(GetAllStoreAccountTeamsForExcelInput input)
        {

            var filteredStoreAccountTeams = _storeAccountTeamRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.EmployeeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PrimaryFilter.HasValue && input.PrimaryFilter > -1, e => (input.PrimaryFilter == 1 && e.Primary) || (input.PrimaryFilter == 0 && !e.Primary))
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(input.OrderEmailNotificationFilter.HasValue && input.OrderEmailNotificationFilter > -1, e => (input.OrderEmailNotificationFilter == 1 && e.OrderEmailNotification) || (input.OrderEmailNotificationFilter == 0 && !e.OrderEmailNotification))
                        .WhereIf(input.OrderSmsNotificationFilter.HasValue && input.OrderSmsNotificationFilter > -1, e => (input.OrderSmsNotificationFilter == 1 && e.OrderSmsNotification) || (input.OrderSmsNotificationFilter == 0 && !e.OrderSmsNotification))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter);

            var query = (from o in filteredStoreAccountTeams
                         join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetStoreAccountTeamForViewDto()
                         {
                             StoreAccountTeam = new StoreAccountTeamDto
                             {
                                 Primary = o.Primary,
                                 Active = o.Active,
                                 OrderEmailNotification = o.OrderEmailNotification,
                                 OrderSmsNotification = o.OrderSmsNotification,
                                 Id = o.Id
                             },
                             StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             EmployeeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var storeAccountTeamListDtos = await query.ToListAsync();

            return _storeAccountTeamsExcelExporter.ExportToFile(storeAccountTeamListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreAccountTeams)]
        public async Task<PagedResultDto<StoreAccountTeamStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreAccountTeamStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreAccountTeamStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreAccountTeamStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreAccountTeams)]
        public async Task<PagedResultDto<StoreAccountTeamEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_employeeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var employeeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreAccountTeamEmployeeLookupTableDto>();
            foreach (var employee in employeeList)
            {
                lookupTableDtoList.Add(new StoreAccountTeamEmployeeLookupTableDto
                {
                    Id = employee.Id,
                    DisplayName = employee.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreAccountTeamEmployeeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}