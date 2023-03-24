using SoftGrid.Shop;
using SoftGrid.CRM;

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
    [AbpAuthorize(AppPermissions.Pages_StoreBusinessCustomerMaps)]
    public class StoreBusinessCustomerMapsAppService : SoftGridAppServiceBase, IStoreBusinessCustomerMapsAppService
    {
        private readonly IRepository<StoreBusinessCustomerMap> _storeBusinessCustomerMapRepository;
        private readonly IStoreBusinessCustomerMapsExcelExporter _storeBusinessCustomerMapsExcelExporter;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<Business, long> _lookup_businessRepository;

        public StoreBusinessCustomerMapsAppService(IRepository<StoreBusinessCustomerMap> storeBusinessCustomerMapRepository, IStoreBusinessCustomerMapsExcelExporter storeBusinessCustomerMapsExcelExporter, IRepository<Store, long> lookup_storeRepository, IRepository<Business, long> lookup_businessRepository)
        {
            _storeBusinessCustomerMapRepository = storeBusinessCustomerMapRepository;
            _storeBusinessCustomerMapsExcelExporter = storeBusinessCustomerMapsExcelExporter;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_businessRepository = lookup_businessRepository;

        }

        public async Task<PagedResultDto<GetStoreBusinessCustomerMapForViewDto>> GetAll(GetAllStoreBusinessCustomerMapsInput input)
        {

            var filteredStoreBusinessCustomerMaps = _storeBusinessCustomerMapRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.BusinessFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PaidCustomerFilter.HasValue && input.PaidCustomerFilter > -1, e => (input.PaidCustomerFilter == 1 && e.PaidCustomer) || (input.PaidCustomerFilter == 0 && !e.PaidCustomer))
                        .WhereIf(input.MinLifeTimeSalesAmountFilter != null, e => e.LifeTimeSalesAmount >= input.MinLifeTimeSalesAmountFilter)
                        .WhereIf(input.MaxLifeTimeSalesAmountFilter != null, e => e.LifeTimeSalesAmount <= input.MaxLifeTimeSalesAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter);

            var pagedAndFilteredStoreBusinessCustomerMaps = filteredStoreBusinessCustomerMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeBusinessCustomerMaps = from o in pagedAndFilteredStoreBusinessCustomerMaps
                                            join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                                            from s1 in j1.DefaultIfEmpty()

                                            join o2 in _lookup_businessRepository.GetAll() on o.BusinessId equals o2.Id into j2
                                            from s2 in j2.DefaultIfEmpty()

                                            select new
                                            {

                                                o.PaidCustomer,
                                                o.LifeTimeSalesAmount,
                                                Id = o.Id,
                                                StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                                BusinessName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                            };

            var totalCount = await filteredStoreBusinessCustomerMaps.CountAsync();

            var dbList = await storeBusinessCustomerMaps.ToListAsync();
            var results = new List<GetStoreBusinessCustomerMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreBusinessCustomerMapForViewDto()
                {
                    StoreBusinessCustomerMap = new StoreBusinessCustomerMapDto
                    {

                        PaidCustomer = o.PaidCustomer,
                        LifeTimeSalesAmount = o.LifeTimeSalesAmount,
                        Id = o.Id,
                    },
                    StoreName = o.StoreName,
                    BusinessName = o.BusinessName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreBusinessCustomerMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreBusinessCustomerMapForViewDto> GetStoreBusinessCustomerMapForView(int id)
        {
            var storeBusinessCustomerMap = await _storeBusinessCustomerMapRepository.GetAsync(id);

            var output = new GetStoreBusinessCustomerMapForViewDto { StoreBusinessCustomerMap = ObjectMapper.Map<StoreBusinessCustomerMapDto>(storeBusinessCustomerMap) };

            if (output.StoreBusinessCustomerMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreBusinessCustomerMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreBusinessCustomerMap.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.StoreBusinessCustomerMap.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreBusinessCustomerMaps_Edit)]
        public async Task<GetStoreBusinessCustomerMapForEditOutput> GetStoreBusinessCustomerMapForEdit(EntityDto input)
        {
            var storeBusinessCustomerMap = await _storeBusinessCustomerMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreBusinessCustomerMapForEditOutput { StoreBusinessCustomerMap = ObjectMapper.Map<CreateOrEditStoreBusinessCustomerMapDto>(storeBusinessCustomerMap) };

            if (output.StoreBusinessCustomerMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreBusinessCustomerMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreBusinessCustomerMap.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.StoreBusinessCustomerMap.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreBusinessCustomerMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreBusinessCustomerMaps_Create)]
        protected virtual async Task Create(CreateOrEditStoreBusinessCustomerMapDto input)
        {
            var storeBusinessCustomerMap = ObjectMapper.Map<StoreBusinessCustomerMap>(input);

            if (AbpSession.TenantId != null)
            {
                storeBusinessCustomerMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeBusinessCustomerMapRepository.InsertAsync(storeBusinessCustomerMap);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreBusinessCustomerMaps_Edit)]
        protected virtual async Task Update(CreateOrEditStoreBusinessCustomerMapDto input)
        {
            var storeBusinessCustomerMap = await _storeBusinessCustomerMapRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, storeBusinessCustomerMap);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreBusinessCustomerMaps_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _storeBusinessCustomerMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreBusinessCustomerMapsToExcel(GetAllStoreBusinessCustomerMapsForExcelInput input)
        {

            var filteredStoreBusinessCustomerMaps = _storeBusinessCustomerMapRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.BusinessFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PaidCustomerFilter.HasValue && input.PaidCustomerFilter > -1, e => (input.PaidCustomerFilter == 1 && e.PaidCustomer) || (input.PaidCustomerFilter == 0 && !e.PaidCustomer))
                        .WhereIf(input.MinLifeTimeSalesAmountFilter != null, e => e.LifeTimeSalesAmount >= input.MinLifeTimeSalesAmountFilter)
                        .WhereIf(input.MaxLifeTimeSalesAmountFilter != null, e => e.LifeTimeSalesAmount <= input.MaxLifeTimeSalesAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter);

            var query = (from o in filteredStoreBusinessCustomerMaps
                         join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_businessRepository.GetAll() on o.BusinessId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetStoreBusinessCustomerMapForViewDto()
                         {
                             StoreBusinessCustomerMap = new StoreBusinessCustomerMapDto
                             {
                                 PaidCustomer = o.PaidCustomer,
                                 LifeTimeSalesAmount = o.LifeTimeSalesAmount,
                                 Id = o.Id
                             },
                             StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             BusinessName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var storeBusinessCustomerMapListDtos = await query.ToListAsync();

            return _storeBusinessCustomerMapsExcelExporter.ExportToFile(storeBusinessCustomerMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreBusinessCustomerMaps)]
        public async Task<PagedResultDto<StoreBusinessCustomerMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreBusinessCustomerMapStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreBusinessCustomerMapStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreBusinessCustomerMapStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreBusinessCustomerMaps)]
        public async Task<PagedResultDto<StoreBusinessCustomerMapBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_businessRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var businessList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreBusinessCustomerMapBusinessLookupTableDto>();
            foreach (var business in businessList)
            {
                lookupTableDtoList.Add(new StoreBusinessCustomerMapBusinessLookupTableDto
                {
                    Id = business.Id,
                    DisplayName = business.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreBusinessCustomerMapBusinessLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}