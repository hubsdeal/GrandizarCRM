using SoftGrid.DiscountManagement;
using SoftGrid.Shop;
using SoftGrid.Shop;
using SoftGrid.LookupData;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.DiscountManagement.Exporting;
using SoftGrid.DiscountManagement.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.DiscountManagement
{
    [AbpAuthorize(AppPermissions.Pages_DiscountCodeMaps)]
    public class DiscountCodeMapsAppService : SoftGridAppServiceBase, IDiscountCodeMapsAppService
    {
        private readonly IRepository<DiscountCodeMap, long> _discountCodeMapRepository;
        private readonly IDiscountCodeMapsExcelExporter _discountCodeMapsExcelExporter;
        private readonly IRepository<DiscountCodeGenerator, long> _lookup_discountCodeGeneratorRepository;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<MembershipType, long> _lookup_membershipTypeRepository;

        public DiscountCodeMapsAppService(IRepository<DiscountCodeMap, long> discountCodeMapRepository, IDiscountCodeMapsExcelExporter discountCodeMapsExcelExporter, IRepository<DiscountCodeGenerator, long> lookup_discountCodeGeneratorRepository, IRepository<Store, long> lookup_storeRepository, IRepository<Product, long> lookup_productRepository, IRepository<MembershipType, long> lookup_membershipTypeRepository)
        {
            _discountCodeMapRepository = discountCodeMapRepository;
            _discountCodeMapsExcelExporter = discountCodeMapsExcelExporter;
            _lookup_discountCodeGeneratorRepository = lookup_discountCodeGeneratorRepository;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_productRepository = lookup_productRepository;
            _lookup_membershipTypeRepository = lookup_membershipTypeRepository;

        }

        public async Task<PagedResultDto<GetDiscountCodeMapForViewDto>> GetAll(GetAllDiscountCodeMapsInput input)
        {

            var filteredDiscountCodeMaps = _discountCodeMapRepository.GetAll()
                        .Include(e => e.DiscountCodeGeneratorFk)
                        .Include(e => e.StoreFk)
                        .Include(e => e.ProductFk)
                        .Include(e => e.MembershipTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DiscountCodeGeneratorNameFilter), e => e.DiscountCodeGeneratorFk != null && e.DiscountCodeGeneratorFk.Name == input.DiscountCodeGeneratorNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MembershipTypeNameFilter), e => e.MembershipTypeFk != null && e.MembershipTypeFk.Name == input.MembershipTypeNameFilter);

            var pagedAndFilteredDiscountCodeMaps = filteredDiscountCodeMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var discountCodeMaps = from o in pagedAndFilteredDiscountCodeMaps
                                   join o1 in _lookup_discountCodeGeneratorRepository.GetAll() on o.DiscountCodeGeneratorId equals o1.Id into j1
                                   from s1 in j1.DefaultIfEmpty()

                                   join o2 in _lookup_storeRepository.GetAll() on o.StoreId equals o2.Id into j2
                                   from s2 in j2.DefaultIfEmpty()

                                   join o3 in _lookup_productRepository.GetAll() on o.ProductId equals o3.Id into j3
                                   from s3 in j3.DefaultIfEmpty()

                                   join o4 in _lookup_membershipTypeRepository.GetAll() on o.MembershipTypeId equals o4.Id into j4
                                   from s4 in j4.DefaultIfEmpty()

                                   select new
                                   {

                                       Id = o.Id,
                                       DiscountCodeGeneratorName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                       StoreName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                       ProductName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                                       MembershipTypeName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                                   };

            var totalCount = await filteredDiscountCodeMaps.CountAsync();

            var dbList = await discountCodeMaps.ToListAsync();
            var results = new List<GetDiscountCodeMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDiscountCodeMapForViewDto()
                {
                    DiscountCodeMap = new DiscountCodeMapDto
                    {

                        Id = o.Id,
                    },
                    DiscountCodeGeneratorName = o.DiscountCodeGeneratorName,
                    StoreName = o.StoreName,
                    ProductName = o.ProductName,
                    MembershipTypeName = o.MembershipTypeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDiscountCodeMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetDiscountCodeMapForViewDto> GetDiscountCodeMapForView(long id)
        {
            var discountCodeMap = await _discountCodeMapRepository.GetAsync(id);

            var output = new GetDiscountCodeMapForViewDto { DiscountCodeMap = ObjectMapper.Map<DiscountCodeMapDto>(discountCodeMap) };

            if (output.DiscountCodeMap.DiscountCodeGeneratorId != null)
            {
                var _lookupDiscountCodeGenerator = await _lookup_discountCodeGeneratorRepository.FirstOrDefaultAsync((long)output.DiscountCodeMap.DiscountCodeGeneratorId);
                output.DiscountCodeGeneratorName = _lookupDiscountCodeGenerator?.Name?.ToString();
            }

            if (output.DiscountCodeMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.DiscountCodeMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.DiscountCodeMap.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.DiscountCodeMap.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.DiscountCodeMap.MembershipTypeId != null)
            {
                var _lookupMembershipType = await _lookup_membershipTypeRepository.FirstOrDefaultAsync((long)output.DiscountCodeMap.MembershipTypeId);
                output.MembershipTypeName = _lookupMembershipType?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeMaps_Edit)]
        public async Task<GetDiscountCodeMapForEditOutput> GetDiscountCodeMapForEdit(EntityDto<long> input)
        {
            var discountCodeMap = await _discountCodeMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDiscountCodeMapForEditOutput { DiscountCodeMap = ObjectMapper.Map<CreateOrEditDiscountCodeMapDto>(discountCodeMap) };

            if (output.DiscountCodeMap.DiscountCodeGeneratorId != null)
            {
                var _lookupDiscountCodeGenerator = await _lookup_discountCodeGeneratorRepository.FirstOrDefaultAsync((long)output.DiscountCodeMap.DiscountCodeGeneratorId);
                output.DiscountCodeGeneratorName = _lookupDiscountCodeGenerator?.Name?.ToString();
            }

            if (output.DiscountCodeMap.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.DiscountCodeMap.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.DiscountCodeMap.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.DiscountCodeMap.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.DiscountCodeMap.MembershipTypeId != null)
            {
                var _lookupMembershipType = await _lookup_membershipTypeRepository.FirstOrDefaultAsync((long)output.DiscountCodeMap.MembershipTypeId);
                output.MembershipTypeName = _lookupMembershipType?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDiscountCodeMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeMaps_Create)]
        protected virtual async Task Create(CreateOrEditDiscountCodeMapDto input)
        {
            var discountCodeMap = ObjectMapper.Map<DiscountCodeMap>(input);

            if (AbpSession.TenantId != null)
            {
                discountCodeMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _discountCodeMapRepository.InsertAsync(discountCodeMap);

        }

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeMaps_Edit)]
        protected virtual async Task Update(CreateOrEditDiscountCodeMapDto input)
        {
            var discountCodeMap = await _discountCodeMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, discountCodeMap);

        }

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _discountCodeMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDiscountCodeMapsToExcel(GetAllDiscountCodeMapsForExcelInput input)
        {

            var filteredDiscountCodeMaps = _discountCodeMapRepository.GetAll()
                        .Include(e => e.DiscountCodeGeneratorFk)
                        .Include(e => e.StoreFk)
                        .Include(e => e.ProductFk)
                        .Include(e => e.MembershipTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DiscountCodeGeneratorNameFilter), e => e.DiscountCodeGeneratorFk != null && e.DiscountCodeGeneratorFk.Name == input.DiscountCodeGeneratorNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MembershipTypeNameFilter), e => e.MembershipTypeFk != null && e.MembershipTypeFk.Name == input.MembershipTypeNameFilter);

            var query = (from o in filteredDiscountCodeMaps
                         join o1 in _lookup_discountCodeGeneratorRepository.GetAll() on o.DiscountCodeGeneratorId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_storeRepository.GetAll() on o.StoreId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_productRepository.GetAll() on o.ProductId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_membershipTypeRepository.GetAll() on o.MembershipTypeId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetDiscountCodeMapForViewDto()
                         {
                             DiscountCodeMap = new DiscountCodeMapDto
                             {
                                 Id = o.Id
                             },
                             DiscountCodeGeneratorName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             StoreName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             ProductName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             MembershipTypeName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                         });

            var discountCodeMapListDtos = await query.ToListAsync();

            return _discountCodeMapsExcelExporter.ExportToFile(discountCodeMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeMaps)]
        public async Task<PagedResultDto<DiscountCodeMapDiscountCodeGeneratorLookupTableDto>> GetAllDiscountCodeGeneratorForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_discountCodeGeneratorRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var discountCodeGeneratorList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<DiscountCodeMapDiscountCodeGeneratorLookupTableDto>();
            foreach (var discountCodeGenerator in discountCodeGeneratorList)
            {
                lookupTableDtoList.Add(new DiscountCodeMapDiscountCodeGeneratorLookupTableDto
                {
                    Id = discountCodeGenerator.Id,
                    DisplayName = discountCodeGenerator.Name?.ToString()
                });
            }

            return new PagedResultDto<DiscountCodeMapDiscountCodeGeneratorLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeMaps)]
        public async Task<PagedResultDto<DiscountCodeMapStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<DiscountCodeMapStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new DiscountCodeMapStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<DiscountCodeMapStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeMaps)]
        public async Task<PagedResultDto<DiscountCodeMapProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<DiscountCodeMapProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new DiscountCodeMapProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<DiscountCodeMapProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeMaps)]
        public async Task<PagedResultDto<DiscountCodeMapMembershipTypeLookupTableDto>> GetAllMembershipTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_membershipTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var membershipTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<DiscountCodeMapMembershipTypeLookupTableDto>();
            foreach (var membershipType in membershipTypeList)
            {
                lookupTableDtoList.Add(new DiscountCodeMapMembershipTypeLookupTableDto
                {
                    Id = membershipType.Id,
                    DisplayName = membershipType.Name?.ToString()
                });
            }

            return new PagedResultDto<DiscountCodeMapMembershipTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}