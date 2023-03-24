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
    [AbpAuthorize(AppPermissions.Pages_StoreContactMaps)]
    public class StoreContactMapsAppService : SoftGridAppServiceBase, IStoreContactMapsAppService
    {
        private readonly IRepository<StoreContactMap, long> _storeContactMapRepository;
        private readonly IStoreContactMapsExcelExporter _storeContactMapsExcelExporter;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<Contact, long> _lookup_contactRepository;

        public StoreContactMapsAppService(IRepository<StoreContactMap, long> storeContactMapRepository, IStoreContactMapsExcelExporter storeContactMapsExcelExporter, IRepository<Store, long> lookup_storeRepository, IRepository<Contact, long> lookup_contactRepository)
        {
            _storeContactMapRepository = storeContactMapRepository;
            _storeContactMapsExcelExporter = storeContactMapsExcelExporter;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_contactRepository = lookup_contactRepository;

        }

        public async Task<PagedResultDto<GetStoreContactMapForViewDto>> GetAll(GetAllStoreContactMapsInput input)
        {

            var filteredStoreContactMaps = _storeContactMapRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PaidCustomerFilter.HasValue && input.PaidCustomerFilter > -1, e => (input.PaidCustomerFilter == 1 && e.PaidCustomer) || (input.PaidCustomerFilter == 0 && !e.PaidCustomer))
                        .WhereIf(input.MinLifeTimeSalesAmountFilter != null, e => e.LifeTimeSalesAmount >= input.MinLifeTimeSalesAmountFilter)
                        .WhereIf(input.MaxLifeTimeSalesAmountFilter != null, e => e.LifeTimeSalesAmount <= input.MaxLifeTimeSalesAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter);

            var pagedAndFilteredStoreContactMaps = filteredStoreContactMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeContactMaps = from o in pagedAndFilteredStoreContactMaps
                                   join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                                   from s1 in j1.DefaultIfEmpty()

                                   join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                                   from s2 in j2.DefaultIfEmpty()

                                   select new
                                   {

                                       o.PaidCustomer,
                                       o.LifeTimeSalesAmount,
                                       Id = o.Id,
                                       StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                       ContactFullName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString()
                                   };

            var totalCount = await filteredStoreContactMaps.CountAsync();

            var dbList = await storeContactMaps.ToListAsync();
            var results = new List<GetStoreContactMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreContactMapForViewDto()
                {
                    StoreContactMap = new StoreContactMapDto
                    {

                        PaidCustomer = o.PaidCustomer,
                        LifeTimeSalesAmount = o.LifeTimeSalesAmount,
                        Id = o.Id,
                    },
                    StoreName = o.StoreName,
                    ContactFullName = o.ContactFullName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreContactMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreContactMapForViewDto> GetStoreContactMapForView(long id)
        {
            var storeContactMap = await _storeContactMapRepository.GetAsync(id);

            var output = new GetStoreContactMapForViewDto { StoreContactMap = ObjectMapper.Map<StoreContactMapDto>(storeContactMap) };

            if (output.StoreContactMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreContactMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreContactMap.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.StoreContactMap.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreContactMaps_Edit)]
        public async Task<GetStoreContactMapForEditOutput> GetStoreContactMapForEdit(EntityDto<long> input)
        {
            var storeContactMap = await _storeContactMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreContactMapForEditOutput { StoreContactMap = ObjectMapper.Map<CreateOrEditStoreContactMapDto>(storeContactMap) };

            if (output.StoreContactMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreContactMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreContactMap.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.StoreContactMap.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreContactMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreContactMaps_Create)]
        protected virtual async Task Create(CreateOrEditStoreContactMapDto input)
        {
            var storeContactMap = ObjectMapper.Map<StoreContactMap>(input);

            if (AbpSession.TenantId != null)
            {
                storeContactMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeContactMapRepository.InsertAsync(storeContactMap);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreContactMaps_Edit)]
        protected virtual async Task Update(CreateOrEditStoreContactMapDto input)
        {
            var storeContactMap = await _storeContactMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeContactMap);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreContactMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeContactMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreContactMapsToExcel(GetAllStoreContactMapsForExcelInput input)
        {

            var filteredStoreContactMaps = _storeContactMapRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PaidCustomerFilter.HasValue && input.PaidCustomerFilter > -1, e => (input.PaidCustomerFilter == 1 && e.PaidCustomer) || (input.PaidCustomerFilter == 0 && !e.PaidCustomer))
                        .WhereIf(input.MinLifeTimeSalesAmountFilter != null, e => e.LifeTimeSalesAmount >= input.MinLifeTimeSalesAmountFilter)
                        .WhereIf(input.MaxLifeTimeSalesAmountFilter != null, e => e.LifeTimeSalesAmount <= input.MaxLifeTimeSalesAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter);

            var query = (from o in filteredStoreContactMaps
                         join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetStoreContactMapForViewDto()
                         {
                             StoreContactMap = new StoreContactMapDto
                             {
                                 PaidCustomer = o.PaidCustomer,
                                 LifeTimeSalesAmount = o.LifeTimeSalesAmount,
                                 Id = o.Id
                             },
                             StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ContactFullName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString()
                         });

            var storeContactMapListDtos = await query.ToListAsync();

            return _storeContactMapsExcelExporter.ExportToFile(storeContactMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreContactMaps)]
        public async Task<PagedResultDto<StoreContactMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreContactMapStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreContactMapStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreContactMapStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreContactMaps)]
        public async Task<PagedResultDto<StoreContactMapContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreContactMapContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new StoreContactMapContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<StoreContactMapContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}