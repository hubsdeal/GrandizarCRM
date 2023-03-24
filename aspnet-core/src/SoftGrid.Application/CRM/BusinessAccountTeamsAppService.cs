using SoftGrid.CRM;
using SoftGrid.CRM;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.CRM.Exporting;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.CRM
{
    [AbpAuthorize(AppPermissions.Pages_BusinessAccountTeams)]
    public class BusinessAccountTeamsAppService : SoftGridAppServiceBase, IBusinessAccountTeamsAppService
    {
        private readonly IRepository<BusinessAccountTeam, long> _businessAccountTeamRepository;
        private readonly IBusinessAccountTeamsExcelExporter _businessAccountTeamsExcelExporter;
        private readonly IRepository<Business, long> _lookup_businessRepository;
        private readonly IRepository<Employee, long> _lookup_employeeRepository;

        public BusinessAccountTeamsAppService(IRepository<BusinessAccountTeam, long> businessAccountTeamRepository, IBusinessAccountTeamsExcelExporter businessAccountTeamsExcelExporter, IRepository<Business, long> lookup_businessRepository, IRepository<Employee, long> lookup_employeeRepository)
        {
            _businessAccountTeamRepository = businessAccountTeamRepository;
            _businessAccountTeamsExcelExporter = businessAccountTeamsExcelExporter;
            _lookup_businessRepository = lookup_businessRepository;
            _lookup_employeeRepository = lookup_employeeRepository;

        }

        public async Task<PagedResultDto<GetBusinessAccountTeamForViewDto>> GetAll(GetAllBusinessAccountTeamsInput input)
        {

            var filteredBusinessAccountTeams = _businessAccountTeamRepository.GetAll()
                        .Include(e => e.BusinessFk)
                        .Include(e => e.EmployeeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PrimaryFilter.HasValue && input.PrimaryFilter > -1, e => (input.PrimaryFilter == 1 && e.Primary) || (input.PrimaryFilter == 0 && !e.Primary))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter);

            var pagedAndFilteredBusinessAccountTeams = filteredBusinessAccountTeams
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var businessAccountTeams = from o in pagedAndFilteredBusinessAccountTeams
                                       join o1 in _lookup_businessRepository.GetAll() on o.BusinessId equals o1.Id into j1
                                       from s1 in j1.DefaultIfEmpty()

                                       join o2 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o2.Id into j2
                                       from s2 in j2.DefaultIfEmpty()

                                       select new
                                       {

                                           o.Primary,
                                           Id = o.Id,
                                           BusinessName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                           EmployeeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                       };

            var totalCount = await filteredBusinessAccountTeams.CountAsync();

            var dbList = await businessAccountTeams.ToListAsync();
            var results = new List<GetBusinessAccountTeamForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBusinessAccountTeamForViewDto()
                {
                    BusinessAccountTeam = new BusinessAccountTeamDto
                    {

                        Primary = o.Primary,
                        Id = o.Id,
                    },
                    BusinessName = o.BusinessName,
                    EmployeeName = o.EmployeeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetBusinessAccountTeamForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetBusinessAccountTeamForViewDto> GetBusinessAccountTeamForView(long id)
        {
            var businessAccountTeam = await _businessAccountTeamRepository.GetAsync(id);

            var output = new GetBusinessAccountTeamForViewDto { BusinessAccountTeam = ObjectMapper.Map<BusinessAccountTeamDto>(businessAccountTeam) };

            if (output.BusinessAccountTeam.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.BusinessAccountTeam.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            if (output.BusinessAccountTeam.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.BusinessAccountTeam.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessAccountTeams_Edit)]
        public async Task<GetBusinessAccountTeamForEditOutput> GetBusinessAccountTeamForEdit(EntityDto<long> input)
        {
            var businessAccountTeam = await _businessAccountTeamRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBusinessAccountTeamForEditOutput { BusinessAccountTeam = ObjectMapper.Map<CreateOrEditBusinessAccountTeamDto>(businessAccountTeam) };

            if (output.BusinessAccountTeam.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.BusinessAccountTeam.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            if (output.BusinessAccountTeam.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.BusinessAccountTeam.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBusinessAccountTeamDto input)
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

        [AbpAuthorize(AppPermissions.Pages_BusinessAccountTeams_Create)]
        protected virtual async Task Create(CreateOrEditBusinessAccountTeamDto input)
        {
            var businessAccountTeam = ObjectMapper.Map<BusinessAccountTeam>(input);

            if (AbpSession.TenantId != null)
            {
                businessAccountTeam.TenantId = (int?)AbpSession.TenantId;
            }

            await _businessAccountTeamRepository.InsertAsync(businessAccountTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessAccountTeams_Edit)]
        protected virtual async Task Update(CreateOrEditBusinessAccountTeamDto input)
        {
            var businessAccountTeam = await _businessAccountTeamRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, businessAccountTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessAccountTeams_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _businessAccountTeamRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetBusinessAccountTeamsToExcel(GetAllBusinessAccountTeamsForExcelInput input)
        {

            var filteredBusinessAccountTeams = _businessAccountTeamRepository.GetAll()
                        .Include(e => e.BusinessFk)
                        .Include(e => e.EmployeeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PrimaryFilter.HasValue && input.PrimaryFilter > -1, e => (input.PrimaryFilter == 1 && e.Primary) || (input.PrimaryFilter == 0 && !e.Primary))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter);

            var query = (from o in filteredBusinessAccountTeams
                         join o1 in _lookup_businessRepository.GetAll() on o.BusinessId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetBusinessAccountTeamForViewDto()
                         {
                             BusinessAccountTeam = new BusinessAccountTeamDto
                             {
                                 Primary = o.Primary,
                                 Id = o.Id
                             },
                             BusinessName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             EmployeeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var businessAccountTeamListDtos = await query.ToListAsync();

            return _businessAccountTeamsExcelExporter.ExportToFile(businessAccountTeamListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessAccountTeams)]
        public async Task<PagedResultDto<BusinessAccountTeamBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_businessRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var businessList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BusinessAccountTeamBusinessLookupTableDto>();
            foreach (var business in businessList)
            {
                lookupTableDtoList.Add(new BusinessAccountTeamBusinessLookupTableDto
                {
                    Id = business.Id,
                    DisplayName = business.Name?.ToString()
                });
            }

            return new PagedResultDto<BusinessAccountTeamBusinessLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessAccountTeams)]
        public async Task<PagedResultDto<BusinessAccountTeamEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_employeeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var employeeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BusinessAccountTeamEmployeeLookupTableDto>();
            foreach (var employee in employeeList)
            {
                lookupTableDtoList.Add(new BusinessAccountTeamEmployeeLookupTableDto
                {
                    Id = employee.Id,
                    DisplayName = employee.Name?.ToString()
                });
            }

            return new PagedResultDto<BusinessAccountTeamEmployeeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}