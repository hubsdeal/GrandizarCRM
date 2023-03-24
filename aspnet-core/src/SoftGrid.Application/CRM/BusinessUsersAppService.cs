using SoftGrid.CRM;
using SoftGrid.Authorization.Users;

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
    [AbpAuthorize(AppPermissions.Pages_BusinessUsers)]
    public class BusinessUsersAppService : SoftGridAppServiceBase, IBusinessUsersAppService
    {
        private readonly IRepository<BusinessUser, long> _businessUserRepository;
        private readonly IBusinessUsersExcelExporter _businessUsersExcelExporter;
        private readonly IRepository<Business, long> _lookup_businessRepository;
        private readonly IRepository<User, long> _lookup_userRepository;

        public BusinessUsersAppService(IRepository<BusinessUser, long> businessUserRepository, IBusinessUsersExcelExporter businessUsersExcelExporter, IRepository<Business, long> lookup_businessRepository, IRepository<User, long> lookup_userRepository)
        {
            _businessUserRepository = businessUserRepository;
            _businessUsersExcelExporter = businessUsersExcelExporter;
            _lookup_businessRepository = lookup_businessRepository;
            _lookup_userRepository = lookup_userRepository;

        }

        public async Task<PagedResultDto<GetBusinessUserForViewDto>> GetAll(GetAllBusinessUsersInput input)
        {

            var filteredBusinessUsers = _businessUserRepository.GetAll()
                        .Include(e => e.BusinessFk)
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var pagedAndFilteredBusinessUsers = filteredBusinessUsers
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var businessUsers = from o in pagedAndFilteredBusinessUsers
                                join o1 in _lookup_businessRepository.GetAll() on o.BusinessId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()

                                select new
                                {

                                    Id = o.Id,
                                    BusinessName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                    UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                };

            var totalCount = await filteredBusinessUsers.CountAsync();

            var dbList = await businessUsers.ToListAsync();
            var results = new List<GetBusinessUserForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBusinessUserForViewDto()
                {
                    BusinessUser = new BusinessUserDto
                    {

                        Id = o.Id,
                    },
                    BusinessName = o.BusinessName,
                    UserName = o.UserName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetBusinessUserForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetBusinessUserForViewDto> GetBusinessUserForView(long id)
        {
            var businessUser = await _businessUserRepository.GetAsync(id);

            var output = new GetBusinessUserForViewDto { BusinessUser = ObjectMapper.Map<BusinessUserDto>(businessUser) };

            if (output.BusinessUser.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.BusinessUser.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            if (output.BusinessUser.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.BusinessUser.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessUsers_Edit)]
        public async Task<GetBusinessUserForEditOutput> GetBusinessUserForEdit(EntityDto<long> input)
        {
            var businessUser = await _businessUserRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBusinessUserForEditOutput { BusinessUser = ObjectMapper.Map<CreateOrEditBusinessUserDto>(businessUser) };

            if (output.BusinessUser.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.BusinessUser.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            if (output.BusinessUser.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.BusinessUser.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBusinessUserDto input)
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

        [AbpAuthorize(AppPermissions.Pages_BusinessUsers_Create)]
        protected virtual async Task Create(CreateOrEditBusinessUserDto input)
        {
            var businessUser = ObjectMapper.Map<BusinessUser>(input);

            if (AbpSession.TenantId != null)
            {
                businessUser.TenantId = (int?)AbpSession.TenantId;
            }

            await _businessUserRepository.InsertAsync(businessUser);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessUsers_Edit)]
        protected virtual async Task Update(CreateOrEditBusinessUserDto input)
        {
            var businessUser = await _businessUserRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, businessUser);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessUsers_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _businessUserRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetBusinessUsersToExcel(GetAllBusinessUsersForExcelInput input)
        {

            var filteredBusinessUsers = _businessUserRepository.GetAll()
                        .Include(e => e.BusinessFk)
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var query = (from o in filteredBusinessUsers
                         join o1 in _lookup_businessRepository.GetAll() on o.BusinessId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetBusinessUserForViewDto()
                         {
                             BusinessUser = new BusinessUserDto
                             {
                                 Id = o.Id
                             },
                             BusinessName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var businessUserListDtos = await query.ToListAsync();

            return _businessUsersExcelExporter.ExportToFile(businessUserListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessUsers)]
        public async Task<PagedResultDto<BusinessUserBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_businessRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var businessList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BusinessUserBusinessLookupTableDto>();
            foreach (var business in businessList)
            {
                lookupTableDtoList.Add(new BusinessUserBusinessLookupTableDto
                {
                    Id = business.Id,
                    DisplayName = business.Name?.ToString()
                });
            }

            return new PagedResultDto<BusinessUserBusinessLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessUsers)]
        public async Task<PagedResultDto<BusinessUserUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BusinessUserUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new BusinessUserUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<BusinessUserUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}