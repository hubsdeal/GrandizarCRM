using SoftGrid.Shop;
using SoftGrid.LookupData;

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
    [AbpAuthorize(AppPermissions.Pages_StoreDocuments)]
    public class StoreDocumentsAppService : SoftGridAppServiceBase, IStoreDocumentsAppService
    {
        private readonly IRepository<StoreDocument, long> _storeDocumentRepository;
        private readonly IStoreDocumentsExcelExporter _storeDocumentsExcelExporter;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<DocumentType, long> _lookup_documentTypeRepository;

        public StoreDocumentsAppService(IRepository<StoreDocument, long> storeDocumentRepository, IStoreDocumentsExcelExporter storeDocumentsExcelExporter, IRepository<Store, long> lookup_storeRepository, IRepository<DocumentType, long> lookup_documentTypeRepository)
        {
            _storeDocumentRepository = storeDocumentRepository;
            _storeDocumentsExcelExporter = storeDocumentsExcelExporter;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_documentTypeRepository = lookup_documentTypeRepository;

        }

        public async Task<PagedResultDto<GetStoreDocumentForViewDto>> GetAll(GetAllStoreDocumentsInput input)
        {

            var filteredStoreDocuments = _storeDocumentRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.DocumentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentTitle.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTitleFilter), e => e.DocumentTitle.Contains(input.DocumentTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileBinaryObjectIdFilter.ToString()), e => e.FileBinaryObjectId.ToString() == input.FileBinaryObjectIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTypeNameFilter), e => e.DocumentTypeFk != null && e.DocumentTypeFk.Name == input.DocumentTypeNameFilter);

            var pagedAndFilteredStoreDocuments = filteredStoreDocuments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeDocuments = from o in pagedAndFilteredStoreDocuments
                                 join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()

                                 join o2 in _lookup_documentTypeRepository.GetAll() on o.DocumentTypeId equals o2.Id into j2
                                 from s2 in j2.DefaultIfEmpty()

                                 select new
                                 {

                                     o.DocumentTitle,
                                     o.FileBinaryObjectId,
                                     Id = o.Id,
                                     StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                     DocumentTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                 };

            var totalCount = await filteredStoreDocuments.CountAsync();

            var dbList = await storeDocuments.ToListAsync();
            var results = new List<GetStoreDocumentForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreDocumentForViewDto()
                {
                    StoreDocument = new StoreDocumentDto
                    {

                        DocumentTitle = o.DocumentTitle,
                        FileBinaryObjectId = o.FileBinaryObjectId,
                        Id = o.Id,
                    },
                    StoreName = o.StoreName,
                    DocumentTypeName = o.DocumentTypeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreDocumentForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_StoreDocuments_Edit)]
        public async Task<GetStoreDocumentForEditOutput> GetStoreDocumentForEdit(EntityDto<long> input)
        {
            var storeDocument = await _storeDocumentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreDocumentForEditOutput { StoreDocument = ObjectMapper.Map<CreateOrEditStoreDocumentDto>(storeDocument) };

            if (output.StoreDocument.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreDocument.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreDocument.DocumentTypeId != null)
            {
                var _lookupDocumentType = await _lookup_documentTypeRepository.FirstOrDefaultAsync((long)output.StoreDocument.DocumentTypeId);
                output.DocumentTypeName = _lookupDocumentType?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreDocumentDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreDocuments_Create)]
        protected virtual async Task Create(CreateOrEditStoreDocumentDto input)
        {
            var storeDocument = ObjectMapper.Map<StoreDocument>(input);

            if (AbpSession.TenantId != null)
            {
                storeDocument.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeDocumentRepository.InsertAsync(storeDocument);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreDocuments_Edit)]
        protected virtual async Task Update(CreateOrEditStoreDocumentDto input)
        {
            var storeDocument = await _storeDocumentRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeDocument);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreDocuments_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeDocumentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreDocumentsToExcel(GetAllStoreDocumentsForExcelInput input)
        {

            var filteredStoreDocuments = _storeDocumentRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.DocumentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentTitle.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTitleFilter), e => e.DocumentTitle.Contains(input.DocumentTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileBinaryObjectIdFilter.ToString()), e => e.FileBinaryObjectId.ToString() == input.FileBinaryObjectIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTypeNameFilter), e => e.DocumentTypeFk != null && e.DocumentTypeFk.Name == input.DocumentTypeNameFilter);

            var query = (from o in filteredStoreDocuments
                         join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_documentTypeRepository.GetAll() on o.DocumentTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetStoreDocumentForViewDto()
                         {
                             StoreDocument = new StoreDocumentDto
                             {
                                 DocumentTitle = o.DocumentTitle,
                                 FileBinaryObjectId = o.FileBinaryObjectId,
                                 Id = o.Id
                             },
                             StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             DocumentTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var storeDocumentListDtos = await query.ToListAsync();

            return _storeDocumentsExcelExporter.ExportToFile(storeDocumentListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreDocuments)]
        public async Task<PagedResultDto<StoreDocumentStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreDocumentStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreDocumentStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreDocumentStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreDocuments)]
        public async Task<PagedResultDto<StoreDocumentDocumentTypeLookupTableDto>> GetAllDocumentTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_documentTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var documentTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreDocumentDocumentTypeLookupTableDto>();
            foreach (var documentType in documentTypeList)
            {
                lookupTableDtoList.Add(new StoreDocumentDocumentTypeLookupTableDto
                {
                    Id = documentType.Id,
                    DisplayName = documentType.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreDocumentDocumentTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}