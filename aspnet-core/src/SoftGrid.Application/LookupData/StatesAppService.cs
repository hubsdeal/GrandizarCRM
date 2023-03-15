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

namespace SoftGrid.LookupData
{
    [AbpAuthorize(AppPermissions.Pages_States)]
    public class StatesAppService : SoftGridAppServiceBase, IStatesAppService
    {
        private readonly IRepository<State, long> _stateRepository;
        private readonly IStatesExcelExporter _statesExcelExporter;
        private readonly IRepository<Country, long> _lookup_countryRepository;

        public StatesAppService(IRepository<State, long> stateRepository, IStatesExcelExporter statesExcelExporter, IRepository<Country, long> lookup_countryRepository)
        {
            _stateRepository = stateRepository;
            _statesExcelExporter = statesExcelExporter;
            _lookup_countryRepository = lookup_countryRepository;

        }

        public async Task<PagedResultDto<GetStateForViewDto>> GetAll(GetAllStatesInput input)
        {

            var filteredStates = _stateRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Ticker.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TickerFilter), e => e.Ticker.Contains(input.TickerFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter);

            var pagedAndFilteredStates = filteredStates
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var states = from o in pagedAndFilteredStates
                         join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new
                         {

                             o.Name,
                             o.Ticker,
                             Id = o.Id,
                             CountryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         };

            var totalCount = await filteredStates.CountAsync();

            var dbList = await states.ToListAsync();
            var results = new List<GetStateForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStateForViewDto()
                {
                    State = new StateDto
                    {

                        Name = o.Name,
                        Ticker = o.Ticker,
                        Id = o.Id,
                    },
                    CountryName = o.CountryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStateForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStateForViewDto> GetStateForView(long id)
        {
            var state = await _stateRepository.GetAsync(id);

            var output = new GetStateForViewDto { State = ObjectMapper.Map<StateDto>(state) };

            if (output.State.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.State.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_States_Edit)]
        public async Task<GetStateForEditOutput> GetStateForEdit(EntityDto<long> input)
        {
            var state = await _stateRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStateForEditOutput { State = ObjectMapper.Map<CreateOrEditStateDto>(state) };

            if (output.State.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.State.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStateDto input)
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

        [AbpAuthorize(AppPermissions.Pages_States_Create)]
        protected virtual async Task Create(CreateOrEditStateDto input)
        {
            var state = ObjectMapper.Map<State>(input);

            if (AbpSession.TenantId != null)
            {
                state.TenantId = (int?)AbpSession.TenantId;
            }

            await _stateRepository.InsertAsync(state);

        }

        [AbpAuthorize(AppPermissions.Pages_States_Edit)]
        protected virtual async Task Update(CreateOrEditStateDto input)
        {
            var state = await _stateRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, state);

        }

        [AbpAuthorize(AppPermissions.Pages_States_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _stateRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStatesToExcel(GetAllStatesForExcelInput input)
        {

            var filteredStates = _stateRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Ticker.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TickerFilter), e => e.Ticker.Contains(input.TickerFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter);

            var query = (from o in filteredStates
                         join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetStateForViewDto()
                         {
                             State = new StateDto
                             {
                                 Name = o.Name,
                                 Ticker = o.Ticker,
                                 Id = o.Id
                             },
                             CountryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var stateListDtos = await query.ToListAsync();

            return _statesExcelExporter.ExportToFile(stateListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_States)]
        public async Task<List<StateCountryLookupTableDto>> GetAllCountryForTableDropdown()
        {
            return await _lookup_countryRepository.GetAll()
                .Select(country => new StateCountryLookupTableDto
                {
                    Id = country.Id,
                    DisplayName = country == null || country.Name == null ? "" : country.Name.ToString()
                }).ToListAsync();
        }

    }
}