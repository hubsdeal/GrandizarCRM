using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.LookupData.Exporting;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.LookupData
{
    [AbpAuthorize(AppPermissions.Pages_MembershipTypes)]
    public class MembershipTypesAppService : SoftGridAppServiceBase, IMembershipTypesAppService
    {
        private readonly IRepository<MembershipType, long> _membershipTypeRepository;
        private readonly IMembershipTypesExcelExporter _membershipTypesExcelExporter;

        public MembershipTypesAppService(IRepository<MembershipType, long> membershipTypeRepository, IMembershipTypesExcelExporter membershipTypesExcelExporter)
        {
            _membershipTypeRepository = membershipTypeRepository;
            _membershipTypesExcelExporter = membershipTypesExcelExporter;

        }

        public async Task<PagedResultDto<GetMembershipTypeForViewDto>> GetAll(GetAllMembershipTypesInput input)
        {

            var filteredMembershipTypes = _membershipTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredMembershipTypes = filteredMembershipTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var membershipTypes = from o in pagedAndFilteredMembershipTypes
                                  select new
                                  {

                                      o.Name,
                                      o.Description,
                                      Id = o.Id
                                  };

            var totalCount = await filteredMembershipTypes.CountAsync();

            var dbList = await membershipTypes.ToListAsync();
            var results = new List<GetMembershipTypeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetMembershipTypeForViewDto()
                {
                    MembershipType = new MembershipTypeDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetMembershipTypeForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetMembershipTypeForViewDto> GetMembershipTypeForView(long id)
        {
            var membershipType = await _membershipTypeRepository.GetAsync(id);

            var output = new GetMembershipTypeForViewDto { MembershipType = ObjectMapper.Map<MembershipTypeDto>(membershipType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MembershipTypes_Edit)]
        public async Task<GetMembershipTypeForEditOutput> GetMembershipTypeForEdit(EntityDto<long> input)
        {
            var membershipType = await _membershipTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMembershipTypeForEditOutput { MembershipType = ObjectMapper.Map<CreateOrEditMembershipTypeDto>(membershipType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMembershipTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_MembershipTypes_Create)]
        protected virtual async Task Create(CreateOrEditMembershipTypeDto input)
        {
            var membershipType = ObjectMapper.Map<MembershipType>(input);

            if (AbpSession.TenantId != null)
            {
                membershipType.TenantId = (int?)AbpSession.TenantId;
            }

            await _membershipTypeRepository.InsertAsync(membershipType);

        }

        [AbpAuthorize(AppPermissions.Pages_MembershipTypes_Edit)]
        protected virtual async Task Update(CreateOrEditMembershipTypeDto input)
        {
            var membershipType = await _membershipTypeRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, membershipType);

        }

        [AbpAuthorize(AppPermissions.Pages_MembershipTypes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _membershipTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMembershipTypesToExcel(GetAllMembershipTypesForExcelInput input)
        {

            var filteredMembershipTypes = _membershipTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredMembershipTypes
                         select new GetMembershipTypeForViewDto()
                         {
                             MembershipType = new MembershipTypeDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Id = o.Id
                             }
                         });

            var membershipTypeListDtos = await query.ToListAsync();

            return _membershipTypesExcelExporter.ExportToFile(membershipTypeListDtos);
        }

    }
}