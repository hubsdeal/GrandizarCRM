using SoftGrid.CRM;
using SoftGrid.LookupData;

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
    [AbpAuthorize(AppPermissions.Pages_EmployeeDocuments)]
    public class EmployeeDocumentsAppService : SoftGridAppServiceBase, IEmployeeDocumentsAppService
    {
        private readonly IRepository<EmployeeDocument, long> _employeeDocumentRepository;
        private readonly IEmployeeDocumentsExcelExporter _employeeDocumentsExcelExporter;
        private readonly IRepository<Employee, long> _lookup_employeeRepository;
        private readonly IRepository<DocumentType, long> _lookup_documentTypeRepository;

        public EmployeeDocumentsAppService(IRepository<EmployeeDocument, long> employeeDocumentRepository, IEmployeeDocumentsExcelExporter employeeDocumentsExcelExporter, IRepository<Employee, long> lookup_employeeRepository, IRepository<DocumentType, long> lookup_documentTypeRepository)
        {
            _employeeDocumentRepository = employeeDocumentRepository;
            _employeeDocumentsExcelExporter = employeeDocumentsExcelExporter;
            _lookup_employeeRepository = lookup_employeeRepository;
            _lookup_documentTypeRepository = lookup_documentTypeRepository;

        }

        public async Task<PagedResultDto<GetEmployeeDocumentForViewDto>> GetAll(GetAllEmployeeDocumentsInput input)
        {

            var filteredEmployeeDocuments = _employeeDocumentRepository.GetAll()
                        .Include(e => e.EmployeeFk)
                        .Include(e => e.DocumentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentTitle.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTitleFilter), e => e.DocumentTitle.Contains(input.DocumentTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileBinaryObjectIdFilter.ToString()), e => e.FileBinaryObjectId.ToString() == input.FileBinaryObjectIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTypeNameFilter), e => e.DocumentTypeFk != null && e.DocumentTypeFk.Name == input.DocumentTypeNameFilter);

            var pagedAndFilteredEmployeeDocuments = filteredEmployeeDocuments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var employeeDocuments = from o in pagedAndFilteredEmployeeDocuments
                                    join o1 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    join o2 in _lookup_documentTypeRepository.GetAll() on o.DocumentTypeId equals o2.Id into j2
                                    from s2 in j2.DefaultIfEmpty()

                                    select new
                                    {

                                        o.DocumentTitle,
                                        o.FileBinaryObjectId,
                                        Id = o.Id,
                                        EmployeeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                        DocumentTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                    };

            var totalCount = await filteredEmployeeDocuments.CountAsync();

            var dbList = await employeeDocuments.ToListAsync();
            var results = new List<GetEmployeeDocumentForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetEmployeeDocumentForViewDto()
                {
                    EmployeeDocument = new EmployeeDocumentDto
                    {

                        DocumentTitle = o.DocumentTitle,
                        FileBinaryObjectId = o.FileBinaryObjectId,
                        Id = o.Id,
                    },
                    EmployeeName = o.EmployeeName,
                    DocumentTypeName = o.DocumentTypeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetEmployeeDocumentForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_EmployeeDocuments_Edit)]
        public async Task<GetEmployeeDocumentForEditOutput> GetEmployeeDocumentForEdit(EntityDto<long> input)
        {
            var employeeDocument = await _employeeDocumentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetEmployeeDocumentForEditOutput { EmployeeDocument = ObjectMapper.Map<CreateOrEditEmployeeDocumentDto>(employeeDocument) };

            if (output.EmployeeDocument.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.EmployeeDocument.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            if (output.EmployeeDocument.DocumentTypeId != null)
            {
                var _lookupDocumentType = await _lookup_documentTypeRepository.FirstOrDefaultAsync((long)output.EmployeeDocument.DocumentTypeId);
                output.DocumentTypeName = _lookupDocumentType?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditEmployeeDocumentDto input)
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

        [AbpAuthorize(AppPermissions.Pages_EmployeeDocuments_Create)]
        protected virtual async Task Create(CreateOrEditEmployeeDocumentDto input)
        {
            var employeeDocument = ObjectMapper.Map<EmployeeDocument>(input);

            if (AbpSession.TenantId != null)
            {
                employeeDocument.TenantId = (int?)AbpSession.TenantId;
            }

            await _employeeDocumentRepository.InsertAsync(employeeDocument);

        }

        [AbpAuthorize(AppPermissions.Pages_EmployeeDocuments_Edit)]
        protected virtual async Task Update(CreateOrEditEmployeeDocumentDto input)
        {
            var employeeDocument = await _employeeDocumentRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, employeeDocument);

        }

        [AbpAuthorize(AppPermissions.Pages_EmployeeDocuments_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _employeeDocumentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetEmployeeDocumentsToExcel(GetAllEmployeeDocumentsForExcelInput input)
        {

            var filteredEmployeeDocuments = _employeeDocumentRepository.GetAll()
                        .Include(e => e.EmployeeFk)
                        .Include(e => e.DocumentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentTitle.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTitleFilter), e => e.DocumentTitle.Contains(input.DocumentTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileBinaryObjectIdFilter.ToString()), e => e.FileBinaryObjectId.ToString() == input.FileBinaryObjectIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTypeNameFilter), e => e.DocumentTypeFk != null && e.DocumentTypeFk.Name == input.DocumentTypeNameFilter);

            var query = (from o in filteredEmployeeDocuments
                         join o1 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_documentTypeRepository.GetAll() on o.DocumentTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetEmployeeDocumentForViewDto()
                         {
                             EmployeeDocument = new EmployeeDocumentDto
                             {
                                 DocumentTitle = o.DocumentTitle,
                                 FileBinaryObjectId = o.FileBinaryObjectId,
                                 Id = o.Id
                             },
                             EmployeeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             DocumentTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var employeeDocumentListDtos = await query.ToListAsync();

            return _employeeDocumentsExcelExporter.ExportToFile(employeeDocumentListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_EmployeeDocuments)]
        public async Task<PagedResultDto<EmployeeDocumentEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_employeeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var employeeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<EmployeeDocumentEmployeeLookupTableDto>();
            foreach (var employee in employeeList)
            {
                lookupTableDtoList.Add(new EmployeeDocumentEmployeeLookupTableDto
                {
                    Id = employee.Id,
                    DisplayName = employee.Name?.ToString()
                });
            }

            return new PagedResultDto<EmployeeDocumentEmployeeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_EmployeeDocuments)]
        public async Task<PagedResultDto<EmployeeDocumentDocumentTypeLookupTableDto>> GetAllDocumentTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_documentTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var documentTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<EmployeeDocumentDocumentTypeLookupTableDto>();
            foreach (var documentType in documentTypeList)
            {
                lookupTableDtoList.Add(new EmployeeDocumentDocumentTypeLookupTableDto
                {
                    Id = documentType.Id,
                    DisplayName = documentType.Name?.ToString()
                });
            }

            return new PagedResultDto<EmployeeDocumentDocumentTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}