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
    [AbpAuthorize(AppPermissions.Pages_Stores)]
    public class StoresAppService : SoftGridAppServiceBase, IStoresAppService
    {
        private readonly IRepository<Store, long> _storeRepository;
        private readonly IStoresExcelExporter _storesExcelExporter;
        private readonly IRepository<MediaLibrary, long> _lookup_mediaLibraryRepository;
        private readonly IRepository<Country, long> _lookup_countryRepository;
        private readonly IRepository<State, long> _lookup_stateRepository;
        private readonly IRepository<RatingLike, long> _lookup_ratingLikeRepository;
        private readonly IRepository<MasterTag, long> _lookup_masterTagRepository;

        public StoresAppService(IRepository<Store, long> storeRepository, IStoresExcelExporter storesExcelExporter, IRepository<MediaLibrary, long> lookup_mediaLibraryRepository, IRepository<Country, long> lookup_countryRepository, IRepository<State, long> lookup_stateRepository, IRepository<RatingLike, long> lookup_ratingLikeRepository, IRepository<MasterTag, long> lookup_masterTagRepository)
        {
            _storeRepository = storeRepository;
            _storesExcelExporter = storesExcelExporter;
            _lookup_mediaLibraryRepository = lookup_mediaLibraryRepository;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_stateRepository = lookup_stateRepository;
            _lookup_ratingLikeRepository = lookup_ratingLikeRepository;
            _lookup_masterTagRepository = lookup_masterTagRepository;

        }

        public async Task<PagedResultDto<GetStoreForViewDto>> GetAll(GetAllStoresInput input)
        {

            var filteredStores = _storeRepository.GetAll()
                        .Include(e => e.LogoMediaLibraryFk)
                        .Include(e => e.CountryFk)
                        .Include(e => e.StateFk)
                        .Include(e => e.RatingLikeFk)
                        .Include(e => e.StoreCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.StoreUrl.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.MetaTag.Contains(input.Filter) || e.MetaDescription.Contains(input.Filter) || e.FullAddress.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.City.Contains(input.Filter) || e.Phone.Contains(input.Filter) || e.Mobile.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.Facebook.Contains(input.Filter) || e.Instagram.Contains(input.Filter) || e.LinkedIn.Contains(input.Filter) || e.Youtube.Contains(input.Filter) || e.Fax.Contains(input.Filter) || e.ZipCode.Contains(input.Filter) || e.Website.Contains(input.Filter) || e.YearOfEstablishment.Contains(input.Filter) || e.LegalName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreUrlFilter), e => e.StoreUrl.Contains(input.StoreUrlFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaTagFilter), e => e.MetaTag.Contains(input.MetaTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaDescriptionFilter), e => e.MetaDescription.Contains(input.MetaDescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FullAddressFilter), e => e.FullAddress.Contains(input.FullAddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.Contains(input.CityFilter))
                        .WhereIf(input.MinLatitudeFilter != null, e => e.Latitude >= input.MinLatitudeFilter)
                        .WhereIf(input.MaxLatitudeFilter != null, e => e.Latitude <= input.MaxLatitudeFilter)
                        .WhereIf(input.MinLongitudeFilter != null, e => e.Longitude >= input.MinLongitudeFilter)
                        .WhereIf(input.MaxLongitudeFilter != null, e => e.Longitude <= input.MaxLongitudeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneFilter), e => e.Phone.Contains(input.PhoneFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MobileFilter), e => e.Mobile.Contains(input.MobileFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(input.IsPublishedFilter.HasValue && input.IsPublishedFilter > -1, e => (input.IsPublishedFilter == 1 && e.IsPublished) || (input.IsPublishedFilter == 0 && !e.IsPublished))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FacebookFilter), e => e.Facebook.Contains(input.FacebookFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InstagramFilter), e => e.Instagram.Contains(input.InstagramFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LinkedInFilter), e => e.LinkedIn.Contains(input.LinkedInFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.YoutubeFilter), e => e.Youtube.Contains(input.YoutubeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FaxFilter), e => e.Fax.Contains(input.FaxFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WebsiteFilter), e => e.Website.Contains(input.WebsiteFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.YearOfEstablishmentFilter), e => e.YearOfEstablishment.Contains(input.YearOfEstablishmentFilter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(input.MinScoreFilter != null, e => e.Score >= input.MinScoreFilter)
                        .WhereIf(input.MaxScoreFilter != null, e => e.Score <= input.MaxScoreFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LegalNameFilter), e => e.LegalName.Contains(input.LegalNameFilter))
                        .WhereIf(input.IsLocalOrOnlineStoreFilter.HasValue && input.IsLocalOrOnlineStoreFilter > -1, e => (input.IsLocalOrOnlineStoreFilter == 1 && e.IsLocalOrOnlineStore) || (input.IsLocalOrOnlineStoreFilter == 0 && !e.IsLocalOrOnlineStore))
                        .WhereIf(input.IsVerifiedFilter.HasValue && input.IsVerifiedFilter > -1, e => (input.IsVerifiedFilter == 1 && e.IsVerified) || (input.IsVerifiedFilter == 0 && !e.IsVerified))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.LogoMediaLibraryFk != null && e.LogoMediaLibraryFk.Name == input.MediaLibraryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RatingLikeNameFilter), e => e.RatingLikeFk != null && e.RatingLikeFk.Name == input.RatingLikeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.StoreCategoryFk != null && e.StoreCategoryFk.Name == input.MasterTagNameFilter);

            var pagedAndFilteredStores = filteredStores
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var stores = from o in pagedAndFilteredStores
                         join o1 in _lookup_mediaLibraryRepository.GetAll() on o.LogoMediaLibraryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_countryRepository.GetAll() on o.CountryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_stateRepository.GetAll() on o.StateId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_ratingLikeRepository.GetAll() on o.RatingLikeId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         join o5 in _lookup_masterTagRepository.GetAll() on o.StoreCategoryId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()

                         select new
                         {

                             o.Name,
                             o.StoreUrl,
                             o.Description,
                             o.MetaTag,
                             o.MetaDescription,
                             o.FullAddress,
                             o.Address,
                             o.City,
                             o.Latitude,
                             o.Longitude,
                             o.Phone,
                             o.Mobile,
                             o.Email,
                             o.IsPublished,
                             o.Facebook,
                             o.Instagram,
                             o.LinkedIn,
                             o.Youtube,
                             o.Fax,
                             o.ZipCode,
                             o.Website,
                             o.YearOfEstablishment,
                             o.DisplaySequence,
                             o.Score,
                             o.LegalName,
                             o.IsLocalOrOnlineStore,
                             o.IsVerified,
                             Id = o.Id,
                             MediaLibraryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             CountryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             StateName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             RatingLikeName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                             MasterTagName = s5 == null || s5.Name == null ? "" : s5.Name.ToString()
                         };

            var totalCount = await filteredStores.CountAsync();

            var dbList = await stores.ToListAsync();
            var results = new List<GetStoreForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreForViewDto()
                {
                    Store = new StoreDto
                    {

                        Name = o.Name,
                        StoreUrl = o.StoreUrl,
                        Description = o.Description,
                        MetaTag = o.MetaTag,
                        MetaDescription = o.MetaDescription,
                        FullAddress = o.FullAddress,
                        Address = o.Address,
                        City = o.City,
                        Latitude = o.Latitude,
                        Longitude = o.Longitude,
                        Phone = o.Phone,
                        Mobile = o.Mobile,
                        Email = o.Email,
                        IsPublished = o.IsPublished,
                        Facebook = o.Facebook,
                        Instagram = o.Instagram,
                        LinkedIn = o.LinkedIn,
                        Youtube = o.Youtube,
                        Fax = o.Fax,
                        ZipCode = o.ZipCode,
                        Website = o.Website,
                        YearOfEstablishment = o.YearOfEstablishment,
                        DisplaySequence = o.DisplaySequence,
                        Score = o.Score,
                        LegalName = o.LegalName,
                        IsLocalOrOnlineStore = o.IsLocalOrOnlineStore,
                        IsVerified = o.IsVerified,
                        Id = o.Id,
                    },
                    MediaLibraryName = o.MediaLibraryName,
                    CountryName = o.CountryName,
                    StateName = o.StateName,
                    RatingLikeName = o.RatingLikeName,
                    MasterTagName = o.MasterTagName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreForViewDto> GetStoreForView(long id)
        {
            var store = await _storeRepository.GetAsync(id);

            var output = new GetStoreForViewDto { Store = ObjectMapper.Map<StoreDto>(store) };

            if (output.Store.LogoMediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.Store.LogoMediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            if (output.Store.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.Store.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.Store.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.Store.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            if (output.Store.RatingLikeId != null)
            {
                var _lookupRatingLike = await _lookup_ratingLikeRepository.FirstOrDefaultAsync((long)output.Store.RatingLikeId);
                output.RatingLikeName = _lookupRatingLike?.Name?.ToString();
            }

            if (output.Store.StoreCategoryId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.Store.StoreCategoryId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Stores_Edit)]
        public async Task<GetStoreForEditOutput> GetStoreForEdit(EntityDto<long> input)
        {
            var store = await _storeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreForEditOutput { Store = ObjectMapper.Map<CreateOrEditStoreDto>(store) };

            if (output.Store.LogoMediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.Store.LogoMediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            if (output.Store.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.Store.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.Store.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.Store.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            if (output.Store.RatingLikeId != null)
            {
                var _lookupRatingLike = await _lookup_ratingLikeRepository.FirstOrDefaultAsync((long)output.Store.RatingLikeId);
                output.RatingLikeName = _lookupRatingLike?.Name?.ToString();
            }

            if (output.Store.StoreCategoryId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.Store.StoreCategoryId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Stores_Create)]
        protected virtual async Task Create(CreateOrEditStoreDto input)
        {
            var store = ObjectMapper.Map<Store>(input);

            if (AbpSession.TenantId != null)
            {
                store.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeRepository.InsertAsync(store);

        }

        [AbpAuthorize(AppPermissions.Pages_Stores_Edit)]
        protected virtual async Task Update(CreateOrEditStoreDto input)
        {
            var store = await _storeRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, store);

        }

        [AbpAuthorize(AppPermissions.Pages_Stores_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoresToExcel(GetAllStoresForExcelInput input)
        {

            var filteredStores = _storeRepository.GetAll()
                        .Include(e => e.LogoMediaLibraryFk)
                        .Include(e => e.CountryFk)
                        .Include(e => e.StateFk)
                        .Include(e => e.RatingLikeFk)
                        .Include(e => e.StoreCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.StoreUrl.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.MetaTag.Contains(input.Filter) || e.MetaDescription.Contains(input.Filter) || e.FullAddress.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.City.Contains(input.Filter) || e.Phone.Contains(input.Filter) || e.Mobile.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.Facebook.Contains(input.Filter) || e.Instagram.Contains(input.Filter) || e.LinkedIn.Contains(input.Filter) || e.Youtube.Contains(input.Filter) || e.Fax.Contains(input.Filter) || e.ZipCode.Contains(input.Filter) || e.Website.Contains(input.Filter) || e.YearOfEstablishment.Contains(input.Filter) || e.LegalName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreUrlFilter), e => e.StoreUrl.Contains(input.StoreUrlFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaTagFilter), e => e.MetaTag.Contains(input.MetaTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MetaDescriptionFilter), e => e.MetaDescription.Contains(input.MetaDescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FullAddressFilter), e => e.FullAddress.Contains(input.FullAddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.Contains(input.CityFilter))
                        .WhereIf(input.MinLatitudeFilter != null, e => e.Latitude >= input.MinLatitudeFilter)
                        .WhereIf(input.MaxLatitudeFilter != null, e => e.Latitude <= input.MaxLatitudeFilter)
                        .WhereIf(input.MinLongitudeFilter != null, e => e.Longitude >= input.MinLongitudeFilter)
                        .WhereIf(input.MaxLongitudeFilter != null, e => e.Longitude <= input.MaxLongitudeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneFilter), e => e.Phone.Contains(input.PhoneFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MobileFilter), e => e.Mobile.Contains(input.MobileFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(input.IsPublishedFilter.HasValue && input.IsPublishedFilter > -1, e => (input.IsPublishedFilter == 1 && e.IsPublished) || (input.IsPublishedFilter == 0 && !e.IsPublished))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FacebookFilter), e => e.Facebook.Contains(input.FacebookFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InstagramFilter), e => e.Instagram.Contains(input.InstagramFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LinkedInFilter), e => e.LinkedIn.Contains(input.LinkedInFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.YoutubeFilter), e => e.Youtube.Contains(input.YoutubeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FaxFilter), e => e.Fax.Contains(input.FaxFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WebsiteFilter), e => e.Website.Contains(input.WebsiteFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.YearOfEstablishmentFilter), e => e.YearOfEstablishment.Contains(input.YearOfEstablishmentFilter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(input.MinScoreFilter != null, e => e.Score >= input.MinScoreFilter)
                        .WhereIf(input.MaxScoreFilter != null, e => e.Score <= input.MaxScoreFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LegalNameFilter), e => e.LegalName.Contains(input.LegalNameFilter))
                        .WhereIf(input.IsLocalOrOnlineStoreFilter.HasValue && input.IsLocalOrOnlineStoreFilter > -1, e => (input.IsLocalOrOnlineStoreFilter == 1 && e.IsLocalOrOnlineStore) || (input.IsLocalOrOnlineStoreFilter == 0 && !e.IsLocalOrOnlineStore))
                        .WhereIf(input.IsVerifiedFilter.HasValue && input.IsVerifiedFilter > -1, e => (input.IsVerifiedFilter == 1 && e.IsVerified) || (input.IsVerifiedFilter == 0 && !e.IsVerified))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.LogoMediaLibraryFk != null && e.LogoMediaLibraryFk.Name == input.MediaLibraryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RatingLikeNameFilter), e => e.RatingLikeFk != null && e.RatingLikeFk.Name == input.RatingLikeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.StoreCategoryFk != null && e.StoreCategoryFk.Name == input.MasterTagNameFilter);

            var query = (from o in filteredStores
                         join o1 in _lookup_mediaLibraryRepository.GetAll() on o.LogoMediaLibraryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_countryRepository.GetAll() on o.CountryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_stateRepository.GetAll() on o.StateId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_ratingLikeRepository.GetAll() on o.RatingLikeId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         join o5 in _lookup_masterTagRepository.GetAll() on o.StoreCategoryId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()

                         select new GetStoreForViewDto()
                         {
                             Store = new StoreDto
                             {
                                 Name = o.Name,
                                 StoreUrl = o.StoreUrl,
                                 Description = o.Description,
                                 MetaTag = o.MetaTag,
                                 MetaDescription = o.MetaDescription,
                                 FullAddress = o.FullAddress,
                                 Address = o.Address,
                                 City = o.City,
                                 Latitude = o.Latitude,
                                 Longitude = o.Longitude,
                                 Phone = o.Phone,
                                 Mobile = o.Mobile,
                                 Email = o.Email,
                                 IsPublished = o.IsPublished,
                                 Facebook = o.Facebook,
                                 Instagram = o.Instagram,
                                 LinkedIn = o.LinkedIn,
                                 Youtube = o.Youtube,
                                 Fax = o.Fax,
                                 ZipCode = o.ZipCode,
                                 Website = o.Website,
                                 YearOfEstablishment = o.YearOfEstablishment,
                                 DisplaySequence = o.DisplaySequence,
                                 Score = o.Score,
                                 LegalName = o.LegalName,
                                 IsLocalOrOnlineStore = o.IsLocalOrOnlineStore,
                                 IsVerified = o.IsVerified,
                                 Id = o.Id
                             },
                             MediaLibraryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             CountryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             StateName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             RatingLikeName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                             MasterTagName = s5 == null || s5.Name == null ? "" : s5.Name.ToString()
                         });

            var storeListDtos = await query.ToListAsync();

            return _storesExcelExporter.ExportToFile(storeListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Stores)]
        public async Task<PagedResultDto<StoreMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_mediaLibraryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var mediaLibraryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreMediaLibraryLookupTableDto>();
            foreach (var mediaLibrary in mediaLibraryList)
            {
                lookupTableDtoList.Add(new StoreMediaLibraryLookupTableDto
                {
                    Id = mediaLibrary.Id,
                    DisplayName = mediaLibrary.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreMediaLibraryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        [AbpAuthorize(AppPermissions.Pages_Stores)]
        public async Task<List<StoreCountryLookupTableDto>> GetAllCountryForTableDropdown()
        {
            return await _lookup_countryRepository.GetAll()
                .Select(country => new StoreCountryLookupTableDto
                {
                    Id = country.Id,
                    DisplayName = country == null || country.Name == null ? "" : country.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Stores)]
        public async Task<List<StoreStateLookupTableDto>> GetAllStateForTableDropdown()
        {
            return await _lookup_stateRepository.GetAll()
                .Select(state => new StoreStateLookupTableDto
                {
                    Id = state.Id,
                    DisplayName = state == null || state.Name == null ? "" : state.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Stores)]
        public async Task<List<StoreRatingLikeLookupTableDto>> GetAllRatingLikeForTableDropdown()
        {
            return await _lookup_ratingLikeRepository.GetAll()
                .Select(ratingLike => new StoreRatingLikeLookupTableDto
                {
                    Id = ratingLike.Id,
                    DisplayName = ratingLike == null || ratingLike.Name == null ? "" : ratingLike.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Stores)]
        public async Task<PagedResultDto<StoreMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreMasterTagLookupTableDto>();
            foreach (var masterTag in masterTagList)
            {
                lookupTableDtoList.Add(new StoreMasterTagLookupTableDto
                {
                    Id = masterTag.Id,
                    DisplayName = masterTag.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreMasterTagLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}