using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SoftGrid.Authorization;
using SoftGrid.Authorization.Users;
using SoftGrid.Dto;
using SoftGrid.EntityFrameworkCore.Repositories;
using SoftGrid.LookupData;
using SoftGrid.LookupData.Enums;
using SoftGrid.Shop.Dtos;
using SoftGrid.Shop.Exporting;
using SoftGrid.Storage;
using SoftGrid.Territory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        private readonly IStoredProcedureRepository _storedProcedureRepository;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IRepository<ProductCategory, long> _productCategoryRepository;
        private readonly IRepository<StoreTag, long> _storeTagRepository;
        private readonly IRepository<StoreNote, long> _storeNoteRepository;
        private readonly IRepository<StoreReview, long> _storeReviewRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<StoreAccountTeam, long> _storAccountTeamRepository;
        private readonly IRepository<HubStore, long> _hubStoreRepository;
        private readonly IRepository<Hub, long> _hubRepository;
        private readonly IRepository<StoreProductMap, long> _storeProductMapRepository;
        private readonly IRepository<HubProduct, long> _hubProductMapRepository;
        private readonly IRepository<StoreProductCategoryMap, long> _storeProductCategoryMapRepository;
        private readonly IRepository<HubProductCategory, long> _hubProductCategoryRepository;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        public StoresAppService(IRepository<Store, long> storeRepository, IStoresExcelExporter storesExcelExporter, IRepository<MediaLibrary, long> lookup_mediaLibraryRepository, IRepository<Country, long> lookup_countryRepository, IRepository<State, long> lookup_stateRepository, IRepository<RatingLike, long> lookup_ratingLikeRepository, IRepository<MasterTag, long> lookup_masterTagRepository,
            IStoredProcedureRepository storedProcedureRepository, IBinaryObjectManager binaryObjectManager, IRepository<ProductCategory, long> productCategoryRepository, IRepository<StoreTag, long> storeTagRepository, IRepository<StoreNote, long> storeNoteRepository, IRepository<StoreReview, long> storeReviewRepository, IRepository<User, long> userRepository,
            IRepository<StoreAccountTeam, long> storAccountTeamRepository, ITempFileCacheManager tempFileCacheManager,
            IRepository<HubStore, long> hubStoreRepository, IRepository<Hub, long> hubRepository,
            IRepository<StoreProductMap, long> storeProductMapRepository, IRepository<HubProduct, long> hubProductMapRepository, IRepository<StoreProductCategoryMap, long> storeProductCategoryMapRepository, IRepository<HubProductCategory, long> hubProductCategoryRepository)
        {
            _storeRepository = storeRepository;
            _storesExcelExporter = storesExcelExporter;
            _lookup_mediaLibraryRepository = lookup_mediaLibraryRepository;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_stateRepository = lookup_stateRepository;
            _lookup_ratingLikeRepository = lookup_ratingLikeRepository;
            _lookup_masterTagRepository = lookup_masterTagRepository;
            _storedProcedureRepository = storedProcedureRepository;
            _binaryObjectManager = binaryObjectManager;
            _productCategoryRepository = productCategoryRepository;
            _storeTagRepository = storeTagRepository;
            _storeNoteRepository = storeNoteRepository;
            _storeReviewRepository = storeReviewRepository;
            _userRepository = userRepository;
            _storAccountTeamRepository = storAccountTeamRepository;
            _tempFileCacheManager = tempFileCacheManager;
            _hubStoreRepository = hubStoreRepository;
            _hubRepository = hubRepository;
            _storeProductMapRepository = storeProductMapRepository;
            _hubProductMapRepository = hubProductMapRepository;
            _storeProductCategoryMapRepository = storeProductCategoryMapRepository;
            _hubProductCategoryRepository = hubProductCategoryRepository;
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
                if (_lookupMediaLibrary.BinaryObjectId != Guid.Empty)
                {
                    output.Picture = await _binaryObjectManager.GetStorePictureUrlAsync(_lookupMediaLibrary.BinaryObjectId, ".png");
                }
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




            if (output.Store.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _productCategoryRepository.FirstOrDefaultAsync((long)output.Store.ProductCategoryId);
                output.Productcategoryname = _lookupProductCategory?.Name?.ToString();
            }

            var tags = _storeTagRepository.GetAll().Include(e => e.MasterTagFk).Where(e => e.StoreId == output.Store.Id);
            foreach (var tag in tags)
            {
                if (tag.MasterTagFk != null && tag.MasterTagFk.Name != null)
                {
                    output.StoreTags.Add(tag.MasterTagFk.Name);
                }
                else if (tag.CustomTag != null)
                {
                    output.StoreTags.Add(tag.CustomTag);
                }
            }

            //output.NumberOfTasks = await _taskStoreMapRepository.CountAsync(e => e.StoreId == output.Store.Id);
            output.NumberOfNotes = await _storeNoteRepository.CountAsync(e => e.StoreId == output.Store.Id);

            output.NumberOfRatings = await _storeReviewRepository.CountAsync(e => e.StoreId == output.Store.Id);
            if (output.NumberOfRatings > 0)
            {
                output.RatingScore = (_storeReviewRepository.GetAll().Where(e => e.StoreId == output.Store.Id && e.RatingLikeId != null).Select(e => e.RatingLikeId).Sum()) / output.NumberOfRatings;

            }
            else
            {
                output.RatingScore = 0;

            }

            var primaryCategory = _storeTagRepository.GetAll().Include(e => e.MasterTagFk).Where(e => e.StoreId == output.Store.Id && e.MasterTagCategoryId == 37 && e.MasterTagId != null).FirstOrDefault();
            if (primaryCategory != null)
            {
                output.Store.PrimaryCategoryId = primaryCategory.MasterTagId;
                output.PrimaryCategoryName = primaryCategory.MasterTagFk.Name;
            }

            return output;
        }

        public async Task<long?> CreateOrEdit(CreateOrEditStoreDto input)
        {
            if (input.FileToken != null)
            {
                input = await SaveStorePhoto(input);
            }

            if (input.Id == null)
            {
                long storeId = await Create(input);
                return storeId;

            }
            else
            {
                await Update(input);
                return input.Id;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Stores_Create)]
        protected virtual async Task<long> Create(CreateOrEditStoreDto input)
        {
            var store = ObjectMapper.Map<Store>(input);

            if (AbpSession.TenantId != null)
            {
                store.TenantId = (int?)AbpSession.TenantId;
            }

            store.StoreUrl = GetStoreSlugUrl(store.Name);
            long storeId = await _storeRepository.InsertAndGetIdAsync(store);

            if (input.PrimaryCategoryId != null)
            {
                var primaryCategory = new StoreTag();
                primaryCategory.MasterTagCategoryId = 37;
                primaryCategory.StoreId = storeId;
                primaryCategory.MasterTagId = input.PrimaryCategoryId;
                await _storeTagRepository.InsertAsync(primaryCategory);
            }

            //if (AbpSession.UserId != null)
            //{
            //    var user = await _userRepository.FirstOrDefaultAsync((long)AbpSession.UserId);
            //    if (user.EmployeeId != null)
            //    {
            //        var team = new StoreAccountTeam();
            //        team.EmployeeId = user.EmployeeId;
            //        team.StoreId = storeId;
            //        team.Active = true;
            //        await _storAccountTeamRepository.InsertAsync(team);
            //    }
            //}

            return storeId;

        }

        [AbpAuthorize(AppPermissions.Pages_Stores_Edit)]
        protected virtual async Task Update(CreateOrEditStoreDto input)
        {
            if (input.StoreUrl != null && input.StoreUrl != "")
            {
                input.StoreUrl = input.StoreUrl.TrimEnd();
                input.StoreUrl = input.StoreUrl.Replace(" ", "-");
                if (!(await CheckUrlAvailability((long)input.Id, input.StoreUrl)))
                {
                    throw new UserFriendlyException("Url already exist");
                }
            }
            else
            {
                input.StoreUrl = GetStoreSlugUrl(input.Name);
            }

            var store = await _storeRepository.FirstOrDefaultAsync((long)input.Id);

            if (input.PrimaryCategoryId != null)
            {
                var category = await _storeTagRepository.FirstOrDefaultAsync(e => e.StoreId == input.Id && e.MasterTagCategoryId == 37);
                if (category != null)
                {
                    category.MasterTagId = input.PrimaryCategoryId;
                    await _storeTagRepository.UpdateAsync(category);
                }
                else
                {
                    var primaryCategory = new StoreTag();
                    primaryCategory.MasterTagCategoryId = 37;
                    primaryCategory.StoreId = input.Id;
                    primaryCategory.MasterTagId = input.PrimaryCategoryId;
                    await _storeTagRepository.InsertAsync(primaryCategory);
                }

            }
            ObjectMapper.Map(input, store);
            //var store = await _storeRepository.FirstOrDefaultAsync((long)input.Id);
            //ObjectMapper.Map(input, store);

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
        public async Task<List<StoreHubsLookupTableDto>> GetAllHubForTableDropdown(long? countryId, long? storeId)
        {
            var selectedHubIds = _hubStoreRepository.GetAll().Where(e => e.StoreId == storeId).Select(e => e.HubId);

            var results = await _hubRepository.GetAll().Where(e => e.Live == true).WhereIf(countryId != null, e => e.CountryId == countryId).WhereIf(selectedHubIds != null, e => !selectedHubIds.Contains(e.Id))
                .Select(hub => new StoreHubsLookupTableDto
                {
                    Id = hub.Id,
                    DisplayName = hub == null || hub.Name == null ? "" : hub.Name.ToString()
                }).ToListAsync();
            return results;
        }

        [AbpAuthorize(AppPermissions.Pages_Stores)]
        public async Task<List<StoreHubsLookupTableDto>> GetAllSelectedHubByStore(long? storeId)
        {

            var results = await _hubStoreRepository.GetAll().Include(e => e.HubFk).Where(e => e.StoreId == storeId && e.HubId != null)
                .Select(hub => new StoreHubsLookupTableDto
                {
                    Id = hub.HubFk.Id,
                    DisplayName = hub == null || hub.HubFk == null ? "" : hub.HubFk.Name.ToString()
                }).ToListAsync();
            return results;
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

        public async Task<GetStoresBySpForView> GetAllStoresBySp(GetAllStoresInputForSp input)
        {
            List<SqlParameter> parameters = PrepareSearchParameterForGetAllStoresBySp(input);
            var result = await _storedProcedureRepository.ExecuteStoredProcedure<GetStoresBySpForView>("usp_GetAllStores", CommandType.StoredProcedure, parameters.ToArray());

            foreach (var item in result.Stores)
            {
                if (item.StoreLogoLink != null && item.StoreLogoLink != Guid.Empty)
                {
                    item.Picture = await _binaryObjectManager.GetProductPictureUrlAsync((Guid)item.StoreLogoLink, ".png");
                }
            }
            foreach (var item in parameters)
            {
                if (item.ParameterName == "@IsFavoriteOnly")
                {
                    result.IsFavoriteOnly = (int)item.Value;
                    break;
                }
            }

            return result;
        }

        private static List<SqlParameter> PrepareSearchParameterForGetAllStoresBySp(GetAllStoresInputForSp input)
        {
            if (input.Filter != null || input.NameFilter != null || input.AddressFilter != null || input.CityFilter != null || input.PhoneFilter != null ||
                input.MobileFilter != null || input.EmailFilter != null || input.IsPublishedFilter != -1 || input.IsLocalOrOnlineStore != -1 || input.IsVerified != -1 ||
                input.StateIdFilter != null || input.CountryIdFilter != null || input.ZipCodeFilter != null || input.MasterTagCategoryIdFilter != null || input.MasterTagIdFilter != null)
            {
                input.IsFavoriteOnly = -1;

            }
            input.EmployeeIdFilter = input.IsFromMasterList == true ? null : input.EmployeeIdFilter;

            List<SqlParameter> sqlParameters = new List<SqlParameter>();



            if (input.Filter != null)
            {
                input.Filter = input.Filter[0] == '"' && input.Filter[input.Filter.Length - 1] == '"' ? "*" + input.Filter + "*" : '"' + "*" + input.Filter + "*" + '"';
            }
            SqlParameter filter = new SqlParameter("@Filter", input.Filter == null ? "\"\"" : input.Filter);
            sqlParameters.Add(filter);

            if (input.NameFilter != null)
            {
                input.NameFilter = input.NameFilter[0] == '"' && input.NameFilter[input.NameFilter.Length - 1] == '"' ? "*" + input.NameFilter + "*" : '"' + "*" + input.NameFilter + "*" + '"';
            }
            SqlParameter nameFilter = new SqlParameter("@Name", input.NameFilter == null ? "\"\"" : input.NameFilter);
            sqlParameters.Add(nameFilter);

            if (input.PhoneFilter != null)
            {
                input.PhoneFilter = input.PhoneFilter[0] == '"' && input.PhoneFilter[input.PhoneFilter.Length - 1] == '"' ? "*" + input.PhoneFilter + "*" : '"' + "*" + input.PhoneFilter + "*" + '"';
            }
            SqlParameter phoneFilter = new SqlParameter("@Phone", input.PhoneFilter == null ? "\"\"" : input.PhoneFilter);
            sqlParameters.Add(phoneFilter);

            if (input.EmailFilter != null)
            {
                input.EmailFilter = input.EmailFilter[0] == '"' && input.EmailFilter[input.EmailFilter.Length - 1] == '"' ? "*" + input.EmailFilter + "*" : '"' + "*" + input.EmailFilter + "*" + '"';
            }
            SqlParameter emailFilter = new SqlParameter("@Email", input.EmailFilter == null ? "\"\"" : input.EmailFilter);
            sqlParameters.Add(emailFilter);

            if (input.MobileFilter != null)
            {
                input.MobileFilter = input.MobileFilter[0] == '"' && input.MobileFilter[input.MobileFilter.Length - 1] == '"' ? "*" + input.MobileFilter + "*" : '"' + "*" + input.MobileFilter + "*" + '"';
            }
            SqlParameter mobileFilter = new SqlParameter("@Mobile", input.MobileFilter == null ? "\"\"" : input.MobileFilter);
            sqlParameters.Add(mobileFilter);


            if (input.AddressFilter != null)
            {
                input.AddressFilter = input.AddressFilter[0] == '"' && input.AddressFilter[input.AddressFilter.Length - 1] == '"' ? "*" + input.AddressFilter + "*" : '"' + "*" + input.AddressFilter + "*" + '"';
            }
            SqlParameter addressFilter = new SqlParameter("@Address", input.AddressFilter == null ? "\"\"" : input.AddressFilter);
            sqlParameters.Add(addressFilter);

            if (input.CityFilter != null)
            {
                input.CityFilter = input.CityFilter[0] == '"' && input.CityFilter[input.CityFilter.Length - 1] == '"' ? "*" + input.CityFilter + "*" : '"' + "*" + input.CityFilter + "*" + '"';
            }
            SqlParameter cityFilter = new SqlParameter("@City", input.CityFilter == null ? "\"\"" : input.CityFilter);
            sqlParameters.Add(cityFilter);

            SqlParameter stateIdFilter = new SqlParameter("@StateId", input.StateIdFilter == null ? (object)DBNull.Value : input.StateIdFilter);
            sqlParameters.Add(stateIdFilter);

            SqlParameter countryIdFilter = new SqlParameter("@CountryId", input.CountryIdFilter == null ? (object)DBNull.Value : input.CountryIdFilter);
            sqlParameters.Add(countryIdFilter);

            SqlParameter isPublishedFilter = new SqlParameter("@IsPublished", input.IsPublishedFilter == -1 ? (object)DBNull.Value : input.IsPublishedFilter);
            sqlParameters.Add(isPublishedFilter);

            SqlParameter onlineStoreFilter = new SqlParameter("@OnlineStore", input.IsLocalOrOnlineStore == -1 ? (object)DBNull.Value : input.IsLocalOrOnlineStore);
            sqlParameters.Add(onlineStoreFilter);

            SqlParameter verifiedFilter = new SqlParameter("@Verified", input.IsVerified == -1 ? (object)DBNull.Value : input.IsVerified);
            sqlParameters.Add(verifiedFilter);

            SqlParameter employeeIdFilter = new SqlParameter("@EmployeeIdFilter", input.EmployeeIdFilter == null ? (object)DBNull.Value : input.EmployeeIdFilter);
            sqlParameters.Add(employeeIdFilter);

            SqlParameter isFavoriteOnly = new SqlParameter("@IsFavoriteOnly", input.IsFavoriteOnly == -1 ? 0 : input.IsFavoriteOnly);
            sqlParameters.Add(isFavoriteOnly);

            SqlParameter contactIdFilter = new SqlParameter("@ContactIdFilter", input.ContactIdFilter == null ? (object)DBNull.Value : input.ContactIdFilter);
            sqlParameters.Add(contactIdFilter);

            SqlParameter zipCodeFilter = new SqlParameter("@ZipCodeFilter", input.ZipCodeFilter == null ? (object)DBNull.Value : input.ZipCodeFilter);
            sqlParameters.Add(zipCodeFilter);

            SqlParameter skipCount = new SqlParameter("@SkipCount", input.SkipCount);
            sqlParameters.Add(skipCount);

            SqlParameter maxResultCount = new SqlParameter("@MaxResultCount", input.MaxResultCount);
            sqlParameters.Add(maxResultCount);

            SqlParameter masterTagCategoryIdFilter = new SqlParameter("@MasterTagCategoryIdFilter", input.MasterTagCategoryIdFilter == null ? (object)DBNull.Value : input.MasterTagCategoryIdFilter);
            sqlParameters.Add(masterTagCategoryIdFilter);

            SqlParameter masterTagIdFilter = new SqlParameter("@MasterTagIdFilter", input.MasterTagIdFilter == null ? (object)DBNull.Value : input.MasterTagIdFilter);
            sqlParameters.Add(masterTagIdFilter);

            return sqlParameters;
        }

        private async Task<CreateOrEditStoreDto> SaveStorePhoto(CreateOrEditStoreDto input)
        {
            CreateOrEditStoreDto storeDto = input;

            byte[] byteArray;

            var imageBytes = _tempFileCacheManager.GetFile(input.FileToken);
            if (imageBytes == null)
            {
                throw new UserFriendlyException("There is no such image file with the token: " + input.FileToken);
            }

            using (var bmpImage = new Bitmap(new MemoryStream(imageBytes)))
            {
                using (var stream = new MemoryStream())
                {
                    byteArray = stream.ToArray();
                }
            }
            byteArray = imageBytes;

            //if (byteArray.Length > MaxProfilPictureBytes)
            //{
            //    throw new UserFriendlyException(L("ResizedProfilePicture_Warn_SizeLimit", AppConsts.ResizedMaxProfilPictureBytesUserFriendlyValue));
            //}

            var storedFile = new BinaryObject(AbpSession.TenantId, byteArray);
            await _binaryObjectManager.SaveAsync(storedFile);
            var mediaLibrary = new MediaLibrary();
            mediaLibrary.BinaryObjectId = storedFile.Id;
            mediaLibrary.Name = input.Name + "_" + "Logo";
            mediaLibrary.MasterTagCategoryId = (long)MasterTagCategoryEnum.Media_Type;
            mediaLibrary.MasterTagId = 1;
            mediaLibrary.FileExtension = ".png";
            mediaLibrary.Size = (byteArray.Length / 1024).ToString() + " kb";
            Image image = Image.FromStream(new MemoryStream(byteArray));
            mediaLibrary.Dimension = image.Width.ToString() + "*" + image.Height.ToString();

            storeDto.LogoMediaLibraryId = await _lookup_mediaLibraryRepository.InsertAndGetIdAsync(mediaLibrary);
            return storeDto;

        }



        private string GetStoreSlugUrl(string name)
        {
            string str = RemoveAccent(name).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens 
            int count = _storeRepository.Count(e => e.StoreUrl.Equals(str));
            str = count > 0 ? str + "-" + (count + 1).ToString() : str;
            return str;
        }

        private string GetProductStoreSlugUrl(string name)
        {
            string str = RemoveAccent(name).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens 
            //int count = _storeRepository.Count(e => e.StoreUrl.Equals(str));
            int count = 0;
            str = count > 0 ? str + "-" + (count + 1).ToString() : str;
            return str;
        }
        public string RemoveAccent(string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        public async Task<bool> CheckUrlAvailability(long storeId, string url)
        {
            var store = await _storeRepository.FirstOrDefaultAsync(e => e.Id != storeId && e.StoreUrl == url);
            if (store == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task BulkHubMap(long storeId, long[] ids)
        {
            foreach (var id in ids)
            {
                var store = await _hubStoreRepository.FirstOrDefaultAsync(e => e.StoreId == storeId && e.HubId == id);
                {
                    if (store == null)
                    {
                        var hubStore = new HubStore();
                        hubStore.StoreId = storeId;
                        hubStore.HubId = id;
                        hubStore.Published = true;
                        await _hubStoreRepository.InsertAsync(hubStore);
                        //await AssignStoreProductsToHub(storeId, id);
                        //await AssignStoreCategoriesToHub(storeId, id);
                    }
                }

            }
        }

        private async Task AssignStoreProductsToHub(long storeId, long hubId)
        {
            var storeProducts = _storeProductMapRepository.GetAll().Where(e => e.StoreId == storeId);
            foreach (var item in storeProducts)
            {
                var isAdded = await _hubProductMapRepository.FirstOrDefaultAsync(e => e.ProductId == item.ProductId && e.HubId == hubId);
                if (isAdded == null)
                {
                    var hubProduct = new HubProduct();
                    hubProduct.HubId = hubId;
                    hubProduct.ProductId = item.ProductId;
                    hubProduct.Published = true;
                    await _hubProductMapRepository.InsertAsync(hubProduct);
                }

            }
        }

        private async Task AssignStoreCategoriesToHub(long storeId, long hubId)
        {
            var storeCategories = _storeProductCategoryMapRepository.GetAll().Where(e => e.StoreId == storeId);
            foreach (var item in storeCategories)
            {
                var isAdded = await _hubProductCategoryRepository.FirstOrDefaultAsync(e => e.ProductCategoryId == item.ProductCategoryId && e.HubId == hubId);
                if (isAdded == null)
                {
                    var hubCategory = new HubProductCategory();
                    hubCategory.HubId = hubId;
                    hubCategory.ProductCategoryId = item.ProductCategoryId;
                    hubCategory.Published = true;
                    await _hubProductCategoryRepository.InsertAsync(hubCategory);
                }

            }
        }

        public async Task<List<StorePrimaryCategoryLookupTableDto>> GetAllStorePrimaryCategoryForTableDropdown()
        {
            return await _lookup_masterTagRepository.GetAll()
                .Where(e => e.MasterTagCategoryId == 18)
                .Select(taxRate => new StorePrimaryCategoryLookupTableDto
                {
                    Id = taxRate.Id,
                    DisplayName = taxRate == null || taxRate.Name == null ? "" : taxRate.Name.ToString()
                }).ToListAsync();
        }
    }
}