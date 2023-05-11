using SoftGrid.LookupData;
using SoftGrid.LookupData;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.Territory.Exporting;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;
using Microsoft.Data.SqlClient;
using System.Data;
using SoftGrid.EntityFrameworkCore.Repositories;

namespace SoftGrid.Territory
{
    [AbpAuthorize(AppPermissions.Pages_Hubs)]
    public class HubsAppService : SoftGridAppServiceBase, IHubsAppService
    {
        private readonly IRepository<Hub, long> _hubRepository;
        private readonly IHubsExcelExporter _hubsExcelExporter;
        private readonly IRepository<Country, long> _lookup_countryRepository;
        private readonly IRepository<State, long> _lookup_stateRepository;
        private readonly IRepository<City, long> _lookup_cityRepository;
        private readonly IRepository<County, long> _lookup_countyRepository;
        private readonly IRepository<HubType, long> _lookup_hubTypeRepository;
        private readonly IRepository<Currency, long> _lookup_currencyRepository;
        private readonly IRepository<MediaLibrary, long> _lookup_mediaLibraryRepository;
        private readonly IStoredProcedureRepository _storedProcedureRepository;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public HubsAppService(IRepository<Hub, long> hubRepository, IHubsExcelExporter hubsExcelExporter, IRepository<Country, long> lookup_countryRepository, IRepository<State, long> lookup_stateRepository, IRepository<City, long> lookup_cityRepository, IRepository<County, long> lookup_countyRepository, IRepository<HubType, long> lookup_hubTypeRepository, IRepository<Currency, long> lookup_currencyRepository, IRepository<MediaLibrary, long> lookup_mediaLibraryRepository,
            IStoredProcedureRepository storedProcedureRepository, IBinaryObjectManager binaryObjectManager)
        {
            _hubRepository = hubRepository;
            _hubsExcelExporter = hubsExcelExporter;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_stateRepository = lookup_stateRepository;
            _lookup_cityRepository = lookup_cityRepository;
            _lookup_countyRepository = lookup_countyRepository;
            _lookup_hubTypeRepository = lookup_hubTypeRepository;
            _lookup_currencyRepository = lookup_currencyRepository;
            _lookup_mediaLibraryRepository = lookup_mediaLibraryRepository;
            _storedProcedureRepository = storedProcedureRepository;
            _binaryObjectManager = binaryObjectManager;
        }

        public async Task<PagedResultDto<GetHubForViewDto>> GetAll(GetAllHubsInput input)
        {

            var filteredHubs = _hubRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .Include(e => e.StateFk)
                        .Include(e => e.CityFk)
                        .Include(e => e.CountyFk)
                        .Include(e => e.HubTypeFk)
                        .Include(e => e.CurrencyFk)
                        .Include(e => e.PictureMediaLibraryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Url.Contains(input.Filter) || e.OfficeFullAddress.Contains(input.Filter) || e.Phone.Contains(input.Filter) || e.YearlyRevenue.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.MinEstimatedPopulationFilter != null, e => e.EstimatedPopulation >= input.MinEstimatedPopulationFilter)
                        .WhereIf(input.MaxEstimatedPopulationFilter != null, e => e.EstimatedPopulation <= input.MaxEstimatedPopulationFilter)
                        .WhereIf(input.HasParentHubFilter.HasValue && input.HasParentHubFilter > -1, e => (input.HasParentHubFilter == 1 && e.HasParentHub) || (input.HasParentHubFilter == 0 && !e.HasParentHub))
                        .WhereIf(input.MinParentHubIdFilter != null, e => e.ParentHubId >= input.MinParentHubIdFilter)
                        .WhereIf(input.MaxParentHubIdFilter != null, e => e.ParentHubId <= input.MaxParentHubIdFilter)
                        .WhereIf(input.MinLatitudeFilter != null, e => e.Latitude >= input.MinLatitudeFilter)
                        .WhereIf(input.MaxLatitudeFilter != null, e => e.Latitude <= input.MaxLatitudeFilter)
                        .WhereIf(input.MinLongitudeFilter != null, e => e.Longitude >= input.MinLongitudeFilter)
                        .WhereIf(input.MaxLongitudeFilter != null, e => e.Longitude <= input.MaxLongitudeFilter)
                        .WhereIf(input.LiveFilter.HasValue && input.LiveFilter > -1, e => (input.LiveFilter == 1 && e.Live) || (input.LiveFilter == 0 && !e.Live))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UrlFilter), e => e.Url.Contains(input.UrlFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OfficeFullAddressFilter), e => e.OfficeFullAddress.Contains(input.OfficeFullAddressFilter))
                        .WhereIf(input.PartnerOrOwnedFilter.HasValue && input.PartnerOrOwnedFilter > -1, e => (input.PartnerOrOwnedFilter == 1 && e.PartnerOrOwned) || (input.PartnerOrOwnedFilter == 0 && !e.PartnerOrOwned))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneFilter), e => e.Phone.Contains(input.PhoneFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.YearlyRevenueFilter), e => e.YearlyRevenue.Contains(input.YearlyRevenueFilter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityNameFilter), e => e.CityFk != null && e.CityFk.Name == input.CityNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountyNameFilter), e => e.CountyFk != null && e.CountyFk.Name == input.CountyNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubTypeNameFilter), e => e.HubTypeFk != null && e.HubTypeFk.Name == input.HubTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyNameFilter), e => e.CurrencyFk != null && e.CurrencyFk.Name == input.CurrencyNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.PictureMediaLibraryFk != null && e.PictureMediaLibraryFk.Name == input.MediaLibraryNameFilter)
                        .WhereIf(input.CountryIdFilter != 0, e => e.CountryId == input.CountryIdFilter)
                        .WhereIf(input.StateIdFilter != 0, e => e.StateId == input.StateIdFilter)
                        .WhereIf(input.HubTypeIdFilter != 0, e => e.HubTypeId == input.HubTypeIdFilter);

            var pagedAndFilteredHubs = filteredHubs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var hubs = from o in pagedAndFilteredHubs
                       join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                       from s1 in j1.DefaultIfEmpty()

                       join o2 in _lookup_stateRepository.GetAll() on o.StateId equals o2.Id into j2
                       from s2 in j2.DefaultIfEmpty()

                       join o3 in _lookup_cityRepository.GetAll() on o.CityId equals o3.Id into j3
                       from s3 in j3.DefaultIfEmpty()

                       join o4 in _lookup_countyRepository.GetAll() on o.CountyId equals o4.Id into j4
                       from s4 in j4.DefaultIfEmpty()

                       join o5 in _lookup_hubTypeRepository.GetAll() on o.HubTypeId equals o5.Id into j5
                       from s5 in j5.DefaultIfEmpty()

                       join o6 in _lookup_currencyRepository.GetAll() on o.CurrencyId equals o6.Id into j6
                       from s6 in j6.DefaultIfEmpty()

                       join o7 in _lookup_mediaLibraryRepository.GetAll() on o.PictureMediaLibraryId equals o7.Id into j7
                       from s7 in j7.DefaultIfEmpty()

                       select new
                       {

                           o.Name,
                           o.Description,
                           o.EstimatedPopulation,
                           o.HasParentHub,
                           o.ParentHubId,
                           o.Latitude,
                           o.Longitude,
                           o.Live,
                           o.Url,
                           o.OfficeFullAddress,
                           o.PartnerOrOwned,
                           o.Phone,
                           o.YearlyRevenue,
                           o.DisplaySequence,
                           Id = o.Id,
                           CountryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                           StateName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                           CityName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                           CountyName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                           HubTypeName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
                           CurrencyName = s6 == null || s6.Name == null ? "" : s6.Name.ToString(),
                           MediaLibraryName = s7 == null || s7.Name == null ? "" : s7.Name.ToString()
                       };

            var totalCount = await filteredHubs.CountAsync();

            var dbList = await hubs.ToListAsync();
            var results = new List<GetHubForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetHubForViewDto()
                {
                    Hub = new HubDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        EstimatedPopulation = o.EstimatedPopulation,
                        HasParentHub = o.HasParentHub,
                        ParentHubId = o.ParentHubId,
                        Latitude = o.Latitude,
                        Longitude = o.Longitude,
                        Live = o.Live,
                        Url = o.Url,
                        OfficeFullAddress = o.OfficeFullAddress,
                        PartnerOrOwned = o.PartnerOrOwned,
                        Phone = o.Phone,
                        YearlyRevenue = o.YearlyRevenue,
                        DisplaySequence = o.DisplaySequence,
                        Id = o.Id,
                    },
                    CountryName = o.CountryName,
                    StateName = o.StateName,
                    CityName = o.CityName,
                    CountyName = o.CountyName,
                    HubTypeName = o.HubTypeName,
                    CurrencyName = o.CurrencyName,
                    MediaLibraryName = o.MediaLibraryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetHubForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetHubForViewDto> GetHubForView(long id)
        {
            var hub = await _hubRepository.GetAsync(id);

            var output = new GetHubForViewDto { Hub = ObjectMapper.Map<HubDto>(hub) };

            if (output.Hub.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.Hub.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.Hub.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.Hub.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            if (output.Hub.CityId != null)
            {
                var _lookupCity = await _lookup_cityRepository.FirstOrDefaultAsync((long)output.Hub.CityId);
                output.CityName = _lookupCity?.Name?.ToString();
            }

            if (output.Hub.CountyId != null)
            {
                var _lookupCounty = await _lookup_countyRepository.FirstOrDefaultAsync((long)output.Hub.CountyId);
                output.CountyName = _lookupCounty?.Name?.ToString();
            }

            if (output.Hub.HubTypeId != null)
            {
                var _lookupHubType = await _lookup_hubTypeRepository.FirstOrDefaultAsync((long)output.Hub.HubTypeId);
                output.HubTypeName = _lookupHubType?.Name?.ToString();
            }

            if (output.Hub.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((long)output.Hub.CurrencyId);
                output.CurrencyName = _lookupCurrency?.Name?.ToString();
            }

            if (output.Hub.PictureMediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.Hub.PictureMediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Hubs_Edit)]
        public async Task<GetHubForEditOutput> GetHubForEdit(EntityDto<long> input)
        {
            var hub = await _hubRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHubForEditOutput { Hub = ObjectMapper.Map<CreateOrEditHubDto>(hub) };

            if (output.Hub.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.Hub.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.Hub.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.Hub.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            if (output.Hub.CityId != null)
            {
                var _lookupCity = await _lookup_cityRepository.FirstOrDefaultAsync((long)output.Hub.CityId);
                output.CityName = _lookupCity?.Name?.ToString();
            }

            if (output.Hub.CountyId != null)
            {
                var _lookupCounty = await _lookup_countyRepository.FirstOrDefaultAsync((long)output.Hub.CountyId);
                output.CountyName = _lookupCounty?.Name?.ToString();
            }

            if (output.Hub.HubTypeId != null)
            {
                var _lookupHubType = await _lookup_hubTypeRepository.FirstOrDefaultAsync((long)output.Hub.HubTypeId);
                output.HubTypeName = _lookupHubType?.Name?.ToString();
            }

            if (output.Hub.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((long)output.Hub.CurrencyId);
                output.CurrencyName = _lookupCurrency?.Name?.ToString();
            }

            if (output.Hub.PictureMediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.Hub.PictureMediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHubDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Hubs_Create)]
        protected virtual async Task Create(CreateOrEditHubDto input)
        {
            var hub = ObjectMapper.Map<Hub>(input);

            if (AbpSession.TenantId != null)
            {
                hub.TenantId = (int?)AbpSession.TenantId;
            }

            await _hubRepository.InsertAsync(hub);

        }

        [AbpAuthorize(AppPermissions.Pages_Hubs_Edit)]
        protected virtual async Task Update(CreateOrEditHubDto input)
        {
            var hub = await _hubRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, hub);

        }

        [AbpAuthorize(AppPermissions.Pages_Hubs_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _hubRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetHubsToExcel(GetAllHubsForExcelInput input)
        {

            var filteredHubs = _hubRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .Include(e => e.StateFk)
                        .Include(e => e.CityFk)
                        .Include(e => e.CountyFk)
                        .Include(e => e.HubTypeFk)
                        .Include(e => e.CurrencyFk)
                        .Include(e => e.PictureMediaLibraryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Url.Contains(input.Filter) || e.OfficeFullAddress.Contains(input.Filter) || e.Phone.Contains(input.Filter) || e.YearlyRevenue.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.MinEstimatedPopulationFilter != null, e => e.EstimatedPopulation >= input.MinEstimatedPopulationFilter)
                        .WhereIf(input.MaxEstimatedPopulationFilter != null, e => e.EstimatedPopulation <= input.MaxEstimatedPopulationFilter)
                        .WhereIf(input.HasParentHubFilter.HasValue && input.HasParentHubFilter > -1, e => (input.HasParentHubFilter == 1 && e.HasParentHub) || (input.HasParentHubFilter == 0 && !e.HasParentHub))
                        .WhereIf(input.MinParentHubIdFilter != null, e => e.ParentHubId >= input.MinParentHubIdFilter)
                        .WhereIf(input.MaxParentHubIdFilter != null, e => e.ParentHubId <= input.MaxParentHubIdFilter)
                        .WhereIf(input.MinLatitudeFilter != null, e => e.Latitude >= input.MinLatitudeFilter)
                        .WhereIf(input.MaxLatitudeFilter != null, e => e.Latitude <= input.MaxLatitudeFilter)
                        .WhereIf(input.MinLongitudeFilter != null, e => e.Longitude >= input.MinLongitudeFilter)
                        .WhereIf(input.MaxLongitudeFilter != null, e => e.Longitude <= input.MaxLongitudeFilter)
                        .WhereIf(input.LiveFilter.HasValue && input.LiveFilter > -1, e => (input.LiveFilter == 1 && e.Live) || (input.LiveFilter == 0 && !e.Live))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UrlFilter), e => e.Url.Contains(input.UrlFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OfficeFullAddressFilter), e => e.OfficeFullAddress.Contains(input.OfficeFullAddressFilter))
                        .WhereIf(input.PartnerOrOwnedFilter.HasValue && input.PartnerOrOwnedFilter > -1, e => (input.PartnerOrOwnedFilter == 1 && e.PartnerOrOwned) || (input.PartnerOrOwnedFilter == 0 && !e.PartnerOrOwned))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneFilter), e => e.Phone.Contains(input.PhoneFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.YearlyRevenueFilter), e => e.YearlyRevenue.Contains(input.YearlyRevenueFilter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityNameFilter), e => e.CityFk != null && e.CityFk.Name == input.CityNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountyNameFilter), e => e.CountyFk != null && e.CountyFk.Name == input.CountyNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubTypeNameFilter), e => e.HubTypeFk != null && e.HubTypeFk.Name == input.HubTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyNameFilter), e => e.CurrencyFk != null && e.CurrencyFk.Name == input.CurrencyNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.PictureMediaLibraryFk != null && e.PictureMediaLibraryFk.Name == input.MediaLibraryNameFilter);

            var query = (from o in filteredHubs
                         join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_stateRepository.GetAll() on o.StateId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_cityRepository.GetAll() on o.CityId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_countyRepository.GetAll() on o.CountyId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         join o5 in _lookup_hubTypeRepository.GetAll() on o.HubTypeId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()

                         join o6 in _lookup_currencyRepository.GetAll() on o.CurrencyId equals o6.Id into j6
                         from s6 in j6.DefaultIfEmpty()

                         join o7 in _lookup_mediaLibraryRepository.GetAll() on o.PictureMediaLibraryId equals o7.Id into j7
                         from s7 in j7.DefaultIfEmpty()

                         select new GetHubForViewDto()
                         {
                             Hub = new HubDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 EstimatedPopulation = o.EstimatedPopulation,
                                 HasParentHub = o.HasParentHub,
                                 ParentHubId = o.ParentHubId,
                                 Latitude = o.Latitude,
                                 Longitude = o.Longitude,
                                 Live = o.Live,
                                 Url = o.Url,
                                 OfficeFullAddress = o.OfficeFullAddress,
                                 PartnerOrOwned = o.PartnerOrOwned,
                                 Phone = o.Phone,
                                 YearlyRevenue = o.YearlyRevenue,
                                 DisplaySequence = o.DisplaySequence,
                                 Id = o.Id
                             },
                             CountryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             StateName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             CityName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             CountyName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                             HubTypeName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
                             CurrencyName = s6 == null || s6.Name == null ? "" : s6.Name.ToString(),
                             MediaLibraryName = s7 == null || s7.Name == null ? "" : s7.Name.ToString()
                         });

            var hubListDtos = await query.ToListAsync();

            return _hubsExcelExporter.ExportToFile(hubListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Hubs)]
        public async Task<List<HubCountryLookupTableDto>> GetAllCountryForTableDropdown()
        {
            return await _lookup_countryRepository.GetAll()
                .Select(country => new HubCountryLookupTableDto
                {
                    Id = country.Id,
                    DisplayName = country == null || country.Name == null ? "" : country.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Hubs)]
        public async Task<List<HubStateLookupTableDto>> GetAllStateForTableDropdown()
        {
            return await _lookup_stateRepository.GetAll()
                .Select(state => new HubStateLookupTableDto
                {
                    Id = state.Id,
                    DisplayName = state == null || state.Name == null ? "" : state.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Hubs)]
        public async Task<List<HubCityLookupTableDto>> GetAllCityForTableDropdown()
        {
            return await _lookup_cityRepository.GetAll()
                .Select(city => new HubCityLookupTableDto
                {
                    Id = city.Id,
                    DisplayName = city == null || city.Name == null ? "" : city.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Hubs)]
        public async Task<List<HubCountyLookupTableDto>> GetAllCountyForTableDropdown()
        {
            return await _lookup_countyRepository.GetAll()
                .Select(county => new HubCountyLookupTableDto
                {
                    Id = county.Id,
                    DisplayName = county == null || county.Name == null ? "" : county.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Hubs)]
        public async Task<List<HubHubTypeLookupTableDto>> GetAllHubTypeForTableDropdown()
        {
            return await _lookup_hubTypeRepository.GetAll()
                .Select(hubType => new HubHubTypeLookupTableDto
                {
                    Id = hubType.Id,
                    DisplayName = hubType == null || hubType.Name == null ? "" : hubType.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Hubs)]
        public async Task<List<HubCurrencyLookupTableDto>> GetAllCurrencyForTableDropdown()
        {
            return await _lookup_currencyRepository.GetAll()
                .Select(currency => new HubCurrencyLookupTableDto
                {
                    Id = currency.Id,
                    DisplayName = currency == null || currency.Name == null ? "" : currency.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Hubs)]
        public async Task<PagedResultDto<HubMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_mediaLibraryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var mediaLibraryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubMediaLibraryLookupTableDto>();
            foreach (var mediaLibrary in mediaLibraryList)
            {
                lookupTableDtoList.Add(new HubMediaLibraryLookupTableDto
                {
                    Id = mediaLibrary.Id,
                    DisplayName = mediaLibrary.Name?.ToString()
                });
            }

            return new PagedResultDto<HubMediaLibraryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }


        public async Task<GetHubsBySpForView> GetAllHubsBySp(GetAllHubsInputForSp input)
        {
            List<SqlParameter> parameters = PrepareSearchParameterForGetAllHubsBySp(input);
            var result = await _storedProcedureRepository.ExecuteStoredProcedure<GetHubsBySpForView>("usp_GetAllHubs", CommandType.StoredProcedure, parameters.ToArray());

            foreach (var item in result.Hubs)
            {
                if (item.BinaryObjectId != null && item.BinaryObjectId != Guid.Empty)
                {
                    item.Picture = await _binaryObjectManager.GetProductPictureUrlAsync((Guid)item.BinaryObjectId, ".png");
                }
            }
           

            return result;
        }

        private static List<SqlParameter> PrepareSearchParameterForGetAllHubsBySp(GetAllHubsInputForSp input)
        {
           

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

            if (input.CityNameFilter != null)
            {
                input.CityNameFilter = input.CityNameFilter[0] == '"' && input.CityNameFilter[input.CityNameFilter.Length - 1] == '"' ? "*" + input.CityNameFilter + "*" : '"' + "*" + input.CityNameFilter + "*" + '"';
            }
            SqlParameter cityFilter = new SqlParameter("@City", input.CityNameFilter == null ? "\"\"" : input.CityNameFilter);
            sqlParameters.Add(cityFilter);

            SqlParameter stateIdFilter = new SqlParameter("@StateId", input.StateIdFilter == null ? (object)DBNull.Value : input.StateIdFilter);
            sqlParameters.Add(stateIdFilter);

            SqlParameter countryIdFilter = new SqlParameter("@CountryId", input.CountryIdFilter == null ? (object)DBNull.Value : input.CountryIdFilter);
            sqlParameters.Add(countryIdFilter);

            SqlParameter zipCodeFilter = new SqlParameter("@ZipCode", input.ZipCodeFilter == null ? (object)DBNull.Value : input.ZipCodeFilter);
            sqlParameters.Add(zipCodeFilter);

            SqlParameter liveFilter = new SqlParameter("@Live", input.LiveFilter == -1 ? (object)DBNull.Value : input.LiveFilter);
            sqlParameters.Add(liveFilter);

            SqlParameter ownerFilter = new SqlParameter("@Owner", input.PartnerOrOwnedFilter == -1 ? (object)DBNull.Value : input.PartnerOrOwnedFilter);
            sqlParameters.Add(ownerFilter);

            SqlParameter hubTypeIdFilter = new SqlParameter("@HubTypeId", input.HubTypeIdFilter == null ? (object)DBNull.Value : input.HubTypeIdFilter);
            sqlParameters.Add(hubTypeIdFilter);

            SqlParameter skipCount = new SqlParameter("@SkipCount", input.SkipCount);
            sqlParameters.Add(skipCount);

            SqlParameter maxResultCount = new SqlParameter("@MaxResultCount", input.MaxResultCount);
            sqlParameters.Add(maxResultCount);

           

            return sqlParameters;
        }
    }
}