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
    [AbpAuthorize(AppPermissions.Pages_ContactDocuments)]
    public class ContactDocumentsAppService : SoftGridAppServiceBase, IContactDocumentsAppService
    {
        private readonly IRepository<ContactDocument, long> _contactDocumentRepository;
        private readonly IContactDocumentsExcelExporter _contactDocumentsExcelExporter;
        private readonly IRepository<Contact, long> _lookup_contactRepository;
        private readonly IRepository<DocumentType, long> _lookup_documentTypeRepository;

        public ContactDocumentsAppService(IRepository<ContactDocument, long> contactDocumentRepository, IContactDocumentsExcelExporter contactDocumentsExcelExporter, IRepository<Contact, long> lookup_contactRepository, IRepository<DocumentType, long> lookup_documentTypeRepository)
        {
            _contactDocumentRepository = contactDocumentRepository;
            _contactDocumentsExcelExporter = contactDocumentsExcelExporter;
            _lookup_contactRepository = lookup_contactRepository;
            _lookup_documentTypeRepository = lookup_documentTypeRepository;

        }

        public async Task<PagedResultDto<GetContactDocumentForViewDto>> GetAll(GetAllContactDocumentsInput input)
        {

            var filteredContactDocuments = _contactDocumentRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .Include(e => e.DocumentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentTitle.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTitleFilter), e => e.DocumentTitle.Contains(input.DocumentTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileBinaryObjectIdFilter.ToString()), e => e.FileBinaryObjectId.ToString() == input.FileBinaryObjectIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTypeNameFilter), e => e.DocumentTypeFk != null && e.DocumentTypeFk.Name == input.DocumentTypeNameFilter);

            var pagedAndFilteredContactDocuments = filteredContactDocuments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var contactDocuments = from o in pagedAndFilteredContactDocuments
                                   join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                                   from s1 in j1.DefaultIfEmpty()

                                   join o2 in _lookup_documentTypeRepository.GetAll() on o.DocumentTypeId equals o2.Id into j2
                                   from s2 in j2.DefaultIfEmpty()

                                   select new
                                   {

                                       o.DocumentTitle,
                                       o.FileBinaryObjectId,
                                       Id = o.Id,
                                       ContactFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                                       DocumentTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                   };

            var totalCount = await filteredContactDocuments.CountAsync();

            var dbList = await contactDocuments.ToListAsync();
            var results = new List<GetContactDocumentForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetContactDocumentForViewDto()
                {
                    ContactDocument = new ContactDocumentDto
                    {

                        DocumentTitle = o.DocumentTitle,
                        FileBinaryObjectId = o.FileBinaryObjectId,
                        Id = o.Id,
                    },
                    ContactFullName = o.ContactFullName,
                    DocumentTypeName = o.DocumentTypeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetContactDocumentForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_ContactDocuments_Edit)]
        public async Task<GetContactDocumentForEditOutput> GetContactDocumentForEdit(EntityDto<long> input)
        {
            var contactDocument = await _contactDocumentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetContactDocumentForEditOutput { ContactDocument = ObjectMapper.Map<CreateOrEditContactDocumentDto>(contactDocument) };

            if (output.ContactDocument.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.ContactDocument.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.ContactDocument.DocumentTypeId != null)
            {
                var _lookupDocumentType = await _lookup_documentTypeRepository.FirstOrDefaultAsync((long)output.ContactDocument.DocumentTypeId);
                output.DocumentTypeName = _lookupDocumentType?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditContactDocumentDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ContactDocuments_Create)]
        protected virtual async Task Create(CreateOrEditContactDocumentDto input)
        {
            var contactDocument = ObjectMapper.Map<ContactDocument>(input);

            if (AbpSession.TenantId != null)
            {
                contactDocument.TenantId = (int?)AbpSession.TenantId;
            }

            await _contactDocumentRepository.InsertAsync(contactDocument);

        }

        [AbpAuthorize(AppPermissions.Pages_ContactDocuments_Edit)]
        protected virtual async Task Update(CreateOrEditContactDocumentDto input)
        {
            var contactDocument = await _contactDocumentRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, contactDocument);

        }

        [AbpAuthorize(AppPermissions.Pages_ContactDocuments_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _contactDocumentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetContactDocumentsToExcel(GetAllContactDocumentsForExcelInput input)
        {

            var filteredContactDocuments = _contactDocumentRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .Include(e => e.DocumentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentTitle.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTitleFilter), e => e.DocumentTitle.Contains(input.DocumentTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileBinaryObjectIdFilter.ToString()), e => e.FileBinaryObjectId.ToString() == input.FileBinaryObjectIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTypeNameFilter), e => e.DocumentTypeFk != null && e.DocumentTypeFk.Name == input.DocumentTypeNameFilter);

            var query = (from o in filteredContactDocuments
                         join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_documentTypeRepository.GetAll() on o.DocumentTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetContactDocumentForViewDto()
                         {
                             ContactDocument = new ContactDocumentDto
                             {
                                 DocumentTitle = o.DocumentTitle,
                                 FileBinaryObjectId = o.FileBinaryObjectId,
                                 Id = o.Id
                             },
                             ContactFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                             DocumentTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var contactDocumentListDtos = await query.ToListAsync();

            return _contactDocumentsExcelExporter.ExportToFile(contactDocumentListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ContactDocuments)]
        public async Task<PagedResultDto<ContactDocumentContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ContactDocumentContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new ContactDocumentContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<ContactDocumentContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ContactDocuments)]
        public async Task<PagedResultDto<ContactDocumentDocumentTypeLookupTableDto>> GetAllDocumentTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_documentTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var documentTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ContactDocumentDocumentTypeLookupTableDto>();
            foreach (var documentType in documentTypeList)
            {
                lookupTableDtoList.Add(new ContactDocumentDocumentTypeLookupTableDto
                {
                    Id = documentType.Id,
                    DisplayName = documentType.Name?.ToString()
                });
            }

            return new PagedResultDto<ContactDocumentDocumentTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}