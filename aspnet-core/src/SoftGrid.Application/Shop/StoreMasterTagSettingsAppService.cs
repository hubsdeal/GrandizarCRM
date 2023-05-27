using SoftGrid.Shop;
using SoftGrid.LookupData;

using SoftGrid.Shop.Enums;

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
    [AbpAuthorize(AppPermissions.Pages_StoreMasterTagSettings)]
    public class StoreMasterTagSettingsAppService : SoftGridAppServiceBase, IStoreMasterTagSettingsAppService
    {
        private readonly IRepository<StoreMasterTagSetting, long> _storeMasterTagSettingRepository;
        private readonly IStoreMasterTagSettingsExcelExporter _storeMasterTagSettingsExcelExporter;
        private readonly IRepository<StoreTagSettingCategory, long> _lookup_storeTagSettingCategoryRepository;
        private readonly IRepository<MasterTagCategory, long> _lookup_masterTagCategoryRepository;

        public StoreMasterTagSettingsAppService(IRepository<StoreMasterTagSetting, long> storeMasterTagSettingRepository, IStoreMasterTagSettingsExcelExporter storeMasterTagSettingsExcelExporter, IRepository<StoreTagSettingCategory, long> lookup_storeTagSettingCategoryRepository, IRepository<MasterTagCategory, long> lookup_masterTagCategoryRepository)
        {
            _storeMasterTagSettingRepository = storeMasterTagSettingRepository;
            _storeMasterTagSettingsExcelExporter = storeMasterTagSettingsExcelExporter;
            _lookup_storeTagSettingCategoryRepository = lookup_storeTagSettingCategoryRepository;
            _lookup_masterTagCategoryRepository = lookup_masterTagCategoryRepository;

        }

        public async Task<PagedResultDto<GetStoreMasterTagSettingForViewDto>> GetAll(GetAllStoreMasterTagSettingsInput input)
        {
            var answerTypeIdFilter = input.AnswerTypeIdFilter.HasValue
                        ? (AnswerType)input.AnswerTypeIdFilter
                        : default;

            var filteredStoreMasterTagSettings = _storeMasterTagSettingRepository.GetAll()
                        .Include(e => e.StoreTagSettingCategoryFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTagTitle.Contains(input.Filter) || e.CustomTagChatQuestion.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagTitleFilter), e => e.CustomTagTitle.Contains(input.CustomTagTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagChatQuestionFilter), e => e.CustomTagChatQuestion.Contains(input.CustomTagChatQuestionFilter))
                        .WhereIf(input.DisplayPublicFilter.HasValue && input.DisplayPublicFilter > -1, e => (input.DisplayPublicFilter == 1 && e.DisplayPublic) || (input.DisplayPublicFilter == 0 && !e.DisplayPublic))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(input.AnswerTypeIdFilter.HasValue && input.AnswerTypeIdFilter > -1, e => e.AnswerTypeId == answerTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreTagSettingCategoryNameFilter), e => e.StoreTagSettingCategoryFk != null && e.StoreTagSettingCategoryFk.Name == input.StoreTagSettingCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter);

            var pagedAndFilteredStoreMasterTagSettings = filteredStoreMasterTagSettings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeMasterTagSettings = from o in pagedAndFilteredStoreMasterTagSettings
                                         join o1 in _lookup_storeTagSettingCategoryRepository.GetAll() on o.StoreTagSettingCategoryId equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()

                                         join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                                         from s2 in j2.DefaultIfEmpty()

                                         select new
                                         {

                                             o.CustomTagTitle,
                                             o.CustomTagChatQuestion,
                                             o.DisplayPublic,
                                             o.DisplaySequence,
                                             o.AnswerTypeId,
                                             Id = o.Id,
                                             StoreTagSettingCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                             MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                         };

            var totalCount = await filteredStoreMasterTagSettings.CountAsync();

            var dbList = await storeMasterTagSettings.ToListAsync();
            var results = new List<GetStoreMasterTagSettingForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreMasterTagSettingForViewDto()
                {
                    StoreMasterTagSetting = new StoreMasterTagSettingDto
                    {

                        CustomTagTitle = o.CustomTagTitle,
                        CustomTagChatQuestion = o.CustomTagChatQuestion,
                        DisplayPublic = o.DisplayPublic,
                        DisplaySequence = o.DisplaySequence,
                        AnswerTypeId = o.AnswerTypeId,
                        Id = o.Id,
                    },
                    StoreTagSettingCategoryName = o.StoreTagSettingCategoryName,
                    MasterTagCategoryName = o.MasterTagCategoryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreMasterTagSettingForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreMasterTagSettingForViewDto> GetStoreMasterTagSettingForView(long id)
        {
            var storeMasterTagSetting = await _storeMasterTagSettingRepository.GetAsync(id);

            var output = new GetStoreMasterTagSettingForViewDto { StoreMasterTagSetting = ObjectMapper.Map<StoreMasterTagSettingDto>(storeMasterTagSetting) };

            if (output.StoreMasterTagSetting.StoreTagSettingCategoryId != null)
            {
                var _lookupStoreTagSettingCategory = await _lookup_storeTagSettingCategoryRepository.FirstOrDefaultAsync((long)output.StoreMasterTagSetting.StoreTagSettingCategoryId);
                output.StoreTagSettingCategoryName = _lookupStoreTagSettingCategory?.Name?.ToString();
            }

            if (output.StoreMasterTagSetting.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.StoreMasterTagSetting.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreMasterTagSettings_Edit)]
        public async Task<GetStoreMasterTagSettingForEditOutput> GetStoreMasterTagSettingForEdit(EntityDto<long> input)
        {
            var storeMasterTagSetting = await _storeMasterTagSettingRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreMasterTagSettingForEditOutput { StoreMasterTagSetting = ObjectMapper.Map<CreateOrEditStoreMasterTagSettingDto>(storeMasterTagSetting) };

            if (output.StoreMasterTagSetting.StoreTagSettingCategoryId != null)
            {
                var _lookupStoreTagSettingCategory = await _lookup_storeTagSettingCategoryRepository.FirstOrDefaultAsync((long)output.StoreMasterTagSetting.StoreTagSettingCategoryId);
                output.StoreTagSettingCategoryName = _lookupStoreTagSettingCategory?.Name?.ToString();
            }

            if (output.StoreMasterTagSetting.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.StoreMasterTagSetting.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreMasterTagSettingDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreMasterTagSettings_Create)]
        protected virtual async Task Create(CreateOrEditStoreMasterTagSettingDto input)
        {
            var storeMasterTagSetting = ObjectMapper.Map<StoreMasterTagSetting>(input);

            if (AbpSession.TenantId != null)
            {
                storeMasterTagSetting.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeMasterTagSettingRepository.InsertAsync(storeMasterTagSetting);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreMasterTagSettings_Edit)]
        protected virtual async Task Update(CreateOrEditStoreMasterTagSettingDto input)
        {
            var storeMasterTagSetting = await _storeMasterTagSettingRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeMasterTagSetting);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreMasterTagSettings_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeMasterTagSettingRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreMasterTagSettingsToExcel(GetAllStoreMasterTagSettingsForExcelInput input)
        {
            var answerTypeIdFilter = input.AnswerTypeIdFilter.HasValue
                        ? (AnswerType)input.AnswerTypeIdFilter
                        : default;

            var filteredStoreMasterTagSettings = _storeMasterTagSettingRepository.GetAll()
                        .Include(e => e.StoreTagSettingCategoryFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTagTitle.Contains(input.Filter) || e.CustomTagChatQuestion.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagTitleFilter), e => e.CustomTagTitle.Contains(input.CustomTagTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagChatQuestionFilter), e => e.CustomTagChatQuestion.Contains(input.CustomTagChatQuestionFilter))
                        .WhereIf(input.DisplayPublicFilter.HasValue && input.DisplayPublicFilter > -1, e => (input.DisplayPublicFilter == 1 && e.DisplayPublic) || (input.DisplayPublicFilter == 0 && !e.DisplayPublic))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(input.AnswerTypeIdFilter.HasValue && input.AnswerTypeIdFilter > -1, e => e.AnswerTypeId == answerTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreTagSettingCategoryNameFilter), e => e.StoreTagSettingCategoryFk != null && e.StoreTagSettingCategoryFk.Name == input.StoreTagSettingCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter);

            var query = (from o in filteredStoreMasterTagSettings
                         join o1 in _lookup_storeTagSettingCategoryRepository.GetAll() on o.StoreTagSettingCategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetStoreMasterTagSettingForViewDto()
                         {
                             StoreMasterTagSetting = new StoreMasterTagSettingDto
                             {
                                 CustomTagTitle = o.CustomTagTitle,
                                 CustomTagChatQuestion = o.CustomTagChatQuestion,
                                 DisplayPublic = o.DisplayPublic,
                                 DisplaySequence = o.DisplaySequence,
                                 AnswerTypeId = o.AnswerTypeId,
                                 Id = o.Id
                             },
                             StoreTagSettingCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var storeMasterTagSettingListDtos = await query.ToListAsync();

            return _storeMasterTagSettingsExcelExporter.ExportToFile(storeMasterTagSettingListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreMasterTagSettings)]
        public async Task<PagedResultDto<StoreMasterTagSettingStoreTagSettingCategoryLookupTableDto>> GetAllStoreTagSettingCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeTagSettingCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeTagSettingCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreMasterTagSettingStoreTagSettingCategoryLookupTableDto>();
            foreach (var storeTagSettingCategory in storeTagSettingCategoryList)
            {
                lookupTableDtoList.Add(new StoreMasterTagSettingStoreTagSettingCategoryLookupTableDto
                {
                    Id = storeTagSettingCategory.Id,
                    DisplayName = storeTagSettingCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreMasterTagSettingStoreTagSettingCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreMasterTagSettings)]
        public async Task<PagedResultDto<StoreMasterTagSettingMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreMasterTagSettingMasterTagCategoryLookupTableDto>();
            foreach (var masterTagCategory in masterTagCategoryList)
            {
                lookupTableDtoList.Add(new StoreMasterTagSettingMasterTagCategoryLookupTableDto
                {
                    Id = masterTagCategory.Id,
                    DisplayName = masterTagCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreMasterTagSettingMasterTagCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}