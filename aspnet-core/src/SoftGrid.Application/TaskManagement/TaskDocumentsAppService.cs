using SoftGrid.TaskManagement;
using SoftGrid.LookupData;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.TaskManagement.Exporting;
using SoftGrid.TaskManagement.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.TaskManagement
{
    [AbpAuthorize(AppPermissions.Pages_TaskDocuments)]
    public class TaskDocumentsAppService : SoftGridAppServiceBase, ITaskDocumentsAppService
    {
        private readonly IRepository<TaskDocument, long> _taskDocumentRepository;
        private readonly ITaskDocumentsExcelExporter _taskDocumentsExcelExporter;
        private readonly IRepository<TaskEvent, long> _lookup_taskEventRepository;
        private readonly IRepository<DocumentType, long> _lookup_documentTypeRepository;

        public TaskDocumentsAppService(IRepository<TaskDocument, long> taskDocumentRepository, ITaskDocumentsExcelExporter taskDocumentsExcelExporter, IRepository<TaskEvent, long> lookup_taskEventRepository, IRepository<DocumentType, long> lookup_documentTypeRepository)
        {
            _taskDocumentRepository = taskDocumentRepository;
            _taskDocumentsExcelExporter = taskDocumentsExcelExporter;
            _lookup_taskEventRepository = lookup_taskEventRepository;
            _lookup_documentTypeRepository = lookup_documentTypeRepository;

        }

        public async Task<PagedResultDto<GetTaskDocumentForViewDto>> GetAll(GetAllTaskDocumentsInput input)
        {

            var filteredTaskDocuments = _taskDocumentRepository.GetAll()
                        .Include(e => e.TaskEventFk)
                        .Include(e => e.DocumentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentTitle.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTitleFilter), e => e.DocumentTitle.Contains(input.DocumentTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileBinaryObjectIdFilter.ToString()), e => e.FileBinaryObjectId.ToString() == input.FileBinaryObjectIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskEventNameFilter), e => e.TaskEventFk != null && e.TaskEventFk.Name == input.TaskEventNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTypeNameFilter), e => e.DocumentTypeFk != null && e.DocumentTypeFk.Name == input.DocumentTypeNameFilter);

            var pagedAndFilteredTaskDocuments = filteredTaskDocuments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var taskDocuments = from o in pagedAndFilteredTaskDocuments
                                join o1 in _lookup_taskEventRepository.GetAll() on o.TaskEventId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                join o2 in _lookup_documentTypeRepository.GetAll() on o.DocumentTypeId equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()

                                select new
                                {

                                    o.DocumentTitle,
                                    o.FileBinaryObjectId,
                                    Id = o.Id,
                                    TaskEventName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                    DocumentTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                };

            var totalCount = await filteredTaskDocuments.CountAsync();

            var dbList = await taskDocuments.ToListAsync();
            var results = new List<GetTaskDocumentForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTaskDocumentForViewDto()
                {
                    TaskDocument = new TaskDocumentDto
                    {

                        DocumentTitle = o.DocumentTitle,
                        FileBinaryObjectId = o.FileBinaryObjectId,
                        Id = o.Id,
                    },
                    TaskEventName = o.TaskEventName,
                    DocumentTypeName = o.DocumentTypeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTaskDocumentForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetTaskDocumentForViewDto> GetTaskDocumentForView(long id)
        {
            var taskDocument = await _taskDocumentRepository.GetAsync(id);

            var output = new GetTaskDocumentForViewDto { TaskDocument = ObjectMapper.Map<TaskDocumentDto>(taskDocument) };

            if (output.TaskDocument.TaskEventId != null)
            {
                var _lookupTaskEvent = await _lookup_taskEventRepository.FirstOrDefaultAsync((long)output.TaskDocument.TaskEventId);
                output.TaskEventName = _lookupTaskEvent?.Name?.ToString();
            }

            if (output.TaskDocument.DocumentTypeId != null)
            {
                var _lookupDocumentType = await _lookup_documentTypeRepository.FirstOrDefaultAsync((long)output.TaskDocument.DocumentTypeId);
                output.DocumentTypeName = _lookupDocumentType?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TaskDocuments_Edit)]
        public async Task<GetTaskDocumentForEditOutput> GetTaskDocumentForEdit(EntityDto<long> input)
        {
            var taskDocument = await _taskDocumentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTaskDocumentForEditOutput { TaskDocument = ObjectMapper.Map<CreateOrEditTaskDocumentDto>(taskDocument) };

            if (output.TaskDocument.TaskEventId != null)
            {
                var _lookupTaskEvent = await _lookup_taskEventRepository.FirstOrDefaultAsync((long)output.TaskDocument.TaskEventId);
                output.TaskEventName = _lookupTaskEvent?.Name?.ToString();
            }

            if (output.TaskDocument.DocumentTypeId != null)
            {
                var _lookupDocumentType = await _lookup_documentTypeRepository.FirstOrDefaultAsync((long)output.TaskDocument.DocumentTypeId);
                output.DocumentTypeName = _lookupDocumentType?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTaskDocumentDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TaskDocuments_Create)]
        protected virtual async Task Create(CreateOrEditTaskDocumentDto input)
        {
            var taskDocument = ObjectMapper.Map<TaskDocument>(input);

            if (AbpSession.TenantId != null)
            {
                taskDocument.TenantId = (int?)AbpSession.TenantId;
            }

            await _taskDocumentRepository.InsertAsync(taskDocument);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskDocuments_Edit)]
        protected virtual async Task Update(CreateOrEditTaskDocumentDto input)
        {
            var taskDocument = await _taskDocumentRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, taskDocument);

        }

        [AbpAuthorize(AppPermissions.Pages_TaskDocuments_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _taskDocumentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTaskDocumentsToExcel(GetAllTaskDocumentsForExcelInput input)
        {

            var filteredTaskDocuments = _taskDocumentRepository.GetAll()
                        .Include(e => e.TaskEventFk)
                        .Include(e => e.DocumentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentTitle.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTitleFilter), e => e.DocumentTitle.Contains(input.DocumentTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileBinaryObjectIdFilter.ToString()), e => e.FileBinaryObjectId.ToString() == input.FileBinaryObjectIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaskEventNameFilter), e => e.TaskEventFk != null && e.TaskEventFk.Name == input.TaskEventNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTypeNameFilter), e => e.DocumentTypeFk != null && e.DocumentTypeFk.Name == input.DocumentTypeNameFilter);

            var query = (from o in filteredTaskDocuments
                         join o1 in _lookup_taskEventRepository.GetAll() on o.TaskEventId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_documentTypeRepository.GetAll() on o.DocumentTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetTaskDocumentForViewDto()
                         {
                             TaskDocument = new TaskDocumentDto
                             {
                                 DocumentTitle = o.DocumentTitle,
                                 FileBinaryObjectId = o.FileBinaryObjectId,
                                 Id = o.Id
                             },
                             TaskEventName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             DocumentTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var taskDocumentListDtos = await query.ToListAsync();

            return _taskDocumentsExcelExporter.ExportToFile(taskDocumentListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_TaskDocuments)]
        public async Task<PagedResultDto<TaskDocumentTaskEventLookupTableDto>> GetAllTaskEventForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_taskEventRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var taskEventList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TaskDocumentTaskEventLookupTableDto>();
            foreach (var taskEvent in taskEventList)
            {
                lookupTableDtoList.Add(new TaskDocumentTaskEventLookupTableDto
                {
                    Id = taskEvent.Id,
                    DisplayName = taskEvent.Name?.ToString()
                });
            }

            return new PagedResultDto<TaskDocumentTaskEventLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_TaskDocuments)]
        public async Task<PagedResultDto<TaskDocumentDocumentTypeLookupTableDto>> GetAllDocumentTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_documentTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var documentTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TaskDocumentDocumentTypeLookupTableDto>();
            foreach (var documentType in documentTypeList)
            {
                lookupTableDtoList.Add(new TaskDocumentDocumentTypeLookupTableDto
                {
                    Id = documentType.Id,
                    DisplayName = documentType.Name?.ToString()
                });
            }

            return new PagedResultDto<TaskDocumentDocumentTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}