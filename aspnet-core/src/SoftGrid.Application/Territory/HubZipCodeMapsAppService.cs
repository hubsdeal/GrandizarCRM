using SoftGrid.Territory;
using SoftGrid.LookupData;
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
    [AbpAuthorize(AppPermissions.Pages_HubZipCodeMaps)]
    public class HubZipCodeMapsAppService : SoftGridAppServiceBase, IHubZipCodeMapsAppService
    {
        private readonly IRepository<HubZipCodeMap, long> _hubZipCodeMapRepository;
        private readonly IHubZipCodeMapsExcelExporter _hubZipCodeMapsExcelExporter;
        private readonly IRepository<Hub, long> _lookup_hubRepository;
        private readonly IRepository<City, long> _lookup_cityRepository;
        private readonly IRepository<ZipCode, long> _lookup_zipCodeRepository;

        public HubZipCodeMapsAppService(IRepository<HubZipCodeMap, long> hubZipCodeMapRepository, IHubZipCodeMapsExcelExporter hubZipCodeMapsExcelExporter, IRepository<Hub, long> lookup_hubRepository, IRepository<City, long> lookup_cityRepository, IRepository<ZipCode, long> lookup_zipCodeRepository)
        {
            _hubZipCodeMapRepository = hubZipCodeMapRepository;
            _hubZipCodeMapsExcelExporter = hubZipCodeMapsExcelExporter;
            _lookup_hubRepository = lookup_hubRepository;
            _lookup_cityRepository = lookup_cityRepository;
            _lookup_zipCodeRepository = lookup_zipCodeRepository;

        }

        public async Task<PagedResultDto<GetHubZipCodeMapForViewDto>> GetAll(GetAllHubZipCodeMapsInput input)
        {

            var filteredHubZipCodeMaps = _hubZipCodeMapRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.CityFk)
                        .Include(e => e.ZipCodeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CityName.Contains(input.Filter) || e.ZipCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityNameFilter), e => e.CityName.Contains(input.CityNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityNameFilter), e => e.CityFk != null && e.CityFk.Name == input.CityNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeNameFilter), e => e.ZipCodeFk != null && e.ZipCodeFk.Name == input.ZipCodeNameFilter);

            var pagedAndFilteredHubZipCodeMaps = filteredHubZipCodeMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var hubZipCodeMaps = from o in pagedAndFilteredHubZipCodeMaps
                                 join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()

                                 join o2 in _lookup_cityRepository.GetAll() on o.CityId equals o2.Id into j2
                                 from s2 in j2.DefaultIfEmpty()

                                 join o3 in _lookup_zipCodeRepository.GetAll() on o.ZipCodeId equals o3.Id into j3
                                 from s3 in j3.DefaultIfEmpty()

                                 select new
                                 {

                                     o.CityName,
                                     o.ZipCode,
                                     Id = o.Id,
                                     HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                     //CityName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                     ZipCodeName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                                 };

            var totalCount = await filteredHubZipCodeMaps.CountAsync();

            var dbList = await hubZipCodeMaps.ToListAsync();
            var results = new List<GetHubZipCodeMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetHubZipCodeMapForViewDto()
                {
                    HubZipCodeMap = new HubZipCodeMapDto
                    {

                        CityName = o.CityName,
                        ZipCode = o.ZipCode,
                        Id = o.Id,
                    },
                    HubName = o.HubName,
                    CityName = o.CityName,
                    ZipCodeName = o.ZipCodeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetHubZipCodeMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetHubZipCodeMapForViewDto> GetHubZipCodeMapForView(long id)
        {
            var hubZipCodeMap = await _hubZipCodeMapRepository.GetAsync(id);

            var output = new GetHubZipCodeMapForViewDto { HubZipCodeMap = ObjectMapper.Map<HubZipCodeMapDto>(hubZipCodeMap) };

            if (output.HubZipCodeMap.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.HubZipCodeMap.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.HubZipCodeMap.CityId != null)
            {
                var _lookupCity = await _lookup_cityRepository.FirstOrDefaultAsync((long)output.HubZipCodeMap.CityId);
                output.CityName = _lookupCity?.Name?.ToString();
            }

            if (output.HubZipCodeMap.ZipCodeId != null)
            {
                var _lookupZipCode = await _lookup_zipCodeRepository.FirstOrDefaultAsync((long)output.HubZipCodeMap.ZipCodeId);
                output.ZipCodeName = _lookupZipCode?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_HubZipCodeMaps_Edit)]
        public async Task<GetHubZipCodeMapForEditOutput> GetHubZipCodeMapForEdit(EntityDto<long> input)
        {
            var hubZipCodeMap = await _hubZipCodeMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHubZipCodeMapForEditOutput { HubZipCodeMap = ObjectMapper.Map<CreateOrEditHubZipCodeMapDto>(hubZipCodeMap) };

            if (output.HubZipCodeMap.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.HubZipCodeMap.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.HubZipCodeMap.CityId != null)
            {
                var _lookupCity = await _lookup_cityRepository.FirstOrDefaultAsync((long)output.HubZipCodeMap.CityId);
                output.CityName = _lookupCity?.Name?.ToString();
            }

            if (output.HubZipCodeMap.ZipCodeId != null)
            {
                var _lookupZipCode = await _lookup_zipCodeRepository.FirstOrDefaultAsync((long)output.HubZipCodeMap.ZipCodeId);
                output.ZipCodeName = _lookupZipCode?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHubZipCodeMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_HubZipCodeMaps_Create)]
        protected virtual async Task Create(CreateOrEditHubZipCodeMapDto input)
        {
            var hubZipCodeMap = ObjectMapper.Map<HubZipCodeMap>(input);

            if (AbpSession.TenantId != null)
            {
                hubZipCodeMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _hubZipCodeMapRepository.InsertAsync(hubZipCodeMap);

        }

        [AbpAuthorize(AppPermissions.Pages_HubZipCodeMaps_Edit)]
        protected virtual async Task Update(CreateOrEditHubZipCodeMapDto input)
        {
            var hubZipCodeMap = await _hubZipCodeMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, hubZipCodeMap);

        }

        [AbpAuthorize(AppPermissions.Pages_HubZipCodeMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _hubZipCodeMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetHubZipCodeMapsToExcel(GetAllHubZipCodeMapsForExcelInput input)
        {

            var filteredHubZipCodeMaps = _hubZipCodeMapRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.CityFk)
                        .Include(e => e.ZipCodeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CityName.Contains(input.Filter) || e.ZipCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityNameFilter), e => e.CityName.Contains(input.CityNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityNameFilter), e => e.CityFk != null && e.CityFk.Name == input.CityNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeNameFilter), e => e.ZipCodeFk != null && e.ZipCodeFk.Name == input.ZipCodeNameFilter);

            var query = (from o in filteredHubZipCodeMaps
                         join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_cityRepository.GetAll() on o.CityId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_zipCodeRepository.GetAll() on o.ZipCodeId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetHubZipCodeMapForViewDto()
                         {
                             HubZipCodeMap = new HubZipCodeMapDto
                             {
                                 CityName = o.CityName,
                                 ZipCode = o.ZipCode,
                                 Id = o.Id
                             },
                             HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             CityName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             ZipCodeName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var hubZipCodeMapListDtos = await query.ToListAsync();

            return _hubZipCodeMapsExcelExporter.ExportToFile(hubZipCodeMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_HubZipCodeMaps)]
        public async Task<PagedResultDto<HubZipCodeMapHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_hubRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var hubList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubZipCodeMapHubLookupTableDto>();
            foreach (var hub in hubList)
            {
                lookupTableDtoList.Add(new HubZipCodeMapHubLookupTableDto
                {
                    Id = hub.Id,
                    DisplayName = hub.Name?.ToString()
                });
            }

            return new PagedResultDto<HubZipCodeMapHubLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HubZipCodeMaps)]
        public async Task<PagedResultDto<HubZipCodeMapCityLookupTableDto>> GetAllCityForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_cityRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var cityList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubZipCodeMapCityLookupTableDto>();
            foreach (var city in cityList)
            {
                lookupTableDtoList.Add(new HubZipCodeMapCityLookupTableDto
                {
                    Id = city.Id,
                    DisplayName = city.Name?.ToString()
                });
            }

            return new PagedResultDto<HubZipCodeMapCityLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HubZipCodeMaps)]
        public async Task<PagedResultDto<HubZipCodeMapZipCodeLookupTableDto>> GetAllZipCodeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_zipCodeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var zipCodeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubZipCodeMapZipCodeLookupTableDto>();
            foreach (var zipCode in zipCodeList)
            {
                lookupTableDtoList.Add(new HubZipCodeMapZipCodeLookupTableDto
                {
                    Id = zipCode.Id,
                    DisplayName = zipCode.Name?.ToString()
                });
            }

            return new PagedResultDto<HubZipCodeMapZipCodeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}