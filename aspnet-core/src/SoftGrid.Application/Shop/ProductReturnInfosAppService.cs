using SoftGrid.Shop;
using SoftGrid.LookupData;
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
    [AbpAuthorize(AppPermissions.Pages_ProductReturnInfos)]
    public class ProductReturnInfosAppService : SoftGridAppServiceBase, IProductReturnInfosAppService
    {
        private readonly IRepository<ProductReturnInfo, long> _productReturnInfoRepository;
        private readonly IProductReturnInfosExcelExporter _productReturnInfosExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<ReturnType, long> _lookup_returnTypeRepository;
        private readonly IRepository<ReturnStatus, long> _lookup_returnStatusRepository;

        public ProductReturnInfosAppService(IRepository<ProductReturnInfo, long> productReturnInfoRepository, IProductReturnInfosExcelExporter productReturnInfosExcelExporter, IRepository<Product, long> lookup_productRepository, IRepository<ReturnType, long> lookup_returnTypeRepository, IRepository<ReturnStatus, long> lookup_returnStatusRepository)
        {
            _productReturnInfoRepository = productReturnInfoRepository;
            _productReturnInfosExcelExporter = productReturnInfosExcelExporter;
            _lookup_productRepository = lookup_productRepository;
            _lookup_returnTypeRepository = lookup_returnTypeRepository;
            _lookup_returnStatusRepository = lookup_returnStatusRepository;

        }

        public async Task<PagedResultDto<GetProductReturnInfoForViewDto>> GetAll(GetAllProductReturnInfosInput input)
        {

            var filteredProductReturnInfos = _productReturnInfoRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.ReturnTypeFk)
                        .Include(e => e.ReturnStatusFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomerNote.Contains(input.Filter) || e.AdminNote.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerNoteFilter), e => e.CustomerNote.Contains(input.CustomerNoteFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdminNoteFilter), e => e.AdminNote.Contains(input.AdminNoteFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReturnTypeNameFilter), e => e.ReturnTypeFk != null && e.ReturnTypeFk.Name == input.ReturnTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReturnStatusNameFilter), e => e.ReturnStatusFk != null && e.ReturnStatusFk.Name == input.ReturnStatusNameFilter);

            var pagedAndFilteredProductReturnInfos = filteredProductReturnInfos
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productReturnInfos = from o in pagedAndFilteredProductReturnInfos
                                     join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                     from s1 in j1.DefaultIfEmpty()

                                     join o2 in _lookup_returnTypeRepository.GetAll() on o.ReturnTypeId equals o2.Id into j2
                                     from s2 in j2.DefaultIfEmpty()

                                     join o3 in _lookup_returnStatusRepository.GetAll() on o.ReturnStatusId equals o3.Id into j3
                                     from s3 in j3.DefaultIfEmpty()

                                     select new
                                     {

                                         o.CustomerNote,
                                         o.AdminNote,
                                         Id = o.Id,
                                         ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                         ReturnTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                         ReturnStatusName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                                     };

            var totalCount = await filteredProductReturnInfos.CountAsync();

            var dbList = await productReturnInfos.ToListAsync();
            var results = new List<GetProductReturnInfoForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductReturnInfoForViewDto()
                {
                    ProductReturnInfo = new ProductReturnInfoDto
                    {

                        CustomerNote = o.CustomerNote,
                        AdminNote = o.AdminNote,
                        Id = o.Id,
                    },
                    ProductName = o.ProductName,
                    ReturnTypeName = o.ReturnTypeName,
                    ReturnStatusName = o.ReturnStatusName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductReturnInfoForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductReturnInfoForViewDto> GetProductReturnInfoForView(long id)
        {
            var productReturnInfo = await _productReturnInfoRepository.GetAsync(id);

            var output = new GetProductReturnInfoForViewDto { ProductReturnInfo = ObjectMapper.Map<ProductReturnInfoDto>(productReturnInfo) };

            if (output.ProductReturnInfo.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductReturnInfo.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductReturnInfo.ReturnTypeId != null)
            {
                var _lookupReturnType = await _lookup_returnTypeRepository.FirstOrDefaultAsync((long)output.ProductReturnInfo.ReturnTypeId);
                output.ReturnTypeName = _lookupReturnType?.Name?.ToString();
            }

            if (output.ProductReturnInfo.ReturnStatusId != null)
            {
                var _lookupReturnStatus = await _lookup_returnStatusRepository.FirstOrDefaultAsync((long)output.ProductReturnInfo.ReturnStatusId);
                output.ReturnStatusName = _lookupReturnStatus?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductReturnInfos_Edit)]
        public async Task<GetProductReturnInfoForEditOutput> GetProductReturnInfoForEdit(EntityDto<long> input)
        {
            var productReturnInfo = await _productReturnInfoRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductReturnInfoForEditOutput { ProductReturnInfo = ObjectMapper.Map<CreateOrEditProductReturnInfoDto>(productReturnInfo) };

            if (output.ProductReturnInfo.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductReturnInfo.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductReturnInfo.ReturnTypeId != null)
            {
                var _lookupReturnType = await _lookup_returnTypeRepository.FirstOrDefaultAsync((long)output.ProductReturnInfo.ReturnTypeId);
                output.ReturnTypeName = _lookupReturnType?.Name?.ToString();
            }

            if (output.ProductReturnInfo.ReturnStatusId != null)
            {
                var _lookupReturnStatus = await _lookup_returnStatusRepository.FirstOrDefaultAsync((long)output.ProductReturnInfo.ReturnStatusId);
                output.ReturnStatusName = _lookupReturnStatus?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductReturnInfoDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductReturnInfos_Create)]
        protected virtual async Task Create(CreateOrEditProductReturnInfoDto input)
        {
            var productReturnInfo = ObjectMapper.Map<ProductReturnInfo>(input);

            if (AbpSession.TenantId != null)
            {
                productReturnInfo.TenantId = (int?)AbpSession.TenantId;
            }

            await _productReturnInfoRepository.InsertAsync(productReturnInfo);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductReturnInfos_Edit)]
        protected virtual async Task Update(CreateOrEditProductReturnInfoDto input)
        {
            var productReturnInfo = await _productReturnInfoRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productReturnInfo);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductReturnInfos_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productReturnInfoRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductReturnInfosToExcel(GetAllProductReturnInfosForExcelInput input)
        {

            var filteredProductReturnInfos = _productReturnInfoRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.ReturnTypeFk)
                        .Include(e => e.ReturnStatusFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomerNote.Contains(input.Filter) || e.AdminNote.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerNoteFilter), e => e.CustomerNote.Contains(input.CustomerNoteFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdminNoteFilter), e => e.AdminNote.Contains(input.AdminNoteFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReturnTypeNameFilter), e => e.ReturnTypeFk != null && e.ReturnTypeFk.Name == input.ReturnTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReturnStatusNameFilter), e => e.ReturnStatusFk != null && e.ReturnStatusFk.Name == input.ReturnStatusNameFilter);

            var query = (from o in filteredProductReturnInfos
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_returnTypeRepository.GetAll() on o.ReturnTypeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_returnStatusRepository.GetAll() on o.ReturnStatusId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetProductReturnInfoForViewDto()
                         {
                             ProductReturnInfo = new ProductReturnInfoDto
                             {
                                 CustomerNote = o.CustomerNote,
                                 AdminNote = o.AdminNote,
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ReturnTypeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             ReturnStatusName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var productReturnInfoListDtos = await query.ToListAsync();

            return _productReturnInfosExcelExporter.ExportToFile(productReturnInfoListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductReturnInfos)]
        public async Task<PagedResultDto<ProductReturnInfoProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductReturnInfoProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductReturnInfoProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductReturnInfoProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductReturnInfos)]
        public async Task<PagedResultDto<ProductReturnInfoReturnTypeLookupTableDto>> GetAllReturnTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_returnTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var returnTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductReturnInfoReturnTypeLookupTableDto>();
            foreach (var returnType in returnTypeList)
            {
                lookupTableDtoList.Add(new ProductReturnInfoReturnTypeLookupTableDto
                {
                    Id = returnType.Id,
                    DisplayName = returnType.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductReturnInfoReturnTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductReturnInfos)]
        public async Task<PagedResultDto<ProductReturnInfoReturnStatusLookupTableDto>> GetAllReturnStatusForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_returnStatusRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var returnStatusList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductReturnInfoReturnStatusLookupTableDto>();
            foreach (var returnStatus in returnStatusList)
            {
                lookupTableDtoList.Add(new ProductReturnInfoReturnStatusLookupTableDto
                {
                    Id = returnStatus.Id,
                    DisplayName = returnStatus.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductReturnInfoReturnStatusLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}