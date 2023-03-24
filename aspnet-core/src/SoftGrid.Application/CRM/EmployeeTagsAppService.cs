using SoftGrid.CRM;
using SoftGrid.LookupData;
using SoftGrid.LookupData;

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
    [AbpAuthorize(AppPermissions.Pages_EmployeeTags)]
    public class EmployeeTagsAppService : SoftGridAppServiceBase, IEmployeeTagsAppService
    {
        private readonly IRepository<EmployeeTag, long> _employeeTagRepository;
        private readonly IEmployeeTagsExcelExporter _employeeTagsExcelExporter;
        private readonly IRepository<Employee, long> _lookup_employeeRepository;
        private readonly IRepository<MasterTagCategory, long> _lookup_masterTagCategoryRepository;
        private readonly IRepository<MasterTag, long> _lookup_masterTagRepository;

        public EmployeeTagsAppService(IRepository<EmployeeTag, long> employeeTagRepository, IEmployeeTagsExcelExporter employeeTagsExcelExporter, IRepository<Employee, long> lookup_employeeRepository, IRepository<MasterTagCategory, long> lookup_masterTagCategoryRepository, IRepository<MasterTag, long> lookup_masterTagRepository)
        {
            _employeeTagRepository = employeeTagRepository;
            _employeeTagsExcelExporter = employeeTagsExcelExporter;
            _lookup_employeeRepository = lookup_employeeRepository;
            _lookup_masterTagCategoryRepository = lookup_masterTagCategoryRepository;
            _lookup_masterTagRepository = lookup_masterTagRepository;

        }

        public async Task<PagedResultDto<GetEmployeeTagForViewDto>> GetAll(GetAllEmployeeTagsInput input)
        {

            var filteredEmployeeTags = _employeeTagRepository.GetAll()
                        .Include(e => e.EmployeeFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.MasterTagFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTag.Contains(input.Filter) || e.TagValue.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagFilter), e => e.CustomTag.Contains(input.CustomTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TagValueFilter), e => e.TagValue.Contains(input.TagValueFilter))
                        .WhereIf(input.VerifiedFilter.HasValue && input.VerifiedFilter > -1, e => (input.VerifiedFilter == 1 && e.Verified) || (input.VerifiedFilter == 0 && !e.Verified))
                        .WhereIf(input.MinSequenceFilter != null, e => e.Sequence >= input.MinSequenceFilter)
                        .WhereIf(input.MaxSequenceFilter != null, e => e.Sequence <= input.MaxSequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.MasterTagFk != null && e.MasterTagFk.Name == input.MasterTagNameFilter);

            var pagedAndFilteredEmployeeTags = filteredEmployeeTags
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var employeeTags = from o in pagedAndFilteredEmployeeTags
                               join o1 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()

                               join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                               from s2 in j2.DefaultIfEmpty()

                               join o3 in _lookup_masterTagRepository.GetAll() on o.MasterTagId equals o3.Id into j3
                               from s3 in j3.DefaultIfEmpty()

                               select new
                               {

                                   o.CustomTag,
                                   o.TagValue,
                                   o.Verified,
                                   o.Sequence,
                                   Id = o.Id,
                                   EmployeeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                   MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                   MasterTagName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                               };

            var totalCount = await filteredEmployeeTags.CountAsync();

            var dbList = await employeeTags.ToListAsync();
            var results = new List<GetEmployeeTagForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetEmployeeTagForViewDto()
                {
                    EmployeeTag = new EmployeeTagDto
                    {

                        CustomTag = o.CustomTag,
                        TagValue = o.TagValue,
                        Verified = o.Verified,
                        Sequence = o.Sequence,
                        Id = o.Id,
                    },
                    EmployeeName = o.EmployeeName,
                    MasterTagCategoryName = o.MasterTagCategoryName,
                    MasterTagName = o.MasterTagName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetEmployeeTagForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetEmployeeTagForViewDto> GetEmployeeTagForView(long id)
        {
            var employeeTag = await _employeeTagRepository.GetAsync(id);

            var output = new GetEmployeeTagForViewDto { EmployeeTag = ObjectMapper.Map<EmployeeTagDto>(employeeTag) };

            if (output.EmployeeTag.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.EmployeeTag.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            if (output.EmployeeTag.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.EmployeeTag.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.EmployeeTag.MasterTagId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.EmployeeTag.MasterTagId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_EmployeeTags_Edit)]
        public async Task<GetEmployeeTagForEditOutput> GetEmployeeTagForEdit(EntityDto<long> input)
        {
            var employeeTag = await _employeeTagRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetEmployeeTagForEditOutput { EmployeeTag = ObjectMapper.Map<CreateOrEditEmployeeTagDto>(employeeTag) };

            if (output.EmployeeTag.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.EmployeeTag.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            if (output.EmployeeTag.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.EmployeeTag.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.EmployeeTag.MasterTagId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.EmployeeTag.MasterTagId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditEmployeeTagDto input)
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

        [AbpAuthorize(AppPermissions.Pages_EmployeeTags_Create)]
        protected virtual async Task Create(CreateOrEditEmployeeTagDto input)
        {
            var employeeTag = ObjectMapper.Map<EmployeeTag>(input);

            if (AbpSession.TenantId != null)
            {
                employeeTag.TenantId = (int?)AbpSession.TenantId;
            }

            await _employeeTagRepository.InsertAsync(employeeTag);

        }

        [AbpAuthorize(AppPermissions.Pages_EmployeeTags_Edit)]
        protected virtual async Task Update(CreateOrEditEmployeeTagDto input)
        {
            var employeeTag = await _employeeTagRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, employeeTag);

        }

        [AbpAuthorize(AppPermissions.Pages_EmployeeTags_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _employeeTagRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetEmployeeTagsToExcel(GetAllEmployeeTagsForExcelInput input)
        {

            var filteredEmployeeTags = _employeeTagRepository.GetAll()
                        .Include(e => e.EmployeeFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.MasterTagFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTag.Contains(input.Filter) || e.TagValue.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagFilter), e => e.CustomTag.Contains(input.CustomTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TagValueFilter), e => e.TagValue.Contains(input.TagValueFilter))
                        .WhereIf(input.VerifiedFilter.HasValue && input.VerifiedFilter > -1, e => (input.VerifiedFilter == 1 && e.Verified) || (input.VerifiedFilter == 0 && !e.Verified))
                        .WhereIf(input.MinSequenceFilter != null, e => e.Sequence >= input.MinSequenceFilter)
                        .WhereIf(input.MaxSequenceFilter != null, e => e.Sequence <= input.MaxSequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.MasterTagFk != null && e.MasterTagFk.Name == input.MasterTagNameFilter);

            var query = (from o in filteredEmployeeTags
                         join o1 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_masterTagRepository.GetAll() on o.MasterTagId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetEmployeeTagForViewDto()
                         {
                             EmployeeTag = new EmployeeTagDto
                             {
                                 CustomTag = o.CustomTag,
                                 TagValue = o.TagValue,
                                 Verified = o.Verified,
                                 Sequence = o.Sequence,
                                 Id = o.Id
                             },
                             EmployeeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             MasterTagName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var employeeTagListDtos = await query.ToListAsync();

            return _employeeTagsExcelExporter.ExportToFile(employeeTagListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_EmployeeTags)]
        public async Task<PagedResultDto<EmployeeTagEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_employeeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var employeeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<EmployeeTagEmployeeLookupTableDto>();
            foreach (var employee in employeeList)
            {
                lookupTableDtoList.Add(new EmployeeTagEmployeeLookupTableDto
                {
                    Id = employee.Id,
                    DisplayName = employee.Name?.ToString()
                });
            }

            return new PagedResultDto<EmployeeTagEmployeeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_EmployeeTags)]
        public async Task<PagedResultDto<EmployeeTagMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<EmployeeTagMasterTagCategoryLookupTableDto>();
            foreach (var masterTagCategory in masterTagCategoryList)
            {
                lookupTableDtoList.Add(new EmployeeTagMasterTagCategoryLookupTableDto
                {
                    Id = masterTagCategory.Id,
                    DisplayName = masterTagCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<EmployeeTagMasterTagCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_EmployeeTags)]
        public async Task<PagedResultDto<EmployeeTagMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<EmployeeTagMasterTagLookupTableDto>();
            foreach (var masterTag in masterTagList)
            {
                lookupTableDtoList.Add(new EmployeeTagMasterTagLookupTableDto
                {
                    Id = masterTag.Id,
                    DisplayName = masterTag.Name?.ToString()
                });
            }

            return new PagedResultDto<EmployeeTagMasterTagLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}