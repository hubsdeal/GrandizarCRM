using SoftGrid.Territory;
using SoftGrid.Territory;

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
    [AbpAuthorize(AppPermissions.Pages_HubNavigationMenus)]
    public class HubNavigationMenusAppService : SoftGridAppServiceBase, IHubNavigationMenusAppService
    {
        private readonly IRepository<HubNavigationMenu, long> _hubNavigationMenuRepository;
        private readonly IHubNavigationMenusExcelExporter _hubNavigationMenusExcelExporter;
        private readonly IRepository<Hub, long> _lookup_hubRepository;
        private readonly IRepository<MasterNavigationMenu, long> _lookup_masterNavigationMenuRepository;

        public HubNavigationMenusAppService(IRepository<HubNavigationMenu, long> hubNavigationMenuRepository, IHubNavigationMenusExcelExporter hubNavigationMenusExcelExporter, IRepository<Hub, long> lookup_hubRepository, IRepository<MasterNavigationMenu, long> lookup_masterNavigationMenuRepository)
        {
            _hubNavigationMenuRepository = hubNavigationMenuRepository;
            _hubNavigationMenusExcelExporter = hubNavigationMenusExcelExporter;
            _lookup_hubRepository = lookup_hubRepository;
            _lookup_masterNavigationMenuRepository = lookup_masterNavigationMenuRepository;

        }

        public async Task<PagedResultDto<GetHubNavigationMenuForViewDto>> GetAll(GetAllHubNavigationMenusInput input)
        {

            var filteredHubNavigationMenus = _hubNavigationMenuRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.MasterNavigationMenuFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomName.Contains(input.Filter) || e.NavigationLink.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomNameFilter), e => e.CustomName.Contains(input.CustomNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NavigationLinkFilter), e => e.NavigationLink.Contains(input.NavigationLinkFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterNavigationMenuNameFilter), e => e.MasterNavigationMenuFk != null && e.MasterNavigationMenuFk.Name == input.MasterNavigationMenuNameFilter);

            var pagedAndFilteredHubNavigationMenus = filteredHubNavigationMenus
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var hubNavigationMenus = from o in pagedAndFilteredHubNavigationMenus
                                     join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                                     from s1 in j1.DefaultIfEmpty()

                                     join o2 in _lookup_masterNavigationMenuRepository.GetAll() on o.MasterNavigationMenuId equals o2.Id into j2
                                     from s2 in j2.DefaultIfEmpty()

                                     select new
                                     {

                                         o.CustomName,
                                         o.NavigationLink,
                                         Id = o.Id,
                                         HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                         MasterNavigationMenuName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                     };

            var totalCount = await filteredHubNavigationMenus.CountAsync();

            var dbList = await hubNavigationMenus.ToListAsync();
            var results = new List<GetHubNavigationMenuForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetHubNavigationMenuForViewDto()
                {
                    HubNavigationMenu = new HubNavigationMenuDto
                    {

                        CustomName = o.CustomName,
                        NavigationLink = o.NavigationLink,
                        Id = o.Id,
                    },
                    HubName = o.HubName,
                    MasterNavigationMenuName = o.MasterNavigationMenuName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetHubNavigationMenuForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetHubNavigationMenuForViewDto> GetHubNavigationMenuForView(long id)
        {
            var hubNavigationMenu = await _hubNavigationMenuRepository.GetAsync(id);

            var output = new GetHubNavigationMenuForViewDto { HubNavigationMenu = ObjectMapper.Map<HubNavigationMenuDto>(hubNavigationMenu) };

            if (output.HubNavigationMenu.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.HubNavigationMenu.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.HubNavigationMenu.MasterNavigationMenuId != null)
            {
                var _lookupMasterNavigationMenu = await _lookup_masterNavigationMenuRepository.FirstOrDefaultAsync((long)output.HubNavigationMenu.MasterNavigationMenuId);
                output.MasterNavigationMenuName = _lookupMasterNavigationMenu?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_HubNavigationMenus_Edit)]
        public async Task<GetHubNavigationMenuForEditOutput> GetHubNavigationMenuForEdit(EntityDto<long> input)
        {
            var hubNavigationMenu = await _hubNavigationMenuRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHubNavigationMenuForEditOutput { HubNavigationMenu = ObjectMapper.Map<CreateOrEditHubNavigationMenuDto>(hubNavigationMenu) };

            if (output.HubNavigationMenu.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.HubNavigationMenu.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.HubNavigationMenu.MasterNavigationMenuId != null)
            {
                var _lookupMasterNavigationMenu = await _lookup_masterNavigationMenuRepository.FirstOrDefaultAsync((long)output.HubNavigationMenu.MasterNavigationMenuId);
                output.MasterNavigationMenuName = _lookupMasterNavigationMenu?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHubNavigationMenuDto input)
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

        [AbpAuthorize(AppPermissions.Pages_HubNavigationMenus_Create)]
        protected virtual async Task Create(CreateOrEditHubNavigationMenuDto input)
        {
            var hubNavigationMenu = ObjectMapper.Map<HubNavigationMenu>(input);

            if (AbpSession.TenantId != null)
            {
                hubNavigationMenu.TenantId = (int?)AbpSession.TenantId;
            }

            await _hubNavigationMenuRepository.InsertAsync(hubNavigationMenu);

        }

        [AbpAuthorize(AppPermissions.Pages_HubNavigationMenus_Edit)]
        protected virtual async Task Update(CreateOrEditHubNavigationMenuDto input)
        {
            var hubNavigationMenu = await _hubNavigationMenuRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, hubNavigationMenu);

        }

        [AbpAuthorize(AppPermissions.Pages_HubNavigationMenus_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _hubNavigationMenuRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetHubNavigationMenusToExcel(GetAllHubNavigationMenusForExcelInput input)
        {

            var filteredHubNavigationMenus = _hubNavigationMenuRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.MasterNavigationMenuFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomName.Contains(input.Filter) || e.NavigationLink.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomNameFilter), e => e.CustomName.Contains(input.CustomNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NavigationLinkFilter), e => e.NavigationLink.Contains(input.NavigationLinkFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterNavigationMenuNameFilter), e => e.MasterNavigationMenuFk != null && e.MasterNavigationMenuFk.Name == input.MasterNavigationMenuNameFilter);

            var query = (from o in filteredHubNavigationMenus
                         join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_masterNavigationMenuRepository.GetAll() on o.MasterNavigationMenuId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetHubNavigationMenuForViewDto()
                         {
                             HubNavigationMenu = new HubNavigationMenuDto
                             {
                                 CustomName = o.CustomName,
                                 NavigationLink = o.NavigationLink,
                                 Id = o.Id
                             },
                             HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MasterNavigationMenuName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var hubNavigationMenuListDtos = await query.ToListAsync();

            return _hubNavigationMenusExcelExporter.ExportToFile(hubNavigationMenuListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_HubNavigationMenus)]
        public async Task<PagedResultDto<HubNavigationMenuHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_hubRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var hubList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubNavigationMenuHubLookupTableDto>();
            foreach (var hub in hubList)
            {
                lookupTableDtoList.Add(new HubNavigationMenuHubLookupTableDto
                {
                    Id = hub.Id,
                    DisplayName = hub.Name?.ToString()
                });
            }

            return new PagedResultDto<HubNavigationMenuHubLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HubNavigationMenus)]
        public async Task<PagedResultDto<HubNavigationMenuMasterNavigationMenuLookupTableDto>> GetAllMasterNavigationMenuForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterNavigationMenuRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterNavigationMenuList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubNavigationMenuMasterNavigationMenuLookupTableDto>();
            foreach (var masterNavigationMenu in masterNavigationMenuList)
            {
                lookupTableDtoList.Add(new HubNavigationMenuMasterNavigationMenuLookupTableDto
                {
                    Id = masterNavigationMenu.Id,
                    DisplayName = masterNavigationMenu.Name?.ToString()
                });
            }

            return new PagedResultDto<HubNavigationMenuMasterNavigationMenuLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}