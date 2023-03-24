using SoftGrid.Shop;
using SoftGrid.Authorization.Users;

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
    [AbpAuthorize(AppPermissions.Pages_StoreOwnerTeams)]
    public class StoreOwnerTeamsAppService : SoftGridAppServiceBase, IStoreOwnerTeamsAppService
    {
        private readonly IRepository<StoreOwnerTeam, long> _storeOwnerTeamRepository;
        private readonly IStoreOwnerTeamsExcelExporter _storeOwnerTeamsExcelExporter;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<User, long> _lookup_userRepository;

        public StoreOwnerTeamsAppService(IRepository<StoreOwnerTeam, long> storeOwnerTeamRepository, IStoreOwnerTeamsExcelExporter storeOwnerTeamsExcelExporter, IRepository<Store, long> lookup_storeRepository, IRepository<User, long> lookup_userRepository)
        {
            _storeOwnerTeamRepository = storeOwnerTeamRepository;
            _storeOwnerTeamsExcelExporter = storeOwnerTeamsExcelExporter;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_userRepository = lookup_userRepository;

        }

        public async Task<PagedResultDto<GetStoreOwnerTeamForViewDto>> GetAll(GetAllStoreOwnerTeamsInput input)
        {

            var filteredStoreOwnerTeams = _storeOwnerTeamRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(input.PrimaryFilter.HasValue && input.PrimaryFilter > -1, e => (input.PrimaryFilter == 1 && e.Primary) || (input.PrimaryFilter == 0 && !e.Primary))
                        .WhereIf(input.OrderEmailNotificationFilter.HasValue && input.OrderEmailNotificationFilter > -1, e => (input.OrderEmailNotificationFilter == 1 && e.OrderEmailNotification) || (input.OrderEmailNotificationFilter == 0 && !e.OrderEmailNotification))
                        .WhereIf(input.OrderSmsNotificationFilter.HasValue && input.OrderSmsNotificationFilter > -1, e => (input.OrderSmsNotificationFilter == 1 && e.OrderSmsNotification) || (input.OrderSmsNotificationFilter == 0 && !e.OrderSmsNotification))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var pagedAndFilteredStoreOwnerTeams = filteredStoreOwnerTeams
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeOwnerTeams = from o in pagedAndFilteredStoreOwnerTeams
                                  join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                                  from s1 in j1.DefaultIfEmpty()

                                  join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                                  from s2 in j2.DefaultIfEmpty()

                                  select new
                                  {

                                      o.Active,
                                      o.Primary,
                                      o.OrderEmailNotification,
                                      o.OrderSmsNotification,
                                      Id = o.Id,
                                      StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                      UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                  };

            var totalCount = await filteredStoreOwnerTeams.CountAsync();

            var dbList = await storeOwnerTeams.ToListAsync();
            var results = new List<GetStoreOwnerTeamForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreOwnerTeamForViewDto()
                {
                    StoreOwnerTeam = new StoreOwnerTeamDto
                    {

                        Active = o.Active,
                        Primary = o.Primary,
                        OrderEmailNotification = o.OrderEmailNotification,
                        OrderSmsNotification = o.OrderSmsNotification,
                        Id = o.Id,
                    },
                    StoreName = o.StoreName,
                    UserName = o.UserName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreOwnerTeamForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreOwnerTeamForViewDto> GetStoreOwnerTeamForView(long id)
        {
            var storeOwnerTeam = await _storeOwnerTeamRepository.GetAsync(id);

            var output = new GetStoreOwnerTeamForViewDto { StoreOwnerTeam = ObjectMapper.Map<StoreOwnerTeamDto>(storeOwnerTeam) };

            if (output.StoreOwnerTeam.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreOwnerTeam.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreOwnerTeam.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.StoreOwnerTeam.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreOwnerTeams_Edit)]
        public async Task<GetStoreOwnerTeamForEditOutput> GetStoreOwnerTeamForEdit(EntityDto<long> input)
        {
            var storeOwnerTeam = await _storeOwnerTeamRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreOwnerTeamForEditOutput { StoreOwnerTeam = ObjectMapper.Map<CreateOrEditStoreOwnerTeamDto>(storeOwnerTeam) };

            if (output.StoreOwnerTeam.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreOwnerTeam.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreOwnerTeam.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.StoreOwnerTeam.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreOwnerTeamDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreOwnerTeams_Create)]
        protected virtual async Task Create(CreateOrEditStoreOwnerTeamDto input)
        {
            var storeOwnerTeam = ObjectMapper.Map<StoreOwnerTeam>(input);

            if (AbpSession.TenantId != null)
            {
                storeOwnerTeam.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeOwnerTeamRepository.InsertAsync(storeOwnerTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreOwnerTeams_Edit)]
        protected virtual async Task Update(CreateOrEditStoreOwnerTeamDto input)
        {
            var storeOwnerTeam = await _storeOwnerTeamRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeOwnerTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreOwnerTeams_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeOwnerTeamRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreOwnerTeamsToExcel(GetAllStoreOwnerTeamsForExcelInput input)
        {

            var filteredStoreOwnerTeams = _storeOwnerTeamRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(input.PrimaryFilter.HasValue && input.PrimaryFilter > -1, e => (input.PrimaryFilter == 1 && e.Primary) || (input.PrimaryFilter == 0 && !e.Primary))
                        .WhereIf(input.OrderEmailNotificationFilter.HasValue && input.OrderEmailNotificationFilter > -1, e => (input.OrderEmailNotificationFilter == 1 && e.OrderEmailNotification) || (input.OrderEmailNotificationFilter == 0 && !e.OrderEmailNotification))
                        .WhereIf(input.OrderSmsNotificationFilter.HasValue && input.OrderSmsNotificationFilter > -1, e => (input.OrderSmsNotificationFilter == 1 && e.OrderSmsNotification) || (input.OrderSmsNotificationFilter == 0 && !e.OrderSmsNotification))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var query = (from o in filteredStoreOwnerTeams
                         join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetStoreOwnerTeamForViewDto()
                         {
                             StoreOwnerTeam = new StoreOwnerTeamDto
                             {
                                 Active = o.Active,
                                 Primary = o.Primary,
                                 OrderEmailNotification = o.OrderEmailNotification,
                                 OrderSmsNotification = o.OrderSmsNotification,
                                 Id = o.Id
                             },
                             StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var storeOwnerTeamListDtos = await query.ToListAsync();

            return _storeOwnerTeamsExcelExporter.ExportToFile(storeOwnerTeamListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreOwnerTeams)]
        public async Task<PagedResultDto<StoreOwnerTeamStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreOwnerTeamStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreOwnerTeamStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreOwnerTeamStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreOwnerTeams)]
        public async Task<PagedResultDto<StoreOwnerTeamUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreOwnerTeamUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new StoreOwnerTeamUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreOwnerTeamUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}