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
    [AbpAuthorize(AppPermissions.Pages_MasterNavigationMenus)]
    public class MasterNavigationMenusAppService : SoftGridAppServiceBase, IMasterNavigationMenusAppService
    {
        private readonly IRepository<MasterNavigationMenu, long> _masterNavigationMenuRepository;
        private readonly IMasterNavigationMenusExcelExporter _masterNavigationMenusExcelExporter;

        public MasterNavigationMenusAppService(IRepository<MasterNavigationMenu, long> masterNavigationMenuRepository, IMasterNavigationMenusExcelExporter masterNavigationMenusExcelExporter)
        {
            _masterNavigationMenuRepository = masterNavigationMenuRepository;
            _masterNavigationMenusExcelExporter = masterNavigationMenusExcelExporter;

        }

        public async Task<PagedResultDto<GetMasterNavigationMenuForViewDto>> GetAll(GetAllMasterNavigationMenusInput input)
        {

            var filteredMasterNavigationMenus = _masterNavigationMenuRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.HasParentMenuFilter.HasValue && input.HasParentMenuFilter > -1, e => (input.HasParentMenuFilter == 1 && e.HasParentMenu) || (input.HasParentMenuFilter == 0 && !e.HasParentMenu))
                        .WhereIf(input.MinParentMenuIdFilter != null, e => e.ParentMenuId >= input.MinParentMenuIdFilter)
                        .WhereIf(input.MaxParentMenuIdFilter != null, e => e.ParentMenuId <= input.MaxParentMenuIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IconLinkFilter.ToString()), e => e.IconLink.ToString() == input.IconLinkFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLinkFilter.ToString()), e => e.ContentLink.ToString() == input.MediaLinkFilter.ToString());

            var pagedAndFilteredMasterNavigationMenus = filteredMasterNavigationMenus
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var masterNavigationMenus = from o in pagedAndFilteredMasterNavigationMenus
                                        select new
                                        {

                                            o.Name,
                                            o.HasParentMenu,
                                            o.ParentMenuId,
                                            o.IconLink,
                                            o.ContentLink,
                                            Id = o.Id
                                        };

            var totalCount = await filteredMasterNavigationMenus.CountAsync();

            var dbList = await masterNavigationMenus.ToListAsync();
            var results = new List<GetMasterNavigationMenuForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetMasterNavigationMenuForViewDto()
                {
                    MasterNavigationMenu = new MasterNavigationMenuDto
                    {

                        Name = o.Name,
                        HasParentMenu = o.HasParentMenu,
                        ParentMenuId = o.ParentMenuId,
                        IconLink = o.IconLink,
                        ContentLink = o.ContentLink,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetMasterNavigationMenuForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetMasterNavigationMenuForViewDto> GetMasterNavigationMenuForView(long id)
        {
            var masterNavigationMenu = await _masterNavigationMenuRepository.GetAsync(id);

            var output = new GetMasterNavigationMenuForViewDto { MasterNavigationMenu = ObjectMapper.Map<MasterNavigationMenuDto>(masterNavigationMenu) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MasterNavigationMenus_Edit)]
        public async Task<GetMasterNavigationMenuForEditOutput> GetMasterNavigationMenuForEdit(EntityDto<long> input)
        {
            var masterNavigationMenu = await _masterNavigationMenuRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMasterNavigationMenuForEditOutput { MasterNavigationMenu = ObjectMapper.Map<CreateOrEditMasterNavigationMenuDto>(masterNavigationMenu) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMasterNavigationMenuDto input)
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

        [AbpAuthorize(AppPermissions.Pages_MasterNavigationMenus_Create)]
        protected virtual async Task Create(CreateOrEditMasterNavigationMenuDto input)
        {
            var masterNavigationMenu = ObjectMapper.Map<MasterNavigationMenu>(input);

            if (AbpSession.TenantId != null)
            {
                masterNavigationMenu.TenantId = (int?)AbpSession.TenantId;
            }

            await _masterNavigationMenuRepository.InsertAsync(masterNavigationMenu);

        }

        [AbpAuthorize(AppPermissions.Pages_MasterNavigationMenus_Edit)]
        protected virtual async Task Update(CreateOrEditMasterNavigationMenuDto input)
        {
            var masterNavigationMenu = await _masterNavigationMenuRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, masterNavigationMenu);

        }

        [AbpAuthorize(AppPermissions.Pages_MasterNavigationMenus_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _masterNavigationMenuRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMasterNavigationMenusToExcel(GetAllMasterNavigationMenusForExcelInput input)
        {

            var filteredMasterNavigationMenus = _masterNavigationMenuRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.HasParentMenuFilter.HasValue && input.HasParentMenuFilter > -1, e => (input.HasParentMenuFilter == 1 && e.HasParentMenu) || (input.HasParentMenuFilter == 0 && !e.HasParentMenu))
                        .WhereIf(input.MinParentMenuIdFilter != null, e => e.ParentMenuId >= input.MinParentMenuIdFilter)
                        .WhereIf(input.MaxParentMenuIdFilter != null, e => e.ParentMenuId <= input.MaxParentMenuIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IconLinkFilter.ToString()), e => e.IconLink.ToString() == input.IconLinkFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLinkFilter.ToString()), e => e.ContentLink.ToString() == input.MediaLinkFilter.ToString());

            var query = (from o in filteredMasterNavigationMenus
                         select new GetMasterNavigationMenuForViewDto()
                         {
                             MasterNavigationMenu = new MasterNavigationMenuDto
                             {
                                 Name = o.Name,
                                 HasParentMenu = o.HasParentMenu,
                                 ParentMenuId = o.ParentMenuId,
                                 IconLink = o.IconLink,
                                 ContentLink = o.ContentLink,
                                 Id = o.Id
                             }
                         });

            var masterNavigationMenuListDtos = await query.ToListAsync();

            return _masterNavigationMenusExcelExporter.ExportToFile(masterNavigationMenuListDtos);
        }

    }
}