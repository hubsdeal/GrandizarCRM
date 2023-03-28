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
    [AbpAuthorize(AppPermissions.Pages_HubContacts)]
    public class HubContactsAppService : SoftGridAppServiceBase, IHubContactsAppService
    {
        private readonly IRepository<HubContact, long> _hubContactRepository;
        private readonly IHubContactsExcelExporter _hubContactsExcelExporter;
        private readonly IRepository<Hub, long> _lookup_hubRepository;
        private readonly IRepository<Contact, long> _lookup_contactRepository;

        public HubContactsAppService(IRepository<HubContact, long> hubContactRepository, IHubContactsExcelExporter hubContactsExcelExporter, IRepository<Hub, long> lookup_hubRepository, IRepository<Contact, long> lookup_contactRepository)
        {
            _hubContactRepository = hubContactRepository;
            _hubContactsExcelExporter = hubContactsExcelExporter;
            _lookup_hubRepository = lookup_hubRepository;
            _lookup_contactRepository = lookup_contactRepository;

        }

        public async Task<PagedResultDto<GetHubContactForViewDto>> GetAll(GetAllHubContactsInput input)
        {

            var filteredHubContacts = _hubContactRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDisplayScoreFilter != null, e => e.DisplayScore >= input.MinDisplayScoreFilter)
                        .WhereIf(input.MaxDisplayScoreFilter != null, e => e.DisplayScore <= input.MaxDisplayScoreFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter);

            var pagedAndFilteredHubContacts = filteredHubContacts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var hubContacts = from o in pagedAndFilteredHubContacts
                              join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                              from s1 in j1.DefaultIfEmpty()

                              join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                              from s2 in j2.DefaultIfEmpty()

                              select new
                              {

                                  o.DisplayScore,
                                  Id = o.Id,
                                  HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                  ContactFullName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString()
                              };

            var totalCount = await filteredHubContacts.CountAsync();

            var dbList = await hubContacts.ToListAsync();
            var results = new List<GetHubContactForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetHubContactForViewDto()
                {
                    HubContact = new HubContactDto
                    {

                        DisplayScore = o.DisplayScore,
                        Id = o.Id,
                    },
                    HubName = o.HubName,
                    ContactFullName = o.ContactFullName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetHubContactForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetHubContactForViewDto> GetHubContactForView(long id)
        {
            var hubContact = await _hubContactRepository.GetAsync(id);

            var output = new GetHubContactForViewDto { HubContact = ObjectMapper.Map<HubContactDto>(hubContact) };

            if (output.HubContact.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.HubContact.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.HubContact.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.HubContact.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_HubContacts_Edit)]
        public async Task<GetHubContactForEditOutput> GetHubContactForEdit(EntityDto<long> input)
        {
            var hubContact = await _hubContactRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHubContactForEditOutput { HubContact = ObjectMapper.Map<CreateOrEditHubContactDto>(hubContact) };

            if (output.HubContact.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.HubContact.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.HubContact.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.HubContact.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHubContactDto input)
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

        [AbpAuthorize(AppPermissions.Pages_HubContacts_Create)]
        protected virtual async Task Create(CreateOrEditHubContactDto input)
        {
            var hubContact = ObjectMapper.Map<HubContact>(input);

            if (AbpSession.TenantId != null)
            {
                hubContact.TenantId = (int?)AbpSession.TenantId;
            }

            await _hubContactRepository.InsertAsync(hubContact);

        }

        [AbpAuthorize(AppPermissions.Pages_HubContacts_Edit)]
        protected virtual async Task Update(CreateOrEditHubContactDto input)
        {
            var hubContact = await _hubContactRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, hubContact);

        }

        [AbpAuthorize(AppPermissions.Pages_HubContacts_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _hubContactRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetHubContactsToExcel(GetAllHubContactsForExcelInput input)
        {

            var filteredHubContacts = _hubContactRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDisplayScoreFilter != null, e => e.DisplayScore >= input.MinDisplayScoreFilter)
                        .WhereIf(input.MaxDisplayScoreFilter != null, e => e.DisplayScore <= input.MaxDisplayScoreFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter);

            var query = (from o in filteredHubContacts
                         join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetHubContactForViewDto()
                         {
                             HubContact = new HubContactDto
                             {
                                 DisplayScore = o.DisplayScore,
                                 Id = o.Id
                             },
                             HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ContactFullName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString()
                         });

            var hubContactListDtos = await query.ToListAsync();

            return _hubContactsExcelExporter.ExportToFile(hubContactListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_HubContacts)]
        public async Task<PagedResultDto<HubContactHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_hubRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var hubList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubContactHubLookupTableDto>();
            foreach (var hub in hubList)
            {
                lookupTableDtoList.Add(new HubContactHubLookupTableDto
                {
                    Id = hub.Id,
                    DisplayName = hub.Name?.ToString()
                });
            }

            return new PagedResultDto<HubContactHubLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HubContacts)]
        public async Task<PagedResultDto<HubContactContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubContactContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new HubContactContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<HubContactContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}