using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.LookupData.Exporting;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.LookupData
{
    [AbpAuthorize(AppPermissions.Pages_DocumentTypes)]
    public class DocumentTypesAppService : SoftGridAppServiceBase, IDocumentTypesAppService
    {
        private readonly IRepository<DocumentType, long> _documentTypeRepository;
        private readonly IDocumentTypesExcelExporter _documentTypesExcelExporter;

        public DocumentTypesAppService(IRepository<DocumentType, long> documentTypeRepository, IDocumentTypesExcelExporter documentTypesExcelExporter)
        {
            _documentTypeRepository = documentTypeRepository;
            _documentTypesExcelExporter = documentTypesExcelExporter;

        }

        public async Task<PagedResultDto<GetDocumentTypeForViewDto>> GetAll(GetAllDocumentTypesInput input)
        {

            var filteredDocumentTypes = _documentTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredDocumentTypes = filteredDocumentTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var documentTypes = from o in pagedAndFilteredDocumentTypes
                                select new
                                {

                                    o.Name,
                                    Id = o.Id
                                };

            var totalCount = await filteredDocumentTypes.CountAsync();

            var dbList = await documentTypes.ToListAsync();
            var results = new List<GetDocumentTypeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDocumentTypeForViewDto()
                {
                    DocumentType = new DocumentTypeDto
                    {

                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDocumentTypeForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetDocumentTypeForViewDto> GetDocumentTypeForView(long id)
        {
            var documentType = await _documentTypeRepository.GetAsync(id);

            var output = new GetDocumentTypeForViewDto { DocumentType = ObjectMapper.Map<DocumentTypeDto>(documentType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DocumentTypes_Edit)]
        public async Task<GetDocumentTypeForEditOutput> GetDocumentTypeForEdit(EntityDto<long> input)
        {
            var documentType = await _documentTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDocumentTypeForEditOutput { DocumentType = ObjectMapper.Map<CreateOrEditDocumentTypeDto>(documentType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDocumentTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_DocumentTypes_Create)]
        protected virtual async Task Create(CreateOrEditDocumentTypeDto input)
        {
            var documentType = ObjectMapper.Map<DocumentType>(input);

            if (AbpSession.TenantId != null)
            {
                documentType.TenantId = (int?)AbpSession.TenantId;
            }

            await _documentTypeRepository.InsertAsync(documentType);

        }

        [AbpAuthorize(AppPermissions.Pages_DocumentTypes_Edit)]
        protected virtual async Task Update(CreateOrEditDocumentTypeDto input)
        {
            var documentType = await _documentTypeRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, documentType);

        }

        [AbpAuthorize(AppPermissions.Pages_DocumentTypes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _documentTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDocumentTypesToExcel(GetAllDocumentTypesForExcelInput input)
        {

            var filteredDocumentTypes = _documentTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredDocumentTypes
                         select new GetDocumentTypeForViewDto()
                         {
                             DocumentType = new DocumentTypeDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var documentTypeListDtos = await query.ToListAsync();

            return _documentTypesExcelExporter.ExportToFile(documentTypeListDtos);
        }

    }
}