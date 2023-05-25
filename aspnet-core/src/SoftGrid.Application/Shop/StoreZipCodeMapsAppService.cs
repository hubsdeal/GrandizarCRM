using SoftGrid.Shop;
using SoftGrid.LookupData;

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
    [AbpAuthorize(AppPermissions.Pages_StoreZipCodeMaps)]
    public class StoreZipCodeMapsAppService : SoftGridAppServiceBase, IStoreZipCodeMapsAppService
    {
        private readonly IRepository<StoreZipCodeMap, long> _storeZipCodeMapRepository;
        private readonly IStoreZipCodeMapsExcelExporter _storeZipCodeMapsExcelExporter;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<ZipCode, long> _lookup_zipCodeRepository;

        public StoreZipCodeMapsAppService(IRepository<StoreZipCodeMap, long> storeZipCodeMapRepository, IStoreZipCodeMapsExcelExporter storeZipCodeMapsExcelExporter, IRepository<Store, long> lookup_storeRepository, IRepository<ZipCode, long> lookup_zipCodeRepository)
        {
            _storeZipCodeMapRepository = storeZipCodeMapRepository;
            _storeZipCodeMapsExcelExporter = storeZipCodeMapsExcelExporter;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_zipCodeRepository = lookup_zipCodeRepository;

        }

        public async Task<PagedResultDto<GetStoreZipCodeMapForViewDto>> GetAll(GetAllStoreZipCodeMapsInput input)
        {

            var filteredStoreZipCodeMaps = _storeZipCodeMapRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.ZipCodeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ZipCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeNameFilter), e => e.ZipCodeFk != null && e.ZipCodeFk.Name == input.ZipCodeNameFilter);

            var pagedAndFilteredStoreZipCodeMaps = filteredStoreZipCodeMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeZipCodeMaps = from o in pagedAndFilteredStoreZipCodeMaps
                                   join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                                   from s1 in j1.DefaultIfEmpty()

                                   join o2 in _lookup_zipCodeRepository.GetAll() on o.ZipCodeId equals o2.Id into j2
                                   from s2 in j2.DefaultIfEmpty()

                                   select new
                                   {

                                       o.ZipCode,
                                       Id = o.Id,
                                       StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                       ZipCodeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                   };

            var totalCount = await filteredStoreZipCodeMaps.CountAsync();

            var dbList = await storeZipCodeMaps.ToListAsync();
            var results = new List<GetStoreZipCodeMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreZipCodeMapForViewDto()
                {
                    StoreZipCodeMap = new StoreZipCodeMapDto
                    {

                        ZipCode = o.ZipCode,
                        Id = o.Id,
                    },
                    StoreName = o.StoreName,
                    ZipCodeName = o.ZipCodeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreZipCodeMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreZipCodeMapForViewDto> GetStoreZipCodeMapForView(long id)
        {
            var storeZipCodeMap = await _storeZipCodeMapRepository.GetAsync(id);

            var output = new GetStoreZipCodeMapForViewDto { StoreZipCodeMap = ObjectMapper.Map<StoreZipCodeMapDto>(storeZipCodeMap) };

            if (output.StoreZipCodeMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreZipCodeMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreZipCodeMap.ZipCodeId != null)
            {
                var _lookupZipCode = await _lookup_zipCodeRepository.FirstOrDefaultAsync((long)output.StoreZipCodeMap.ZipCodeId);
                output.ZipCodeName = _lookupZipCode?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreZipCodeMaps_Edit)]
        public async Task<GetStoreZipCodeMapForEditOutput> GetStoreZipCodeMapForEdit(EntityDto<long> input)
        {
            var storeZipCodeMap = await _storeZipCodeMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreZipCodeMapForEditOutput { StoreZipCodeMap = ObjectMapper.Map<CreateOrEditStoreZipCodeMapDto>(storeZipCodeMap) };

            if (output.StoreZipCodeMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreZipCodeMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreZipCodeMap.ZipCodeId != null)
            {
                var _lookupZipCode = await _lookup_zipCodeRepository.FirstOrDefaultAsync((long)output.StoreZipCodeMap.ZipCodeId);
                output.ZipCodeName = _lookupZipCode?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreZipCodeMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreZipCodeMaps_Create)]
        protected virtual async Task Create(CreateOrEditStoreZipCodeMapDto input)
        {
            var storeZipCodeMap = ObjectMapper.Map<StoreZipCodeMap>(input);

            if (AbpSession.TenantId != null)
            {
                storeZipCodeMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeZipCodeMapRepository.InsertAsync(storeZipCodeMap);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreZipCodeMaps_Edit)]
        protected virtual async Task Update(CreateOrEditStoreZipCodeMapDto input)
        {
            var storeZipCodeMap = await _storeZipCodeMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeZipCodeMap);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreZipCodeMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeZipCodeMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreZipCodeMapsToExcel(GetAllStoreZipCodeMapsForExcelInput input)
        {

            var filteredStoreZipCodeMaps = _storeZipCodeMapRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.ZipCodeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ZipCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeNameFilter), e => e.ZipCodeFk != null && e.ZipCodeFk.Name == input.ZipCodeNameFilter);

            var query = (from o in filteredStoreZipCodeMaps
                         join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_zipCodeRepository.GetAll() on o.ZipCodeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetStoreZipCodeMapForViewDto()
                         {
                             StoreZipCodeMap = new StoreZipCodeMapDto
                             {
                                 ZipCode = o.ZipCode,
                                 Id = o.Id
                             },
                             StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ZipCodeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var storeZipCodeMapListDtos = await query.ToListAsync();

            return _storeZipCodeMapsExcelExporter.ExportToFile(storeZipCodeMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreZipCodeMaps)]
        public async Task<PagedResultDto<StoreZipCodeMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreZipCodeMapStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreZipCodeMapStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreZipCodeMapStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreZipCodeMaps)]
        public async Task<PagedResultDto<StoreZipCodeMapZipCodeLookupTableDto>> GetAllZipCodeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_zipCodeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var zipCodeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreZipCodeMapZipCodeLookupTableDto>();
            foreach (var zipCode in zipCodeList)
            {
                lookupTableDtoList.Add(new StoreZipCodeMapZipCodeLookupTableDto
                {
                    Id = zipCode.Id,
                    DisplayName = zipCode.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreZipCodeMapZipCodeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }



        public async Task<PagedResultDto<GetStoreZipCodeMapForViewDto>> GetAllByStoreId(long storeId)
        {

            var filteredStoreZipCodeMaps = _storeZipCodeMapRepository.GetAll()
                        .Include(e => e.ZipCodeFk)
                        .Include(e => e.StoreFk)
                        .Where(e => e.StoreId == storeId);

            var pagedAndFilteredStoreZipCodeMaps = filteredStoreZipCodeMaps
                .OrderBy("id desc");


            var storeZipCodeMaps = from o in pagedAndFilteredStoreZipCodeMaps
                                   join o1 in _lookup_zipCodeRepository.GetAll() on o.ZipCodeId equals o1.Id into j1
                                   from s1 in j1.DefaultIfEmpty()

                                   join o2 in _lookup_storeRepository.GetAll() on o.StoreId equals o2.Id into j2
                                   from s2 in j2.DefaultIfEmpty()

                                   select new
                                   {

                                       o.ZipCode,
                                       Id = o.Id,
                                       ZipCodeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                       StoreName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                   };

            var totalCount = await filteredStoreZipCodeMaps.CountAsync();

            var dbList = await storeZipCodeMaps.ToListAsync();
            var results = new List<GetStoreZipCodeMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreZipCodeMapForViewDto()
                {
                    StoreZipCodeMap = new StoreZipCodeMapDto
                    {

                        ZipCode = o.ZipCode,
                        Id = o.Id
                    },
                    ZipCodeName = o.ZipCodeName,
                    StoreName = o.StoreName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreZipCodeMapForViewDto>(
                totalCount,
                results
            );

        }
    }
}