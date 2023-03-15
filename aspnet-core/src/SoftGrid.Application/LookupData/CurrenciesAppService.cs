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
    [AbpAuthorize(AppPermissions.Pages_Currencies)]
    public class CurrenciesAppService : SoftGridAppServiceBase, ICurrenciesAppService
    {
        private readonly IRepository<Currency, long> _currencyRepository;
        private readonly ICurrenciesExcelExporter _currenciesExcelExporter;

        public CurrenciesAppService(IRepository<Currency, long> currencyRepository, ICurrenciesExcelExporter currenciesExcelExporter)
        {
            _currencyRepository = currencyRepository;
            _currenciesExcelExporter = currenciesExcelExporter;

        }

        public async Task<PagedResultDto<GetCurrencyForViewDto>> GetAll(GetAllCurrenciesInput input)
        {

            var filteredCurrencies = _currencyRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Ticker.Contains(input.Filter) || e.Icon.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TickerFilter), e => e.Ticker.Contains(input.TickerFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IconFilter), e => e.Icon.Contains(input.IconFilter));

            var pagedAndFilteredCurrencies = filteredCurrencies
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var currencies = from o in pagedAndFilteredCurrencies
                             select new
                             {

                                 o.Name,
                                 o.Ticker,
                                 o.Icon,
                                 Id = o.Id
                             };

            var totalCount = await filteredCurrencies.CountAsync();

            var dbList = await currencies.ToListAsync();
            var results = new List<GetCurrencyForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCurrencyForViewDto()
                {
                    Currency = new CurrencyDto
                    {

                        Name = o.Name,
                        Ticker = o.Ticker,
                        Icon = o.Icon,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCurrencyForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCurrencyForViewDto> GetCurrencyForView(long id)
        {
            var currency = await _currencyRepository.GetAsync(id);

            var output = new GetCurrencyForViewDto { Currency = ObjectMapper.Map<CurrencyDto>(currency) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Currencies_Edit)]
        public async Task<GetCurrencyForEditOutput> GetCurrencyForEdit(EntityDto<long> input)
        {
            var currency = await _currencyRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCurrencyForEditOutput { Currency = ObjectMapper.Map<CreateOrEditCurrencyDto>(currency) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCurrencyDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Currencies_Create)]
        protected virtual async Task Create(CreateOrEditCurrencyDto input)
        {
            var currency = ObjectMapper.Map<Currency>(input);

            if (AbpSession.TenantId != null)
            {
                currency.TenantId = (int?)AbpSession.TenantId;
            }

            await _currencyRepository.InsertAsync(currency);

        }

        [AbpAuthorize(AppPermissions.Pages_Currencies_Edit)]
        protected virtual async Task Update(CreateOrEditCurrencyDto input)
        {
            var currency = await _currencyRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, currency);

        }

        [AbpAuthorize(AppPermissions.Pages_Currencies_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _currencyRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCurrenciesToExcel(GetAllCurrenciesForExcelInput input)
        {

            var filteredCurrencies = _currencyRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Ticker.Contains(input.Filter) || e.Icon.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TickerFilter), e => e.Ticker.Contains(input.TickerFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IconFilter), e => e.Icon.Contains(input.IconFilter));

            var query = (from o in filteredCurrencies
                         select new GetCurrencyForViewDto()
                         {
                             Currency = new CurrencyDto
                             {
                                 Name = o.Name,
                                 Ticker = o.Ticker,
                                 Icon = o.Icon,
                                 Id = o.Id
                             }
                         });

            var currencyListDtos = await query.ToListAsync();

            return _currenciesExcelExporter.ExportToFile(currencyListDtos);
        }

    }
}