using SoftGrid.SalesLeadManagement;
using SoftGrid.CRM;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.SalesLeadManagement.Exporting;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.SalesLeadManagement
{
    [AbpAuthorize(AppPermissions.Pages_LeadSalesTeams)]
    public class LeadSalesTeamsAppService : SoftGridAppServiceBase, ILeadSalesTeamsAppService
    {
        private readonly IRepository<LeadSalesTeam, long> _leadSalesTeamRepository;
        private readonly ILeadSalesTeamsExcelExporter _leadSalesTeamsExcelExporter;
        private readonly IRepository<Lead, long> _lookup_leadRepository;
        private readonly IRepository<Employee, long> _lookup_employeeRepository;

        public LeadSalesTeamsAppService(IRepository<LeadSalesTeam, long> leadSalesTeamRepository, ILeadSalesTeamsExcelExporter leadSalesTeamsExcelExporter, IRepository<Lead, long> lookup_leadRepository, IRepository<Employee, long> lookup_employeeRepository)
        {
            _leadSalesTeamRepository = leadSalesTeamRepository;
            _leadSalesTeamsExcelExporter = leadSalesTeamsExcelExporter;
            _lookup_leadRepository = lookup_leadRepository;
            _lookup_employeeRepository = lookup_employeeRepository;

        }

        public async Task<PagedResultDto<GetLeadSalesTeamForViewDto>> GetAll(GetAllLeadSalesTeamsInput input)
        {

            var filteredLeadSalesTeams = _leadSalesTeamRepository.GetAll()
                        .Include(e => e.LeadFk)
                        .Include(e => e.EmployeeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PrimaryFilter.HasValue && input.PrimaryFilter > -1, e => (input.PrimaryFilter == 1 && e.Primary) || (input.PrimaryFilter == 0 && !e.Primary))
                        .WhereIf(input.MinAssignedDateFilter != null, e => e.AssignedDate >= input.MinAssignedDateFilter)
                        .WhereIf(input.MaxAssignedDateFilter != null, e => e.AssignedDate <= input.MaxAssignedDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LeadFirstNameFilter), e => e.LeadFk != null && e.LeadFk.FirstName == input.LeadFirstNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter);

            var pagedAndFilteredLeadSalesTeams = filteredLeadSalesTeams
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var leadSalesTeams = from o in pagedAndFilteredLeadSalesTeams
                                 join o1 in _lookup_leadRepository.GetAll() on o.LeadId equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()

                                 join o2 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o2.Id into j2
                                 from s2 in j2.DefaultIfEmpty()

                                 select new
                                 {

                                     o.Primary,
                                     o.AssignedDate,
                                     Id = o.Id,
                                     LeadFirstName = s1 == null || s1.FirstName == null ? "" : s1.FirstName.ToString(),
                                     EmployeeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                 };

            var totalCount = await filteredLeadSalesTeams.CountAsync();

            var dbList = await leadSalesTeams.ToListAsync();
            var results = new List<GetLeadSalesTeamForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetLeadSalesTeamForViewDto()
                {
                    LeadSalesTeam = new LeadSalesTeamDto
                    {

                        Primary = o.Primary,
                        AssignedDate = o.AssignedDate,
                        Id = o.Id,
                    },
                    LeadFirstName = o.LeadFirstName,
                    EmployeeName = o.EmployeeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetLeadSalesTeamForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetLeadSalesTeamForViewDto> GetLeadSalesTeamForView(long id)
        {
            var leadSalesTeam = await _leadSalesTeamRepository.GetAsync(id);

            var output = new GetLeadSalesTeamForViewDto { LeadSalesTeam = ObjectMapper.Map<LeadSalesTeamDto>(leadSalesTeam) };

            if (output.LeadSalesTeam.LeadId != null)
            {
                var _lookupLead = await _lookup_leadRepository.FirstOrDefaultAsync((long)output.LeadSalesTeam.LeadId);
                output.LeadFirstName = _lookupLead?.FirstName?.ToString();
            }

            if (output.LeadSalesTeam.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.LeadSalesTeam.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_LeadSalesTeams_Edit)]
        public async Task<GetLeadSalesTeamForEditOutput> GetLeadSalesTeamForEdit(EntityDto<long> input)
        {
            var leadSalesTeam = await _leadSalesTeamRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLeadSalesTeamForEditOutput { LeadSalesTeam = ObjectMapper.Map<CreateOrEditLeadSalesTeamDto>(leadSalesTeam) };

            if (output.LeadSalesTeam.LeadId != null)
            {
                var _lookupLead = await _lookup_leadRepository.FirstOrDefaultAsync((long)output.LeadSalesTeam.LeadId);
                output.LeadFirstName = _lookupLead?.FirstName?.ToString();
            }

            if (output.LeadSalesTeam.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.LeadSalesTeam.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditLeadSalesTeamDto input)
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

        [AbpAuthorize(AppPermissions.Pages_LeadSalesTeams_Create)]
        protected virtual async Task Create(CreateOrEditLeadSalesTeamDto input)
        {
            var leadSalesTeam = ObjectMapper.Map<LeadSalesTeam>(input);

            if (AbpSession.TenantId != null)
            {
                leadSalesTeam.TenantId = (int?)AbpSession.TenantId;
            }

            await _leadSalesTeamRepository.InsertAsync(leadSalesTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_LeadSalesTeams_Edit)]
        protected virtual async Task Update(CreateOrEditLeadSalesTeamDto input)
        {
            var leadSalesTeam = await _leadSalesTeamRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, leadSalesTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_LeadSalesTeams_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _leadSalesTeamRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetLeadSalesTeamsToExcel(GetAllLeadSalesTeamsForExcelInput input)
        {

            var filteredLeadSalesTeams = _leadSalesTeamRepository.GetAll()
                        .Include(e => e.LeadFk)
                        .Include(e => e.EmployeeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PrimaryFilter.HasValue && input.PrimaryFilter > -1, e => (input.PrimaryFilter == 1 && e.Primary) || (input.PrimaryFilter == 0 && !e.Primary))
                        .WhereIf(input.MinAssignedDateFilter != null, e => e.AssignedDate >= input.MinAssignedDateFilter)
                        .WhereIf(input.MaxAssignedDateFilter != null, e => e.AssignedDate <= input.MaxAssignedDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LeadFirstNameFilter), e => e.LeadFk != null && e.LeadFk.FirstName == input.LeadFirstNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter);

            var query = (from o in filteredLeadSalesTeams
                         join o1 in _lookup_leadRepository.GetAll() on o.LeadId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetLeadSalesTeamForViewDto()
                         {
                             LeadSalesTeam = new LeadSalesTeamDto
                             {
                                 Primary = o.Primary,
                                 AssignedDate = o.AssignedDate,
                                 Id = o.Id
                             },
                             LeadFirstName = s1 == null || s1.FirstName == null ? "" : s1.FirstName.ToString(),
                             EmployeeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var leadSalesTeamListDtos = await query.ToListAsync();

            return _leadSalesTeamsExcelExporter.ExportToFile(leadSalesTeamListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_LeadSalesTeams)]
        public async Task<PagedResultDto<LeadSalesTeamLeadLookupTableDto>> GetAllLeadForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_leadRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FirstName != null && e.FirstName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var leadList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadSalesTeamLeadLookupTableDto>();
            foreach (var lead in leadList)
            {
                lookupTableDtoList.Add(new LeadSalesTeamLeadLookupTableDto
                {
                    Id = lead.Id,
                    DisplayName = lead.FirstName?.ToString()
                });
            }

            return new PagedResultDto<LeadSalesTeamLeadLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_LeadSalesTeams)]
        public async Task<PagedResultDto<LeadSalesTeamEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_employeeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var employeeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadSalesTeamEmployeeLookupTableDto>();
            foreach (var employee in employeeList)
            {
                lookupTableDtoList.Add(new LeadSalesTeamEmployeeLookupTableDto
                {
                    Id = employee.Id,
                    DisplayName = employee.Name?.ToString()
                });
            }

            return new PagedResultDto<LeadSalesTeamEmployeeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}