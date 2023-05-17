using SoftGrid.LookupData;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.LookupData.Exporting;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;
using SoftGrid.Territory.Dtos;

namespace SoftGrid.LookupData
{
    [AbpAuthorize(AppPermissions.Pages_Cities)]
    public class CitiesAppService : SoftGridAppServiceBase, ICitiesAppService
    {
        private readonly IRepository<City, long> _cityRepository;
        private readonly ICitiesExcelExporter _citiesExcelExporter;
        private readonly IRepository<Country, long> _lookup_countryRepository;
        private readonly IRepository<State, long> _lookup_stateRepository;
        private readonly IRepository<County, long> _lookup_countyRepository;

        public CitiesAppService(IRepository<City, long> cityRepository, ICitiesExcelExporter citiesExcelExporter, IRepository<Country, long> lookup_countryRepository, IRepository<State, long> lookup_stateRepository, IRepository<County, long> lookup_countyRepository)
        {
            _cityRepository = cityRepository;
            _citiesExcelExporter = citiesExcelExporter;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_stateRepository = lookup_stateRepository;
            _lookup_countyRepository = lookup_countyRepository;

        }

        public async Task<PagedResultDto<GetCityForViewDto>> GetAll(GetAllCitiesInput input)
        {

            var filteredCities = _cityRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .Include(e => e.StateFk)
                        .Include(e => e.CountyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountyNameFilter), e => e.CountyFk != null && e.CountyFk.Name == input.CountyNameFilter);

            var pagedAndFilteredCities = filteredCities
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var cities = from o in pagedAndFilteredCities
                         join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_stateRepository.GetAll() on o.StateId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_countyRepository.GetAll() on o.CountyId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new
                         {

                             o.Name,
                             Id = o.Id,
                             CountryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             StateName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             CountyName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         };

            var totalCount = await filteredCities.CountAsync();

            var dbList = await cities.ToListAsync();
            var results = new List<GetCityForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCityForViewDto()
                {
                    City = new CityDto
                    {

                        Name = o.Name,
                        Id = o.Id,
                    },
                    CountryName = o.CountryName,
                    StateName = o.StateName,
                    CountyName = o.CountyName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCityForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCityForViewDto> GetCityForView(long id)
        {
            var city = await _cityRepository.GetAsync(id);

            var output = new GetCityForViewDto { City = ObjectMapper.Map<CityDto>(city) };

            if (output.City.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.City.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.City.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.City.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            if (output.City.CountyId != null)
            {
                var _lookupCounty = await _lookup_countyRepository.FirstOrDefaultAsync((long)output.City.CountyId);
                output.CountyName = _lookupCounty?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Cities_Edit)]
        public async Task<GetCityForEditOutput> GetCityForEdit(EntityDto<long> input)
        {
            var city = await _cityRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCityForEditOutput { City = ObjectMapper.Map<CreateOrEditCityDto>(city) };

            if (output.City.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.City.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.City.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.City.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            if (output.City.CountyId != null)
            {
                var _lookupCounty = await _lookup_countyRepository.FirstOrDefaultAsync((long)output.City.CountyId);
                output.CountyName = _lookupCounty?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCityDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Cities_Create)]
        protected virtual async Task Create(CreateOrEditCityDto input)
        {
            var city = ObjectMapper.Map<City>(input);

            if (AbpSession.TenantId != null)
            {
                city.TenantId = (int?)AbpSession.TenantId;
            }

            await _cityRepository.InsertAsync(city);

        }

        [AbpAuthorize(AppPermissions.Pages_Cities_Edit)]
        protected virtual async Task Update(CreateOrEditCityDto input)
        {
            var city = await _cityRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, city);

        }

        [AbpAuthorize(AppPermissions.Pages_Cities_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _cityRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCitiesToExcel(GetAllCitiesForExcelInput input)
        {

            var filteredCities = _cityRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .Include(e => e.StateFk)
                        .Include(e => e.CountyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountyNameFilter), e => e.CountyFk != null && e.CountyFk.Name == input.CountyNameFilter);

            var query = (from o in filteredCities
                         join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_stateRepository.GetAll() on o.StateId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_countyRepository.GetAll() on o.CountyId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetCityForViewDto()
                         {
                             City = new CityDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             },
                             CountryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             StateName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             CountyName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var cityListDtos = await query.ToListAsync();

            return _citiesExcelExporter.ExportToFile(cityListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Cities)]
        public async Task<List<CityCountryLookupTableDto>> GetAllCountryForTableDropdown()
        {
            return await _lookup_countryRepository.GetAll()
                .Select(country => new CityCountryLookupTableDto
                {
                    Id = country.Id,
                    DisplayName = country == null || country.Name == null ? "" : country.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Cities)]
        public async Task<List<CityStateLookupTableDto>> GetAllStateForTableDropdown()
        {
            return await _lookup_stateRepository.GetAll()
                .Select(state => new CityStateLookupTableDto
                {
                    Id = state.Id,
                    DisplayName = state == null || state.Name == null ? "" : state.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Cities)]
        public async Task<List<CityCountyLookupTableDto>> GetAllCountyForTableDropdown()
        {
            return await _lookup_countyRepository.GetAll()
                .Select(county => new CityCountyLookupTableDto
                {
                    Id = county.Id,
                    DisplayName = county == null || county.Name == null ? "" : county.Name.ToString()
                }).ToListAsync();
        }

        public async Task<List<HubCityLookupTableDto>> GetAllCityForTableDropdown(long countryId,long? stateId,long? countyId)
        {
            return await _cityRepository.GetAll().Where(e=>e.CountryId==countryId)
                .WhereIf(stateId!=null,e=>e.StateId==stateId)
                .WhereIf(countyId!=null,e=>e.CountyId==countyId)
                .Select(city => new HubCityLookupTableDto
                {
                    Id = city.Id,
                    DisplayName = city == null || city.Name == null ? "" : city.Name.ToString()
                }).ToListAsync();
        }

    }
}