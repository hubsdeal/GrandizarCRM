using SoftGrid.CRM;
using SoftGrid.Shop;
using SoftGrid.Shop;
using SoftGrid.Authorization.Users;

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
    [AbpAuthorize(AppPermissions.Pages_ProductOwnerPublicContactInfos)]
    public class ProductOwnerPublicContactInfosAppService : SoftGridAppServiceBase, IProductOwnerPublicContactInfosAppService
    {
        private readonly IRepository<ProductOwnerPublicContactInfo, long> _productOwnerPublicContactInfoRepository;
        private readonly IProductOwnerPublicContactInfosExcelExporter _productOwnerPublicContactInfosExcelExporter;
        private readonly IRepository<Contact, long> _lookup_contactRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<User, long> _lookup_userRepository;

        public ProductOwnerPublicContactInfosAppService(IRepository<ProductOwnerPublicContactInfo, long> productOwnerPublicContactInfoRepository, IProductOwnerPublicContactInfosExcelExporter productOwnerPublicContactInfosExcelExporter, IRepository<Contact, long> lookup_contactRepository, IRepository<Product, long> lookup_productRepository, IRepository<Store, long> lookup_storeRepository, IRepository<User, long> lookup_userRepository)
        {
            _productOwnerPublicContactInfoRepository = productOwnerPublicContactInfoRepository;
            _productOwnerPublicContactInfosExcelExporter = productOwnerPublicContactInfosExcelExporter;
            _lookup_contactRepository = lookup_contactRepository;
            _lookup_productRepository = lookup_productRepository;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_userRepository = lookup_userRepository;

        }

        public async Task<PagedResultDto<GetProductOwnerPublicContactInfoForViewDto>> GetAll(GetAllProductOwnerPublicContactInfosInput input)
        {

            var filteredProductOwnerPublicContactInfos = _productOwnerPublicContactInfoRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .Include(e => e.ProductFk)
                        .Include(e => e.StoreFk)
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Mobile.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.ShortBio.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MobileFilter), e => e.Mobile.Contains(input.MobileFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ShortBioFilter), e => e.ShortBio.Contains(input.ShortBioFilter))
                        .WhereIf(input.PublishFilter.HasValue && input.PublishFilter > -1, e => (input.PublishFilter == 1 && e.Publish) || (input.PublishFilter == 0 && !e.Publish))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhotoIdFilter.ToString()), e => e.PhotoId.ToString() == input.PhotoIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var pagedAndFilteredProductOwnerPublicContactInfos = filteredProductOwnerPublicContactInfos
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productOwnerPublicContactInfos = from o in pagedAndFilteredProductOwnerPublicContactInfos
                                                 join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                                                 from s1 in j1.DefaultIfEmpty()

                                                 join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                                                 from s2 in j2.DefaultIfEmpty()

                                                 join o3 in _lookup_storeRepository.GetAll() on o.StoreId equals o3.Id into j3
                                                 from s3 in j3.DefaultIfEmpty()

                                                 join o4 in _lookup_userRepository.GetAll() on o.UserId equals o4.Id into j4
                                                 from s4 in j4.DefaultIfEmpty()

                                                 select new
                                                 {

                                                     o.Name,
                                                     o.Mobile,
                                                     o.Email,
                                                     o.ShortBio,
                                                     o.Publish,
                                                     o.PhotoId,
                                                     Id = o.Id,
                                                     ContactFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                                                     ProductName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                                     StoreName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                                                     UserName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                                                 };

            var totalCount = await filteredProductOwnerPublicContactInfos.CountAsync();

            var dbList = await productOwnerPublicContactInfos.ToListAsync();
            var results = new List<GetProductOwnerPublicContactInfoForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductOwnerPublicContactInfoForViewDto()
                {
                    ProductOwnerPublicContactInfo = new ProductOwnerPublicContactInfoDto
                    {

                        Name = o.Name,
                        Mobile = o.Mobile,
                        Email = o.Email,
                        ShortBio = o.ShortBio,
                        Publish = o.Publish,
                        PhotoId = o.PhotoId,
                        Id = o.Id,
                    },
                    ContactFullName = o.ContactFullName,
                    ProductName = o.ProductName,
                    StoreName = o.StoreName,
                    UserName = o.UserName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductOwnerPublicContactInfoForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductOwnerPublicContactInfoForViewDto> GetProductOwnerPublicContactInfoForView(long id)
        {
            var productOwnerPublicContactInfo = await _productOwnerPublicContactInfoRepository.GetAsync(id);

            var output = new GetProductOwnerPublicContactInfoForViewDto { ProductOwnerPublicContactInfo = ObjectMapper.Map<ProductOwnerPublicContactInfoDto>(productOwnerPublicContactInfo) };

            if (output.ProductOwnerPublicContactInfo.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.ProductOwnerPublicContactInfo.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.ProductOwnerPublicContactInfo.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductOwnerPublicContactInfo.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductOwnerPublicContactInfo.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.ProductOwnerPublicContactInfo.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.ProductOwnerPublicContactInfo.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.ProductOwnerPublicContactInfo.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductOwnerPublicContactInfos_Edit)]
        public async Task<GetProductOwnerPublicContactInfoForEditOutput> GetProductOwnerPublicContactInfoForEdit(EntityDto<long> input)
        {
            var productOwnerPublicContactInfo = await _productOwnerPublicContactInfoRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductOwnerPublicContactInfoForEditOutput { ProductOwnerPublicContactInfo = ObjectMapper.Map<CreateOrEditProductOwnerPublicContactInfoDto>(productOwnerPublicContactInfo) };

            if (output.ProductOwnerPublicContactInfo.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.ProductOwnerPublicContactInfo.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.ProductOwnerPublicContactInfo.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductOwnerPublicContactInfo.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductOwnerPublicContactInfo.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.ProductOwnerPublicContactInfo.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.ProductOwnerPublicContactInfo.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.ProductOwnerPublicContactInfo.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductOwnerPublicContactInfoDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductOwnerPublicContactInfos_Create)]
        protected virtual async Task Create(CreateOrEditProductOwnerPublicContactInfoDto input)
        {
            var productOwnerPublicContactInfo = ObjectMapper.Map<ProductOwnerPublicContactInfo>(input);

            if (AbpSession.TenantId != null)
            {
                productOwnerPublicContactInfo.TenantId = (int?)AbpSession.TenantId;
            }

            await _productOwnerPublicContactInfoRepository.InsertAsync(productOwnerPublicContactInfo);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductOwnerPublicContactInfos_Edit)]
        protected virtual async Task Update(CreateOrEditProductOwnerPublicContactInfoDto input)
        {
            var productOwnerPublicContactInfo = await _productOwnerPublicContactInfoRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productOwnerPublicContactInfo);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductOwnerPublicContactInfos_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productOwnerPublicContactInfoRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductOwnerPublicContactInfosToExcel(GetAllProductOwnerPublicContactInfosForExcelInput input)
        {

            var filteredProductOwnerPublicContactInfos = _productOwnerPublicContactInfoRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .Include(e => e.ProductFk)
                        .Include(e => e.StoreFk)
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Mobile.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.ShortBio.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MobileFilter), e => e.Mobile.Contains(input.MobileFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ShortBioFilter), e => e.ShortBio.Contains(input.ShortBioFilter))
                        .WhereIf(input.PublishFilter.HasValue && input.PublishFilter > -1, e => (input.PublishFilter == 1 && e.Publish) || (input.PublishFilter == 0 && !e.Publish))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhotoIdFilter.ToString()), e => e.PhotoId.ToString() == input.PhotoIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var query = (from o in filteredProductOwnerPublicContactInfos
                         join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_storeRepository.GetAll() on o.StoreId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_userRepository.GetAll() on o.UserId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetProductOwnerPublicContactInfoForViewDto()
                         {
                             ProductOwnerPublicContactInfo = new ProductOwnerPublicContactInfoDto
                             {
                                 Name = o.Name,
                                 Mobile = o.Mobile,
                                 Email = o.Email,
                                 ShortBio = o.ShortBio,
                                 Publish = o.Publish,
                                 PhotoId = o.PhotoId,
                                 Id = o.Id
                             },
                             ContactFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                             ProductName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             StoreName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             UserName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                         });

            var productOwnerPublicContactInfoListDtos = await query.ToListAsync();

            return _productOwnerPublicContactInfosExcelExporter.ExportToFile(productOwnerPublicContactInfoListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductOwnerPublicContactInfos)]
        public async Task<PagedResultDto<ProductOwnerPublicContactInfoContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductOwnerPublicContactInfoContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new ProductOwnerPublicContactInfoContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<ProductOwnerPublicContactInfoContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductOwnerPublicContactInfos)]
        public async Task<PagedResultDto<ProductOwnerPublicContactInfoProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductOwnerPublicContactInfoProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductOwnerPublicContactInfoProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductOwnerPublicContactInfoProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductOwnerPublicContactInfos)]
        public async Task<PagedResultDto<ProductOwnerPublicContactInfoStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductOwnerPublicContactInfoStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new ProductOwnerPublicContactInfoStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductOwnerPublicContactInfoStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductOwnerPublicContactInfos)]
        public async Task<PagedResultDto<ProductOwnerPublicContactInfoUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductOwnerPublicContactInfoUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new ProductOwnerPublicContactInfoUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductOwnerPublicContactInfoUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}