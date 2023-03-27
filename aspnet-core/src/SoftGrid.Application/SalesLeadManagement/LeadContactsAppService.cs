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
    [AbpAuthorize(AppPermissions.Pages_LeadContacts)]
    public class LeadContactsAppService : SoftGridAppServiceBase, ILeadContactsAppService
    {
        private readonly IRepository<LeadContact, long> _leadContactRepository;
        private readonly ILeadContactsExcelExporter _leadContactsExcelExporter;
        private readonly IRepository<Lead, long> _lookup_leadRepository;
        private readonly IRepository<Contact, long> _lookup_contactRepository;

        public LeadContactsAppService(IRepository<LeadContact, long> leadContactRepository, ILeadContactsExcelExporter leadContactsExcelExporter, IRepository<Lead, long> lookup_leadRepository, IRepository<Contact, long> lookup_contactRepository)
        {
            _leadContactRepository = leadContactRepository;
            _leadContactsExcelExporter = leadContactsExcelExporter;
            _lookup_leadRepository = lookup_leadRepository;
            _lookup_contactRepository = lookup_contactRepository;

        }

        public async Task<PagedResultDto<GetLeadContactForViewDto>> GetAll(GetAllLeadContactsInput input)
        {

            var filteredLeadContacts = _leadContactRepository.GetAll()
                        .Include(e => e.LeadFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Notes.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes.Contains(input.NotesFilter))
                        .WhereIf(input.MinInfluenceScoreFilter != null, e => e.InfluenceScore >= input.MinInfluenceScoreFilter)
                        .WhereIf(input.MaxInfluenceScoreFilter != null, e => e.InfluenceScore <= input.MaxInfluenceScoreFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LeadTitleFilter), e => e.LeadFk != null && e.LeadFk.Title == input.LeadTitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter);

            var pagedAndFilteredLeadContacts = filteredLeadContacts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var leadContacts = from o in pagedAndFilteredLeadContacts
                               join o1 in _lookup_leadRepository.GetAll() on o.LeadId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()

                               join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                               from s2 in j2.DefaultIfEmpty()

                               select new
                               {

                                   o.Notes,
                                   o.InfluenceScore,
                                   Id = o.Id,
                                   LeadTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString(),
                                   ContactFullName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString()
                               };

            var totalCount = await filteredLeadContacts.CountAsync();

            var dbList = await leadContacts.ToListAsync();
            var results = new List<GetLeadContactForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetLeadContactForViewDto()
                {
                    LeadContact = new LeadContactDto
                    {

                        Notes = o.Notes,
                        InfluenceScore = o.InfluenceScore,
                        Id = o.Id,
                    },
                    LeadTitle = o.LeadTitle,
                    ContactFullName = o.ContactFullName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetLeadContactForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetLeadContactForViewDto> GetLeadContactForView(long id)
        {
            var leadContact = await _leadContactRepository.GetAsync(id);

            var output = new GetLeadContactForViewDto { LeadContact = ObjectMapper.Map<LeadContactDto>(leadContact) };

            if (output.LeadContact.LeadId != null)
            {
                var _lookupLead = await _lookup_leadRepository.FirstOrDefaultAsync((long)output.LeadContact.LeadId);
                output.LeadTitle = _lookupLead?.Title?.ToString();
            }

            if (output.LeadContact.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.LeadContact.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_LeadContacts_Edit)]
        public async Task<GetLeadContactForEditOutput> GetLeadContactForEdit(EntityDto<long> input)
        {
            var leadContact = await _leadContactRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLeadContactForEditOutput { LeadContact = ObjectMapper.Map<CreateOrEditLeadContactDto>(leadContact) };

            if (output.LeadContact.LeadId != null)
            {
                var _lookupLead = await _lookup_leadRepository.FirstOrDefaultAsync((long)output.LeadContact.LeadId);
                output.LeadTitle = _lookupLead?.Title?.ToString();
            }

            if (output.LeadContact.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.LeadContact.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditLeadContactDto input)
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

        [AbpAuthorize(AppPermissions.Pages_LeadContacts_Create)]
        protected virtual async Task Create(CreateOrEditLeadContactDto input)
        {
            var leadContact = ObjectMapper.Map<LeadContact>(input);

            if (AbpSession.TenantId != null)
            {
                leadContact.TenantId = (int?)AbpSession.TenantId;
            }

            await _leadContactRepository.InsertAsync(leadContact);

        }

        [AbpAuthorize(AppPermissions.Pages_LeadContacts_Edit)]
        protected virtual async Task Update(CreateOrEditLeadContactDto input)
        {
            var leadContact = await _leadContactRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, leadContact);

        }

        [AbpAuthorize(AppPermissions.Pages_LeadContacts_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _leadContactRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetLeadContactsToExcel(GetAllLeadContactsForExcelInput input)
        {

            var filteredLeadContacts = _leadContactRepository.GetAll()
                        .Include(e => e.LeadFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Notes.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes.Contains(input.NotesFilter))
                        .WhereIf(input.MinInfluenceScoreFilter != null, e => e.InfluenceScore >= input.MinInfluenceScoreFilter)
                        .WhereIf(input.MaxInfluenceScoreFilter != null, e => e.InfluenceScore <= input.MaxInfluenceScoreFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LeadTitleFilter), e => e.LeadFk != null && e.LeadFk.Title == input.LeadTitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter);

            var query = (from o in filteredLeadContacts
                         join o1 in _lookup_leadRepository.GetAll() on o.LeadId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetLeadContactForViewDto()
                         {
                             LeadContact = new LeadContactDto
                             {
                                 Notes = o.Notes,
                                 InfluenceScore = o.InfluenceScore,
                                 Id = o.Id
                             },
                             LeadTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString(),
                             ContactFullName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString()
                         });

            var leadContactListDtos = await query.ToListAsync();

            return _leadContactsExcelExporter.ExportToFile(leadContactListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_LeadContacts)]
        public async Task<PagedResultDto<LeadContactLeadLookupTableDto>> GetAllLeadForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_leadRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Title != null && e.Title.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var leadList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadContactLeadLookupTableDto>();
            foreach (var lead in leadList)
            {
                lookupTableDtoList.Add(new LeadContactLeadLookupTableDto
                {
                    Id = lead.Id,
                    DisplayName = lead.Title?.ToString()
                });
            }

            return new PagedResultDto<LeadContactLeadLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_LeadContacts)]
        public async Task<PagedResultDto<LeadContactContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadContactContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new LeadContactContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<LeadContactContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}