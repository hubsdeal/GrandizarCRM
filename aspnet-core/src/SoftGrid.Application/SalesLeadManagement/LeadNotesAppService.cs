using SoftGrid.SalesLeadManagement;

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
    [AbpAuthorize(AppPermissions.Pages_LeadNotes)]
    public class LeadNotesAppService : SoftGridAppServiceBase, ILeadNotesAppService
    {
        private readonly IRepository<LeadNote, long> _leadNoteRepository;
        private readonly ILeadNotesExcelExporter _leadNotesExcelExporter;
        private readonly IRepository<Lead, long> _lookup_leadRepository;

        public LeadNotesAppService(IRepository<LeadNote, long> leadNoteRepository, ILeadNotesExcelExporter leadNotesExcelExporter, IRepository<Lead, long> lookup_leadRepository)
        {
            _leadNoteRepository = leadNoteRepository;
            _leadNotesExcelExporter = leadNotesExcelExporter;
            _lookup_leadRepository = lookup_leadRepository;

        }

        public async Task<PagedResultDto<GetLeadNoteForViewDto>> GetAll(GetAllLeadNotesInput input)
        {

            var filteredLeadNotes = _leadNoteRepository.GetAll()
                        .Include(e => e.LeadFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Notes.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes.Contains(input.NotesFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LeadTitleFilter), e => e.LeadFk != null && e.LeadFk.Title == input.LeadTitleFilter);

            var pagedAndFilteredLeadNotes = filteredLeadNotes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var leadNotes = from o in pagedAndFilteredLeadNotes
                            join o1 in _lookup_leadRepository.GetAll() on o.LeadId equals o1.Id into j1
                            from s1 in j1.DefaultIfEmpty()

                            select new
                            {

                                o.Notes,
                                Id = o.Id,
                                LeadTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString()
                            };

            var totalCount = await filteredLeadNotes.CountAsync();

            var dbList = await leadNotes.ToListAsync();
            var results = new List<GetLeadNoteForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetLeadNoteForViewDto()
                {
                    LeadNote = new LeadNoteDto
                    {

                        Notes = o.Notes,
                        Id = o.Id,
                    },
                    LeadTitle = o.LeadTitle
                };

                results.Add(res);
            }

            return new PagedResultDto<GetLeadNoteForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetLeadNoteForViewDto> GetLeadNoteForView(long id)
        {
            var leadNote = await _leadNoteRepository.GetAsync(id);

            var output = new GetLeadNoteForViewDto { LeadNote = ObjectMapper.Map<LeadNoteDto>(leadNote) };

            if (output.LeadNote.LeadId != null)
            {
                var _lookupLead = await _lookup_leadRepository.FirstOrDefaultAsync((long)output.LeadNote.LeadId);
                output.LeadTitle = _lookupLead?.Title?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_LeadNotes_Edit)]
        public async Task<GetLeadNoteForEditOutput> GetLeadNoteForEdit(EntityDto<long> input)
        {
            var leadNote = await _leadNoteRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLeadNoteForEditOutput { LeadNote = ObjectMapper.Map<CreateOrEditLeadNoteDto>(leadNote) };

            if (output.LeadNote.LeadId != null)
            {
                var _lookupLead = await _lookup_leadRepository.FirstOrDefaultAsync((long)output.LeadNote.LeadId);
                output.LeadTitle = _lookupLead?.Title?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditLeadNoteDto input)
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

        [AbpAuthorize(AppPermissions.Pages_LeadNotes_Create)]
        protected virtual async Task Create(CreateOrEditLeadNoteDto input)
        {
            var leadNote = ObjectMapper.Map<LeadNote>(input);

            if (AbpSession.TenantId != null)
            {
                leadNote.TenantId = (int?)AbpSession.TenantId;
            }

            await _leadNoteRepository.InsertAsync(leadNote);

        }

        [AbpAuthorize(AppPermissions.Pages_LeadNotes_Edit)]
        protected virtual async Task Update(CreateOrEditLeadNoteDto input)
        {
            var leadNote = await _leadNoteRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, leadNote);

        }

        [AbpAuthorize(AppPermissions.Pages_LeadNotes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _leadNoteRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetLeadNotesToExcel(GetAllLeadNotesForExcelInput input)
        {

            var filteredLeadNotes = _leadNoteRepository.GetAll()
                        .Include(e => e.LeadFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Notes.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes.Contains(input.NotesFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LeadTitleFilter), e => e.LeadFk != null && e.LeadFk.Title == input.LeadTitleFilter);

            var query = (from o in filteredLeadNotes
                         join o1 in _lookup_leadRepository.GetAll() on o.LeadId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetLeadNoteForViewDto()
                         {
                             LeadNote = new LeadNoteDto
                             {
                                 Notes = o.Notes,
                                 Id = o.Id
                             },
                             LeadTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString()
                         });

            var leadNoteListDtos = await query.ToListAsync();

            return _leadNotesExcelExporter.ExportToFile(leadNoteListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_LeadNotes)]
        public async Task<PagedResultDto<LeadNoteLeadLookupTableDto>> GetAllLeadForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_leadRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Title != null && e.Title.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var leadList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadNoteLeadLookupTableDto>();
            foreach (var lead in leadList)
            {
                lookupTableDtoList.Add(new LeadNoteLeadLookupTableDto
                {
                    Id = lead.Id,
                    DisplayName = lead.Title?.ToString()
                });
            }

            return new PagedResultDto<LeadNoteLeadLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}