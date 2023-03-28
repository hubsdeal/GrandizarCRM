using SoftGrid.Territory;
using SoftGrid.Shop;
using SoftGrid.Shop;
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

namespace SoftGrid.Territory
{
    [AbpAuthorize(AppPermissions.Pages_HubSalesProjections)]
    public class HubSalesProjectionsAppService : SoftGridAppServiceBase, IHubSalesProjectionsAppService
    {
        private readonly IRepository<HubSalesProjection, long> _hubSalesProjectionRepository;
        private readonly IHubSalesProjectionsExcelExporter _hubSalesProjectionsExcelExporter;
        private readonly IRepository<Hub, long> _lookup_hubRepository;
        private readonly IRepository<ProductCategory, long> _lookup_productCategoryRepository;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<Currency, long> _lookup_currencyRepository;

        public HubSalesProjectionsAppService(IRepository<HubSalesProjection, long> hubSalesProjectionRepository, IHubSalesProjectionsExcelExporter hubSalesProjectionsExcelExporter, IRepository<Hub, long> lookup_hubRepository, IRepository<ProductCategory, long> lookup_productCategoryRepository, IRepository<Store, long> lookup_storeRepository, IRepository<Currency, long> lookup_currencyRepository)
        {
            _hubSalesProjectionRepository = hubSalesProjectionRepository;
            _hubSalesProjectionsExcelExporter = hubSalesProjectionsExcelExporter;
            _lookup_hubRepository = lookup_hubRepository;
            _lookup_productCategoryRepository = lookup_productCategoryRepository;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_currencyRepository = lookup_currencyRepository;

        }

        public async Task<PagedResultDto<GetHubSalesProjectionForViewDto>> GetAll(GetAllHubSalesProjectionsInput input)
        {

            var filteredHubSalesProjections = _hubSalesProjectionRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.ProductCategoryFk)
                        .Include(e => e.StoreFk)
                        .Include(e => e.CurrencyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDurationTypeIdFilter != null, e => e.DurationTypeId >= input.MinDurationTypeIdFilter)
                        .WhereIf(input.MaxDurationTypeIdFilter != null, e => e.DurationTypeId <= input.MaxDurationTypeIdFilter)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(input.MinExpectedSalesAmountFilter != null, e => e.ExpectedSalesAmount >= input.MinExpectedSalesAmountFilter)
                        .WhereIf(input.MaxExpectedSalesAmountFilter != null, e => e.ExpectedSalesAmount <= input.MaxExpectedSalesAmountFilter)
                        .WhereIf(input.MinActualSalesAmountFilter != null, e => e.ActualSalesAmount >= input.MinActualSalesAmountFilter)
                        .WhereIf(input.MaxActualSalesAmountFilter != null, e => e.ActualSalesAmount <= input.MaxActualSalesAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyNameFilter), e => e.CurrencyFk != null && e.CurrencyFk.Name == input.CurrencyNameFilter);

            var pagedAndFilteredHubSalesProjections = filteredHubSalesProjections
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var hubSalesProjections = from o in pagedAndFilteredHubSalesProjections
                                      join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                                      from s1 in j1.DefaultIfEmpty()

                                      join o2 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o2.Id into j2
                                      from s2 in j2.DefaultIfEmpty()

                                      join o3 in _lookup_storeRepository.GetAll() on o.StoreId equals o3.Id into j3
                                      from s3 in j3.DefaultIfEmpty()

                                      join o4 in _lookup_currencyRepository.GetAll() on o.CurrencyId equals o4.Id into j4
                                      from s4 in j4.DefaultIfEmpty()

                                      select new
                                      {

                                          o.DurationTypeId,
                                          o.StartDate,
                                          o.EndDate,
                                          o.ExpectedSalesAmount,
                                          o.ActualSalesAmount,
                                          Id = o.Id,
                                          HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                          ProductCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                          StoreName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                                          CurrencyName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                                      };

            var totalCount = await filteredHubSalesProjections.CountAsync();

            var dbList = await hubSalesProjections.ToListAsync();
            var results = new List<GetHubSalesProjectionForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetHubSalesProjectionForViewDto()
                {
                    HubSalesProjection = new HubSalesProjectionDto
                    {

                        DurationTypeId = o.DurationTypeId,
                        StartDate = o.StartDate,
                        EndDate = o.EndDate,
                        ExpectedSalesAmount = o.ExpectedSalesAmount,
                        ActualSalesAmount = o.ActualSalesAmount,
                        Id = o.Id,
                    },
                    HubName = o.HubName,
                    ProductCategoryName = o.ProductCategoryName,
                    StoreName = o.StoreName,
                    CurrencyName = o.CurrencyName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetHubSalesProjectionForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetHubSalesProjectionForViewDto> GetHubSalesProjectionForView(long id)
        {
            var hubSalesProjection = await _hubSalesProjectionRepository.GetAsync(id);

            var output = new GetHubSalesProjectionForViewDto { HubSalesProjection = ObjectMapper.Map<HubSalesProjectionDto>(hubSalesProjection) };

            if (output.HubSalesProjection.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.HubSalesProjection.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.HubSalesProjection.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.HubSalesProjection.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            if (output.HubSalesProjection.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.HubSalesProjection.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.HubSalesProjection.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((long)output.HubSalesProjection.CurrencyId);
                output.CurrencyName = _lookupCurrency?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_HubSalesProjections_Edit)]
        public async Task<GetHubSalesProjectionForEditOutput> GetHubSalesProjectionForEdit(EntityDto<long> input)
        {
            var hubSalesProjection = await _hubSalesProjectionRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHubSalesProjectionForEditOutput { HubSalesProjection = ObjectMapper.Map<CreateOrEditHubSalesProjectionDto>(hubSalesProjection) };

            if (output.HubSalesProjection.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.HubSalesProjection.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.HubSalesProjection.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.HubSalesProjection.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            if (output.HubSalesProjection.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.HubSalesProjection.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.HubSalesProjection.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((long)output.HubSalesProjection.CurrencyId);
                output.CurrencyName = _lookupCurrency?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHubSalesProjectionDto input)
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

        [AbpAuthorize(AppPermissions.Pages_HubSalesProjections_Create)]
        protected virtual async Task Create(CreateOrEditHubSalesProjectionDto input)
        {
            var hubSalesProjection = ObjectMapper.Map<HubSalesProjection>(input);

            if (AbpSession.TenantId != null)
            {
                hubSalesProjection.TenantId = (int?)AbpSession.TenantId;
            }

            await _hubSalesProjectionRepository.InsertAsync(hubSalesProjection);

        }

        [AbpAuthorize(AppPermissions.Pages_HubSalesProjections_Edit)]
        protected virtual async Task Update(CreateOrEditHubSalesProjectionDto input)
        {
            var hubSalesProjection = await _hubSalesProjectionRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, hubSalesProjection);

        }

        [AbpAuthorize(AppPermissions.Pages_HubSalesProjections_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _hubSalesProjectionRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetHubSalesProjectionsToExcel(GetAllHubSalesProjectionsForExcelInput input)
        {

            var filteredHubSalesProjections = _hubSalesProjectionRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.ProductCategoryFk)
                        .Include(e => e.StoreFk)
                        .Include(e => e.CurrencyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDurationTypeIdFilter != null, e => e.DurationTypeId >= input.MinDurationTypeIdFilter)
                        .WhereIf(input.MaxDurationTypeIdFilter != null, e => e.DurationTypeId <= input.MaxDurationTypeIdFilter)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(input.MinExpectedSalesAmountFilter != null, e => e.ExpectedSalesAmount >= input.MinExpectedSalesAmountFilter)
                        .WhereIf(input.MaxExpectedSalesAmountFilter != null, e => e.ExpectedSalesAmount <= input.MaxExpectedSalesAmountFilter)
                        .WhereIf(input.MinActualSalesAmountFilter != null, e => e.ActualSalesAmount >= input.MinActualSalesAmountFilter)
                        .WhereIf(input.MaxActualSalesAmountFilter != null, e => e.ActualSalesAmount <= input.MaxActualSalesAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyNameFilter), e => e.CurrencyFk != null && e.CurrencyFk.Name == input.CurrencyNameFilter);

            var query = (from o in filteredHubSalesProjections
                         join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_storeRepository.GetAll() on o.StoreId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_currencyRepository.GetAll() on o.CurrencyId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetHubSalesProjectionForViewDto()
                         {
                             HubSalesProjection = new HubSalesProjectionDto
                             {
                                 DurationTypeId = o.DurationTypeId,
                                 StartDate = o.StartDate,
                                 EndDate = o.EndDate,
                                 ExpectedSalesAmount = o.ExpectedSalesAmount,
                                 ActualSalesAmount = o.ActualSalesAmount,
                                 Id = o.Id
                             },
                             HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ProductCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             StoreName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             CurrencyName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                         });

            var hubSalesProjectionListDtos = await query.ToListAsync();

            return _hubSalesProjectionsExcelExporter.ExportToFile(hubSalesProjectionListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_HubSalesProjections)]
        public async Task<PagedResultDto<HubSalesProjectionHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_hubRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var hubList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubSalesProjectionHubLookupTableDto>();
            foreach (var hub in hubList)
            {
                lookupTableDtoList.Add(new HubSalesProjectionHubLookupTableDto
                {
                    Id = hub.Id,
                    DisplayName = hub.Name?.ToString()
                });
            }

            return new PagedResultDto<HubSalesProjectionHubLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HubSalesProjections)]
        public async Task<PagedResultDto<HubSalesProjectionProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubSalesProjectionProductCategoryLookupTableDto>();
            foreach (var productCategory in productCategoryList)
            {
                lookupTableDtoList.Add(new HubSalesProjectionProductCategoryLookupTableDto
                {
                    Id = productCategory.Id,
                    DisplayName = productCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<HubSalesProjectionProductCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HubSalesProjections)]
        public async Task<PagedResultDto<HubSalesProjectionStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubSalesProjectionStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new HubSalesProjectionStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<HubSalesProjectionStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HubSalesProjections)]
        public async Task<PagedResultDto<HubSalesProjectionCurrencyLookupTableDto>> GetAllCurrencyForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_currencyRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var currencyList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubSalesProjectionCurrencyLookupTableDto>();
            foreach (var currency in currencyList)
            {
                lookupTableDtoList.Add(new HubSalesProjectionCurrencyLookupTableDto
                {
                    Id = currency.Id,
                    DisplayName = currency.Name?.ToString()
                });
            }

            return new PagedResultDto<HubSalesProjectionCurrencyLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}