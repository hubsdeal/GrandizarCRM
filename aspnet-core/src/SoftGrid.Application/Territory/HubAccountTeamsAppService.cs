using SoftGrid.Territory;
using SoftGrid.CRM;
using SoftGrid.Authorization.Users;

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
    [AbpAuthorize(AppPermissions.Pages_HubAccountTeams)]
    public class HubAccountTeamsAppService : SoftGridAppServiceBase, IHubAccountTeamsAppService
    {
        private readonly IRepository<HubAccountTeam, long> _hubAccountTeamRepository;
        private readonly IHubAccountTeamsExcelExporter _hubAccountTeamsExcelExporter;
        private readonly IRepository<Hub, long> _lookup_hubRepository;
        private readonly IRepository<Employee, long> _lookup_employeeRepository;
        private readonly IRepository<User, long> _lookup_userRepository;

        public HubAccountTeamsAppService(IRepository<HubAccountTeam, long> hubAccountTeamRepository, IHubAccountTeamsExcelExporter hubAccountTeamsExcelExporter, IRepository<Hub, long> lookup_hubRepository, IRepository<Employee, long> lookup_employeeRepository, IRepository<User, long> lookup_userRepository)
        {
            _hubAccountTeamRepository = hubAccountTeamRepository;
            _hubAccountTeamsExcelExporter = hubAccountTeamsExcelExporter;
            _lookup_hubRepository = lookup_hubRepository;
            _lookup_employeeRepository = lookup_employeeRepository;
            _lookup_userRepository = lookup_userRepository;

        }

        public async Task<PagedResultDto<GetHubAccountTeamForViewDto>> GetAll(GetAllHubAccountTeamsInput input)
        {

            var filteredHubAccountTeams = _hubAccountTeamRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.EmployeeFk)
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PrimaryManagerFilter.HasValue && input.PrimaryManagerFilter > -1, e => (input.PrimaryManagerFilter == 1 && e.PrimaryManager) || (input.PrimaryManagerFilter == 0 && !e.PrimaryManager))
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var pagedAndFilteredHubAccountTeams = filteredHubAccountTeams
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var hubAccountTeams = from o in pagedAndFilteredHubAccountTeams
                                  join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                                  from s1 in j1.DefaultIfEmpty()

                                  join o2 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o2.Id into j2
                                  from s2 in j2.DefaultIfEmpty()

                                  join o3 in _lookup_userRepository.GetAll() on o.UserId equals o3.Id into j3
                                  from s3 in j3.DefaultIfEmpty()

                                  select new
                                  {

                                      o.PrimaryManager,
                                      o.StartDate,
                                      o.EndDate,
                                      Id = o.Id,
                                      HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                      EmployeeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                      UserName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                                  };

            var totalCount = await filteredHubAccountTeams.CountAsync();

            var dbList = await hubAccountTeams.ToListAsync();
            var results = new List<GetHubAccountTeamForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetHubAccountTeamForViewDto()
                {
                    HubAccountTeam = new HubAccountTeamDto
                    {

                        PrimaryManager = o.PrimaryManager,
                        StartDate = o.StartDate,
                        EndDate = o.EndDate,
                        Id = o.Id,
                    },
                    HubName = o.HubName,
                    EmployeeName = o.EmployeeName,
                    UserName = o.UserName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetHubAccountTeamForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetHubAccountTeamForViewDto> GetHubAccountTeamForView(long id)
        {
            var hubAccountTeam = await _hubAccountTeamRepository.GetAsync(id);

            var output = new GetHubAccountTeamForViewDto { HubAccountTeam = ObjectMapper.Map<HubAccountTeamDto>(hubAccountTeam) };

            if (output.HubAccountTeam.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.HubAccountTeam.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.HubAccountTeam.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.HubAccountTeam.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            if (output.HubAccountTeam.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.HubAccountTeam.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_HubAccountTeams_Edit)]
        public async Task<GetHubAccountTeamForEditOutput> GetHubAccountTeamForEdit(EntityDto<long> input)
        {
            var hubAccountTeam = await _hubAccountTeamRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHubAccountTeamForEditOutput { HubAccountTeam = ObjectMapper.Map<CreateOrEditHubAccountTeamDto>(hubAccountTeam) };

            if (output.HubAccountTeam.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.HubAccountTeam.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.HubAccountTeam.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.HubAccountTeam.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            if (output.HubAccountTeam.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.HubAccountTeam.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHubAccountTeamDto input)
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

        [AbpAuthorize(AppPermissions.Pages_HubAccountTeams_Create)]
        protected virtual async Task Create(CreateOrEditHubAccountTeamDto input)
        {
            var hubAccountTeam = ObjectMapper.Map<HubAccountTeam>(input);

            if (AbpSession.TenantId != null)
            {
                hubAccountTeam.TenantId = (int?)AbpSession.TenantId;
            }

            await _hubAccountTeamRepository.InsertAsync(hubAccountTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_HubAccountTeams_Edit)]
        protected virtual async Task Update(CreateOrEditHubAccountTeamDto input)
        {
            var hubAccountTeam = await _hubAccountTeamRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, hubAccountTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_HubAccountTeams_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _hubAccountTeamRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetHubAccountTeamsToExcel(GetAllHubAccountTeamsForExcelInput input)
        {

            var filteredHubAccountTeams = _hubAccountTeamRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.EmployeeFk)
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PrimaryManagerFilter.HasValue && input.PrimaryManagerFilter > -1, e => (input.PrimaryManagerFilter == 1 && e.PrimaryManager) || (input.PrimaryManagerFilter == 0 && !e.PrimaryManager))
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var query = (from o in filteredHubAccountTeams
                         join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_userRepository.GetAll() on o.UserId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetHubAccountTeamForViewDto()
                         {
                             HubAccountTeam = new HubAccountTeamDto
                             {
                                 PrimaryManager = o.PrimaryManager,
                                 StartDate = o.StartDate,
                                 EndDate = o.EndDate,
                                 Id = o.Id
                             },
                             HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             EmployeeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             UserName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var hubAccountTeamListDtos = await query.ToListAsync();

            return _hubAccountTeamsExcelExporter.ExportToFile(hubAccountTeamListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_HubAccountTeams)]
        public async Task<PagedResultDto<HubAccountTeamHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_hubRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var hubList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubAccountTeamHubLookupTableDto>();
            foreach (var hub in hubList)
            {
                lookupTableDtoList.Add(new HubAccountTeamHubLookupTableDto
                {
                    Id = hub.Id,
                    DisplayName = hub.Name?.ToString()
                });
            }

            return new PagedResultDto<HubAccountTeamHubLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HubAccountTeams)]
        public async Task<PagedResultDto<HubAccountTeamEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_employeeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var employeeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubAccountTeamEmployeeLookupTableDto>();
            foreach (var employee in employeeList)
            {
                lookupTableDtoList.Add(new HubAccountTeamEmployeeLookupTableDto
                {
                    Id = employee.Id,
                    DisplayName = employee.Name?.ToString()
                });
            }

            return new PagedResultDto<HubAccountTeamEmployeeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HubAccountTeams)]
        public async Task<PagedResultDto<HubAccountTeamUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubAccountTeamUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new HubAccountTeamUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<HubAccountTeamUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}