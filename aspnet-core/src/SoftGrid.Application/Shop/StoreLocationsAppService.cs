using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
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
    [AbpAuthorize(AppPermissions.Pages_StoreLocations)]
    public class StoreLocationsAppService : SoftGridAppServiceBase, IStoreLocationsAppService
    {
        private readonly IRepository<StoreLocation, long> _storeLocationRepository;
        private readonly IStoreLocationsExcelExporter _storeLocationsExcelExporter;
        private readonly IRepository<City, long> _lookup_cityRepository;
        private readonly IRepository<State, long> _lookup_stateRepository;
        private readonly IRepository<Country, long> _lookup_countryRepository;
        private readonly IRepository<Store, long> _lookup_storeRepository;

        public StoreLocationsAppService(IRepository<StoreLocation, long> storeLocationRepository, IStoreLocationsExcelExporter storeLocationsExcelExporter, IRepository<City, long> lookup_cityRepository, IRepository<State, long> lookup_stateRepository, IRepository<Country, long> lookup_countryRepository, IRepository<Store, long> lookup_storeRepository)
        {
            _storeLocationRepository = storeLocationRepository;
            _storeLocationsExcelExporter = storeLocationsExcelExporter;
            _lookup_cityRepository = lookup_cityRepository;
            _lookup_stateRepository = lookup_stateRepository;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_storeRepository = lookup_storeRepository;

        }

        public async Task<PagedResultDto<GetStoreLocationForViewDto>> GetAll(GetAllStoreLocationsInput input)
        {

            var filteredStoreLocations = _storeLocationRepository.GetAll()
                        .Include(e => e.CityFk)
                        .Include(e => e.StateFk)
                        .Include(e => e.CountryFk)
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LocationName.Contains(input.Filter) || e.FullAddress.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.Mobile.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.ZipCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocationNameFilter), e => e.LocationName.Contains(input.LocationNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FullAddressFilter), e => e.FullAddress.Contains(input.FullAddressFilter))
                        .WhereIf(input.MinLatitudeFilter != null, e => e.Latitude >= input.MinLatitudeFilter)
                        .WhereIf(input.MaxLatitudeFilter != null, e => e.Latitude <= input.MaxLatitudeFilter)
                        .WhereIf(input.MinLongitudeFilter != null, e => e.Longitude >= input.MinLongitudeFilter)
                        .WhereIf(input.MaxLongitudeFilter != null, e => e.Longitude <= input.MaxLongitudeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MobileFilter), e => e.Mobile.Contains(input.MobileFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityNameFilter), e => e.CityFk != null && e.CityFk.Name == input.CityNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var pagedAndFilteredStoreLocations = filteredStoreLocations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeLocations = from o in pagedAndFilteredStoreLocations
                                 join o1 in _lookup_cityRepository.GetAll() on o.CityId equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()

                                 join o2 in _lookup_stateRepository.GetAll() on o.StateId equals o2.Id into j2
                                 from s2 in j2.DefaultIfEmpty()

                                 join o3 in _lookup_countryRepository.GetAll() on o.CountryId equals o3.Id into j3
                                 from s3 in j3.DefaultIfEmpty()

                                 join o4 in _lookup_storeRepository.GetAll() on o.StoreId equals o4.Id into j4
                                 from s4 in j4.DefaultIfEmpty()

                                 select new
                                 {

                                     o.LocationName,
                                     o.FullAddress,
                                     o.Latitude,
                                     o.Longitude,
                                     o.Address,
                                     o.Mobile,
                                     o.Email,
                                     o.ZipCode,
                                     Id = o.Id,
                                     CityName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                     StateName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                     CountryName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                                     StoreName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                                 };

            var totalCount = await filteredStoreLocations.CountAsync();

            var dbList = await storeLocations.ToListAsync();
            var results = new List<GetStoreLocationForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreLocationForViewDto()
                {
                    StoreLocation = new StoreLocationDto
                    {

                        LocationName = o.LocationName,
                        FullAddress = o.FullAddress,
                        Latitude = o.Latitude,
                        Longitude = o.Longitude,
                        Address = o.Address,
                        Mobile = o.Mobile,
                        Email = o.Email,
                        ZipCode = o.ZipCode,
                        Id = o.Id,
                    },
                    CityName = o.CityName,
                    StateName = o.StateName,
                    CountryName = o.CountryName,
                    StoreName = o.StoreName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreLocationForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreLocationForViewDto> GetStoreLocationForView(long id)
        {
            var storeLocation = await _storeLocationRepository.GetAsync(id);

            var output = new GetStoreLocationForViewDto { StoreLocation = ObjectMapper.Map<StoreLocationDto>(storeLocation) };

            if (output.StoreLocation.CityId != null)
            {
                var _lookupCity = await _lookup_cityRepository.FirstOrDefaultAsync((long)output.StoreLocation.CityId);
                output.CityName = _lookupCity?.Name?.ToString();
            }

            if (output.StoreLocation.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.StoreLocation.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            if (output.StoreLocation.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.StoreLocation.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.StoreLocation.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreLocation.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreLocations_Edit)]
        public async Task<GetStoreLocationForEditOutput> GetStoreLocationForEdit(EntityDto<long> input)
        {
            var storeLocation = await _storeLocationRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreLocationForEditOutput { StoreLocation = ObjectMapper.Map<CreateOrEditStoreLocationDto>(storeLocation) };

            if (output.StoreLocation.CityId != null)
            {
                var _lookupCity = await _lookup_cityRepository.FirstOrDefaultAsync((long)output.StoreLocation.CityId);
                output.CityName = _lookupCity?.Name?.ToString();
            }

            if (output.StoreLocation.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.StoreLocation.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            if (output.StoreLocation.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.StoreLocation.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.StoreLocation.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreLocation.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreLocationDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreLocations_Create)]
        protected virtual async Task Create(CreateOrEditStoreLocationDto input)
        {
            var storeLocation = ObjectMapper.Map<StoreLocation>(input);

            if (AbpSession.TenantId != null)
            {
                storeLocation.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeLocationRepository.InsertAsync(storeLocation);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreLocations_Edit)]
        protected virtual async Task Update(CreateOrEditStoreLocationDto input)
        {
            var storeLocation = await _storeLocationRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeLocation);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreLocations_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeLocationRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreLocationsToExcel(GetAllStoreLocationsForExcelInput input)
        {

            var filteredStoreLocations = _storeLocationRepository.GetAll()
                        .Include(e => e.CityFk)
                        .Include(e => e.StateFk)
                        .Include(e => e.CountryFk)
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LocationName.Contains(input.Filter) || e.FullAddress.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.Mobile.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.ZipCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocationNameFilter), e => e.LocationName.Contains(input.LocationNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FullAddressFilter), e => e.FullAddress.Contains(input.FullAddressFilter))
                        .WhereIf(input.MinLatitudeFilter != null, e => e.Latitude >= input.MinLatitudeFilter)
                        .WhereIf(input.MaxLatitudeFilter != null, e => e.Latitude <= input.MaxLatitudeFilter)
                        .WhereIf(input.MinLongitudeFilter != null, e => e.Longitude >= input.MinLongitudeFilter)
                        .WhereIf(input.MaxLongitudeFilter != null, e => e.Longitude <= input.MaxLongitudeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MobileFilter), e => e.Mobile.Contains(input.MobileFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityNameFilter), e => e.CityFk != null && e.CityFk.Name == input.CityNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var query = (from o in filteredStoreLocations
                         join o1 in _lookup_cityRepository.GetAll() on o.CityId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_stateRepository.GetAll() on o.StateId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_countryRepository.GetAll() on o.CountryId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_storeRepository.GetAll() on o.StoreId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetStoreLocationForViewDto()
                         {
                             StoreLocation = new StoreLocationDto
                             {
                                 LocationName = o.LocationName,
                                 FullAddress = o.FullAddress,
                                 Latitude = o.Latitude,
                                 Longitude = o.Longitude,
                                 Address = o.Address,
                                 Mobile = o.Mobile,
                                 Email = o.Email,
                                 ZipCode = o.ZipCode,
                                 Id = o.Id
                             },
                             CityName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             StateName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             CountryName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             StoreName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                         });

            var storeLocationListDtos = await query.ToListAsync();

            return _storeLocationsExcelExporter.ExportToFile(storeLocationListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreLocations)]
        public async Task<PagedResultDto<StoreLocationCityLookupTableDto>> GetAllCityForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_cityRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var cityList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreLocationCityLookupTableDto>();
            foreach (var city in cityList)
            {
                lookupTableDtoList.Add(new StoreLocationCityLookupTableDto
                {
                    Id = city.Id,
                    DisplayName = city.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreLocationCityLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreLocations)]
        public async Task<PagedResultDto<StoreLocationStateLookupTableDto>> GetAllStateForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_stateRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var stateList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreLocationStateLookupTableDto>();
            foreach (var state in stateList)
            {
                lookupTableDtoList.Add(new StoreLocationStateLookupTableDto
                {
                    Id = state.Id,
                    DisplayName = state.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreLocationStateLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreLocations)]
        public async Task<PagedResultDto<StoreLocationCountryLookupTableDto>> GetAllCountryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_countryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var countryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreLocationCountryLookupTableDto>();
            foreach (var country in countryList)
            {
                lookupTableDtoList.Add(new StoreLocationCountryLookupTableDto
                {
                    Id = country.Id,
                    DisplayName = country.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreLocationCountryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreLocations)]
        public async Task<PagedResultDto<StoreLocationStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreLocationStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreLocationStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreLocationStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}