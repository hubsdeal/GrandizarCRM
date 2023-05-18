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
    [AbpAuthorize(AppPermissions.Pages_ProductDocuments)]
    public class ProductDocumentsAppService : SoftGridAppServiceBase, IProductDocumentsAppService
    {
        private readonly IRepository<ProductDocument, long> _productDocumentRepository;
        private readonly IProductDocumentsExcelExporter _productDocumentsExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<DocumentType, long> _lookup_documentTypeRepository;

        public ProductDocumentsAppService(IRepository<ProductDocument, long> productDocumentRepository, IProductDocumentsExcelExporter productDocumentsExcelExporter, IRepository<Product, long> lookup_productRepository, IRepository<DocumentType, long> lookup_documentTypeRepository)
        {
            _productDocumentRepository = productDocumentRepository;
            _productDocumentsExcelExporter = productDocumentsExcelExporter;
            _lookup_productRepository = lookup_productRepository;
            _lookup_documentTypeRepository = lookup_documentTypeRepository;

        }

        public async Task<PagedResultDto<GetProductDocumentForViewDto>> GetAll(GetAllProductDocumentsInput input)
        {

            var filteredProductDocuments = _productDocumentRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.DocumentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentTitle.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTitleFilter), e => e.DocumentTitle.Contains(input.DocumentTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileBinaryObjectIdFilter.ToString()), e => e.FileBinaryObjectId.ToString() == input.FileBinaryObjectIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTypeNameFilter), e => e.DocumentTypeFk != null && e.DocumentTypeFk.Name == input.DocumentTypeNameFilter);

            var pagedAndFilteredProductDocuments = filteredProductDocuments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productDocuments = from o in pagedAndFilteredProductDocuments
                                   join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                   from s1 in j1.DefaultIfEmpty()

                                   join o2 in _lookup_documentTypeRepository.GetAll() on o.DocumentTypeId equals o2.Id into j2
                                   from s2 in j2.DefaultIfEmpty()

                                   select new
                                   {

                                       o.DocumentTitle,
                                       o.FileBinaryObjectId,
                                       Id = o.Id,
                                       ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                       DocumentTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                   };

            var totalCount = await filteredProductDocuments.CountAsync();

            var dbList = await productDocuments.ToListAsync();
            var results = new List<GetProductDocumentForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductDocumentForViewDto()
                {
                    ProductDocument = new ProductDocumentDto
                    {

                        DocumentTitle = o.DocumentTitle,
                        FileBinaryObjectId = o.FileBinaryObjectId,
                        Id = o.Id,
                    },
                    ProductName = o.ProductName,
                    DocumentTypeName = o.DocumentTypeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductDocumentForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_ProductDocuments_Edit)]
        public async Task<GetProductDocumentForEditOutput> GetProductDocumentForEdit(EntityDto<long> input)
        {
            var productDocument = await _productDocumentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductDocumentForEditOutput { ProductDocument = ObjectMapper.Map<CreateOrEditProductDocumentDto>(productDocument) };

            if (output.ProductDocument.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductDocument.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductDocument.DocumentTypeId != null)
            {
                var _lookupDocumentType = await _lookup_documentTypeRepository.FirstOrDefaultAsync((long)output.ProductDocument.DocumentTypeId);
                output.DocumentTypeName = _lookupDocumentType?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductDocumentDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductDocuments_Create)]
        protected virtual async Task Create(CreateOrEditProductDocumentDto input)
        {
            var productDocument = ObjectMapper.Map<ProductDocument>(input);

            if (AbpSession.TenantId != null)
            {
                productDocument.TenantId = (int?)AbpSession.TenantId;
            }

            await _productDocumentRepository.InsertAsync(productDocument);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductDocuments_Edit)]
        protected virtual async Task Update(CreateOrEditProductDocumentDto input)
        {
            var productDocument = await _productDocumentRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productDocument);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductDocuments_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productDocumentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductDocumentsToExcel(GetAllProductDocumentsForExcelInput input)
        {

            var filteredProductDocuments = _productDocumentRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.DocumentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentTitle.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTitleFilter), e => e.DocumentTitle.Contains(input.DocumentTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileBinaryObjectIdFilter.ToString()), e => e.FileBinaryObjectId.ToString() == input.FileBinaryObjectIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentTypeNameFilter), e => e.DocumentTypeFk != null && e.DocumentTypeFk.Name == input.DocumentTypeNameFilter);

            var query = (from o in filteredProductDocuments
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_documentTypeRepository.GetAll() on o.DocumentTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetProductDocumentForViewDto()
                         {
                             ProductDocument = new ProductDocumentDto
                             {
                                 DocumentTitle = o.DocumentTitle,
                                 FileBinaryObjectId = o.FileBinaryObjectId,
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             DocumentTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var productDocumentListDtos = await query.ToListAsync();

            return _productDocumentsExcelExporter.ExportToFile(productDocumentListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductDocuments)]
        public async Task<PagedResultDto<ProductDocumentProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductDocumentProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductDocumentProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductDocumentProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductDocuments)]
        public async Task<PagedResultDto<ProductDocumentDocumentTypeLookupTableDto>> GetAllDocumentTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_documentTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var documentTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductDocumentDocumentTypeLookupTableDto>();
            foreach (var documentType in documentTypeList)
            {
                lookupTableDtoList.Add(new ProductDocumentDocumentTypeLookupTableDto
                {
                    Id = documentType.Id,
                    DisplayName = documentType.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductDocumentDocumentTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}