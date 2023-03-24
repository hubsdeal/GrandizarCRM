using SoftGrid.CRM;
using SoftGrid.Shop;

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
    [AbpAuthorize(AppPermissions.Pages_BusinessProductMaps)]
    public class BusinessProductMapsAppService : SoftGridAppServiceBase, IBusinessProductMapsAppService
    {
        private readonly IRepository<BusinessProductMap, long> _businessProductMapRepository;
        private readonly IBusinessProductMapsExcelExporter _businessProductMapsExcelExporter;
        private readonly IRepository<Business, long> _lookup_businessRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;

        public BusinessProductMapsAppService(IRepository<BusinessProductMap, long> businessProductMapRepository, IBusinessProductMapsExcelExporter businessProductMapsExcelExporter, IRepository<Business, long> lookup_businessRepository, IRepository<Product, long> lookup_productRepository)
        {
            _businessProductMapRepository = businessProductMapRepository;
            _businessProductMapsExcelExporter = businessProductMapsExcelExporter;
            _lookup_businessRepository = lookup_businessRepository;
            _lookup_productRepository = lookup_productRepository;

        }

        public async Task<PagedResultDto<GetBusinessProductMapForViewDto>> GetAll(GetAllBusinessProductMapsInput input)
        {

            var filteredBusinessProductMaps = _businessProductMapRepository.GetAll()
                        .Include(e => e.BusinessFk)
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var pagedAndFilteredBusinessProductMaps = filteredBusinessProductMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var businessProductMaps = from o in pagedAndFilteredBusinessProductMaps
                                      join o1 in _lookup_businessRepository.GetAll() on o.BusinessId equals o1.Id into j1
                                      from s1 in j1.DefaultIfEmpty()

                                      join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                                      from s2 in j2.DefaultIfEmpty()

                                      select new
                                      {

                                          Id = o.Id,
                                          BusinessName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                          ProductName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                      };

            var totalCount = await filteredBusinessProductMaps.CountAsync();

            var dbList = await businessProductMaps.ToListAsync();
            var results = new List<GetBusinessProductMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBusinessProductMapForViewDto()
                {
                    BusinessProductMap = new BusinessProductMapDto
                    {

                        Id = o.Id,
                    },
                    BusinessName = o.BusinessName,
                    ProductName = o.ProductName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetBusinessProductMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetBusinessProductMapForViewDto> GetBusinessProductMapForView(long id)
        {
            var businessProductMap = await _businessProductMapRepository.GetAsync(id);

            var output = new GetBusinessProductMapForViewDto { BusinessProductMap = ObjectMapper.Map<BusinessProductMapDto>(businessProductMap) };

            if (output.BusinessProductMap.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.BusinessProductMap.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            if (output.BusinessProductMap.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.BusinessProductMap.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessProductMaps_Edit)]
        public async Task<GetBusinessProductMapForEditOutput> GetBusinessProductMapForEdit(EntityDto<long> input)
        {
            var businessProductMap = await _businessProductMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBusinessProductMapForEditOutput { BusinessProductMap = ObjectMapper.Map<CreateOrEditBusinessProductMapDto>(businessProductMap) };

            if (output.BusinessProductMap.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.BusinessProductMap.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            if (output.BusinessProductMap.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.BusinessProductMap.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBusinessProductMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_BusinessProductMaps_Create)]
        protected virtual async Task Create(CreateOrEditBusinessProductMapDto input)
        {
            var businessProductMap = ObjectMapper.Map<BusinessProductMap>(input);

            if (AbpSession.TenantId != null)
            {
                businessProductMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _businessProductMapRepository.InsertAsync(businessProductMap);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessProductMaps_Edit)]
        protected virtual async Task Update(CreateOrEditBusinessProductMapDto input)
        {
            var businessProductMap = await _businessProductMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, businessProductMap);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessProductMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _businessProductMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetBusinessProductMapsToExcel(GetAllBusinessProductMapsForExcelInput input)
        {

            var filteredBusinessProductMaps = _businessProductMapRepository.GetAll()
                        .Include(e => e.BusinessFk)
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var query = (from o in filteredBusinessProductMaps
                         join o1 in _lookup_businessRepository.GetAll() on o.BusinessId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetBusinessProductMapForViewDto()
                         {
                             BusinessProductMap = new BusinessProductMapDto
                             {
                                 Id = o.Id
                             },
                             BusinessName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ProductName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var businessProductMapListDtos = await query.ToListAsync();

            return _businessProductMapsExcelExporter.ExportToFile(businessProductMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessProductMaps)]
        public async Task<PagedResultDto<BusinessProductMapBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_businessRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var businessList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BusinessProductMapBusinessLookupTableDto>();
            foreach (var business in businessList)
            {
                lookupTableDtoList.Add(new BusinessProductMapBusinessLookupTableDto
                {
                    Id = business.Id,
                    DisplayName = business.Name?.ToString()
                });
            }

            return new PagedResultDto<BusinessProductMapBusinessLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessProductMaps)]
        public async Task<PagedResultDto<BusinessProductMapProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BusinessProductMapProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new BusinessProductMapProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<BusinessProductMapProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}