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
    [AbpAuthorize(AppPermissions.Pages_ContactTags)]
    public class ContactTagsAppService : SoftGridAppServiceBase, IContactTagsAppService
    {
        private readonly IRepository<ContactTag, long> _contactTagRepository;
        private readonly IContactTagsExcelExporter _contactTagsExcelExporter;
        private readonly IRepository<Contact, long> _lookup_contactRepository;
        private readonly IRepository<MasterTagCategory, long> _lookup_masterTagCategoryRepository;
        private readonly IRepository<MasterTag, long> _lookup_masterTagRepository;

        public ContactTagsAppService(IRepository<ContactTag, long> contactTagRepository, IContactTagsExcelExporter contactTagsExcelExporter, IRepository<Contact, long> lookup_contactRepository, IRepository<MasterTagCategory, long> lookup_masterTagCategoryRepository, IRepository<MasterTag, long> lookup_masterTagRepository)
        {
            _contactTagRepository = contactTagRepository;
            _contactTagsExcelExporter = contactTagsExcelExporter;
            _lookup_contactRepository = lookup_contactRepository;
            _lookup_masterTagCategoryRepository = lookup_masterTagCategoryRepository;
            _lookup_masterTagRepository = lookup_masterTagRepository;

        }

        public async Task<PagedResultDto<GetContactTagForViewDto>> GetAll(GetAllContactTagsInput input)
        {

            var filteredContactTags = _contactTagRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.MasterTagFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTag.Contains(input.Filter) || e.TagValue.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagFilter), e => e.CustomTag.Contains(input.CustomTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TagValueFilter), e => e.TagValue.Contains(input.TagValueFilter))
                        .WhereIf(input.VerifiedFilter.HasValue && input.VerifiedFilter > -1, e => (input.VerifiedFilter == 1 && e.Verified) || (input.VerifiedFilter == 0 && !e.Verified))
                        .WhereIf(input.MinSequenceFilter != null, e => e.Sequence >= input.MinSequenceFilter)
                        .WhereIf(input.MaxSequenceFilter != null, e => e.Sequence <= input.MaxSequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.MasterTagFk != null && e.MasterTagFk.Name == input.MasterTagNameFilter);

            var pagedAndFilteredContactTags = filteredContactTags
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var contactTags = from o in pagedAndFilteredContactTags
                              join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
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
                                  ContactFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                                  MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                  MasterTagName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                              };

            var totalCount = await filteredContactTags.CountAsync();

            var dbList = await contactTags.ToListAsync();
            var results = new List<GetContactTagForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetContactTagForViewDto()
                {
                    ContactTag = new ContactTagDto
                    {

                        CustomTag = o.CustomTag,
                        TagValue = o.TagValue,
                        Verified = o.Verified,
                        Sequence = o.Sequence,
                        Id = o.Id,
                    },
                    ContactFullName = o.ContactFullName,
                    MasterTagCategoryName = o.MasterTagCategoryName,
                    MasterTagName = o.MasterTagName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetContactTagForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetContactTagForViewDto> GetContactTagForView(long id)
        {
            var contactTag = await _contactTagRepository.GetAsync(id);

            var output = new GetContactTagForViewDto { ContactTag = ObjectMapper.Map<ContactTagDto>(contactTag) };

            if (output.ContactTag.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.ContactTag.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.ContactTag.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.ContactTag.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.ContactTag.MasterTagId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.ContactTag.MasterTagId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ContactTags_Edit)]
        public async Task<GetContactTagForEditOutput> GetContactTagForEdit(EntityDto<long> input)
        {
            var contactTag = await _contactTagRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetContactTagForEditOutput { ContactTag = ObjectMapper.Map<CreateOrEditContactTagDto>(contactTag) };

            if (output.ContactTag.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.ContactTag.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.ContactTag.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.ContactTag.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.ContactTag.MasterTagId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.ContactTag.MasterTagId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditContactTagDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ContactTags_Create)]
        protected virtual async Task Create(CreateOrEditContactTagDto input)
        {
            var contactTag = ObjectMapper.Map<ContactTag>(input);

            if (AbpSession.TenantId != null)
            {
                contactTag.TenantId = (int?)AbpSession.TenantId;
            }

            await _contactTagRepository.InsertAsync(contactTag);

        }

        [AbpAuthorize(AppPermissions.Pages_ContactTags_Edit)]
        protected virtual async Task Update(CreateOrEditContactTagDto input)
        {
            var contactTag = await _contactTagRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, contactTag);

        }

        [AbpAuthorize(AppPermissions.Pages_ContactTags_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _contactTagRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetContactTagsToExcel(GetAllContactTagsForExcelInput input)
        {

            var filteredContactTags = _contactTagRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.MasterTagFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTag.Contains(input.Filter) || e.TagValue.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagFilter), e => e.CustomTag.Contains(input.CustomTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TagValueFilter), e => e.TagValue.Contains(input.TagValueFilter))
                        .WhereIf(input.VerifiedFilter.HasValue && input.VerifiedFilter > -1, e => (input.VerifiedFilter == 1 && e.Verified) || (input.VerifiedFilter == 0 && !e.Verified))
                        .WhereIf(input.MinSequenceFilter != null, e => e.Sequence >= input.MinSequenceFilter)
                        .WhereIf(input.MaxSequenceFilter != null, e => e.Sequence <= input.MaxSequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.MasterTagFk != null && e.MasterTagFk.Name == input.MasterTagNameFilter);

            var query = (from o in filteredContactTags
                         join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_masterTagRepository.GetAll() on o.MasterTagId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetContactTagForViewDto()
                         {
                             ContactTag = new ContactTagDto
                             {
                                 CustomTag = o.CustomTag,
                                 TagValue = o.TagValue,
                                 Verified = o.Verified,
                                 Sequence = o.Sequence,
                                 Id = o.Id
                             },
                             ContactFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                             MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             MasterTagName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var contactTagListDtos = await query.ToListAsync();

            return _contactTagsExcelExporter.ExportToFile(contactTagListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ContactTags)]
        public async Task<PagedResultDto<ContactTagContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ContactTagContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new ContactTagContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<ContactTagContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ContactTags)]
        public async Task<PagedResultDto<ContactTagMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ContactTagMasterTagCategoryLookupTableDto>();
            foreach (var masterTagCategory in masterTagCategoryList)
            {
                lookupTableDtoList.Add(new ContactTagMasterTagCategoryLookupTableDto
                {
                    Id = masterTagCategory.Id,
                    DisplayName = masterTagCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<ContactTagMasterTagCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ContactTags)]
        public async Task<PagedResultDto<ContactTagMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ContactTagMasterTagLookupTableDto>();
            foreach (var masterTag in masterTagList)
            {
                lookupTableDtoList.Add(new ContactTagMasterTagLookupTableDto
                {
                    Id = masterTag.Id,
                    DisplayName = masterTag.Name?.ToString()
                });
            }

            return new PagedResultDto<ContactTagMasterTagLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}