using SoftGrid.Territory;
using SoftGrid.CRM;

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
    [AbpAuthorize(AppPermissions.Pages_HubBusinesses)]
    public class HubBusinessesAppService : SoftGridAppServiceBase, IHubBusinessesAppService
    {
        private readonly IRepository<HubBusiness, long> _hubBusinessRepository;
        private readonly IHubBusinessesExcelExporter _hubBusinessesExcelExporter;
        private readonly IRepository<Hub, long> _lookup_hubRepository;
        private readonly IRepository<Business, long> _lookup_businessRepository;

        public HubBusinessesAppService(IRepository<HubBusiness, long> hubBusinessRepository, IHubBusinessesExcelExporter hubBusinessesExcelExporter, IRepository<Hub, long> lookup_hubRepository, IRepository<Business, long> lookup_businessRepository)
        {
            _hubBusinessRepository = hubBusinessRepository;
            _hubBusinessesExcelExporter = hubBusinessesExcelExporter;
            _lookup_hubRepository = lookup_hubRepository;
            _lookup_businessRepository = lookup_businessRepository;

        }

        public async Task<PagedResultDto<GetHubBusinessForViewDto>> GetAll(GetAllHubBusinessesInput input)
        {

            var filteredHubBusinesses = _hubBusinessRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.BusinessFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published))
                        .WhereIf(input.MinDisplayScoreFilter != null, e => e.DisplayScore >= input.MinDisplayScoreFilter)
                        .WhereIf(input.MaxDisplayScoreFilter != null, e => e.DisplayScore <= input.MaxDisplayScoreFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter);

            var pagedAndFilteredHubBusinesses = filteredHubBusinesses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var hubBusinesses = from o in pagedAndFilteredHubBusinesses
                                join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                join o2 in _lookup_businessRepository.GetAll() on o.BusinessId equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()

                                select new
                                {

                                    o.Published,
                                    o.DisplayScore,
                                    Id = o.Id,
                                    HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                    BusinessName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                };

            var totalCount = await filteredHubBusinesses.CountAsync();

            var dbList = await hubBusinesses.ToListAsync();
            var results = new List<GetHubBusinessForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetHubBusinessForViewDto()
                {
                    HubBusiness = new HubBusinessDto
                    {

                        Published = o.Published,
                        DisplayScore = o.DisplayScore,
                        Id = o.Id,
                    },
                    HubName = o.HubName,
                    BusinessName = o.BusinessName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetHubBusinessForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetHubBusinessForViewDto> GetHubBusinessForView(long id)
        {
            var hubBusiness = await _hubBusinessRepository.GetAsync(id);

            var output = new GetHubBusinessForViewDto { HubBusiness = ObjectMapper.Map<HubBusinessDto>(hubBusiness) };

            if (output.HubBusiness.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.HubBusiness.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.HubBusiness.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.HubBusiness.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_HubBusinesses_Edit)]
        public async Task<GetHubBusinessForEditOutput> GetHubBusinessForEdit(EntityDto<long> input)
        {
            var hubBusiness = await _hubBusinessRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHubBusinessForEditOutput { HubBusiness = ObjectMapper.Map<CreateOrEditHubBusinessDto>(hubBusiness) };

            if (output.HubBusiness.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.HubBusiness.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.HubBusiness.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.HubBusiness.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHubBusinessDto input)
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

        [AbpAuthorize(AppPermissions.Pages_HubBusinesses_Create)]
        protected virtual async Task Create(CreateOrEditHubBusinessDto input)
        {
            var hubBusiness = ObjectMapper.Map<HubBusiness>(input);

            if (AbpSession.TenantId != null)
            {
                hubBusiness.TenantId = (int?)AbpSession.TenantId;
            }

            await _hubBusinessRepository.InsertAsync(hubBusiness);

        }

        [AbpAuthorize(AppPermissions.Pages_HubBusinesses_Edit)]
        protected virtual async Task Update(CreateOrEditHubBusinessDto input)
        {
            var hubBusiness = await _hubBusinessRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, hubBusiness);

        }

        [AbpAuthorize(AppPermissions.Pages_HubBusinesses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _hubBusinessRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetHubBusinessesToExcel(GetAllHubBusinessesForExcelInput input)
        {

            var filteredHubBusinesses = _hubBusinessRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.BusinessFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published))
                        .WhereIf(input.MinDisplayScoreFilter != null, e => e.DisplayScore >= input.MinDisplayScoreFilter)
                        .WhereIf(input.MaxDisplayScoreFilter != null, e => e.DisplayScore <= input.MaxDisplayScoreFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter);

            var query = (from o in filteredHubBusinesses
                         join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_businessRepository.GetAll() on o.BusinessId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetHubBusinessForViewDto()
                         {
                             HubBusiness = new HubBusinessDto
                             {
                                 Published = o.Published,
                                 DisplayScore = o.DisplayScore,
                                 Id = o.Id
                             },
                             HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             BusinessName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var hubBusinessListDtos = await query.ToListAsync();

            return _hubBusinessesExcelExporter.ExportToFile(hubBusinessListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_HubBusinesses)]
        public async Task<PagedResultDto<HubBusinessHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_hubRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var hubList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubBusinessHubLookupTableDto>();
            foreach (var hub in hubList)
            {
                lookupTableDtoList.Add(new HubBusinessHubLookupTableDto
                {
                    Id = hub.Id,
                    DisplayName = hub.Name?.ToString()
                });
            }

            return new PagedResultDto<HubBusinessHubLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HubBusinesses)]
        public async Task<PagedResultDto<HubBusinessBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_businessRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var businessList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubBusinessBusinessLookupTableDto>();
            foreach (var business in businessList)
            {
                lookupTableDtoList.Add(new HubBusinessBusinessLookupTableDto
                {
                    Id = business.Id,
                    DisplayName = business.Name?.ToString()
                });
            }

            return new PagedResultDto<HubBusinessBusinessLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}