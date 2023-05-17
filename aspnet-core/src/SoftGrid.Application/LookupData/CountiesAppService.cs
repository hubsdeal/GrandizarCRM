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
    [AbpAuthorize(AppPermissions.Pages_Counties)]
    public class CountiesAppService : SoftGridAppServiceBase, ICountiesAppService
    {
        private readonly IRepository<County, long> _countyRepository;
        private readonly ICountiesExcelExporter _countiesExcelExporter;
        private readonly IRepository<Country, long> _lookup_countryRepository;
        private readonly IRepository<State, long> _lookup_stateRepository;

        public CountiesAppService(IRepository<County, long> countyRepository, ICountiesExcelExporter countiesExcelExporter, IRepository<Country, long> lookup_countryRepository, IRepository<State, long> lookup_stateRepository)
        {
            _countyRepository = countyRepository;
            _countiesExcelExporter = countiesExcelExporter;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_stateRepository = lookup_stateRepository;

        }

        public async Task<PagedResultDto<GetCountyForViewDto>> GetAll(GetAllCountiesInput input)
        {

            var filteredCounties = _countyRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .Include(e => e.StateFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter);

            var pagedAndFilteredCounties = filteredCounties
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var counties = from o in pagedAndFilteredCounties
                           join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()

                           join o2 in _lookup_stateRepository.GetAll() on o.StateId equals o2.Id into j2
                           from s2 in j2.DefaultIfEmpty()

                           select new
                           {

                               o.Name,
                               Id = o.Id,
                               CountryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                               StateName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                           };

            var totalCount = await filteredCounties.CountAsync();

            var dbList = await counties.ToListAsync();
            var results = new List<GetCountyForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCountyForViewDto()
                {
                    County = new CountyDto
                    {

                        Name = o.Name,
                        Id = o.Id,
                    },
                    CountryName = o.CountryName,
                    StateName = o.StateName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCountyForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCountyForViewDto> GetCountyForView(long id)
        {
            var county = await _countyRepository.GetAsync(id);

            var output = new GetCountyForViewDto { County = ObjectMapper.Map<CountyDto>(county) };

            if (output.County.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.County.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.County.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.County.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Counties_Edit)]
        public async Task<GetCountyForEditOutput> GetCountyForEdit(EntityDto<long> input)
        {
            var county = await _countyRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCountyForEditOutput { County = ObjectMapper.Map<CreateOrEditCountyDto>(county) };

            if (output.County.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.County.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.County.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.County.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCountyDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Counties_Create)]
        protected virtual async Task Create(CreateOrEditCountyDto input)
        {
            var county = ObjectMapper.Map<County>(input);

            if (AbpSession.TenantId != null)
            {
                county.TenantId = (int?)AbpSession.TenantId;
            }

            await _countyRepository.InsertAsync(county);

        }

        [AbpAuthorize(AppPermissions.Pages_Counties_Edit)]
        protected virtual async Task Update(CreateOrEditCountyDto input)
        {
            var county = await _countyRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, county);

        }

        [AbpAuthorize(AppPermissions.Pages_Counties_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _countyRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCountiesToExcel(GetAllCountiesForExcelInput input)
        {

            var filteredCounties = _countyRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .Include(e => e.StateFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter);

            var query = (from o in filteredCounties
                         join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_stateRepository.GetAll() on o.StateId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetCountyForViewDto()
                         {
                             County = new CountyDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             },
                             CountryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             StateName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var countyListDtos = await query.ToListAsync();

            return _countiesExcelExporter.ExportToFile(countyListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Counties)]
        public async Task<List<CountyCountryLookupTableDto>> GetAllCountryForTableDropdown()
        {
            return await _lookup_countryRepository.GetAll()
                .Select(country => new CountyCountryLookupTableDto
                {
                    Id = country.Id,
                    DisplayName = country == null || country.Name == null ? "" : country.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Counties)]
        public async Task<List<CountyStateLookupTableDto>> GetAllStateForTableDropdown()
        {
            return await _lookup_stateRepository.GetAll()
                .Select(state => new CountyStateLookupTableDto
                {
                    Id = state.Id,
                    DisplayName = state == null || state.Name == null ? "" : state.Name.ToString()
                }).ToListAsync();
        }

        public async Task<List<HubCountyLookupTableDto>> GetAllCountyForTableDropdown(long countryId,long? stateId)
        {
            return await _countyRepository.GetAll().Where(e=>e.CountryId==countryId).WhereIf(stateId!=null,e=>e.StateId==stateId)
                .Select(county => new HubCountyLookupTableDto
                {
                    Id = county.Id,
                    DisplayName = county == null || county.Name == null ? "" : county.Name.ToString()
                }).ToListAsync();
        }

    }
}