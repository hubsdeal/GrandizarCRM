using SoftGrid.Shop;
using SoftGrid.LookupData;
using SoftGrid.LookupData;

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
using SoftGrid.LookupData.Dtos;

namespace SoftGrid.Shop
{ 
    [AbpAuthorize(AppPermissions.Pages_StoreTags)]
    public class StoreTagsAppService : SoftGridAppServiceBase, IStoreTagsAppService
    {
        private readonly IRepository<StoreTag, long> _storeTagRepository;
        private readonly IStoreTagsExcelExporter _storeTagsExcelExporter;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<MasterTagCategory, long> _lookup_masterTagCategoryRepository;
        private readonly IRepository<MasterTag, long> _lookup_masterTagRepository;
        private readonly IRepository<StoreMasterTagSetting, long> _storeMasterTagSettingRepository;

        public StoreTagsAppService(IRepository<StoreTag, long> storeTagRepository, IStoreTagsExcelExporter storeTagsExcelExporter, IRepository<Store, long> lookup_storeRepository, IRepository<MasterTagCategory, long> lookup_masterTagCategoryRepository, IRepository<MasterTag, long> lookup_masterTagRepository,
            IRepository<StoreMasterTagSetting, long> storeMasterTagSettingRepository)
        {
            _storeTagRepository = storeTagRepository;
            _storeTagsExcelExporter = storeTagsExcelExporter;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_masterTagCategoryRepository = lookup_masterTagCategoryRepository;
            _lookup_masterTagRepository = lookup_masterTagRepository;
            _storeMasterTagSettingRepository = storeMasterTagSettingRepository;
        }

        public async Task<PagedResultDto<GetStoreTagForViewDto>> GetAll(GetAllStoreTagsInput input)
        {

            var filteredStoreTags = _storeTagRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.MasterTagFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTag.Contains(input.Filter) || e.TagValue.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagFilter), e => e.CustomTag.Contains(input.CustomTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TagValueFilter), e => e.TagValue.Contains(input.TagValueFilter))
                        .WhereIf(input.VerifiedFilter.HasValue && input.VerifiedFilter > -1, e => (input.VerifiedFilter == 1 && e.Verified) || (input.VerifiedFilter == 0 && !e.Verified))
                        .WhereIf(input.MinSequenceFilter != null, e => e.Sequence >= input.MinSequenceFilter)
                        .WhereIf(input.MaxSequenceFilter != null, e => e.Sequence <= input.MaxSequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.MasterTagFk != null && e.MasterTagFk.Name == input.MasterTagNameFilter);

            var pagedAndFilteredStoreTags = filteredStoreTags
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeTags = from o in pagedAndFilteredStoreTags
                            join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
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
                                StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                MasterTagName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                            };

            var totalCount = await filteredStoreTags.CountAsync();

            var dbList = await storeTags.ToListAsync();
            var results = new List<GetStoreTagForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreTagForViewDto()
                {
                    StoreTag = new StoreTagDto
                    {

                        CustomTag = o.CustomTag,
                        TagValue = o.TagValue,
                        Verified = o.Verified,
                        Sequence = o.Sequence,
                        Id = o.Id,
                    },
                    StoreName = o.StoreName,
                    MasterTagCategoryName = o.MasterTagCategoryName,
                    MasterTagName = o.MasterTagName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreTagForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreTagForViewDto> GetStoreTagForView(long id)
        {
            var storeTag = await _storeTagRepository.GetAsync(id);

            var output = new GetStoreTagForViewDto { StoreTag = ObjectMapper.Map<StoreTagDto>(storeTag) };

            if (output.StoreTag.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreTag.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreTag.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.StoreTag.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.StoreTag.MasterTagId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.StoreTag.MasterTagId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreTags_Edit)]
        public async Task<GetStoreTagForEditOutput> GetStoreTagForEdit(EntityDto<long> input)
        {
            var storeTag = await _storeTagRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreTagForEditOutput { StoreTag = ObjectMapper.Map<CreateOrEditStoreTagDto>(storeTag) };

            if (output.StoreTag.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreTag.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreTag.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.StoreTag.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.StoreTag.MasterTagId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.StoreTag.MasterTagId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreTagDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreTags_Create)]
        protected virtual async Task Create(CreateOrEditStoreTagDto input)
        {
            var storeTag = ObjectMapper.Map<StoreTag>(input);

            if (AbpSession.TenantId != null)
            {
                storeTag.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeTagRepository.InsertAsync(storeTag);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreTags_Edit)]
        protected virtual async Task Update(CreateOrEditStoreTagDto input)
        {
            var storeTag = await _storeTagRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeTag);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreTags_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeTagRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreTagsToExcel(GetAllStoreTagsForExcelInput input)
        {

            var filteredStoreTags = _storeTagRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.MasterTagFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTag.Contains(input.Filter) || e.TagValue.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagFilter), e => e.CustomTag.Contains(input.CustomTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TagValueFilter), e => e.TagValue.Contains(input.TagValueFilter))
                        .WhereIf(input.VerifiedFilter.HasValue && input.VerifiedFilter > -1, e => (input.VerifiedFilter == 1 && e.Verified) || (input.VerifiedFilter == 0 && !e.Verified))
                        .WhereIf(input.MinSequenceFilter != null, e => e.Sequence >= input.MinSequenceFilter)
                        .WhereIf(input.MaxSequenceFilter != null, e => e.Sequence <= input.MaxSequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.MasterTagFk != null && e.MasterTagFk.Name == input.MasterTagNameFilter);

            var query = (from o in filteredStoreTags
                         join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_masterTagRepository.GetAll() on o.MasterTagId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetStoreTagForViewDto()
                         {
                             StoreTag = new StoreTagDto
                             {
                                 CustomTag = o.CustomTag,
                                 TagValue = o.TagValue,
                                 Verified = o.Verified,
                                 Sequence = o.Sequence,
                                 Id = o.Id
                             },
                             StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             MasterTagName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var storeTagListDtos = await query.ToListAsync();

            return _storeTagsExcelExporter.ExportToFile(storeTagListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreTags)]
        public async Task<PagedResultDto<StoreTagStoreLookupTableDto>> GetAllStoreForLookupTable(SoftGrid.Shop.Dtos.GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreTagStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreTagStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreTagStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreTags)]
        public async Task<PagedResultDto<StoreTagMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(SoftGrid.Shop.Dtos.GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreTagMasterTagCategoryLookupTableDto>();
            foreach (var masterTagCategory in masterTagCategoryList)
            {
                lookupTableDtoList.Add(new StoreTagMasterTagCategoryLookupTableDto
                {
                    Id = masterTagCategory.Id,
                    DisplayName = masterTagCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreTagMasterTagCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreTags)]
        public async Task<PagedResultDto<StoreTagMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(SoftGrid.Shop.Dtos.GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreTagMasterTagLookupTableDto>();
            foreach (var masterTag in masterTagList)
            {
                lookupTableDtoList.Add(new StoreTagMasterTagLookupTableDto
                {
                    Id = masterTag.Id,
                    DisplayName = masterTag.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreTagMasterTagLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<List<MasterTagCategoryForDashboardViewDto>> GetStoreTagsByTagSetting(long storeTypeId,long storeId)
        {
            var output=new List<MasterTagCategoryForDashboardViewDto>();
            var masterTagCategories=_storeMasterTagSettingRepository.GetAll().Include(e=>e.MasterTagCategoryFk).Where(e=>e.StoreTagSettingCategoryId==storeTypeId && e.MasterTagCategoryId!=null).OrderBy(e=>e.DisplaySequence);
            foreach (var category in masterTagCategories)
            {
                var item = new MasterTagCategoryForDashboardViewDto();
                item.Name = category.MasterTagCategoryFk?.Name;
                item.Id = category.MasterTagCategoryFk.Id;
                item.DisplayPublic=category.DisplayPublic;
                item.DisplaySequence=category.DisplaySequence;
                item.CustomName = category.CustomTagTitle;
                var masterTags = _lookup_masterTagRepository.GetAll().Where(e => e.MasterTagCategoryId == item.Id);
                foreach(var tag in masterTags)
                {
                    item.MasterTags.Add(new MasterTagForDashboardViewDto()
                    {
                        Name = tag.Name,
                        Id = tag.Id,
                        MasterTagCategoryId = tag.MasterTagCategoryId,
                        IsSelected = await _storeTagRepository.FirstOrDefaultAsync(e => e.StoreId == storeId && e.MasterTagId == tag.Id) != null ? true : false
                    });
                }
                output.Add(item);
            }
            return output;
        }

        public async Task DeleteByStoreAndTag(long storeId, long masterTagId)
        {
            await _storeTagRepository.DeleteAsync(e => e.MasterTagId == masterTagId && e.StoreId == storeId);
        }
    }
}