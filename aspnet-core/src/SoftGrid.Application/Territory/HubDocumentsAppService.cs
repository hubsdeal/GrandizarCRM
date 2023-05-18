using SoftGrid.Territory;
using SoftGrid.LookupData;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.Territory.Exporting;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.Territory
{
    [AbpAuthorize(AppPermissions.Pages_HubDocuments)]
    public class HubDocumentsAppService : SoftGridAppServiceBase, IHubDocumentsAppService
    {
        private readonly IRepository<HubDocument, long> _hubDocumentRepository;
        private readonly IHubDocumentsExcelExporter _hubDocumentsExcelExporter;
        private readonly IRepository<Hub, long> _lookup_hubRepository;
        private readonly IRepository<DocumentType, long> _lookup_documentTypeRepository;

        public HubDocumentsAppService(IRepository<HubDocument, long> hubDocumentRepository, IHubDocumentsExcelExporter hubDocumentsExcelExporter, IRepository<Hub, long> lookup_hubRepository, IRepository<DocumentType, long> lookup_documentTypeRepository)
        {
            _hubDocumentRepository = hubDocumentRepository;
            _hubDocumentsExcelExporter = hubDocumentsExcelExporter;
            _lookup_hubRepository = lookup_hubRepository;
            _lookup_documentTypeRepository = lookup_documentTypeRepository;

        }

        public async Task<PagedResultDto<GetHubDocumentForViewDto>> GetAll(GetAllHubDocumentsInput input)
        {

            var filteredHubDocuments = _hubDocumentRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.DocumentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentTitle.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTitleFilter), e => e.DocumentTitle.Contains(input.DocumentTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileBinaryObjectIdFilter.ToString()), e => e.FileBinaryObjectId.ToString() == input.FileBinaryObjectIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTypeNameFilter), e => e.DocumentTypeFk != null && e.DocumentTypeFk.Name == input.DocumentTypeNameFilter);

            var pagedAndFilteredHubDocuments = filteredHubDocuments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var hubDocuments = from o in pagedAndFilteredHubDocuments
                               join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()

                               join o2 in _lookup_documentTypeRepository.GetAll() on o.DocumentTypeId equals o2.Id into j2
                               from s2 in j2.DefaultIfEmpty()

                               select new
                               {

                                   o.DocumentTitle,
                                   o.FileBinaryObjectId,
                                   Id = o.Id,
                                   HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                   DocumentTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                               };

            var totalCount = await filteredHubDocuments.CountAsync();

            var dbList = await hubDocuments.ToListAsync();
            var results = new List<GetHubDocumentForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetHubDocumentForViewDto()
                {
                    HubDocument = new HubDocumentDto
                    {

                        DocumentTitle = o.DocumentTitle,
                        FileBinaryObjectId = o.FileBinaryObjectId,
                        Id = o.Id,
                    },
                    HubName = o.HubName,
                    DocumentTypeName = o.DocumentTypeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetHubDocumentForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_HubDocuments_Edit)]
        public async Task<GetHubDocumentForEditOutput> GetHubDocumentForEdit(EntityDto<long> input)
        {
            var hubDocument = await _hubDocumentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHubDocumentForEditOutput { HubDocument = ObjectMapper.Map<CreateOrEditHubDocumentDto>(hubDocument) };

            if (output.HubDocument.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.HubDocument.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            if (output.HubDocument.DocumentTypeId != null)
            {
                var _lookupDocumentType = await _lookup_documentTypeRepository.FirstOrDefaultAsync((long)output.HubDocument.DocumentTypeId);
                output.DocumentTypeName = _lookupDocumentType?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHubDocumentDto input)
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

        [AbpAuthorize(AppPermissions.Pages_HubDocuments_Create)]
        protected virtual async Task Create(CreateOrEditHubDocumentDto input)
        {
            var hubDocument = ObjectMapper.Map<HubDocument>(input);

            if (AbpSession.TenantId != null)
            {
                hubDocument.TenantId = (int?)AbpSession.TenantId;
            }

            await _hubDocumentRepository.InsertAsync(hubDocument);

        }

        [AbpAuthorize(AppPermissions.Pages_HubDocuments_Edit)]
        protected virtual async Task Update(CreateOrEditHubDocumentDto input)
        {
            var hubDocument = await _hubDocumentRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, hubDocument);

        }

        [AbpAuthorize(AppPermissions.Pages_HubDocuments_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _hubDocumentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetHubDocumentsToExcel(GetAllHubDocumentsForExcelInput input)
        {

            var filteredHubDocuments = _hubDocumentRepository.GetAll()
                        .Include(e => e.HubFk)
                        .Include(e => e.DocumentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentTitle.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTitleFilter), e => e.DocumentTitle.Contains(input.DocumentTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileBinaryObjectIdFilter.ToString()), e => e.FileBinaryObjectId.ToString() == input.FileBinaryObjectIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTypeNameFilter), e => e.DocumentTypeFk != null && e.DocumentTypeFk.Name == input.DocumentTypeNameFilter);

            var query = (from o in filteredHubDocuments
                         join o1 in _lookup_hubRepository.GetAll() on o.HubId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_documentTypeRepository.GetAll() on o.DocumentTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetHubDocumentForViewDto()
                         {
                             HubDocument = new HubDocumentDto
                             {
                                 DocumentTitle = o.DocumentTitle,
                                 FileBinaryObjectId = o.FileBinaryObjectId,
                                 Id = o.Id
                             },
                             HubName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             DocumentTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var hubDocumentListDtos = await query.ToListAsync();

            return _hubDocumentsExcelExporter.ExportToFile(hubDocumentListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_HubDocuments)]
        public async Task<PagedResultDto<HubDocumentHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_hubRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var hubList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubDocumentHubLookupTableDto>();
            foreach (var hub in hubList)
            {
                lookupTableDtoList.Add(new HubDocumentHubLookupTableDto
                {
                    Id = hub.Id,
                    DisplayName = hub.Name?.ToString()
                });
            }

            return new PagedResultDto<HubDocumentHubLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_HubDocuments)]
        public async Task<PagedResultDto<HubDocumentDocumentTypeLookupTableDto>> GetAllDocumentTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_documentTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var documentTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<HubDocumentDocumentTypeLookupTableDto>();
            foreach (var documentType in documentTypeList)
            {
                lookupTableDtoList.Add(new HubDocumentDocumentTypeLookupTableDto
                {
                    Id = documentType.Id,
                    DisplayName = documentType.Name?.ToString()
                });
            }

            return new PagedResultDto<HubDocumentDocumentTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}