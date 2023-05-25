using SoftGrid.CRM;
using SoftGrid.TaskManagement;

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
    [AbpAuthorize(AppPermissions.Pages_ContactTaskMaps)]
    public class ContactTaskMapsAppService : SoftGridAppServiceBase, IContactTaskMapsAppService
    {
        private readonly IRepository<ContactTaskMap, long> _contactTaskMapRepository;
        private readonly IContactTaskMapsExcelExporter _contactTaskMapsExcelExporter;
        private readonly IRepository<Contact, long> _lookup_contactRepository;
        private readonly IRepository<TaskEvent, long> _lookup_taskEventRepository;

        public ContactTaskMapsAppService(IRepository<ContactTaskMap, long> contactTaskMapRepository, IContactTaskMapsExcelExporter contactTaskMapsExcelExporter, IRepository<Contact, long> lookup_contactRepository, IRepository<TaskEvent, long> lookup_taskEventRepository)
        {
            _contactTaskMapRepository = contactTaskMapRepository;
            _contactTaskMapsExcelExporter = contactTaskMapsExcelExporter;
            _lookup_contactRepository = lookup_contactRepository;
            _lookup_taskEventRepository = lookup_taskEventRepository;

        }

        public async Task<PagedResultDto<GetContactTaskMapForViewDto>> GetAll(GetAllContactTaskMapsInput input)
        {

            var filteredContactTaskMaps = _contactTaskMapRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .Include(e => e.TaskEventFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskEventNameFilter), e => e.TaskEventFk != null && e.TaskEventFk.Name == input.TaskEventNameFilter);

            var pagedAndFilteredContactTaskMaps = filteredContactTaskMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var contactTaskMaps = from o in pagedAndFilteredContactTaskMaps
                                  join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                                  from s1 in j1.DefaultIfEmpty()

                                  join o2 in _lookup_taskEventRepository.GetAll() on o.TaskEventId equals o2.Id into j2
                                  from s2 in j2.DefaultIfEmpty()

                                  select new
                                  {

                                      Id = o.Id,
                                      ContactFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                                      TaskEventName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                  };

            var totalCount = await filteredContactTaskMaps.CountAsync();

            var dbList = await contactTaskMaps.ToListAsync();
            var results = new List<GetContactTaskMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetContactTaskMapForViewDto()
                {
                    ContactTaskMap = new ContactTaskMapDto
                    {

                        Id = o.Id,
                    },
                    ContactFullName = o.ContactFullName,
                    TaskEventName = o.TaskEventName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetContactTaskMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetContactTaskMapForViewDto> GetContactTaskMapForView(long id)
        {
            var contactTaskMap = await _contactTaskMapRepository.GetAsync(id);

            var output = new GetContactTaskMapForViewDto { ContactTaskMap = ObjectMapper.Map<ContactTaskMapDto>(contactTaskMap) };

            if (output.ContactTaskMap.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.ContactTaskMap.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.ContactTaskMap.TaskEventId != null)
            {
                var _lookupTaskEvent = await _lookup_taskEventRepository.FirstOrDefaultAsync((long)output.ContactTaskMap.TaskEventId);
                output.TaskEventName = _lookupTaskEvent?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ContactTaskMaps_Edit)]
        public async Task<GetContactTaskMapForEditOutput> GetContactTaskMapForEdit(EntityDto<long> input)
        {
            var contactTaskMap = await _contactTaskMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetContactTaskMapForEditOutput { ContactTaskMap = ObjectMapper.Map<CreateOrEditContactTaskMapDto>(contactTaskMap) };

            if (output.ContactTaskMap.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.ContactTaskMap.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.ContactTaskMap.TaskEventId != null)
            {
                var _lookupTaskEvent = await _lookup_taskEventRepository.FirstOrDefaultAsync((long)output.ContactTaskMap.TaskEventId);
                output.TaskEventName = _lookupTaskEvent?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditContactTaskMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ContactTaskMaps_Create)]
        protected virtual async Task Create(CreateOrEditContactTaskMapDto input)
        {
            var contactTaskMap = ObjectMapper.Map<ContactTaskMap>(input);

            if (AbpSession.TenantId != null)
            {
                contactTaskMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _contactTaskMapRepository.InsertAsync(contactTaskMap);

        }

        [AbpAuthorize(AppPermissions.Pages_ContactTaskMaps_Edit)]
        protected virtual async Task Update(CreateOrEditContactTaskMapDto input)
        {
            var contactTaskMap = await _contactTaskMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, contactTaskMap);

        }

        [AbpAuthorize(AppPermissions.Pages_ContactTaskMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _contactTaskMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetContactTaskMapsToExcel(GetAllContactTaskMapsForExcelInput input)
        {

            var filteredContactTaskMaps = _contactTaskMapRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .Include(e => e.TaskEventFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskEventNameFilter), e => e.TaskEventFk != null && e.TaskEventFk.Name == input.TaskEventNameFilter);

            var query = (from o in filteredContactTaskMaps
                         join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_taskEventRepository.GetAll() on o.TaskEventId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetContactTaskMapForViewDto()
                         {
                             ContactTaskMap = new ContactTaskMapDto
                             {
                                 Id = o.Id
                             },
                             ContactFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                             TaskEventName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var contactTaskMapListDtos = await query.ToListAsync();

            return _contactTaskMapsExcelExporter.ExportToFile(contactTaskMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ContactTaskMaps)]
        public async Task<PagedResultDto<ContactTaskMapContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ContactTaskMapContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new ContactTaskMapContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<ContactTaskMapContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ContactTaskMaps)]
        public async Task<PagedResultDto<ContactTaskMapTaskEventLookupTableDto>> GetAllTaskEventForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_taskEventRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var taskEventList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ContactTaskMapTaskEventLookupTableDto>();
            foreach (var taskEvent in taskEventList)
            {
                lookupTableDtoList.Add(new ContactTaskMapTaskEventLookupTableDto
                {
                    Id = taskEvent.Id,
                    DisplayName = taskEvent.Name?.ToString()
                });
            }

            return new PagedResultDto<ContactTaskMapTaskEventLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}