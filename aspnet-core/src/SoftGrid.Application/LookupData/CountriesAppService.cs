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

namespace SoftGrid.LookupData
{
    [AbpAuthorize(AppPermissions.Pages_Countries)]
    public class CountriesAppService : SoftGridAppServiceBase, ICountriesAppService
    {
        private readonly IRepository<Country, long> _countryRepository;
        private readonly ICountriesExcelExporter _countriesExcelExporter;

        public CountriesAppService(IRepository<Country, long> countryRepository, ICountriesExcelExporter countriesExcelExporter)
        {
            _countryRepository = countryRepository;
            _countriesExcelExporter = countriesExcelExporter;

        }

        public async Task<PagedResultDto<GetCountryForViewDto>> GetAll(GetAllCountriesInput input)
        {

            var filteredCountries = _countryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Ticker.Contains(input.Filter) || e.FlagIcon.Contains(input.Filter) || e.PhoneCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TickerFilter), e => e.Ticker.Contains(input.TickerFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FlagIconFilter), e => e.FlagIcon.Contains(input.FlagIconFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneCodeFilter), e => e.PhoneCode.Contains(input.PhoneCodeFilter));

            var pagedAndFilteredCountries = filteredCountries
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var countries = from o in pagedAndFilteredCountries
                            select new
                            {

                                o.Name,
                                o.Ticker,
                                o.FlagIcon,
                                o.PhoneCode,
                                Id = o.Id
                            };

            var totalCount = await filteredCountries.CountAsync();

            var dbList = await countries.ToListAsync();
            var results = new List<GetCountryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCountryForViewDto()
                {
                    Country = new CountryDto
                    {

                        Name = o.Name,
                        Ticker = o.Ticker,
                        FlagIcon = o.FlagIcon,
                        PhoneCode = o.PhoneCode,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCountryForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCountryForViewDto> GetCountryForView(long id)
        {
            var country = await _countryRepository.GetAsync(id);

            var output = new GetCountryForViewDto { Country = ObjectMapper.Map<CountryDto>(country) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Countries_Edit)]
        public async Task<GetCountryForEditOutput> GetCountryForEdit(EntityDto<long> input)
        {
            var country = await _countryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCountryForEditOutput { Country = ObjectMapper.Map<CreateOrEditCountryDto>(country) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCountryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Countries_Create)]
        protected virtual async Task Create(CreateOrEditCountryDto input)
        {
            var country = ObjectMapper.Map<Country>(input);

            if (AbpSession.TenantId != null)
            {
                country.TenantId = (int?)AbpSession.TenantId;
            }

            await _countryRepository.InsertAsync(country);

        }

        [AbpAuthorize(AppPermissions.Pages_Countries_Edit)]
        protected virtual async Task Update(CreateOrEditCountryDto input)
        {
            var country = await _countryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, country);

        }

        [AbpAuthorize(AppPermissions.Pages_Countries_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _countryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCountriesToExcel(GetAllCountriesForExcelInput input)
        {

            var filteredCountries = _countryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Ticker.Contains(input.Filter) || e.FlagIcon.Contains(input.Filter) || e.PhoneCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TickerFilter), e => e.Ticker.Contains(input.TickerFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FlagIconFilter), e => e.FlagIcon.Contains(input.FlagIconFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneCodeFilter), e => e.PhoneCode.Contains(input.PhoneCodeFilter));

            var query = (from o in filteredCountries
                         select new GetCountryForViewDto()
                         {
                             Country = new CountryDto
                             {
                                 Name = o.Name,
                                 Ticker = o.Ticker,
                                 FlagIcon = o.FlagIcon,
                                 PhoneCode = o.PhoneCode,
                                 Id = o.Id
                             }
                         });

            var countryListDtos = await query.ToListAsync();

            return _countriesExcelExporter.ExportToFile(countryListDtos);
        }

    }
}