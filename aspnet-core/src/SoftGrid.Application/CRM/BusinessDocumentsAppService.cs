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
    [AbpAuthorize(AppPermissions.Pages_BusinessDocuments)]
    public class BusinessDocumentsAppService : SoftGridAppServiceBase, IBusinessDocumentsAppService
    {
        private readonly IRepository<BusinessDocument, long> _businessDocumentRepository;
        private readonly IBusinessDocumentsExcelExporter _businessDocumentsExcelExporter;
        private readonly IRepository<Business, long> _lookup_businessRepository;
        private readonly IRepository<DocumentType, long> _lookup_documentTypeRepository;

        public BusinessDocumentsAppService(IRepository<BusinessDocument, long> businessDocumentRepository, IBusinessDocumentsExcelExporter businessDocumentsExcelExporter, IRepository<Business, long> lookup_businessRepository, IRepository<DocumentType, long> lookup_documentTypeRepository)
        {
            _businessDocumentRepository = businessDocumentRepository;
            _businessDocumentsExcelExporter = businessDocumentsExcelExporter;
            _lookup_businessRepository = lookup_businessRepository;
            _lookup_documentTypeRepository = lookup_documentTypeRepository;

        }

        public async Task<PagedResultDto<GetBusinessDocumentForViewDto>> GetAll(GetAllBusinessDocumentsInput input)
        {

            var filteredBusinessDocuments = _businessDocumentRepository.GetAll()
                        .Include(e => e.BusinessFk)
                        .Include(e => e.DocumentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentTitle.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTitleFilter), e => e.DocumentTitle.Contains(input.DocumentTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileBinaryObjectIdFilter.ToString()), e => e.FileBinaryObjectId.ToString() == input.FileBinaryObjectIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTypeNameFilter), e => e.DocumentTypeFk != null && e.DocumentTypeFk.Name == input.DocumentTypeNameFilter);

            var pagedAndFilteredBusinessDocuments = filteredBusinessDocuments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var businessDocuments = from o in pagedAndFilteredBusinessDocuments
                                    join o1 in _lookup_businessRepository.GetAll() on o.BusinessId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    join o2 in _lookup_documentTypeRepository.GetAll() on o.DocumentTypeId equals o2.Id into j2
                                    from s2 in j2.DefaultIfEmpty()

                                    select new
                                    {

                                        o.DocumentTitle,
                                        o.FileBinaryObjectId,
                                        Id = o.Id,
                                        BusinessName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                        DocumentTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                    };

            var totalCount = await filteredBusinessDocuments.CountAsync();

            var dbList = await businessDocuments.ToListAsync();
            var results = new List<GetBusinessDocumentForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBusinessDocumentForViewDto()
                {
                    BusinessDocument = new BusinessDocumentDto
                    {

                        DocumentTitle = o.DocumentTitle,
                        FileBinaryObjectId = o.FileBinaryObjectId,
                        Id = o.Id,
                    },
                    BusinessName = o.BusinessName,
                    DocumentTypeName = o.DocumentTypeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetBusinessDocumentForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetBusinessDocumentForViewDto> GetBusinessDocumentForView(long id)
        {
            var businessDocument = await _businessDocumentRepository.GetAsync(id);

            var output = new GetBusinessDocumentForViewDto { BusinessDocument = ObjectMapper.Map<BusinessDocumentDto>(businessDocument) };

            if (output.BusinessDocument.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.BusinessDocument.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            if (output.BusinessDocument.DocumentTypeId != null)
            {
                var _lookupDocumentType = await _lookup_documentTypeRepository.FirstOrDefaultAsync((long)output.BusinessDocument.DocumentTypeId);
                output.DocumentTypeName = _lookupDocumentType?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessDocuments_Edit)]
        public async Task<GetBusinessDocumentForEditOutput> GetBusinessDocumentForEdit(EntityDto<long> input)
        {
            var businessDocument = await _businessDocumentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBusinessDocumentForEditOutput { BusinessDocument = ObjectMapper.Map<CreateOrEditBusinessDocumentDto>(businessDocument) };

            if (output.BusinessDocument.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.BusinessDocument.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            if (output.BusinessDocument.DocumentTypeId != null)
            {
                var _lookupDocumentType = await _lookup_documentTypeRepository.FirstOrDefaultAsync((long)output.BusinessDocument.DocumentTypeId);
                output.DocumentTypeName = _lookupDocumentType?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBusinessDocumentDto input)
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

        [AbpAuthorize(AppPermissions.Pages_BusinessDocuments_Create)]
        protected virtual async Task Create(CreateOrEditBusinessDocumentDto input)
        {
            var businessDocument = ObjectMapper.Map<BusinessDocument>(input);

            if (AbpSession.TenantId != null)
            {
                businessDocument.TenantId = (int?)AbpSession.TenantId;
            }

            await _businessDocumentRepository.InsertAsync(businessDocument);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessDocuments_Edit)]
        protected virtual async Task Update(CreateOrEditBusinessDocumentDto input)
        {
            var businessDocument = await _businessDocumentRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, businessDocument);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessDocuments_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _businessDocumentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetBusinessDocumentsToExcel(GetAllBusinessDocumentsForExcelInput input)
        {

            var filteredBusinessDocuments = _businessDocumentRepository.GetAll()
                        .Include(e => e.BusinessFk)
                        .Include(e => e.DocumentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentTitle.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTitleFilter), e => e.DocumentTitle.Contains(input.DocumentTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileBinaryObjectIdFilter.ToString()), e => e.FileBinaryObjectId.ToString() == input.FileBinaryObjectIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTypeNameFilter), e => e.DocumentTypeFk != null && e.DocumentTypeFk.Name == input.DocumentTypeNameFilter);

            var query = (from o in filteredBusinessDocuments
                         join o1 in _lookup_businessRepository.GetAll() on o.BusinessId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_documentTypeRepository.GetAll() on o.DocumentTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetBusinessDocumentForViewDto()
                         {
                             BusinessDocument = new BusinessDocumentDto
                             {
                                 DocumentTitle = o.DocumentTitle,
                                 FileBinaryObjectId = o.FileBinaryObjectId,
                                 Id = o.Id
                             },
                             BusinessName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             DocumentTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var businessDocumentListDtos = await query.ToListAsync();

            return _businessDocumentsExcelExporter.ExportToFile(businessDocumentListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessDocuments)]
        public async Task<PagedResultDto<BusinessDocumentBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_businessRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var businessList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BusinessDocumentBusinessLookupTableDto>();
            foreach (var business in businessList)
            {
                lookupTableDtoList.Add(new BusinessDocumentBusinessLookupTableDto
                {
                    Id = business.Id,
                    DisplayName = business.Name?.ToString()
                });
            }

            return new PagedResultDto<BusinessDocumentBusinessLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessDocuments)]
        public async Task<PagedResultDto<BusinessDocumentDocumentTypeLookupTableDto>> GetAllDocumentTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_documentTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var documentTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BusinessDocumentDocumentTypeLookupTableDto>();
            foreach (var documentType in documentTypeList)
            {
                lookupTableDtoList.Add(new BusinessDocumentDocumentTypeLookupTableDto
                {
                    Id = documentType.Id,
                    DisplayName = documentType.Name?.ToString()
                });
            }

            return new PagedResultDto<BusinessDocumentDocumentTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}