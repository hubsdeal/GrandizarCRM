using SoftGrid.Shop;
using SoftGrid.LookupData;

using SoftGrid.Shop.Enums;

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
    [AbpAuthorize(AppPermissions.Pages_ProductMasterTagSettings)]
    public class ProductMasterTagSettingsAppService : SoftGridAppServiceBase, IProductMasterTagSettingsAppService
    {
        private readonly IRepository<ProductMasterTagSetting, long> _productMasterTagSettingRepository;
        private readonly IProductMasterTagSettingsExcelExporter _productMasterTagSettingsExcelExporter;
        private readonly IRepository<ProductCategory, long> _lookup_productCategoryRepository;
        private readonly IRepository<MasterTagCategory, long> _lookup_masterTagCategoryRepository;

        public ProductMasterTagSettingsAppService(IRepository<ProductMasterTagSetting, long> productMasterTagSettingRepository, IProductMasterTagSettingsExcelExporter productMasterTagSettingsExcelExporter, IRepository<ProductCategory, long> lookup_productCategoryRepository, IRepository<MasterTagCategory, long> lookup_masterTagCategoryRepository)
        {
            _productMasterTagSettingRepository = productMasterTagSettingRepository;
            _productMasterTagSettingsExcelExporter = productMasterTagSettingsExcelExporter;
            _lookup_productCategoryRepository = lookup_productCategoryRepository;
            _lookup_masterTagCategoryRepository = lookup_masterTagCategoryRepository;

        }

        public async Task<PagedResultDto<GetProductMasterTagSettingForViewDto>> GetAll(GetAllProductMasterTagSettingsInput input)
        {
            var answerTypeIdFilter = input.AnswerTypeIdFilter.HasValue
                        ? (AnswerType)input.AnswerTypeIdFilter
                        : default;

            var filteredProductMasterTagSettings = _productMasterTagSettingRepository.GetAll()
                        .Include(e => e.ProductCategoryFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTagTitle.Contains(input.Filter) || e.CustomTagChatQuestion.Contains(input.Filter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagTitleFilter), e => e.CustomTagTitle.Contains(input.CustomTagTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagChatQuestionFilter), e => e.CustomTagChatQuestion.Contains(input.CustomTagChatQuestionFilter))
                        .WhereIf(input.DisplayPublicFilter.HasValue && input.DisplayPublicFilter > -1, e => (input.DisplayPublicFilter == 1 && e.DisplayPublic) || (input.DisplayPublicFilter == 0 && !e.DisplayPublic))
                        .WhereIf(input.AnswerTypeIdFilter.HasValue && input.AnswerTypeIdFilter > -1, e => e.AnswerTypeId == answerTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter);

            var pagedAndFilteredProductMasterTagSettings = filteredProductMasterTagSettings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productMasterTagSettings = from o in pagedAndFilteredProductMasterTagSettings
                                           join o1 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o1.Id into j1
                                           from s1 in j1.DefaultIfEmpty()

                                           join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                                           from s2 in j2.DefaultIfEmpty()

                                           select new
                                           {

                                               o.DisplaySequence,
                                               o.CustomTagTitle,
                                               o.CustomTagChatQuestion,
                                               o.DisplayPublic,
                                               o.AnswerTypeId,
                                               Id = o.Id,
                                               ProductCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                               MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                           };

            var totalCount = await filteredProductMasterTagSettings.CountAsync();

            var dbList = await productMasterTagSettings.ToListAsync();
            var results = new List<GetProductMasterTagSettingForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductMasterTagSettingForViewDto()
                {
                    ProductMasterTagSetting = new ProductMasterTagSettingDto
                    {

                        DisplaySequence = o.DisplaySequence,
                        CustomTagTitle = o.CustomTagTitle,
                        CustomTagChatQuestion = o.CustomTagChatQuestion,
                        DisplayPublic = o.DisplayPublic,
                        AnswerTypeId = o.AnswerTypeId,
                        Id = o.Id,
                    },
                    ProductCategoryName = o.ProductCategoryName,
                    MasterTagCategoryName = o.MasterTagCategoryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductMasterTagSettingForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductMasterTagSettingForViewDto> GetProductMasterTagSettingForView(long id)
        {
            var productMasterTagSetting = await _productMasterTagSettingRepository.GetAsync(id);

            var output = new GetProductMasterTagSettingForViewDto { ProductMasterTagSetting = ObjectMapper.Map<ProductMasterTagSettingDto>(productMasterTagSetting) };

            if (output.ProductMasterTagSetting.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.ProductMasterTagSetting.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            if (output.ProductMasterTagSetting.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.ProductMasterTagSetting.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductMasterTagSettings_Edit)]
        public async Task<GetProductMasterTagSettingForEditOutput> GetProductMasterTagSettingForEdit(EntityDto<long> input)
        {
            var productMasterTagSetting = await _productMasterTagSettingRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductMasterTagSettingForEditOutput { ProductMasterTagSetting = ObjectMapper.Map<CreateOrEditProductMasterTagSettingDto>(productMasterTagSetting) };

            if (output.ProductMasterTagSetting.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.ProductMasterTagSetting.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            if (output.ProductMasterTagSetting.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.ProductMasterTagSetting.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductMasterTagSettingDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductMasterTagSettings_Create)]
        protected virtual async Task Create(CreateOrEditProductMasterTagSettingDto input)
        {
            var productMasterTagSetting = ObjectMapper.Map<ProductMasterTagSetting>(input);

            if (AbpSession.TenantId != null)
            {
                productMasterTagSetting.TenantId = (int?)AbpSession.TenantId;
            }

            await _productMasterTagSettingRepository.InsertAsync(productMasterTagSetting);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductMasterTagSettings_Edit)]
        protected virtual async Task Update(CreateOrEditProductMasterTagSettingDto input)
        {
            var productMasterTagSetting = await _productMasterTagSettingRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productMasterTagSetting);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductMasterTagSettings_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productMasterTagSettingRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductMasterTagSettingsToExcel(GetAllProductMasterTagSettingsForExcelInput input)
        {
            var answerTypeIdFilter = input.AnswerTypeIdFilter.HasValue
                        ? (AnswerType)input.AnswerTypeIdFilter
                        : default;

            var filteredProductMasterTagSettings = _productMasterTagSettingRepository.GetAll()
                        .Include(e => e.ProductCategoryFk)
                        .Include(e => e.MasterTagCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CustomTagTitle.Contains(input.Filter) || e.CustomTagChatQuestion.Contains(input.Filter))
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagTitleFilter), e => e.CustomTagTitle.Contains(input.CustomTagTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomTagChatQuestionFilter), e => e.CustomTagChatQuestion.Contains(input.CustomTagChatQuestionFilter))
                        .WhereIf(input.DisplayPublicFilter.HasValue && input.DisplayPublicFilter > -1, e => (input.DisplayPublicFilter == 1 && e.DisplayPublic) || (input.DisplayPublicFilter == 0 && !e.DisplayPublic))
                        .WhereIf(input.AnswerTypeIdFilter.HasValue && input.AnswerTypeIdFilter > -1, e => e.AnswerTypeId == answerTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter);

            var query = (from o in filteredProductMasterTagSettings
                         join o1 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetProductMasterTagSettingForViewDto()
                         {
                             ProductMasterTagSetting = new ProductMasterTagSettingDto
                             {
                                 DisplaySequence = o.DisplaySequence,
                                 CustomTagTitle = o.CustomTagTitle,
                                 CustomTagChatQuestion = o.CustomTagChatQuestion,
                                 DisplayPublic = o.DisplayPublic,
                                 AnswerTypeId = o.AnswerTypeId,
                                 Id = o.Id
                             },
                             ProductCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var productMasterTagSettingListDtos = await query.ToListAsync();

            return _productMasterTagSettingsExcelExporter.ExportToFile(productMasterTagSettingListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductMasterTagSettings)]
        public async Task<PagedResultDto<ProductMasterTagSettingProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductMasterTagSettingProductCategoryLookupTableDto>();
            foreach (var productCategory in productCategoryList)
            {
                lookupTableDtoList.Add(new ProductMasterTagSettingProductCategoryLookupTableDto
                {
                    Id = productCategory.Id,
                    DisplayName = productCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductMasterTagSettingProductCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductMasterTagSettings)]
        public async Task<PagedResultDto<ProductMasterTagSettingMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductMasterTagSettingMasterTagCategoryLookupTableDto>();
            foreach (var masterTagCategory in masterTagCategoryList)
            {
                lookupTableDtoList.Add(new ProductMasterTagSettingMasterTagCategoryLookupTableDto
                {
                    Id = masterTagCategory.Id,
                    DisplayName = masterTagCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductMasterTagSettingMasterTagCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}