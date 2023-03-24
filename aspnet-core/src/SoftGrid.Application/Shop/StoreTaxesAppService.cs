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
    [AbpAuthorize(AppPermissions.Pages_StoreTaxes)]
    public class StoreTaxesAppService : SoftGridAppServiceBase, IStoreTaxesAppService
    {
        private readonly IRepository<StoreTax, long> _storeTaxRepository;
        private readonly IStoreTaxesExcelExporter _storeTaxesExcelExporter;
        private readonly IRepository<Store, long> _lookup_storeRepository;

        public StoreTaxesAppService(IRepository<StoreTax, long> storeTaxRepository, IStoreTaxesExcelExporter storeTaxesExcelExporter, IRepository<Store, long> lookup_storeRepository)
        {
            _storeTaxRepository = storeTaxRepository;
            _storeTaxesExcelExporter = storeTaxesExcelExporter;
            _lookup_storeRepository = lookup_storeRepository;

        }

        public async Task<PagedResultDto<GetStoreTaxForViewDto>> GetAll(GetAllStoreTaxesInput input)
        {

            var filteredStoreTaxes = _storeTaxRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TaxName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxNameFilter), e => e.TaxName.Contains(input.TaxNameFilter))
                        .WhereIf(input.PercentageOrAmountFilter.HasValue && input.PercentageOrAmountFilter > -1, e => (input.PercentageOrAmountFilter == 1 && e.PercentageOrAmount) || (input.PercentageOrAmountFilter == 0 && !e.PercentageOrAmount))
                        .WhereIf(input.MinTaxRatePercentageFilter != null, e => e.TaxRatePercentage >= input.MinTaxRatePercentageFilter)
                        .WhereIf(input.MaxTaxRatePercentageFilter != null, e => e.TaxRatePercentage <= input.MaxTaxRatePercentageFilter)
                        .WhereIf(input.MinTaxAmountFilter != null, e => e.TaxAmount >= input.MinTaxAmountFilter)
                        .WhereIf(input.MaxTaxAmountFilter != null, e => e.TaxAmount <= input.MaxTaxAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var pagedAndFilteredStoreTaxes = filteredStoreTaxes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeTaxes = from o in pagedAndFilteredStoreTaxes
                             join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             select new
                             {

                                 o.TaxName,
                                 o.PercentageOrAmount,
                                 o.TaxRatePercentage,
                                 o.TaxAmount,
                                 Id = o.Id,
                                 StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                             };

            var totalCount = await filteredStoreTaxes.CountAsync();

            var dbList = await storeTaxes.ToListAsync();
            var results = new List<GetStoreTaxForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreTaxForViewDto()
                {
                    StoreTax = new StoreTaxDto
                    {

                        TaxName = o.TaxName,
                        PercentageOrAmount = o.PercentageOrAmount,
                        TaxRatePercentage = o.TaxRatePercentage,
                        TaxAmount = o.TaxAmount,
                        Id = o.Id,
                    },
                    StoreName = o.StoreName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreTaxForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreTaxForViewDto> GetStoreTaxForView(long id)
        {
            var storeTax = await _storeTaxRepository.GetAsync(id);

            var output = new GetStoreTaxForViewDto { StoreTax = ObjectMapper.Map<StoreTaxDto>(storeTax) };

            if (output.StoreTax.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreTax.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreTaxes_Edit)]
        public async Task<GetStoreTaxForEditOutput> GetStoreTaxForEdit(EntityDto<long> input)
        {
            var storeTax = await _storeTaxRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreTaxForEditOutput { StoreTax = ObjectMapper.Map<CreateOrEditStoreTaxDto>(storeTax) };

            if (output.StoreTax.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreTax.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreTaxDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreTaxes_Create)]
        protected virtual async Task Create(CreateOrEditStoreTaxDto input)
        {
            var storeTax = ObjectMapper.Map<StoreTax>(input);

            if (AbpSession.TenantId != null)
            {
                storeTax.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeTaxRepository.InsertAsync(storeTax);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreTaxes_Edit)]
        protected virtual async Task Update(CreateOrEditStoreTaxDto input)
        {
            var storeTax = await _storeTaxRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeTax);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreTaxes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeTaxRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreTaxesToExcel(GetAllStoreTaxesForExcelInput input)
        {

            var filteredStoreTaxes = _storeTaxRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TaxName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxNameFilter), e => e.TaxName.Contains(input.TaxNameFilter))
                        .WhereIf(input.PercentageOrAmountFilter.HasValue && input.PercentageOrAmountFilter > -1, e => (input.PercentageOrAmountFilter == 1 && e.PercentageOrAmount) || (input.PercentageOrAmountFilter == 0 && !e.PercentageOrAmount))
                        .WhereIf(input.MinTaxRatePercentageFilter != null, e => e.TaxRatePercentage >= input.MinTaxRatePercentageFilter)
                        .WhereIf(input.MaxTaxRatePercentageFilter != null, e => e.TaxRatePercentage <= input.MaxTaxRatePercentageFilter)
                        .WhereIf(input.MinTaxAmountFilter != null, e => e.TaxAmount >= input.MinTaxAmountFilter)
                        .WhereIf(input.MaxTaxAmountFilter != null, e => e.TaxAmount <= input.MaxTaxAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var query = (from o in filteredStoreTaxes
                         join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetStoreTaxForViewDto()
                         {
                             StoreTax = new StoreTaxDto
                             {
                                 TaxName = o.TaxName,
                                 PercentageOrAmount = o.PercentageOrAmount,
                                 TaxRatePercentage = o.TaxRatePercentage,
                                 TaxAmount = o.TaxAmount,
                                 Id = o.Id
                             },
                             StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var storeTaxListDtos = await query.ToListAsync();

            return _storeTaxesExcelExporter.ExportToFile(storeTaxListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreTaxes)]
        public async Task<PagedResultDto<StoreTaxStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreTaxStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreTaxStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreTaxStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}