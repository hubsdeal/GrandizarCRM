using SoftGrid.Shop;

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
    [AbpAuthorize(AppPermissions.Pages_StoreNotes)]
    public class StoreNotesAppService : SoftGridAppServiceBase, IStoreNotesAppService
    {
        private readonly IRepository<StoreNote, long> _storeNoteRepository;
        private readonly IStoreNotesExcelExporter _storeNotesExcelExporter;
        private readonly IRepository<Store, long> _lookup_storeRepository;

        public StoreNotesAppService(IRepository<StoreNote, long> storeNoteRepository, IStoreNotesExcelExporter storeNotesExcelExporter, IRepository<Store, long> lookup_storeRepository)
        {
            _storeNoteRepository = storeNoteRepository;
            _storeNotesExcelExporter = storeNotesExcelExporter;
            _lookup_storeRepository = lookup_storeRepository;

        }

        public async Task<PagedResultDto<GetStoreNoteForViewDto>> GetAll(GetAllStoreNotesInput input)
        {

            var filteredStoreNotes = _storeNoteRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Notes.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes.Contains(input.NotesFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var pagedAndFilteredStoreNotes = filteredStoreNotes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeNotes = from o in pagedAndFilteredStoreNotes
                             join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             select new
                             {

                                 o.Notes,
                                 Id = o.Id,
                                 StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                             };

            var totalCount = await filteredStoreNotes.CountAsync();

            var dbList = await storeNotes.ToListAsync();
            var results = new List<GetStoreNoteForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreNoteForViewDto()
                {
                    StoreNote = new StoreNoteDto
                    {

                        Notes = o.Notes,
                        Id = o.Id,
                    },
                    StoreName = o.StoreName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreNoteForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreNoteForViewDto> GetStoreNoteForView(long id)
        {
            var storeNote = await _storeNoteRepository.GetAsync(id);

            var output = new GetStoreNoteForViewDto { StoreNote = ObjectMapper.Map<StoreNoteDto>(storeNote) };

            if (output.StoreNote.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreNote.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreNotes_Edit)]
        public async Task<GetStoreNoteForEditOutput> GetStoreNoteForEdit(EntityDto<long> input)
        {
            var storeNote = await _storeNoteRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreNoteForEditOutput { StoreNote = ObjectMapper.Map<CreateOrEditStoreNoteDto>(storeNote) };

            if (output.StoreNote.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreNote.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreNoteDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreNotes_Create)]
        protected virtual async Task Create(CreateOrEditStoreNoteDto input)
        {
            var storeNote = ObjectMapper.Map<StoreNote>(input);

            if (AbpSession.TenantId != null)
            {
                storeNote.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeNoteRepository.InsertAsync(storeNote);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreNotes_Edit)]
        protected virtual async Task Update(CreateOrEditStoreNoteDto input)
        {
            var storeNote = await _storeNoteRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeNote);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreNotes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeNoteRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreNotesToExcel(GetAllStoreNotesForExcelInput input)
        {

            var filteredStoreNotes = _storeNoteRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Notes.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes.Contains(input.NotesFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var query = (from o in filteredStoreNotes
                         join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetStoreNoteForViewDto()
                         {
                             StoreNote = new StoreNoteDto
                             {
                                 Notes = o.Notes,
                                 Id = o.Id
                             },
                             StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var storeNoteListDtos = await query.ToListAsync();

            return _storeNotesExcelExporter.ExportToFile(storeNoteListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreNotes)]
        public async Task<PagedResultDto<StoreNoteStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreNoteStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreNoteStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreNoteStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}