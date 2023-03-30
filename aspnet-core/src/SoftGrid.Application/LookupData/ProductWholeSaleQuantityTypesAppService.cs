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
    [AbpAuthorize(AppPermissions.Pages_ProductWholeSaleQuantityTypes)]
    public class ProductWholeSaleQuantityTypesAppService : SoftGridAppServiceBase, IProductWholeSaleQuantityTypesAppService
    {
        private readonly IRepository<ProductWholeSaleQuantityType, long> _productWholeSaleQuantityTypeRepository;
        private readonly IProductWholeSaleQuantityTypesExcelExporter _productWholeSaleQuantityTypesExcelExporter;

        public ProductWholeSaleQuantityTypesAppService(IRepository<ProductWholeSaleQuantityType, long> productWholeSaleQuantityTypeRepository, IProductWholeSaleQuantityTypesExcelExporter productWholeSaleQuantityTypesExcelExporter)
        {
            _productWholeSaleQuantityTypeRepository = productWholeSaleQuantityTypeRepository;
            _productWholeSaleQuantityTypesExcelExporter = productWholeSaleQuantityTypesExcelExporter;

        }

        public async Task<PagedResultDto<GetProductWholeSaleQuantityTypeForViewDto>> GetAll(GetAllProductWholeSaleQuantityTypesInput input)
        {

            var filteredProductWholeSaleQuantityTypes = _productWholeSaleQuantityTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinMinQtyFilter != null, e => e.MinQty >= input.MinMinQtyFilter)
                        .WhereIf(input.MaxMinQtyFilter != null, e => e.MinQty <= input.MaxMinQtyFilter)
                        .WhereIf(input.MinMaxQtyFilter != null, e => e.MaxQty >= input.MinMaxQtyFilter)
                        .WhereIf(input.MaxMaxQtyFilter != null, e => e.MaxQty <= input.MaxMaxQtyFilter);

            var pagedAndFilteredProductWholeSaleQuantityTypes = filteredProductWholeSaleQuantityTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productWholeSaleQuantityTypes = from o in pagedAndFilteredProductWholeSaleQuantityTypes
                                                select new
                                                {

                                                    o.Name,
                                                    o.MinQty,
                                                    o.MaxQty,
                                                    Id = o.Id
                                                };

            var totalCount = await filteredProductWholeSaleQuantityTypes.CountAsync();

            var dbList = await productWholeSaleQuantityTypes.ToListAsync();
            var results = new List<GetProductWholeSaleQuantityTypeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductWholeSaleQuantityTypeForViewDto()
                {
                    ProductWholeSaleQuantityType = new ProductWholeSaleQuantityTypeDto
                    {

                        Name = o.Name,
                        MinQty = o.MinQty,
                        MaxQty = o.MaxQty,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductWholeSaleQuantityTypeForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductWholeSaleQuantityTypeForViewDto> GetProductWholeSaleQuantityTypeForView(long id)
        {
            var productWholeSaleQuantityType = await _productWholeSaleQuantityTypeRepository.GetAsync(id);

            var output = new GetProductWholeSaleQuantityTypeForViewDto { ProductWholeSaleQuantityType = ObjectMapper.Map<ProductWholeSaleQuantityTypeDto>(productWholeSaleQuantityType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductWholeSaleQuantityTypes_Edit)]
        public async Task<GetProductWholeSaleQuantityTypeForEditOutput> GetProductWholeSaleQuantityTypeForEdit(EntityDto<long> input)
        {
            var productWholeSaleQuantityType = await _productWholeSaleQuantityTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductWholeSaleQuantityTypeForEditOutput { ProductWholeSaleQuantityType = ObjectMapper.Map<CreateOrEditProductWholeSaleQuantityTypeDto>(productWholeSaleQuantityType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductWholeSaleQuantityTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductWholeSaleQuantityTypes_Create)]
        protected virtual async Task Create(CreateOrEditProductWholeSaleQuantityTypeDto input)
        {
            var productWholeSaleQuantityType = ObjectMapper.Map<ProductWholeSaleQuantityType>(input);

            if (AbpSession.TenantId != null)
            {
                productWholeSaleQuantityType.TenantId = (int?)AbpSession.TenantId;
            }

            await _productWholeSaleQuantityTypeRepository.InsertAsync(productWholeSaleQuantityType);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductWholeSaleQuantityTypes_Edit)]
        protected virtual async Task Update(CreateOrEditProductWholeSaleQuantityTypeDto input)
        {
            var productWholeSaleQuantityType = await _productWholeSaleQuantityTypeRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productWholeSaleQuantityType);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductWholeSaleQuantityTypes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productWholeSaleQuantityTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductWholeSaleQuantityTypesToExcel(GetAllProductWholeSaleQuantityTypesForExcelInput input)
        {

            var filteredProductWholeSaleQuantityTypes = _productWholeSaleQuantityTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinMinQtyFilter != null, e => e.MinQty >= input.MinMinQtyFilter)
                        .WhereIf(input.MaxMinQtyFilter != null, e => e.MinQty <= input.MaxMinQtyFilter)
                        .WhereIf(input.MinMaxQtyFilter != null, e => e.MaxQty >= input.MinMaxQtyFilter)
                        .WhereIf(input.MaxMaxQtyFilter != null, e => e.MaxQty <= input.MaxMaxQtyFilter);

            var query = (from o in filteredProductWholeSaleQuantityTypes
                         select new GetProductWholeSaleQuantityTypeForViewDto()
                         {
                             ProductWholeSaleQuantityType = new ProductWholeSaleQuantityTypeDto
                             {
                                 Name = o.Name,
                                 MinQty = o.MinQty,
                                 MaxQty = o.MaxQty,
                                 Id = o.Id
                             }
                         });

            var productWholeSaleQuantityTypeListDtos = await query.ToListAsync();

            return _productWholeSaleQuantityTypesExcelExporter.ExportToFile(productWholeSaleQuantityTypeListDtos);
        }

    }
}