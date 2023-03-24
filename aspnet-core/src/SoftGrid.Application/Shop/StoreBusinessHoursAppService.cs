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

namespace SoftGrid.Shop
{
    [AbpAuthorize(AppPermissions.Pages_StoreBusinessHours)]
    public class StoreBusinessHoursAppService : SoftGridAppServiceBase, IStoreBusinessHoursAppService
    {
        private readonly IRepository<StoreBusinessHour, long> _storeBusinessHourRepository;
        private readonly IStoreBusinessHoursExcelExporter _storeBusinessHoursExcelExporter;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<MasterTagCategory, long> _lookup_masterTagCategoryRepository;
        private readonly IRepository<MasterTag, long> _lookup_masterTagRepository;

        public StoreBusinessHoursAppService(IRepository<StoreBusinessHour, long> storeBusinessHourRepository, IStoreBusinessHoursExcelExporter storeBusinessHoursExcelExporter, IRepository<Store, long> lookup_storeRepository, IRepository<MasterTagCategory, long> lookup_masterTagCategoryRepository, IRepository<MasterTag, long> lookup_masterTagRepository)
        {
            _storeBusinessHourRepository = storeBusinessHourRepository;
            _storeBusinessHoursExcelExporter = storeBusinessHoursExcelExporter;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_masterTagCategoryRepository = lookup_masterTagCategoryRepository;
            _lookup_masterTagRepository = lookup_masterTagRepository;

        }

        public async Task<PagedResultDto<GetStoreBusinessHourForViewDto>> GetAll(GetAllStoreBusinessHoursInput input)
        {

            var filteredStoreBusinessHours = _storeBusinessHourRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.MasterTagFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.MondayStartTime.Contains(input.Filter) || e.MondayEndTime.Contains(input.Filter) || e.TuesdayStartTime.Contains(input.Filter) || e.TuesdayEndTime.Contains(input.Filter) || e.WednesdayStartTime.Contains(input.Filter) || e.WednesdayEndTime.Contains(input.Filter) || e.ThursdayStartTime.Contains(input.Filter) || e.ThursdayEndTime.Contains(input.Filter) || e.FridayStartTime.Contains(input.Filter) || e.FridayEndTime.Contains(input.Filter) || e.SaturdayStartTime.Contains(input.Filter) || e.SaturdayEndTime.Contains(input.Filter) || e.SundayStartTime.Contains(input.Filter) || e.SundayEndTime.Contains(input.Filter))
                        .WhereIf(input.NowOpenOrClosedFilter.HasValue && input.NowOpenOrClosedFilter > -1, e => (input.NowOpenOrClosedFilter == 1 && e.NowOpenOrClosed) || (input.NowOpenOrClosedFilter == 0 && !e.NowOpenOrClosed))
                        .WhereIf(input.IsOpen24HoursFilter.HasValue && input.IsOpen24HoursFilter > -1, e => (input.IsOpen24HoursFilter == 1 && e.IsOpen24Hours) || (input.IsOpen24HoursFilter == 0 && !e.IsOpen24Hours))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MondayStartTimeFilter), e => e.MondayStartTime.Contains(input.MondayStartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MondayEndTimeFilter), e => e.MondayEndTime.Contains(input.MondayEndTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TuesdayStartTimeFilter), e => e.TuesdayStartTime.Contains(input.TuesdayStartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TuesdayEndTimeFilter), e => e.TuesdayEndTime.Contains(input.TuesdayEndTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WednesdayStartTimeFilter), e => e.WednesdayStartTime.Contains(input.WednesdayStartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WednesdayEndTimeFilter), e => e.WednesdayEndTime.Contains(input.WednesdayEndTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ThursdayStartTimeFilter), e => e.ThursdayStartTime.Contains(input.ThursdayStartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ThursdayEndTimeFilter), e => e.ThursdayEndTime.Contains(input.ThursdayEndTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FridayStartTimeFilter), e => e.FridayStartTime.Contains(input.FridayStartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FridayEndTimeFilter), e => e.FridayEndTime.Contains(input.FridayEndTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SaturdayStartTimeFilter), e => e.SaturdayStartTime.Contains(input.SaturdayStartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SaturdayEndTimeFilter), e => e.SaturdayEndTime.Contains(input.SaturdayEndTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SundayStartTimeFilter), e => e.SundayStartTime.Contains(input.SundayStartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SundayEndTimeFilter), e => e.SundayEndTime.Contains(input.SundayEndTimeFilter))
                        .WhereIf(input.IsAcceptOnlyBusinessHourFilter.HasValue && input.IsAcceptOnlyBusinessHourFilter > -1, e => (input.IsAcceptOnlyBusinessHourFilter == 1 && e.IsAcceptOnlyBusinessHour) || (input.IsAcceptOnlyBusinessHourFilter == 0 && !e.IsAcceptOnlyBusinessHour))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.MasterTagFk != null && e.MasterTagFk.Name == input.MasterTagNameFilter);

            var pagedAndFilteredStoreBusinessHours = filteredStoreBusinessHours
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeBusinessHours = from o in pagedAndFilteredStoreBusinessHours
                                     join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                                     from s1 in j1.DefaultIfEmpty()

                                     join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                                     from s2 in j2.DefaultIfEmpty()

                                     join o3 in _lookup_masterTagRepository.GetAll() on o.MasterTagId equals o3.Id into j3
                                     from s3 in j3.DefaultIfEmpty()

                                     select new
                                     {

                                         o.NowOpenOrClosed,
                                         o.IsOpen24Hours,
                                         o.MondayStartTime,
                                         o.MondayEndTime,
                                         o.TuesdayStartTime,
                                         o.TuesdayEndTime,
                                         o.WednesdayStartTime,
                                         o.WednesdayEndTime,
                                         o.ThursdayStartTime,
                                         o.ThursdayEndTime,
                                         o.FridayStartTime,
                                         o.FridayEndTime,
                                         o.SaturdayStartTime,
                                         o.SaturdayEndTime,
                                         o.SundayStartTime,
                                         o.SundayEndTime,
                                         o.IsAcceptOnlyBusinessHour,
                                         Id = o.Id,
                                         StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                         MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                         MasterTagName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                                     };

            var totalCount = await filteredStoreBusinessHours.CountAsync();

            var dbList = await storeBusinessHours.ToListAsync();
            var results = new List<GetStoreBusinessHourForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreBusinessHourForViewDto()
                {
                    StoreBusinessHour = new StoreBusinessHourDto
                    {

                        NowOpenOrClosed = o.NowOpenOrClosed,
                        IsOpen24Hours = o.IsOpen24Hours,
                        MondayStartTime = o.MondayStartTime,
                        MondayEndTime = o.MondayEndTime,
                        TuesdayStartTime = o.TuesdayStartTime,
                        TuesdayEndTime = o.TuesdayEndTime,
                        WednesdayStartTime = o.WednesdayStartTime,
                        WednesdayEndTime = o.WednesdayEndTime,
                        ThursdayStartTime = o.ThursdayStartTime,
                        ThursdayEndTime = o.ThursdayEndTime,
                        FridayStartTime = o.FridayStartTime,
                        FridayEndTime = o.FridayEndTime,
                        SaturdayStartTime = o.SaturdayStartTime,
                        SaturdayEndTime = o.SaturdayEndTime,
                        SundayStartTime = o.SundayStartTime,
                        SundayEndTime = o.SundayEndTime,
                        IsAcceptOnlyBusinessHour = o.IsAcceptOnlyBusinessHour,
                        Id = o.Id,
                    },
                    StoreName = o.StoreName,
                    MasterTagCategoryName = o.MasterTagCategoryName,
                    MasterTagName = o.MasterTagName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreBusinessHourForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreBusinessHourForViewDto> GetStoreBusinessHourForView(long id)
        {
            var storeBusinessHour = await _storeBusinessHourRepository.GetAsync(id);

            var output = new GetStoreBusinessHourForViewDto { StoreBusinessHour = ObjectMapper.Map<StoreBusinessHourDto>(storeBusinessHour) };

            if (output.StoreBusinessHour.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreBusinessHour.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreBusinessHour.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.StoreBusinessHour.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.StoreBusinessHour.MasterTagId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.StoreBusinessHour.MasterTagId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreBusinessHours_Edit)]
        public async Task<GetStoreBusinessHourForEditOutput> GetStoreBusinessHourForEdit(EntityDto<long> input)
        {
            var storeBusinessHour = await _storeBusinessHourRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreBusinessHourForEditOutput { StoreBusinessHour = ObjectMapper.Map<CreateOrEditStoreBusinessHourDto>(storeBusinessHour) };

            if (output.StoreBusinessHour.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreBusinessHour.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreBusinessHour.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.StoreBusinessHour.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.StoreBusinessHour.MasterTagId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.StoreBusinessHour.MasterTagId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreBusinessHourDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreBusinessHours_Create)]
        protected virtual async Task Create(CreateOrEditStoreBusinessHourDto input)
        {
            var storeBusinessHour = ObjectMapper.Map<StoreBusinessHour>(input);

            if (AbpSession.TenantId != null)
            {
                storeBusinessHour.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeBusinessHourRepository.InsertAsync(storeBusinessHour);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreBusinessHours_Edit)]
        protected virtual async Task Update(CreateOrEditStoreBusinessHourDto input)
        {
            var storeBusinessHour = await _storeBusinessHourRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeBusinessHour);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreBusinessHours_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeBusinessHourRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreBusinessHoursToExcel(GetAllStoreBusinessHoursForExcelInput input)
        {

            var filteredStoreBusinessHours = _storeBusinessHourRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.MasterTagFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.MondayStartTime.Contains(input.Filter) || e.MondayEndTime.Contains(input.Filter) || e.TuesdayStartTime.Contains(input.Filter) || e.TuesdayEndTime.Contains(input.Filter) || e.WednesdayStartTime.Contains(input.Filter) || e.WednesdayEndTime.Contains(input.Filter) || e.ThursdayStartTime.Contains(input.Filter) || e.ThursdayEndTime.Contains(input.Filter) || e.FridayStartTime.Contains(input.Filter) || e.FridayEndTime.Contains(input.Filter) || e.SaturdayStartTime.Contains(input.Filter) || e.SaturdayEndTime.Contains(input.Filter) || e.SundayStartTime.Contains(input.Filter) || e.SundayEndTime.Contains(input.Filter))
                        .WhereIf(input.NowOpenOrClosedFilter.HasValue && input.NowOpenOrClosedFilter > -1, e => (input.NowOpenOrClosedFilter == 1 && e.NowOpenOrClosed) || (input.NowOpenOrClosedFilter == 0 && !e.NowOpenOrClosed))
                        .WhereIf(input.IsOpen24HoursFilter.HasValue && input.IsOpen24HoursFilter > -1, e => (input.IsOpen24HoursFilter == 1 && e.IsOpen24Hours) || (input.IsOpen24HoursFilter == 0 && !e.IsOpen24Hours))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MondayStartTimeFilter), e => e.MondayStartTime.Contains(input.MondayStartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MondayEndTimeFilter), e => e.MondayEndTime.Contains(input.MondayEndTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TuesdayStartTimeFilter), e => e.TuesdayStartTime.Contains(input.TuesdayStartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TuesdayEndTimeFilter), e => e.TuesdayEndTime.Contains(input.TuesdayEndTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WednesdayStartTimeFilter), e => e.WednesdayStartTime.Contains(input.WednesdayStartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WednesdayEndTimeFilter), e => e.WednesdayEndTime.Contains(input.WednesdayEndTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ThursdayStartTimeFilter), e => e.ThursdayStartTime.Contains(input.ThursdayStartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ThursdayEndTimeFilter), e => e.ThursdayEndTime.Contains(input.ThursdayEndTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FridayStartTimeFilter), e => e.FridayStartTime.Contains(input.FridayStartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FridayEndTimeFilter), e => e.FridayEndTime.Contains(input.FridayEndTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SaturdayStartTimeFilter), e => e.SaturdayStartTime.Contains(input.SaturdayStartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SaturdayEndTimeFilter), e => e.SaturdayEndTime.Contains(input.SaturdayEndTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SundayStartTimeFilter), e => e.SundayStartTime.Contains(input.SundayStartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SundayEndTimeFilter), e => e.SundayEndTime.Contains(input.SundayEndTimeFilter))
                        .WhereIf(input.IsAcceptOnlyBusinessHourFilter.HasValue && input.IsAcceptOnlyBusinessHourFilter > -1, e => (input.IsAcceptOnlyBusinessHourFilter == 1 && e.IsAcceptOnlyBusinessHour) || (input.IsAcceptOnlyBusinessHourFilter == 0 && !e.IsAcceptOnlyBusinessHour))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.MasterTagFk != null && e.MasterTagFk.Name == input.MasterTagNameFilter);

            var query = (from o in filteredStoreBusinessHours
                         join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_masterTagRepository.GetAll() on o.MasterTagId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetStoreBusinessHourForViewDto()
                         {
                             StoreBusinessHour = new StoreBusinessHourDto
                             {
                                 NowOpenOrClosed = o.NowOpenOrClosed,
                                 IsOpen24Hours = o.IsOpen24Hours,
                                 MondayStartTime = o.MondayStartTime,
                                 MondayEndTime = o.MondayEndTime,
                                 TuesdayStartTime = o.TuesdayStartTime,
                                 TuesdayEndTime = o.TuesdayEndTime,
                                 WednesdayStartTime = o.WednesdayStartTime,
                                 WednesdayEndTime = o.WednesdayEndTime,
                                 ThursdayStartTime = o.ThursdayStartTime,
                                 ThursdayEndTime = o.ThursdayEndTime,
                                 FridayStartTime = o.FridayStartTime,
                                 FridayEndTime = o.FridayEndTime,
                                 SaturdayStartTime = o.SaturdayStartTime,
                                 SaturdayEndTime = o.SaturdayEndTime,
                                 SundayStartTime = o.SundayStartTime,
                                 SundayEndTime = o.SundayEndTime,
                                 IsAcceptOnlyBusinessHour = o.IsAcceptOnlyBusinessHour,
                                 Id = o.Id
                             },
                             StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             MasterTagName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var storeBusinessHourListDtos = await query.ToListAsync();

            return _storeBusinessHoursExcelExporter.ExportToFile(storeBusinessHourListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreBusinessHours)]
        public async Task<PagedResultDto<StoreBusinessHourStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreBusinessHourStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreBusinessHourStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreBusinessHourStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreBusinessHours)]
        public async Task<PagedResultDto<StoreBusinessHourMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreBusinessHourMasterTagCategoryLookupTableDto>();
            foreach (var masterTagCategory in masterTagCategoryList)
            {
                lookupTableDtoList.Add(new StoreBusinessHourMasterTagCategoryLookupTableDto
                {
                    Id = masterTagCategory.Id,
                    DisplayName = masterTagCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreBusinessHourMasterTagCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreBusinessHours)]
        public async Task<PagedResultDto<StoreBusinessHourMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreBusinessHourMasterTagLookupTableDto>();
            foreach (var masterTag in masterTagList)
            {
                lookupTableDtoList.Add(new StoreBusinessHourMasterTagLookupTableDto
                {
                    Id = masterTag.Id,
                    DisplayName = masterTag.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreBusinessHourMasterTagLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}