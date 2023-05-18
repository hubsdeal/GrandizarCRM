using SoftGrid.JobManagement;
using SoftGrid.LookupData;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.JobManagement.Exporting;
using SoftGrid.JobManagement.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.JobManagement
{
    [AbpAuthorize(AppPermissions.Pages_JobDocuments)]
    public class JobDocumentsAppService : SoftGridAppServiceBase, IJobDocumentsAppService
    {
        private readonly IRepository<JobDocument, long> _jobDocumentRepository;
        private readonly IJobDocumentsExcelExporter _jobDocumentsExcelExporter;
        private readonly IRepository<Job, long> _lookup_jobRepository;
        private readonly IRepository<DocumentType, long> _lookup_documentTypeRepository;

        public JobDocumentsAppService(IRepository<JobDocument, long> jobDocumentRepository, IJobDocumentsExcelExporter jobDocumentsExcelExporter, IRepository<Job, long> lookup_jobRepository, IRepository<DocumentType, long> lookup_documentTypeRepository)
        {
            _jobDocumentRepository = jobDocumentRepository;
            _jobDocumentsExcelExporter = jobDocumentsExcelExporter;
            _lookup_jobRepository = lookup_jobRepository;
            _lookup_documentTypeRepository = lookup_documentTypeRepository;

        }

        public async Task<PagedResultDto<GetJobDocumentForViewDto>> GetAll(GetAllJobDocumentsInput input)
        {

            var filteredJobDocuments = _jobDocumentRepository.GetAll()
                        .Include(e => e.JobFk)
                        .Include(e => e.DocumentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentTitle.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTitleFilter), e => e.DocumentTitle.Contains(input.DocumentTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileBinaryObjectIdFilter.ToString()), e => e.FileBinaryObjectId.ToString() == input.FileBinaryObjectIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobTitleFilter), e => e.JobFk != null && e.JobFk.Title == input.JobTitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTypeNameFilter), e => e.DocumentTypeFk != null && e.DocumentTypeFk.Name == input.DocumentTypeNameFilter);

            var pagedAndFilteredJobDocuments = filteredJobDocuments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var jobDocuments = from o in pagedAndFilteredJobDocuments
                               join o1 in _lookup_jobRepository.GetAll() on o.JobId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()

                               join o2 in _lookup_documentTypeRepository.GetAll() on o.DocumentTypeId equals o2.Id into j2
                               from s2 in j2.DefaultIfEmpty()

                               select new
                               {

                                   o.DocumentTitle,
                                   o.FileBinaryObjectId,
                                   Id = o.Id,
                                   JobTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString(),
                                   DocumentTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                               };

            var totalCount = await filteredJobDocuments.CountAsync();

            var dbList = await jobDocuments.ToListAsync();
            var results = new List<GetJobDocumentForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetJobDocumentForViewDto()
                {
                    JobDocument = new JobDocumentDto
                    {

                        DocumentTitle = o.DocumentTitle,
                        FileBinaryObjectId = o.FileBinaryObjectId,
                        Id = o.Id,
                    },
                    JobTitle = o.JobTitle,
                    DocumentTypeName = o.DocumentTypeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetJobDocumentForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetJobDocumentForViewDto> GetJobDocumentForView(long id)
        {
            var jobDocument = await _jobDocumentRepository.GetAsync(id);

            var output = new GetJobDocumentForViewDto { JobDocument = ObjectMapper.Map<JobDocumentDto>(jobDocument) };

            if (output.JobDocument.JobId != null)
            {
                var _lookupJob = await _lookup_jobRepository.FirstOrDefaultAsync((long)output.JobDocument.JobId);
                output.JobTitle = _lookupJob?.Title?.ToString();
            }

            if (output.JobDocument.DocumentTypeId != null)
            {
                var _lookupDocumentType = await _lookup_documentTypeRepository.FirstOrDefaultAsync((long)output.JobDocument.DocumentTypeId);
                output.DocumentTypeName = _lookupDocumentType?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_JobDocuments_Edit)]
        public async Task<GetJobDocumentForEditOutput> GetJobDocumentForEdit(EntityDto<long> input)
        {
            var jobDocument = await _jobDocumentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetJobDocumentForEditOutput { JobDocument = ObjectMapper.Map<CreateOrEditJobDocumentDto>(jobDocument) };

            if (output.JobDocument.JobId != null)
            {
                var _lookupJob = await _lookup_jobRepository.FirstOrDefaultAsync((long)output.JobDocument.JobId);
                output.JobTitle = _lookupJob?.Title?.ToString();
            }

            if (output.JobDocument.DocumentTypeId != null)
            {
                var _lookupDocumentType = await _lookup_documentTypeRepository.FirstOrDefaultAsync((long)output.JobDocument.DocumentTypeId);
                output.DocumentTypeName = _lookupDocumentType?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditJobDocumentDto input)
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

        [AbpAuthorize(AppPermissions.Pages_JobDocuments_Create)]
        protected virtual async Task Create(CreateOrEditJobDocumentDto input)
        {
            var jobDocument = ObjectMapper.Map<JobDocument>(input);

            if (AbpSession.TenantId != null)
            {
                jobDocument.TenantId = (int?)AbpSession.TenantId;
            }

            await _jobDocumentRepository.InsertAsync(jobDocument);

        }

        [AbpAuthorize(AppPermissions.Pages_JobDocuments_Edit)]
        protected virtual async Task Update(CreateOrEditJobDocumentDto input)
        {
            var jobDocument = await _jobDocumentRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, jobDocument);

        }

        [AbpAuthorize(AppPermissions.Pages_JobDocuments_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _jobDocumentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetJobDocumentsToExcel(GetAllJobDocumentsForExcelInput input)
        {

            var filteredJobDocuments = _jobDocumentRepository.GetAll()
                        .Include(e => e.JobFk)
                        .Include(e => e.DocumentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentTitle.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTitleFilter), e => e.DocumentTitle.Contains(input.DocumentTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileBinaryObjectIdFilter.ToString()), e => e.FileBinaryObjectId.ToString() == input.FileBinaryObjectIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobTitleFilter), e => e.JobFk != null && e.JobFk.Title == input.JobTitleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTypeNameFilter), e => e.DocumentTypeFk != null && e.DocumentTypeFk.Name == input.DocumentTypeNameFilter);

            var query = (from o in filteredJobDocuments
                         join o1 in _lookup_jobRepository.GetAll() on o.JobId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_documentTypeRepository.GetAll() on o.DocumentTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetJobDocumentForViewDto()
                         {
                             JobDocument = new JobDocumentDto
                             {
                                 DocumentTitle = o.DocumentTitle,
                                 FileBinaryObjectId = o.FileBinaryObjectId,
                                 Id = o.Id
                             },
                             JobTitle = s1 == null || s1.Title == null ? "" : s1.Title.ToString(),
                             DocumentTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var jobDocumentListDtos = await query.ToListAsync();

            return _jobDocumentsExcelExporter.ExportToFile(jobDocumentListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_JobDocuments)]
        public async Task<PagedResultDto<JobDocumentJobLookupTableDto>> GetAllJobForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_jobRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Title != null && e.Title.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var jobList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<JobDocumentJobLookupTableDto>();
            foreach (var job in jobList)
            {
                lookupTableDtoList.Add(new JobDocumentJobLookupTableDto
                {
                    Id = job.Id,
                    DisplayName = job.Title?.ToString()
                });
            }

            return new PagedResultDto<JobDocumentJobLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_JobDocuments)]
        public async Task<PagedResultDto<JobDocumentDocumentTypeLookupTableDto>> GetAllDocumentTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_documentTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var documentTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<JobDocumentDocumentTypeLookupTableDto>();
            foreach (var documentType in documentTypeList)
            {
                lookupTableDtoList.Add(new JobDocumentDocumentTypeLookupTableDto
                {
                    Id = documentType.Id,
                    DisplayName = documentType.Name?.ToString()
                });
            }

            return new PagedResultDto<JobDocumentDocumentTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}