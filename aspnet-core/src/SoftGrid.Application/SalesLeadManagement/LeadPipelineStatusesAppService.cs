using SoftGrid.SalesLeadManagement;
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
    [AbpAuthorize(AppPermissions.Pages_LeadPipelineStatuses)]
    public class LeadPipelineStatusesAppService : SoftGridAppServiceBase, ILeadPipelineStatusesAppService
    {
        private readonly IRepository<LeadPipelineStatus, long> _leadPipelineStatusRepository;
        private readonly ILeadPipelineStatusesExcelExporter _leadPipelineStatusesExcelExporter;
        private readonly IRepository<Lead, long> _lookup_leadRepository;
        private readonly IRepository<LeadPipelineStage, long> _lookup_leadPipelineStageRepository;
        private readonly IRepository<Employee, long> _lookup_employeeRepository;

        public LeadPipelineStatusesAppService(IRepository<LeadPipelineStatus, long> leadPipelineStatusRepository, ILeadPipelineStatusesExcelExporter leadPipelineStatusesExcelExporter, IRepository<Lead, long> lookup_leadRepository, IRepository<LeadPipelineStage, long> lookup_leadPipelineStageRepository, IRepository<Employee, long> lookup_employeeRepository)
        {
            _leadPipelineStatusRepository = leadPipelineStatusRepository;
            _leadPipelineStatusesExcelExporter = leadPipelineStatusesExcelExporter;
            _lookup_leadRepository = lookup_leadRepository;
            _lookup_leadPipelineStageRepository = lookup_leadPipelineStageRepository;
            _lookup_employeeRepository = lookup_employeeRepository;

        }

        public async Task<PagedResultDto<GetLeadPipelineStatusForViewDto>> GetAll(GetAllLeadPipelineStatusesInput input)
        {

            var filteredLeadPipelineStatuses = _leadPipelineStatusRepository.GetAll()
                        .Include(e => e.LeadFk)
                        .Include(e => e.LeadPipelineStageFk)
                        .Include(e => e.EmployeeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ExitDate.Contains(input.Filter))
                        .WhereIf(input.MinEntryDateFilter != null, e => e.EntryDate >= input.MinEntryDateFilter)
                        .WhereIf(input.MaxEntryDateFilter != null, e => e.EntryDate <= input.MaxEntryDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExitDateFilter), e => e.ExitDate.Contains(input.ExitDateFilter))
                        .WhereIf(input.MinEnteredAtFilter != null, e => e.EnteredAt >= input.MinEnteredAtFilter)
                        .WhereIf(input.MaxEnteredAtFilter != null, e => e.EnteredAt <= input.MaxEnteredAtFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LeadTitleFilter), e => e.LeadFk != null && e.LeadFk.Title == input.LeadTitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LeadPipelineStageNameFilter), e => e.LeadPipelineStageFk != null && e.LeadPipelineStageFk.Name == input.LeadPipelineStageNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter);

            var pagedAndFilteredLeadPipelineStatuses = filteredLeadPipelineStatuses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var leadPipelineStatuses = from o in pagedAndFilteredLeadPipelineStatuses
                                       join o1 in _lookup_leadRepository.GetAll() on o.LeadId equals o1.Id into j1
                                       from s1 in j1.DefaultIfEmpty()

                                       join o2 in _lookup_leadPipelineStageRepository.GetAll() on o.LeadPipelineStageId equals o2.Id into j2
                                       from s2 in j2.DefaultIfEmpty()

                                       join o3 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o3.Id into j3
                                       from s3 in j3.DefaultIfEmpty()

                                       select new
                                       {

                                           o.EntryDate,
                                           o.ExitDate,
                                           o.EnteredAt,
                                           Id = o.Id,
                                           LeadTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString(),
                                           LeadPipelineStageName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                           EmployeeName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                                       };

            var totalCount = await filteredLeadPipelineStatuses.CountAsync();

            var dbList = await leadPipelineStatuses.ToListAsync();
            var results = new List<GetLeadPipelineStatusForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetLeadPipelineStatusForViewDto()
                {
                    LeadPipelineStatus = new LeadPipelineStatusDto
                    {

                        EntryDate = o.EntryDate,
                        ExitDate = o.ExitDate,
                        EnteredAt = o.EnteredAt,
                        Id = o.Id,
                    },
                    LeadTitle = o.LeadTitle,
                    LeadPipelineStageName = o.LeadPipelineStageName,
                    EmployeeName = o.EmployeeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetLeadPipelineStatusForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetLeadPipelineStatusForViewDto> GetLeadPipelineStatusForView(long id)
        {
            var leadPipelineStatus = await _leadPipelineStatusRepository.GetAsync(id);

            var output = new GetLeadPipelineStatusForViewDto { LeadPipelineStatus = ObjectMapper.Map<LeadPipelineStatusDto>(leadPipelineStatus) };

            if (output.LeadPipelineStatus.LeadId != null)
            {
                var _lookupLead = await _lookup_leadRepository.FirstOrDefaultAsync((long)output.LeadPipelineStatus.LeadId);
                output.LeadTitle = _lookupLead?.Title?.ToString();
            }

            if (output.LeadPipelineStatus.LeadPipelineStageId != null)
            {
                var _lookupLeadPipelineStage = await _lookup_leadPipelineStageRepository.FirstOrDefaultAsync((long)output.LeadPipelineStatus.LeadPipelineStageId);
                output.LeadPipelineStageName = _lookupLeadPipelineStage?.Name?.ToString();
            }

            if (output.LeadPipelineStatus.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.LeadPipelineStatus.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_LeadPipelineStatuses_Edit)]
        public async Task<GetLeadPipelineStatusForEditOutput> GetLeadPipelineStatusForEdit(EntityDto<long> input)
        {
            var leadPipelineStatus = await _leadPipelineStatusRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLeadPipelineStatusForEditOutput { LeadPipelineStatus = ObjectMapper.Map<CreateOrEditLeadPipelineStatusDto>(leadPipelineStatus) };

            if (output.LeadPipelineStatus.LeadId != null)
            {
                var _lookupLead = await _lookup_leadRepository.FirstOrDefaultAsync((long)output.LeadPipelineStatus.LeadId);
                output.LeadTitle = _lookupLead?.Title?.ToString();
            }

            if (output.LeadPipelineStatus.LeadPipelineStageId != null)
            {
                var _lookupLeadPipelineStage = await _lookup_leadPipelineStageRepository.FirstOrDefaultAsync((long)output.LeadPipelineStatus.LeadPipelineStageId);
                output.LeadPipelineStageName = _lookupLeadPipelineStage?.Name?.ToString();
            }

            if (output.LeadPipelineStatus.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.LeadPipelineStatus.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditLeadPipelineStatusDto input)
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

        [AbpAuthorize(AppPermissions.Pages_LeadPipelineStatuses_Create)]
        protected virtual async Task Create(CreateOrEditLeadPipelineStatusDto input)
        {
            var leadPipelineStatus = ObjectMapper.Map<LeadPipelineStatus>(input);

            if (AbpSession.TenantId != null)
            {
                leadPipelineStatus.TenantId = (int?)AbpSession.TenantId;
            }

            await _leadPipelineStatusRepository.InsertAsync(leadPipelineStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_LeadPipelineStatuses_Edit)]
        protected virtual async Task Update(CreateOrEditLeadPipelineStatusDto input)
        {
            var leadPipelineStatus = await _leadPipelineStatusRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, leadPipelineStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_LeadPipelineStatuses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _leadPipelineStatusRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetLeadPipelineStatusesToExcel(GetAllLeadPipelineStatusesForExcelInput input)
        {

            var filteredLeadPipelineStatuses = _leadPipelineStatusRepository.GetAll()
                        .Include(e => e.LeadFk)
                        .Include(e => e.LeadPipelineStageFk)
                        .Include(e => e.EmployeeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ExitDate.Contains(input.Filter))
                        .WhereIf(input.MinEntryDateFilter != null, e => e.EntryDate >= input.MinEntryDateFilter)
                        .WhereIf(input.MaxEntryDateFilter != null, e => e.EntryDate <= input.MaxEntryDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExitDateFilter), e => e.ExitDate.Contains(input.ExitDateFilter))
                        .WhereIf(input.MinEnteredAtFilter != null, e => e.EnteredAt >= input.MinEnteredAtFilter)
                        .WhereIf(input.MaxEnteredAtFilter != null, e => e.EnteredAt <= input.MaxEnteredAtFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LeadTitleFilter), e => e.LeadFk != null && e.LeadFk.Title == input.LeadTitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LeadPipelineStageNameFilter), e => e.LeadPipelineStageFk != null && e.LeadPipelineStageFk.Name == input.LeadPipelineStageNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter);

            var query = (from o in filteredLeadPipelineStatuses
                         join o1 in _lookup_leadRepository.GetAll() on o.LeadId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_leadPipelineStageRepository.GetAll() on o.LeadPipelineStageId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetLeadPipelineStatusForViewDto()
                         {
                             LeadPipelineStatus = new LeadPipelineStatusDto
                             {
                                 EntryDate = o.EntryDate,
                                 ExitDate = o.ExitDate,
                                 EnteredAt = o.EnteredAt,
                                 Id = o.Id
                             },
                             LeadTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString(),
                             LeadPipelineStageName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             EmployeeName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var leadPipelineStatusListDtos = await query.ToListAsync();

            return _leadPipelineStatusesExcelExporter.ExportToFile(leadPipelineStatusListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_LeadPipelineStatuses)]
        public async Task<PagedResultDto<LeadPipelineStatusLeadLookupTableDto>> GetAllLeadForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_leadRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Title != null && e.Title.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var leadList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadPipelineStatusLeadLookupTableDto>();
            foreach (var lead in leadList)
            {
                lookupTableDtoList.Add(new LeadPipelineStatusLeadLookupTableDto
                {
                    Id = lead.Id,
                    DisplayName = lead.Title?.ToString()
                });
            }

            return new PagedResultDto<LeadPipelineStatusLeadLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_LeadPipelineStatuses)]
        public async Task<PagedResultDto<LeadPipelineStatusLeadPipelineStageLookupTableDto>> GetAllLeadPipelineStageForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_leadPipelineStageRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var leadPipelineStageList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadPipelineStatusLeadPipelineStageLookupTableDto>();
            foreach (var leadPipelineStage in leadPipelineStageList)
            {
                lookupTableDtoList.Add(new LeadPipelineStatusLeadPipelineStageLookupTableDto
                {
                    Id = leadPipelineStage.Id,
                    DisplayName = leadPipelineStage.Name?.ToString()
                });
            }

            return new PagedResultDto<LeadPipelineStatusLeadPipelineStageLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_LeadPipelineStatuses)]
        public async Task<PagedResultDto<LeadPipelineStatusEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_employeeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var employeeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadPipelineStatusEmployeeLookupTableDto>();
            foreach (var employee in employeeList)
            {
                lookupTableDtoList.Add(new LeadPipelineStatusEmployeeLookupTableDto
                {
                    Id = employee.Id,
                    DisplayName = employee.Name?.ToString()
                });
            }

            return new PagedResultDto<LeadPipelineStatusEmployeeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}