using SoftGrid.CRM;
using SoftGrid.Shop;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.CRM.Exporting;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.CRM
{
    [AbpAuthorize(AppPermissions.Pages_BusinessStoreMaps)]
    public class BusinessStoreMapsAppService : SoftGridAppServiceBase, IBusinessStoreMapsAppService
    {
        private readonly IRepository<BusinessStoreMap, long> _businessStoreMapRepository;
        private readonly IBusinessStoreMapsExcelExporter _businessStoreMapsExcelExporter;
        private readonly IRepository<Business, long> _lookup_businessRepository;
        private readonly IRepository<Store, long> _lookup_storeRepository;

        public BusinessStoreMapsAppService(IRepository<BusinessStoreMap, long> businessStoreMapRepository, IBusinessStoreMapsExcelExporter businessStoreMapsExcelExporter, IRepository<Business, long> lookup_businessRepository, IRepository<Store, long> lookup_storeRepository)
        {
            _businessStoreMapRepository = businessStoreMapRepository;
            _businessStoreMapsExcelExporter = businessStoreMapsExcelExporter;
            _lookup_businessRepository = lookup_businessRepository;
            _lookup_storeRepository = lookup_storeRepository;

        }

        public async Task<PagedResultDto<GetBusinessStoreMapForViewDto>> GetAll(GetAllBusinessStoreMapsInput input)
        {

            var filteredBusinessStoreMaps = _businessStoreMapRepository.GetAll()
                        .Include(e => e.BusinessFk)
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var pagedAndFilteredBusinessStoreMaps = filteredBusinessStoreMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var businessStoreMaps = from o in pagedAndFilteredBusinessStoreMaps
                                    join o1 in _lookup_businessRepository.GetAll() on o.BusinessId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    join o2 in _lookup_storeRepository.GetAll() on o.StoreId equals o2.Id into j2
                                    from s2 in j2.DefaultIfEmpty()

                                    select new
                                    {

                                        Id = o.Id,
                                        BusinessName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                        StoreName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                    };

            var totalCount = await filteredBusinessStoreMaps.CountAsync();

            var dbList = await businessStoreMaps.ToListAsync();
            var results = new List<GetBusinessStoreMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBusinessStoreMapForViewDto()
                {
                    BusinessStoreMap = new BusinessStoreMapDto
                    {

                        Id = o.Id,
                    },
                    BusinessName = o.BusinessName,
                    StoreName = o.StoreName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetBusinessStoreMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetBusinessStoreMapForViewDto> GetBusinessStoreMapForView(long id)
        {
            var businessStoreMap = await _businessStoreMapRepository.GetAsync(id);

            var output = new GetBusinessStoreMapForViewDto { BusinessStoreMap = ObjectMapper.Map<BusinessStoreMapDto>(businessStoreMap) };

            if (output.BusinessStoreMap.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.BusinessStoreMap.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            if (output.BusinessStoreMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.BusinessStoreMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessStoreMaps_Edit)]
        public async Task<GetBusinessStoreMapForEditOutput> GetBusinessStoreMapForEdit(EntityDto<long> input)
        {
            var businessStoreMap = await _businessStoreMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBusinessStoreMapForEditOutput { BusinessStoreMap = ObjectMapper.Map<CreateOrEditBusinessStoreMapDto>(businessStoreMap) };

            if (output.BusinessStoreMap.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.BusinessStoreMap.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            if (output.BusinessStoreMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.BusinessStoreMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBusinessStoreMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_BusinessStoreMaps_Create)]
        protected virtual async Task Create(CreateOrEditBusinessStoreMapDto input)
        {
            var businessStoreMap = ObjectMapper.Map<BusinessStoreMap>(input);

            if (AbpSession.TenantId != null)
            {
                businessStoreMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _businessStoreMapRepository.InsertAsync(businessStoreMap);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessStoreMaps_Edit)]
        protected virtual async Task Update(CreateOrEditBusinessStoreMapDto input)
        {
            var businessStoreMap = await _businessStoreMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, businessStoreMap);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessStoreMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _businessStoreMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetBusinessStoreMapsToExcel(GetAllBusinessStoreMapsForExcelInput input)
        {

            var filteredBusinessStoreMaps = _businessStoreMapRepository.GetAll()
                        .Include(e => e.BusinessFk)
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var query = (from o in filteredBusinessStoreMaps
                         join o1 in _lookup_businessRepository.GetAll() on o.BusinessId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_storeRepository.GetAll() on o.StoreId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetBusinessStoreMapForViewDto()
                         {
                             BusinessStoreMap = new BusinessStoreMapDto
                             {
                                 Id = o.Id
                             },
                             BusinessName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             StoreName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var businessStoreMapListDtos = await query.ToListAsync();

            return _businessStoreMapsExcelExporter.ExportToFile(businessStoreMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessStoreMaps)]
        public async Task<PagedResultDto<BusinessStoreMapBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_businessRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var businessList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BusinessStoreMapBusinessLookupTableDto>();
            foreach (var business in businessList)
            {
                lookupTableDtoList.Add(new BusinessStoreMapBusinessLookupTableDto
                {
                    Id = business.Id,
                    DisplayName = business.Name?.ToString()
                });
            }

            return new PagedResultDto<BusinessStoreMapBusinessLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessStoreMaps)]
        public async Task<PagedResultDto<BusinessStoreMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BusinessStoreMapStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new BusinessStoreMapStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<BusinessStoreMapStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}