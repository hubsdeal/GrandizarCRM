using SoftGrid.Shop;

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
    // [AbpAuthorize(AppPermissions.Pages_StoreReservationSettings)]
    public class StoreReservationSettingsAppService : SoftGridAppServiceBase
        //, IStoreReservationSettingsAppService
    {
        //private readonly IRepository<StoreReservationSetting, long> _storeReservationSettingRepository;
        //private readonly IStoreReservationSettingsExcelExporter _storeReservationSettingsExcelExporter;
        //private readonly IRepository<Store, long> _lookup_storeRepository;

        //public StoreReservationSettingsAppService(IRepository<StoreReservationSetting, long> storeReservationSettingRepository, IStoreReservationSettingsExcelExporter storeReservationSettingsExcelExporter, IRepository<Store, long> lookup_storeRepository)
        //{
        //    _storeReservationSettingRepository = storeReservationSettingRepository;
        //    _storeReservationSettingsExcelExporter = storeReservationSettingsExcelExporter;
        //    _lookup_storeRepository = lookup_storeRepository;

        //}

        //public async Task<PagedResultDto<GetStoreReservationSettingForViewDto>> GetAll(GetAllStoreReservationSettingsInput input)
        //{

        //    var filteredStoreReservationSettings = _storeReservationSettingRepository.GetAll()
        //                .Include(e => e.StoreFk)
        //                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
        //                .WhereIf(input.OfferReservationFilter.HasValue && input.OfferReservationFilter > -1, e => (input.OfferReservationFilter == 1 && e.OfferReservation) || (input.OfferReservationFilter == 0 && !e.OfferReservation))
        //                .WhereIf(input.InstantConfirmationFilter.HasValue && input.InstantConfirmationFilter > -1, e => (input.InstantConfirmationFilter == 1 && e.InstantConfirmation) || (input.InstantConfirmationFilter == 0 && !e.InstantConfirmation))
        //                .WhereIf(input.MessageStoreTeamFilter.HasValue && input.MessageStoreTeamFilter > -1, e => (input.MessageStoreTeamFilter == 1 && e.MessageStoreTeam) || (input.MessageStoreTeamFilter == 0 && !e.MessageStoreTeam))
        //                .WhereIf(input.MinMinNumberOfGuestsFilter != null, e => e.MinNumberOfGuests >= input.MinMinNumberOfGuestsFilter)
        //                .WhereIf(input.MaxMinNumberOfGuestsFilter != null, e => e.MinNumberOfGuests <= input.MaxMinNumberOfGuestsFilter)
        //                .WhereIf(input.MinMaxNumberOfGuestsFilter != null, e => e.MaxNumberOfGuests >= input.MinMaxNumberOfGuestsFilter)
        //                .WhereIf(input.MaxMaxNumberOfGuestsFilter != null, e => e.MaxNumberOfGuests <= input.MaxMaxNumberOfGuestsFilter)
        //                .WhereIf(input.PublishAvailabilityFilter.HasValue && input.PublishAvailabilityFilter > -1, e => (input.PublishAvailabilityFilter == 1 && e.PublishAvailability) || (input.PublishAvailabilityFilter == 0 && !e.PublishAvailability))
        //                .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

        //    var pagedAndFilteredStoreReservationSettings = filteredStoreReservationSettings
        //        .OrderBy(input.Sorting ?? "id asc")
        //        .PageBy(input);

        //    var storeReservationSettings = from o in pagedAndFilteredStoreReservationSettings
        //                                   join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
        //                                   from s1 in j1.DefaultIfEmpty()

        //                                   select new
        //                                   {

        //                                       o.OfferReservation,
        //                                       o.InstantConfirmation,
        //                                       o.MessageStoreTeam,
        //                                       o.MinNumberOfGuests,
        //                                       o.MaxNumberOfGuests,
        //                                       o.PublishAvailability,
        //                                       Id = o.Id,
        //                                       StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
        //                                   };

        //    var totalCount = await filteredStoreReservationSettings.CountAsync();

        //    var dbList = await storeReservationSettings.ToListAsync();
        //    var results = new List<GetStoreReservationSettingForViewDto>();

        //    foreach (var o in dbList)
        //    {
        //        var res = new GetStoreReservationSettingForViewDto()
        //        {
        //            StoreReservationSetting = new StoreReservationSettingDto
        //            {

        //                OfferReservation = o.OfferReservation,
        //                InstantConfirmation = o.InstantConfirmation,
        //                MessageStoreTeam = o.MessageStoreTeam,
        //                MinNumberOfGuests = o.MinNumberOfGuests,
        //                MaxNumberOfGuests = o.MaxNumberOfGuests,
        //                PublishAvailability = o.PublishAvailability,
        //                Id = o.Id,
        //            },
        //            StoreName = o.StoreName
        //        };

        //        results.Add(res);
        //    }

        //    return new PagedResultDto<GetStoreReservationSettingForViewDto>(
        //        totalCount,
        //        results
        //    );

        //}

        //public async Task<GetStoreReservationSettingForViewDto> GetStoreReservationSettingForView(long id)
        //{
        //    var storeReservationSetting = await _storeReservationSettingRepository.GetAsync(id);

        //    var output = new GetStoreReservationSettingForViewDto { StoreReservationSetting = ObjectMapper.Map<StoreReservationSettingDto>(storeReservationSetting) };

        //    if (output.StoreReservationSetting.StoreId != null)
        //    {
        //        var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreReservationSetting.StoreId);
        //        output.StoreName = _lookupStore?.Name?.ToString();
        //    }

        //    return output;
        //}

        //[AbpAuthorize(AppPermissions.Pages_StoreReservationSettings_Edit)]
        //public async Task<GetStoreReservationSettingForEditOutput> GetStoreReservationSettingForEdit(EntityDto<long> input)
        //{
        //    var storeReservationSetting = await _storeReservationSettingRepository.FirstOrDefaultAsync(input.Id);

        //    var output = new GetStoreReservationSettingForEditOutput { StoreReservationSetting = ObjectMapper.Map<CreateOrEditStoreReservationSettingDto>(storeReservationSetting) };

        //    if (output.StoreReservationSetting.StoreId != null)
        //    {
        //        var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreReservationSetting.StoreId);
        //        output.StoreName = _lookupStore?.Name?.ToString();
        //    }

        //    return output;
        //}

        //// [AbpAuthorize(AppPermissions.Pages_StoreReservationSettings_Edit)]
        //[AbpAllowAnonymous]
        //public async Task<GetStoreReservationSettingForEditOutput> GetStoreReservationSettingForEditByStore(EntityDto<long> input)
        //{
        //    var storeReservationSetting = await _storeReservationSettingRepository.FirstOrDefaultAsync(e => e.StoreId == input.Id);
        //    if (storeReservationSetting == null)
        //    {
        //        return null;
        //    }
        //    var output = new GetStoreReservationSettingForEditOutput { StoreReservationSetting = ObjectMapper.Map<CreateOrEditStoreReservationSettingDto>(storeReservationSetting) };

        //    if (output.StoreReservationSetting.StoreId != null)
        //    {
        //        var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreReservationSetting.StoreId);
        //        output.StoreName = _lookupStore?.Name?.ToString();
        //    }

        //    return output;
        //}

        //public async Task CreateOrEdit(CreateOrEditStoreReservationSettingDto input)
        //{
        //    if (input.Id == null)
        //    {
        //        await Create(input);
        //    }
        //    else
        //    {
        //        await Update(input);
        //    }
        //}

        ////[AbpAuthorize(AppPermissions.Pages_StoreReservationSettings_Create)]
        //[AbpAllowAnonymous]
        //protected virtual async Task Create(CreateOrEditStoreReservationSettingDto input)
        //{
        //    var storeReservationSetting = ObjectMapper.Map<StoreReservationSetting>(input);

        //    if (AbpSession.TenantId != null)
        //    {
        //        storeReservationSetting.TenantId = (int?)AbpSession.TenantId;
        //    }

        //    await _storeReservationSettingRepository.InsertAsync(storeReservationSetting);

        //}

        //[AbpAuthorize(AppPermissions.Pages_StoreReservationSettings_Edit)]
        //protected virtual async Task Update(CreateOrEditStoreReservationSettingDto input)
        //{
        //    var storeReservationSetting = await _storeReservationSettingRepository.FirstOrDefaultAsync((long)input.Id);
        //    ObjectMapper.Map(input, storeReservationSetting);

        //}

        //[AbpAuthorize(AppPermissions.Pages_StoreReservationSettings_Delete)]
        //public async Task Delete(EntityDto<long> input)
        //{
        //    await _storeReservationSettingRepository.DeleteAsync(input.Id);
        //}

        //public async Task<FileDto> GetStoreReservationSettingsToExcel(GetAllStoreReservationSettingsForExcelInput input)
        //{

        //    var filteredStoreReservationSettings = _storeReservationSettingRepository.GetAll()
        //                .Include(e => e.StoreFk)
        //                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
        //                .WhereIf(input.OfferReservationFilter.HasValue && input.OfferReservationFilter > -1, e => (input.OfferReservationFilter == 1 && e.OfferReservation) || (input.OfferReservationFilter == 0 && !e.OfferReservation))
        //                .WhereIf(input.InstantConfirmationFilter.HasValue && input.InstantConfirmationFilter > -1, e => (input.InstantConfirmationFilter == 1 && e.InstantConfirmation) || (input.InstantConfirmationFilter == 0 && !e.InstantConfirmation))
        //                .WhereIf(input.MessageStoreTeamFilter.HasValue && input.MessageStoreTeamFilter > -1, e => (input.MessageStoreTeamFilter == 1 && e.MessageStoreTeam) || (input.MessageStoreTeamFilter == 0 && !e.MessageStoreTeam))
        //                .WhereIf(input.MinMinNumberOfGuestsFilter != null, e => e.MinNumberOfGuests >= input.MinMinNumberOfGuestsFilter)
        //                .WhereIf(input.MaxMinNumberOfGuestsFilter != null, e => e.MinNumberOfGuests <= input.MaxMinNumberOfGuestsFilter)
        //                .WhereIf(input.MinMaxNumberOfGuestsFilter != null, e => e.MaxNumberOfGuests >= input.MinMaxNumberOfGuestsFilter)
        //                .WhereIf(input.MaxMaxNumberOfGuestsFilter != null, e => e.MaxNumberOfGuests <= input.MaxMaxNumberOfGuestsFilter)
        //                .WhereIf(input.PublishAvailabilityFilter.HasValue && input.PublishAvailabilityFilter > -1, e => (input.PublishAvailabilityFilter == 1 && e.PublishAvailability) || (input.PublishAvailabilityFilter == 0 && !e.PublishAvailability))
        //                .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

        //    var query = (from o in filteredStoreReservationSettings
        //                 join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
        //                 from s1 in j1.DefaultIfEmpty()

        //                 select new GetStoreReservationSettingForViewDto()
        //                 {
        //                     StoreReservationSetting = new StoreReservationSettingDto
        //                     {
        //                         OfferReservation = o.OfferReservation,
        //                         InstantConfirmation = o.InstantConfirmation,
        //                         MessageStoreTeam = o.MessageStoreTeam,
        //                         MinNumberOfGuests = o.MinNumberOfGuests,
        //                         MaxNumberOfGuests = o.MaxNumberOfGuests,
        //                         PublishAvailability = o.PublishAvailability,
        //                         Id = o.Id
        //                     },
        //                     StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
        //                 });

        //    var storeReservationSettingListDtos = await query.ToListAsync();

        //    return _storeReservationSettingsExcelExporter.ExportToFile(storeReservationSettingListDtos);
        //}

        //[AbpAuthorize(AppPermissions.Pages_StoreReservationSettings)]
        //public async Task<PagedResultDto<StoreReservationSettingStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        //{
        //    var query = _lookup_storeRepository.GetAll().WhereIf(
        //           !string.IsNullOrWhiteSpace(input.Filter),
        //          e => e.Name != null && e.Name.Contains(input.Filter)
        //       );

        //    var totalCount = await query.CountAsync();

        //    var storeList = await query
        //        .PageBy(input)
        //        .ToListAsync();

        //    var lookupTableDtoList = new List<StoreReservationSettingStoreLookupTableDto>();
        //    foreach (var store in storeList)
        //    {
        //        lookupTableDtoList.Add(new StoreReservationSettingStoreLookupTableDto
        //        {
        //            Id = store.Id,
        //            DisplayName = store.Name?.ToString()
        //        });
        //    }

        //    return new PagedResultDto<StoreReservationSettingStoreLookupTableDto>(
        //        totalCount,
        //        lookupTableDtoList
        //    );
        //}

    }
}