using SoftGrid.SalesLeadManagement;
using SoftGrid.CRM;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.SalesLeadManagement.Exporting;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.SalesLeadManagement
{
    [AbpAuthorize(AppPermissions.Pages_LeadReferralRewards)]
    public class LeadReferralRewardsAppService : SoftGridAppServiceBase, ILeadReferralRewardsAppService
    {
        private readonly IRepository<LeadReferralReward, long> _leadReferralRewardRepository;
        private readonly ILeadReferralRewardsExcelExporter _leadReferralRewardsExcelExporter;
        private readonly IRepository<Lead, long> _lookup_leadRepository;
        private readonly IRepository<Contact, long> _lookup_contactRepository;

        public LeadReferralRewardsAppService(IRepository<LeadReferralReward, long> leadReferralRewardRepository, ILeadReferralRewardsExcelExporter leadReferralRewardsExcelExporter, IRepository<Lead, long> lookup_leadRepository, IRepository<Contact, long> lookup_contactRepository)
        {
            _leadReferralRewardRepository = leadReferralRewardRepository;
            _leadReferralRewardsExcelExporter = leadReferralRewardsExcelExporter;
            _lookup_leadRepository = lookup_leadRepository;
            _lookup_contactRepository = lookup_contactRepository;

        }

        public async Task<PagedResultDto<GetLeadReferralRewardForViewDto>> GetAll(GetAllLeadReferralRewardsInput input)
        {

            var filteredLeadReferralRewards = _leadReferralRewardRepository.GetAll()
                        .Include(e => e.LeadFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) || e.Phone.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.RewardType.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter), e => e.FirstName.Contains(input.FirstNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter), e => e.LastName.Contains(input.LastNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneFilter), e => e.Phone.Contains(input.PhoneFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RewardTypeFilter), e => e.RewardType.Contains(input.RewardTypeFilter))
                        .WhereIf(input.MinRewardAmountFilter != null, e => e.RewardAmount >= input.MinRewardAmountFilter)
                        .WhereIf(input.MaxRewardAmountFilter != null, e => e.RewardAmount <= input.MaxRewardAmountFilter)
                        .WhereIf(input.RewardStatusFilter.HasValue && input.RewardStatusFilter > -1, e => (input.RewardStatusFilter == 1 && e.RewardStatus) || (input.RewardStatusFilter == 0 && !e.RewardStatus))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LeadTitleFilter), e => e.LeadFk != null && e.LeadFk.Title == input.LeadTitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter);

            var pagedAndFilteredLeadReferralRewards = filteredLeadReferralRewards
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var leadReferralRewards = from o in pagedAndFilteredLeadReferralRewards
                                      join o1 in _lookup_leadRepository.GetAll() on o.LeadId equals o1.Id into j1
                                      from s1 in j1.DefaultIfEmpty()

                                      join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                                      from s2 in j2.DefaultIfEmpty()

                                      select new
                                      {

                                          o.FirstName,
                                          o.LastName,
                                          o.Phone,
                                          o.Email,
                                          o.RewardType,
                                          o.RewardAmount,
                                          o.RewardStatus,
                                          Id = o.Id,
                                          LeadTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString(),
                                          ContactFullName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString()
                                      };

            var totalCount = await filteredLeadReferralRewards.CountAsync();

            var dbList = await leadReferralRewards.ToListAsync();
            var results = new List<GetLeadReferralRewardForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetLeadReferralRewardForViewDto()
                {
                    LeadReferralReward = new LeadReferralRewardDto
                    {

                        FirstName = o.FirstName,
                        LastName = o.LastName,
                        Phone = o.Phone,
                        Email = o.Email,
                        RewardType = o.RewardType,
                        RewardAmount = o.RewardAmount,
                        RewardStatus = o.RewardStatus,
                        Id = o.Id,
                    },
                    LeadTitle = o.LeadTitle,
                    ContactFullName = o.ContactFullName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetLeadReferralRewardForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetLeadReferralRewardForViewDto> GetLeadReferralRewardForView(long id)
        {
            var leadReferralReward = await _leadReferralRewardRepository.GetAsync(id);

            var output = new GetLeadReferralRewardForViewDto { LeadReferralReward = ObjectMapper.Map<LeadReferralRewardDto>(leadReferralReward) };

            if (output.LeadReferralReward.LeadId != null)
            {
                var _lookupLead = await _lookup_leadRepository.FirstOrDefaultAsync((long)output.LeadReferralReward.LeadId);
                output.LeadTitle = _lookupLead?.Title?.ToString();
            }

            if (output.LeadReferralReward.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.LeadReferralReward.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_LeadReferralRewards_Edit)]
        public async Task<GetLeadReferralRewardForEditOutput> GetLeadReferralRewardForEdit(EntityDto<long> input)
        {
            var leadReferralReward = await _leadReferralRewardRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLeadReferralRewardForEditOutput { LeadReferralReward = ObjectMapper.Map<CreateOrEditLeadReferralRewardDto>(leadReferralReward) };

            if (output.LeadReferralReward.LeadId != null)
            {
                var _lookupLead = await _lookup_leadRepository.FirstOrDefaultAsync((long)output.LeadReferralReward.LeadId);
                output.LeadTitle = _lookupLead?.Title?.ToString();
            }

            if (output.LeadReferralReward.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.LeadReferralReward.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditLeadReferralRewardDto input)
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

        [AbpAuthorize(AppPermissions.Pages_LeadReferralRewards_Create)]
        protected virtual async Task Create(CreateOrEditLeadReferralRewardDto input)
        {
            var leadReferralReward = ObjectMapper.Map<LeadReferralReward>(input);

            if (AbpSession.TenantId != null)
            {
                leadReferralReward.TenantId = (int?)AbpSession.TenantId;
            }

            await _leadReferralRewardRepository.InsertAsync(leadReferralReward);

        }

        [AbpAuthorize(AppPermissions.Pages_LeadReferralRewards_Edit)]
        protected virtual async Task Update(CreateOrEditLeadReferralRewardDto input)
        {
            var leadReferralReward = await _leadReferralRewardRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, leadReferralReward);

        }

        [AbpAuthorize(AppPermissions.Pages_LeadReferralRewards_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _leadReferralRewardRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetLeadReferralRewardsToExcel(GetAllLeadReferralRewardsForExcelInput input)
        {

            var filteredLeadReferralRewards = _leadReferralRewardRepository.GetAll()
                        .Include(e => e.LeadFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) || e.Phone.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.RewardType.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter), e => e.FirstName.Contains(input.FirstNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter), e => e.LastName.Contains(input.LastNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneFilter), e => e.Phone.Contains(input.PhoneFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RewardTypeFilter), e => e.RewardType.Contains(input.RewardTypeFilter))
                        .WhereIf(input.MinRewardAmountFilter != null, e => e.RewardAmount >= input.MinRewardAmountFilter)
                        .WhereIf(input.MaxRewardAmountFilter != null, e => e.RewardAmount <= input.MaxRewardAmountFilter)
                        .WhereIf(input.RewardStatusFilter.HasValue && input.RewardStatusFilter > -1, e => (input.RewardStatusFilter == 1 && e.RewardStatus) || (input.RewardStatusFilter == 0 && !e.RewardStatus))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LeadTitleFilter), e => e.LeadFk != null && e.LeadFk.Title == input.LeadTitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter);

            var query = (from o in filteredLeadReferralRewards
                         join o1 in _lookup_leadRepository.GetAll() on o.LeadId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetLeadReferralRewardForViewDto()
                         {
                             LeadReferralReward = new LeadReferralRewardDto
                             {
                                 FirstName = o.FirstName,
                                 LastName = o.LastName,
                                 Phone = o.Phone,
                                 Email = o.Email,
                                 RewardType = o.RewardType,
                                 RewardAmount = o.RewardAmount,
                                 RewardStatus = o.RewardStatus,
                                 Id = o.Id
                             },
                             LeadTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString(),
                             ContactFullName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString()
                         });

            var leadReferralRewardListDtos = await query.ToListAsync();

            return _leadReferralRewardsExcelExporter.ExportToFile(leadReferralRewardListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_LeadReferralRewards)]
        public async Task<PagedResultDto<LeadReferralRewardLeadLookupTableDto>> GetAllLeadForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_leadRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Title != null && e.Title.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var leadList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadReferralRewardLeadLookupTableDto>();
            foreach (var lead in leadList)
            {
                lookupTableDtoList.Add(new LeadReferralRewardLeadLookupTableDto
                {
                    Id = lead.Id,
                    DisplayName = lead.Title?.ToString()
                });
            }

            return new PagedResultDto<LeadReferralRewardLeadLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_LeadReferralRewards)]
        public async Task<PagedResultDto<LeadReferralRewardContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadReferralRewardContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new LeadReferralRewardContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<LeadReferralRewardContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}