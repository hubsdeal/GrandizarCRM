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
    [AbpAuthorize(AppPermissions.Pages_BusinessMasterTagSettings)]
    public class BusinessMasterTagSettingsAppService : SoftGridAppServiceBase, IBusinessMasterTagSettingsAppService
    {
        private readonly IRepository<BusinessMasterTagSetting, long> _businessMasterTagSettingRepository;
        private readonly IBusinessMasterTagSettingsExcelExporter _businessMasterTagSettingsExcelExporter;
        private readonly IRepository<MasterTagCategory, long> _lookup_masterTagCategoryRepository;
        private readonly IRepository<MasterTag, long> _lookup_masterTagRepository;

        public BusinessMasterTagSettingsAppService(IRepository<BusinessMasterTagSetting, long> businessMasterTagSettingRepository, IBusinessMasterTagSettingsExcelExporter businessMasterTagSettingsExcelExporter, IRepository<MasterTagCategory, long> lookup_masterTagCategoryRepository, IRepository<MasterTag, long> lookup_masterTagRepository)
        {
            _businessMasterTagSettingRepository = businessMasterTagSettingRepository;
            _businessMasterTagSettingsExcelExporter = businessMasterTagSettingsExcelExporter;
            _lookup_masterTagCategoryRepository = lookup_masterTagCategoryRepository;
            _lookup_masterTagRepository = lookup_masterTagRepository;

        }

        public async Task<PagedResultDto<GetBusinessMasterTagSettingForViewDto>> GetAll(GetAllBusinessMasterTagSettingsInput input)
        {
            var answerTypeIdFilter = input.AnswerTypeIdFilter.HasValue
                        ? (AnswerType)input.AnswerTypeIdFilter
                        : default;

            var filteredBusinessMasterTagSettings = _businessMasterTagSettingRepository.GetAll()
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.BusinessTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTagTitle.Contains(input.Filter) || e.CustomTagChatQuestion.Contains(input.Filter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(input.DisplayPublicFilter.HasValue && input.DisplayPublicFilter > -1, e => (input.DisplayPublicFilter == 1 && e.DisplayPublic) || (input.DisplayPublicFilter == 0 && !e.DisplayPublic))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagTitleFilter), e => e.CustomTagTitle.Contains(input.CustomTagTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagChatQuestionFilter), e => e.CustomTagChatQuestion.Contains(input.CustomTagChatQuestionFilter))
                        .WhereIf(input.AnswerTypeIdFilter.HasValue && input.AnswerTypeIdFilter > -1, e => e.AnswerTypeId == answerTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.BusinessTypeFk != null && e.BusinessTypeFk.Name == input.MasterTagNameFilter);

            var pagedAndFilteredBusinessMasterTagSettings = filteredBusinessMasterTagSettings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var businessMasterTagSettings = from o in pagedAndFilteredBusinessMasterTagSettings
                                            join o1 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o1.Id into j1
                                            from s1 in j1.DefaultIfEmpty()

                                            join o2 in _lookup_masterTagRepository.GetAll() on o.BusinessTypeId equals o2.Id into j2
                                            from s2 in j2.DefaultIfEmpty()

                                            select new
                                            {

                                                o.DisplaySequence,
                                                o.DisplayPublic,
                                                o.CustomTagTitle,
                                                o.CustomTagChatQuestion,
                                                o.AnswerTypeId,
                                                Id = o.Id,
                                                MasterTagCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                                MasterTagName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                            };

            var totalCount = await filteredBusinessMasterTagSettings.CountAsync();

            var dbList = await businessMasterTagSettings.ToListAsync();
            var results = new List<GetBusinessMasterTagSettingForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBusinessMasterTagSettingForViewDto()
                {
                    BusinessMasterTagSetting = new BusinessMasterTagSettingDto
                    {

                        DisplaySequence = o.DisplaySequence,
                        DisplayPublic = o.DisplayPublic,
                        CustomTagTitle = o.CustomTagTitle,
                        CustomTagChatQuestion = o.CustomTagChatQuestion,
                        AnswerTypeId = o.AnswerTypeId,
                        Id = o.Id,
                    },
                    MasterTagCategoryName = o.MasterTagCategoryName,
                    MasterTagName = o.MasterTagName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetBusinessMasterTagSettingForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetBusinessMasterTagSettingForViewDto> GetBusinessMasterTagSettingForView(long id)
        {
            var businessMasterTagSetting = await _businessMasterTagSettingRepository.GetAsync(id);

            var output = new GetBusinessMasterTagSettingForViewDto { BusinessMasterTagSetting = ObjectMapper.Map<BusinessMasterTagSettingDto>(businessMasterTagSetting) };

            if (output.BusinessMasterTagSetting.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.BusinessMasterTagSetting.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.BusinessMasterTagSetting.BusinessTypeId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.BusinessMasterTagSetting.BusinessTypeId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessMasterTagSettings_Edit)]
        public async Task<GetBusinessMasterTagSettingForEditOutput> GetBusinessMasterTagSettingForEdit(EntityDto<long> input)
        {
            var businessMasterTagSetting = await _businessMasterTagSettingRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBusinessMasterTagSettingForEditOutput { BusinessMasterTagSetting = ObjectMapper.Map<CreateOrEditBusinessMasterTagSettingDto>(businessMasterTagSetting) };

            if (output.BusinessMasterTagSetting.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.BusinessMasterTagSetting.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.BusinessMasterTagSetting.BusinessTypeId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.BusinessMasterTagSetting.BusinessTypeId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBusinessMasterTagSettingDto input)
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

        [AbpAuthorize(AppPermissions.Pages_BusinessMasterTagSettings_Create)]
        protected virtual async Task Create(CreateOrEditBusinessMasterTagSettingDto input)
        {
            var businessMasterTagSetting = ObjectMapper.Map<BusinessMasterTagSetting>(input);

            if (AbpSession.TenantId != null)
            {
                businessMasterTagSetting.TenantId = (int?)AbpSession.TenantId;
            }

            await _businessMasterTagSettingRepository.InsertAsync(businessMasterTagSetting);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessMasterTagSettings_Edit)]
        protected virtual async Task Update(CreateOrEditBusinessMasterTagSettingDto input)
        {
            var businessMasterTagSetting = await _businessMasterTagSettingRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, businessMasterTagSetting);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessMasterTagSettings_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _businessMasterTagSettingRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetBusinessMasterTagSettingsToExcel(GetAllBusinessMasterTagSettingsForExcelInput input)
        {
            var answerTypeIdFilter = input.AnswerTypeIdFilter.HasValue
                        ? (AnswerType)input.AnswerTypeIdFilter
                        : default;

            var filteredBusinessMasterTagSettings = _businessMasterTagSettingRepository.GetAll()
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.BusinessTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTagTitle.Contains(input.Filter) || e.CustomTagChatQuestion.Contains(input.Filter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(input.DisplayPublicFilter.HasValue && input.DisplayPublicFilter > -1, e => (input.DisplayPublicFilter == 1 && e.DisplayPublic) || (input.DisplayPublicFilter == 0 && !e.DisplayPublic))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagTitleFilter), e => e.CustomTagTitle.Contains(input.CustomTagTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagChatQuestionFilter), e => e.CustomTagChatQuestion.Contains(input.CustomTagChatQuestionFilter))
                        .WhereIf(input.AnswerTypeIdFilter.HasValue && input.AnswerTypeIdFilter > -1, e => e.AnswerTypeId == answerTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.BusinessTypeFk != null && e.BusinessTypeFk.Name == input.MasterTagNameFilter);

            var query = (from o in filteredBusinessMasterTagSettings
                         join o1 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_masterTagRepository.GetAll() on o.BusinessTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetBusinessMasterTagSettingForViewDto()
                         {
                             BusinessMasterTagSetting = new BusinessMasterTagSettingDto
                             {
                                 DisplaySequence = o.DisplaySequence,
                                 DisplayPublic = o.DisplayPublic,
                                 CustomTagTitle = o.CustomTagTitle,
                                 CustomTagChatQuestion = o.CustomTagChatQuestion,
                                 AnswerTypeId = o.AnswerTypeId,
                                 Id = o.Id
                             },
                             MasterTagCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MasterTagName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var businessMasterTagSettingListDtos = await query.ToListAsync();

            return _businessMasterTagSettingsExcelExporter.ExportToFile(businessMasterTagSettingListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessMasterTagSettings)]
        public async Task<PagedResultDto<BusinessMasterTagSettingMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BusinessMasterTagSettingMasterTagCategoryLookupTableDto>();
            foreach (var masterTagCategory in masterTagCategoryList)
            {
                lookupTableDtoList.Add(new BusinessMasterTagSettingMasterTagCategoryLookupTableDto
                {
                    Id = masterTagCategory.Id,
                    DisplayName = masterTagCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<BusinessMasterTagSettingMasterTagCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessMasterTagSettings)]
        public async Task<PagedResultDto<BusinessMasterTagSettingMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BusinessMasterTagSettingMasterTagLookupTableDto>();
            foreach (var masterTag in masterTagList)
            {
                lookupTableDtoList.Add(new BusinessMasterTagSettingMasterTagLookupTableDto
                {
                    Id = masterTag.Id,
                    DisplayName = masterTag.Name?.ToString()
                });
            }

            return new PagedResultDto<BusinessMasterTagSettingMasterTagLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}