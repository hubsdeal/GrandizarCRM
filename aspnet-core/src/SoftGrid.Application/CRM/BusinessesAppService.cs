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
    [AbpAuthorize(AppPermissions.Pages_Businesses)]
    public class BusinessesAppService : SoftGridAppServiceBase, IBusinessesAppService
    {
        private readonly IRepository<Business, long> _businessRepository;
        private readonly IBusinessesExcelExporter _businessesExcelExporter;
        private readonly IRepository<Country, long> _lookup_countryRepository;
        private readonly IRepository<State, long> _lookup_stateRepository;
        private readonly IRepository<City, long> _lookup_cityRepository;
        private readonly IRepository<MediaLibrary, long> _lookup_mediaLibraryRepository;

        public BusinessesAppService(IRepository<Business, long> businessRepository, IBusinessesExcelExporter businessesExcelExporter, IRepository<Country, long> lookup_countryRepository, IRepository<State, long> lookup_stateRepository, IRepository<City, long> lookup_cityRepository, IRepository<MediaLibrary, long> lookup_mediaLibraryRepository)
        {
            _businessRepository = businessRepository;
            _businessesExcelExporter = businessesExcelExporter;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_stateRepository = lookup_stateRepository;
            _lookup_cityRepository = lookup_cityRepository;
            _lookup_mediaLibraryRepository = lookup_mediaLibraryRepository;

        }

        public async Task<PagedResultDto<GetBusinessForViewDto>> GetAll(GetAllBusinessesInput input)
        {

            var filteredBusinesses = _businessRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .Include(e => e.StateFk)
                        .Include(e => e.CityFk)
                        .Include(e => e.LogoMediaLibraryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.TradeName.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.CustomId.Contains(input.Filter) || e.YearOfEstablishment.Contains(input.Filter) || e.LocationTitle.Contains(input.Filter) || e.FullAddress.Contains(input.Filter) || e.Address1.Contains(input.Filter) || e.Address2.Contains(input.Filter) || e.City.Contains(input.Filter) || e.ZipCode.Contains(input.Filter) || e.Phone.Contains(input.Filter) || e.Fax.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.Website.Contains(input.Filter) || e.EinTaxId.Contains(input.Filter) || e.Industry.Contains(input.Filter) || e.InternalRemarks.Contains(input.Filter) || e.Facebook.Contains(input.Filter) || e.LinkedIn.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TradeNameFilter), e => e.TradeName.Contains(input.TradeNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomIdFilter), e => e.CustomId.Contains(input.CustomIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.YearOfEstablishmentFilter), e => e.YearOfEstablishment.Contains(input.YearOfEstablishmentFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocationTitleFilter), e => e.LocationTitle.Contains(input.LocationTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FullAddressFilter), e => e.FullAddress.Contains(input.FullAddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Address1Filter), e => e.Address1.Contains(input.Address1Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Address2Filter), e => e.Address2.Contains(input.Address2Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.Contains(input.CityFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter))
                        .WhereIf(input.MinLatitudeFilter != null, e => e.Latitude >= input.MinLatitudeFilter)
                        .WhereIf(input.MaxLatitudeFilter != null, e => e.Latitude <= input.MaxLatitudeFilter)
                        .WhereIf(input.MinLongitudeFilter != null, e => e.Longitude >= input.MinLongitudeFilter)
                        .WhereIf(input.MaxLongitudeFilter != null, e => e.Longitude <= input.MaxLongitudeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneFilter), e => e.Phone.Contains(input.PhoneFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FaxFilter), e => e.Fax.Contains(input.FaxFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WebsiteFilter), e => e.Website.Contains(input.WebsiteFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EinTaxIdFilter), e => e.EinTaxId.Contains(input.EinTaxIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IndustryFilter), e => e.Industry.Contains(input.IndustryFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InternalRemarksFilter), e => e.InternalRemarks.Contains(input.InternalRemarksFilter))
                        .WhereIf(input.VerifiedFilter.HasValue && input.VerifiedFilter > -1, e => (input.VerifiedFilter == 1 && e.Verified) || (input.VerifiedFilter == 0 && !e.Verified))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FacebookFilter), e => e.Facebook.Contains(input.FacebookFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LinkedInFilter), e => e.LinkedIn.Contains(input.LinkedInFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityNameFilter), e => e.CityFk != null && e.CityFk.Name == input.CityNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.LogoMediaLibraryFk != null && e.LogoMediaLibraryFk.Name == input.MediaLibraryNameFilter);

            var pagedAndFilteredBusinesses = filteredBusinesses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var businesses = from o in pagedAndFilteredBusinesses
                             join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             join o2 in _lookup_stateRepository.GetAll() on o.StateId equals o2.Id into j2
                             from s2 in j2.DefaultIfEmpty()

                             join o3 in _lookup_cityRepository.GetAll() on o.CityId equals o3.Id into j3
                             from s3 in j3.DefaultIfEmpty()

                             join o4 in _lookup_mediaLibraryRepository.GetAll() on o.LogoMediaLibraryId equals o4.Id into j4
                             from s4 in j4.DefaultIfEmpty()

                             select new
                             {

                                 o.Name,
                                 o.TradeName,
                                 o.Description,
                                 o.CustomId,
                                 o.YearOfEstablishment,
                                 o.LocationTitle,
                                 o.FullAddress,
                                 o.Address1,
                                 o.Address2,
                                 o.City,
                                 o.ZipCode,
                                 o.Latitude,
                                 o.Longitude,
                                 o.Phone,
                                 o.Fax,
                                 o.Email,
                                 o.Website,
                                 o.EinTaxId,
                                 o.Industry,
                                 o.InternalRemarks,
                                 o.Verified,
                                 o.Facebook,
                                 o.LinkedIn,
                                 Id = o.Id,
                                 CountryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                 StateName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                 CityName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                                 MediaLibraryName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                             };

            var totalCount = await filteredBusinesses.CountAsync();

            var dbList = await businesses.ToListAsync();
            var results = new List<GetBusinessForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBusinessForViewDto()
                {
                    Business = new BusinessDto
                    {

                        Name = o.Name,
                        TradeName = o.TradeName,
                        Description = o.Description,
                        CustomId = o.CustomId,
                        YearOfEstablishment = o.YearOfEstablishment,
                        LocationTitle = o.LocationTitle,
                        FullAddress = o.FullAddress,
                        Address1 = o.Address1,
                        Address2 = o.Address2,
                        City = o.City,
                        ZipCode = o.ZipCode,
                        Latitude = o.Latitude,
                        Longitude = o.Longitude,
                        Phone = o.Phone,
                        Fax = o.Fax,
                        Email = o.Email,
                        Website = o.Website,
                        EinTaxId = o.EinTaxId,
                        Industry = o.Industry,
                        InternalRemarks = o.InternalRemarks,
                        Verified = o.Verified,
                        Facebook = o.Facebook,
                        LinkedIn = o.LinkedIn,
                        Id = o.Id,
                    },
                    CountryName = o.CountryName,
                    StateName = o.StateName,
                    CityName = o.CityName,
                    MediaLibraryName = o.MediaLibraryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetBusinessForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetBusinessForViewDto> GetBusinessForView(long id)
        {
            var business = await _businessRepository.GetAsync(id);

            var output = new GetBusinessForViewDto { Business = ObjectMapper.Map<BusinessDto>(business) };

            if (output.Business.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.Business.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.Business.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.Business.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            if (output.Business.CityId != null)
            {
                var _lookupCity = await _lookup_cityRepository.FirstOrDefaultAsync((long)output.Business.CityId);
                output.CityName = _lookupCity?.Name?.ToString();
            }

            if (output.Business.LogoMediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.Business.LogoMediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Businesses_Edit)]
        public async Task<GetBusinessForEditOutput> GetBusinessForEdit(EntityDto<long> input)
        {
            var business = await _businessRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBusinessForEditOutput { Business = ObjectMapper.Map<CreateOrEditBusinessDto>(business) };

            if (output.Business.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.Business.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.Business.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.Business.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            if (output.Business.CityId != null)
            {
                var _lookupCity = await _lookup_cityRepository.FirstOrDefaultAsync((long)output.Business.CityId);
                output.CityName = _lookupCity?.Name?.ToString();
            }

            if (output.Business.LogoMediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.Business.LogoMediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBusinessDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Businesses_Create)]
        protected virtual async Task Create(CreateOrEditBusinessDto input)
        {
            var business = ObjectMapper.Map<Business>(input);

            if (AbpSession.TenantId != null)
            {
                business.TenantId = (int?)AbpSession.TenantId;
            }

            await _businessRepository.InsertAsync(business);

        }

        [AbpAuthorize(AppPermissions.Pages_Businesses_Edit)]
        protected virtual async Task Update(CreateOrEditBusinessDto input)
        {
            var business = await _businessRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, business);

        }

        [AbpAuthorize(AppPermissions.Pages_Businesses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _businessRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetBusinessesToExcel(GetAllBusinessesForExcelInput input)
        {

            var filteredBusinesses = _businessRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .Include(e => e.StateFk)
                        .Include(e => e.CityFk)
                        .Include(e => e.LogoMediaLibraryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.TradeName.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.CustomId.Contains(input.Filter) || e.YearOfEstablishment.Contains(input.Filter) || e.LocationTitle.Contains(input.Filter) || e.FullAddress.Contains(input.Filter) || e.Address1.Contains(input.Filter) || e.Address2.Contains(input.Filter) || e.City.Contains(input.Filter) || e.ZipCode.Contains(input.Filter) || e.Phone.Contains(input.Filter) || e.Fax.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.Website.Contains(input.Filter) || e.EinTaxId.Contains(input.Filter) || e.Industry.Contains(input.Filter) || e.InternalRemarks.Contains(input.Filter) || e.Facebook.Contains(input.Filter) || e.LinkedIn.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TradeNameFilter), e => e.TradeName.Contains(input.TradeNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomIdFilter), e => e.CustomId.Contains(input.CustomIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.YearOfEstablishmentFilter), e => e.YearOfEstablishment.Contains(input.YearOfEstablishmentFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocationTitleFilter), e => e.LocationTitle.Contains(input.LocationTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FullAddressFilter), e => e.FullAddress.Contains(input.FullAddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Address1Filter), e => e.Address1.Contains(input.Address1Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Address2Filter), e => e.Address2.Contains(input.Address2Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.Contains(input.CityFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter))
                        .WhereIf(input.MinLatitudeFilter != null, e => e.Latitude >= input.MinLatitudeFilter)
                        .WhereIf(input.MaxLatitudeFilter != null, e => e.Latitude <= input.MaxLatitudeFilter)
                        .WhereIf(input.MinLongitudeFilter != null, e => e.Longitude >= input.MinLongitudeFilter)
                        .WhereIf(input.MaxLongitudeFilter != null, e => e.Longitude <= input.MaxLongitudeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneFilter), e => e.Phone.Contains(input.PhoneFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FaxFilter), e => e.Fax.Contains(input.FaxFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WebsiteFilter), e => e.Website.Contains(input.WebsiteFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EinTaxIdFilter), e => e.EinTaxId.Contains(input.EinTaxIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IndustryFilter), e => e.Industry.Contains(input.IndustryFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InternalRemarksFilter), e => e.InternalRemarks.Contains(input.InternalRemarksFilter))
                        .WhereIf(input.VerifiedFilter.HasValue && input.VerifiedFilter > -1, e => (input.VerifiedFilter == 1 && e.Verified) || (input.VerifiedFilter == 0 && !e.Verified))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FacebookFilter), e => e.Facebook.Contains(input.FacebookFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LinkedInFilter), e => e.LinkedIn.Contains(input.LinkedInFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityNameFilter), e => e.CityFk != null && e.CityFk.Name == input.CityNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.LogoMediaLibraryFk != null && e.LogoMediaLibraryFk.Name == input.MediaLibraryNameFilter);

            var query = (from o in filteredBusinesses
                         join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_stateRepository.GetAll() on o.StateId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_cityRepository.GetAll() on o.CityId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_mediaLibraryRepository.GetAll() on o.LogoMediaLibraryId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetBusinessForViewDto()
                         {
                             Business = new BusinessDto
                             {
                                 Name = o.Name,
                                 TradeName = o.TradeName,
                                 Description = o.Description,
                                 CustomId = o.CustomId,
                                 YearOfEstablishment = o.YearOfEstablishment,
                                 LocationTitle = o.LocationTitle,
                                 FullAddress = o.FullAddress,
                                 Address1 = o.Address1,
                                 Address2 = o.Address2,
                                 City = o.City,
                                 ZipCode = o.ZipCode,
                                 Latitude = o.Latitude,
                                 Longitude = o.Longitude,
                                 Phone = o.Phone,
                                 Fax = o.Fax,
                                 Email = o.Email,
                                 Website = o.Website,
                                 EinTaxId = o.EinTaxId,
                                 Industry = o.Industry,
                                 InternalRemarks = o.InternalRemarks,
                                 Verified = o.Verified,
                                 Facebook = o.Facebook,
                                 LinkedIn = o.LinkedIn,
                                 Id = o.Id
                             },
                             CountryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             StateName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             CityName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             MediaLibraryName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                         });

            var businessListDtos = await query.ToListAsync();

            return _businessesExcelExporter.ExportToFile(businessListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Businesses)]
        public async Task<List<BusinessCountryLookupTableDto>> GetAllCountryForTableDropdown()
        {
            return await _lookup_countryRepository.GetAll()
                .Select(country => new BusinessCountryLookupTableDto
                {
                    Id = country.Id,
                    DisplayName = country == null || country.Name == null ? "" : country.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Businesses)]
        public async Task<List<BusinessStateLookupTableDto>> GetAllStateForTableDropdown()
        {
            return await _lookup_stateRepository.GetAll()
                .Select(state => new BusinessStateLookupTableDto
                {
                    Id = state.Id,
                    DisplayName = state == null || state.Name == null ? "" : state.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Businesses)]
        public async Task<List<BusinessCityLookupTableDto>> GetAllCityForTableDropdown()
        {
            return await _lookup_cityRepository.GetAll()
                .Select(city => new BusinessCityLookupTableDto
                {
                    Id = city.Id,
                    DisplayName = city == null || city.Name == null ? "" : city.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Businesses)]
        public async Task<PagedResultDto<BusinessMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_mediaLibraryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var mediaLibraryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BusinessMediaLibraryLookupTableDto>();
            foreach (var mediaLibrary in mediaLibraryList)
            {
                lookupTableDtoList.Add(new BusinessMediaLibraryLookupTableDto
                {
                    Id = mediaLibrary.Id,
                    DisplayName = mediaLibrary.Name?.ToString()
                });
            }

            return new PagedResultDto<BusinessMediaLibraryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}