using SoftGrid.LookupData;
using SoftGrid.LookupData;

using SoftGrid.Shop.Enums;

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
    [AbpAuthorize(AppPermissions.Pages_ContactMasterTagSettings)]
    public class ContactMasterTagSettingsAppService : SoftGridAppServiceBase, IContactMasterTagSettingsAppService
    {
        private readonly IRepository<ContactMasterTagSetting, long> _contactMasterTagSettingRepository;
        private readonly IContactMasterTagSettingsExcelExporter _contactMasterTagSettingsExcelExporter;
        private readonly IRepository<MasterTag, long> _lookup_masterTagRepository;
        private readonly IRepository<MasterTagCategory, long> _lookup_masterTagCategoryRepository;

        public ContactMasterTagSettingsAppService(IRepository<ContactMasterTagSetting, long> contactMasterTagSettingRepository, IContactMasterTagSettingsExcelExporter contactMasterTagSettingsExcelExporter, IRepository<MasterTag, long> lookup_masterTagRepository, IRepository<MasterTagCategory, long> lookup_masterTagCategoryRepository)
        {
            _contactMasterTagSettingRepository = contactMasterTagSettingRepository;
            _contactMasterTagSettingsExcelExporter = contactMasterTagSettingsExcelExporter;
            _lookup_masterTagRepository = lookup_masterTagRepository;
            _lookup_masterTagCategoryRepository = lookup_masterTagCategoryRepository;

        }

        public async Task<PagedResultDto<GetContactMasterTagSettingForViewDto>> GetAll(GetAllContactMasterTagSettingsInput input)
        {
            var answerTypeIdFilter = input.AnswerTypeIdFilter.HasValue
                        ? (AnswerType)input.AnswerTypeIdFilter
                        : default;

            var filteredContactMasterTagSettings = _contactMasterTagSettingRepository.GetAll()
                        .Include(e => e.ContactTypeFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTagTitle.Contains(input.Filter) || e.CustomTagChatQuestion.Contains(input.Filter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(input.DisplayPublicFilter.HasValue && input.DisplayPublicFilter > -1, e => (input.DisplayPublicFilter == 1 && e.DisplayPublic) || (input.DisplayPublicFilter == 0 && !e.DisplayPublic))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagTitleFilter), e => e.CustomTagTitle.Contains(input.CustomTagTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagChatQuestionFilter), e => e.CustomTagChatQuestion.Contains(input.CustomTagChatQuestionFilter))
                        .WhereIf(input.AnswerTypeIdFilter.HasValue && input.AnswerTypeIdFilter > -1, e => e.AnswerTypeId == answerTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.ContactTypeFk != null && e.ContactTypeFk.Name == input.MasterTagNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter);

            var pagedAndFilteredContactMasterTagSettings = filteredContactMasterTagSettings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var contactMasterTagSettings = from o in pagedAndFilteredContactMasterTagSettings
                                           join o1 in _lookup_masterTagRepository.GetAll() on o.ContactTypeId equals o1.Id into j1
                                           from s1 in j1.DefaultIfEmpty()

                                           join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                                           from s2 in j2.DefaultIfEmpty()

                                           select new
                                           {

                                               o.DisplaySequence,
                                               o.DisplayPublic,
                                               o.CustomTagTitle,
                                               o.CustomTagChatQuestion,
                                               o.AnswerTypeId,
                                               Id = o.Id,
                                               MasterTagName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                               MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                           };

            var totalCount = await filteredContactMasterTagSettings.CountAsync();

            var dbList = await contactMasterTagSettings.ToListAsync();
            var results = new List<GetContactMasterTagSettingForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetContactMasterTagSettingForViewDto()
                {
                    ContactMasterTagSetting = new ContactMasterTagSettingDto
                    {

                        DisplaySequence = o.DisplaySequence,
                        DisplayPublic = o.DisplayPublic,
                        CustomTagTitle = o.CustomTagTitle,
                        CustomTagChatQuestion = o.CustomTagChatQuestion,
                        AnswerTypeId = o.AnswerTypeId,
                        Id = o.Id,
                    },
                    MasterTagName = o.MasterTagName,
                    MasterTagCategoryName = o.MasterTagCategoryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetContactMasterTagSettingForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetContactMasterTagSettingForViewDto> GetContactMasterTagSettingForView(long id)
        {
            var contactMasterTagSetting = await _contactMasterTagSettingRepository.GetAsync(id);

            var output = new GetContactMasterTagSettingForViewDto { ContactMasterTagSetting = ObjectMapper.Map<ContactMasterTagSettingDto>(contactMasterTagSetting) };

            if (output.ContactMasterTagSetting.ContactTypeId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.ContactMasterTagSetting.ContactTypeId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            if (output.ContactMasterTagSetting.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.ContactMasterTagSetting.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ContactMasterTagSettings_Edit)]
        public async Task<GetContactMasterTagSettingForEditOutput> GetContactMasterTagSettingForEdit(EntityDto<long> input)
        {
            var contactMasterTagSetting = await _contactMasterTagSettingRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetContactMasterTagSettingForEditOutput { ContactMasterTagSetting = ObjectMapper.Map<CreateOrEditContactMasterTagSettingDto>(contactMasterTagSetting) };

            if (output.ContactMasterTagSetting.ContactTypeId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.ContactMasterTagSetting.ContactTypeId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            if (output.ContactMasterTagSetting.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.ContactMasterTagSetting.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditContactMasterTagSettingDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ContactMasterTagSettings_Create)]
        protected virtual async Task Create(CreateOrEditContactMasterTagSettingDto input)
        {
            var contactMasterTagSetting = ObjectMapper.Map<ContactMasterTagSetting>(input);

            if (AbpSession.TenantId != null)
            {
                contactMasterTagSetting.TenantId = (int?)AbpSession.TenantId;
            }

            await _contactMasterTagSettingRepository.InsertAsync(contactMasterTagSetting);

        }

        [AbpAuthorize(AppPermissions.Pages_ContactMasterTagSettings_Edit)]
        protected virtual async Task Update(CreateOrEditContactMasterTagSettingDto input)
        {
            var contactMasterTagSetting = await _contactMasterTagSettingRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, contactMasterTagSetting);

        }

        [AbpAuthorize(AppPermissions.Pages_ContactMasterTagSettings_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _contactMasterTagSettingRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetContactMasterTagSettingsToExcel(GetAllContactMasterTagSettingsForExcelInput input)
        {
            var answerTypeIdFilter = input.AnswerTypeIdFilter.HasValue
                        ? (AnswerType)input.AnswerTypeIdFilter
                        : default;

            var filteredContactMasterTagSettings = _contactMasterTagSettingRepository.GetAll()
                        .Include(e => e.ContactTypeFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTagTitle.Contains(input.Filter) || e.CustomTagChatQuestion.Contains(input.Filter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(input.DisplayPublicFilter.HasValue && input.DisplayPublicFilter > -1, e => (input.DisplayPublicFilter == 1 && e.DisplayPublic) || (input.DisplayPublicFilter == 0 && !e.DisplayPublic))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagTitleFilter), e => e.CustomTagTitle.Contains(input.CustomTagTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagChatQuestionFilter), e => e.CustomTagChatQuestion.Contains(input.CustomTagChatQuestionFilter))
                        .WhereIf(input.AnswerTypeIdFilter.HasValue && input.AnswerTypeIdFilter > -1, e => e.AnswerTypeId == answerTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.ContactTypeFk != null && e.ContactTypeFk.Name == input.MasterTagNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter);

            var query = (from o in filteredContactMasterTagSettings
                         join o1 in _lookup_masterTagRepository.GetAll() on o.ContactTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetContactMasterTagSettingForViewDto()
                         {
                             ContactMasterTagSetting = new ContactMasterTagSettingDto
                             {
                                 DisplaySequence = o.DisplaySequence,
                                 DisplayPublic = o.DisplayPublic,
                                 CustomTagTitle = o.CustomTagTitle,
                                 CustomTagChatQuestion = o.CustomTagChatQuestion,
                                 AnswerTypeId = o.AnswerTypeId,
                                 Id = o.Id
                             },
                             MasterTagName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var contactMasterTagSettingListDtos = await query.ToListAsync();

            return _contactMasterTagSettingsExcelExporter.ExportToFile(contactMasterTagSettingListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ContactMasterTagSettings)]
        public async Task<PagedResultDto<ContactMasterTagSettingMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ContactMasterTagSettingMasterTagLookupTableDto>();
            foreach (var masterTag in masterTagList)
            {
                lookupTableDtoList.Add(new ContactMasterTagSettingMasterTagLookupTableDto
                {
                    Id = masterTag.Id,
                    DisplayName = masterTag.Name?.ToString()
                });
            }

            return new PagedResultDto<ContactMasterTagSettingMasterTagLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ContactMasterTagSettings)]
        public async Task<PagedResultDto<ContactMasterTagSettingMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ContactMasterTagSettingMasterTagCategoryLookupTableDto>();
            foreach (var masterTagCategory in masterTagCategoryList)
            {
                lookupTableDtoList.Add(new ContactMasterTagSettingMasterTagCategoryLookupTableDto
                {
                    Id = masterTagCategory.Id,
                    DisplayName = masterTagCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<ContactMasterTagSettingMasterTagCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}