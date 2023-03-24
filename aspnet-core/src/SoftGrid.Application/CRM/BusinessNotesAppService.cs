using SoftGrid.CRM;

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
    [AbpAuthorize(AppPermissions.Pages_BusinessNotes)]
    public class BusinessNotesAppService : SoftGridAppServiceBase, IBusinessNotesAppService
    {
        private readonly IRepository<BusinessNote, long> _businessNoteRepository;
        private readonly IBusinessNotesExcelExporter _businessNotesExcelExporter;
        private readonly IRepository<Business, long> _lookup_businessRepository;

        public BusinessNotesAppService(IRepository<BusinessNote, long> businessNoteRepository, IBusinessNotesExcelExporter businessNotesExcelExporter, IRepository<Business, long> lookup_businessRepository)
        {
            _businessNoteRepository = businessNoteRepository;
            _businessNotesExcelExporter = businessNotesExcelExporter;
            _lookup_businessRepository = lookup_businessRepository;

        }

        public async Task<PagedResultDto<GetBusinessNoteForViewDto>> GetAll(GetAllBusinessNotesInput input)
        {

            var filteredBusinessNotes = _businessNoteRepository.GetAll()
                        .Include(e => e.BusinessFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Notes.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes.Contains(input.NotesFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter);

            var pagedAndFilteredBusinessNotes = filteredBusinessNotes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var businessNotes = from o in pagedAndFilteredBusinessNotes
                                join o1 in _lookup_businessRepository.GetAll() on o.BusinessId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                select new
                                {

                                    o.Notes,
                                    Id = o.Id,
                                    BusinessName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                };

            var totalCount = await filteredBusinessNotes.CountAsync();

            var dbList = await businessNotes.ToListAsync();
            var results = new List<GetBusinessNoteForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBusinessNoteForViewDto()
                {
                    BusinessNote = new BusinessNoteDto
                    {

                        Notes = o.Notes,
                        Id = o.Id,
                    },
                    BusinessName = o.BusinessName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetBusinessNoteForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetBusinessNoteForViewDto> GetBusinessNoteForView(long id)
        {
            var businessNote = await _businessNoteRepository.GetAsync(id);

            var output = new GetBusinessNoteForViewDto { BusinessNote = ObjectMapper.Map<BusinessNoteDto>(businessNote) };

            if (output.BusinessNote.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.BusinessNote.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessNotes_Edit)]
        public async Task<GetBusinessNoteForEditOutput> GetBusinessNoteForEdit(EntityDto<long> input)
        {
            var businessNote = await _businessNoteRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBusinessNoteForEditOutput { BusinessNote = ObjectMapper.Map<CreateOrEditBusinessNoteDto>(businessNote) };

            if (output.BusinessNote.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.BusinessNote.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBusinessNoteDto input)
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

        [AbpAuthorize(AppPermissions.Pages_BusinessNotes_Create)]
        protected virtual async Task Create(CreateOrEditBusinessNoteDto input)
        {
            var businessNote = ObjectMapper.Map<BusinessNote>(input);

            if (AbpSession.TenantId != null)
            {
                businessNote.TenantId = (int?)AbpSession.TenantId;
            }

            await _businessNoteRepository.InsertAsync(businessNote);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessNotes_Edit)]
        protected virtual async Task Update(CreateOrEditBusinessNoteDto input)
        {
            var businessNote = await _businessNoteRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, businessNote);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessNotes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _businessNoteRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetBusinessNotesToExcel(GetAllBusinessNotesForExcelInput input)
        {

            var filteredBusinessNotes = _businessNoteRepository.GetAll()
                        .Include(e => e.BusinessFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Notes.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes.Contains(input.NotesFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter);

            var query = (from o in filteredBusinessNotes
                         join o1 in _lookup_businessRepository.GetAll() on o.BusinessId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetBusinessNoteForViewDto()
                         {
                             BusinessNote = new BusinessNoteDto
                             {
                                 Notes = o.Notes,
                                 Id = o.Id
                             },
                             BusinessName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var businessNoteListDtos = await query.ToListAsync();

            return _businessNotesExcelExporter.ExportToFile(businessNoteListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessNotes)]
        public async Task<PagedResultDto<BusinessNoteBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_businessRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var businessList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BusinessNoteBusinessLookupTableDto>();
            foreach (var business in businessList)
            {
                lookupTableDtoList.Add(new BusinessNoteBusinessLookupTableDto
                {
                    Id = business.Id,
                    DisplayName = business.Name?.ToString()
                });
            }

            return new PagedResultDto<BusinessNoteBusinessLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}