using SoftGrid.Shop;

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
    [AbpAuthorize(AppPermissions.Pages_ProductFaqs)]
    public class ProductFaqsAppService : SoftGridAppServiceBase, IProductFaqsAppService
    {
        private readonly IRepository<ProductFaq, long> _productFaqRepository;
        private readonly IProductFaqsExcelExporter _productFaqsExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;

        public ProductFaqsAppService(IRepository<ProductFaq, long> productFaqRepository, IProductFaqsExcelExporter productFaqsExcelExporter, IRepository<Product, long> lookup_productRepository)
        {
            _productFaqRepository = productFaqRepository;
            _productFaqsExcelExporter = productFaqsExcelExporter;
            _lookup_productRepository = lookup_productRepository;

        }

        public async Task<PagedResultDto<GetProductFaqForViewDto>> GetAll(GetAllProductFaqsInput input)
        {

            var filteredProductFaqs = _productFaqRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Question.Contains(input.Filter) || e.Answer.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.QuestionFilter), e => e.Question.Contains(input.QuestionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AnswerFilter), e => e.Answer.Contains(input.AnswerFilter))
                        .WhereIf(input.TemplateFilter.HasValue && input.TemplateFilter > -1, e => (input.TemplateFilter == 1 && e.Template) || (input.TemplateFilter == 0 && !e.Template))
                        .WhereIf(input.PublishFilter.HasValue && input.PublishFilter > -1, e => (input.PublishFilter == 1 && e.Publish) || (input.PublishFilter == 0 && !e.Publish))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var pagedAndFilteredProductFaqs = filteredProductFaqs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productFaqs = from o in pagedAndFilteredProductFaqs
                              join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                              from s1 in j1.DefaultIfEmpty()

                              select new
                              {

                                  o.Question,
                                  o.Answer,
                                  o.Template,
                                  o.Publish,
                                  Id = o.Id,
                                  ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                              };

            var totalCount = await filteredProductFaqs.CountAsync();

            var dbList = await productFaqs.ToListAsync();
            var results = new List<GetProductFaqForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductFaqForViewDto()
                {
                    ProductFaq = new ProductFaqDto
                    {

                        Question = o.Question,
                        Answer = o.Answer,
                        Template = o.Template,
                        Publish = o.Publish,
                        Id = o.Id,
                    },
                    ProductName = o.ProductName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductFaqForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductFaqForViewDto> GetProductFaqForView(long id)
        {
            var productFaq = await _productFaqRepository.GetAsync(id);

            var output = new GetProductFaqForViewDto { ProductFaq = ObjectMapper.Map<ProductFaqDto>(productFaq) };

            if (output.ProductFaq.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductFaq.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductFaqs_Edit)]
        public async Task<GetProductFaqForEditOutput> GetProductFaqForEdit(EntityDto<long> input)
        {
            var productFaq = await _productFaqRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductFaqForEditOutput { ProductFaq = ObjectMapper.Map<CreateOrEditProductFaqDto>(productFaq) };

            if (output.ProductFaq.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductFaq.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductFaqDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductFaqs_Create)]
        protected virtual async Task Create(CreateOrEditProductFaqDto input)
        {
            var productFaq = ObjectMapper.Map<ProductFaq>(input);

            if (AbpSession.TenantId != null)
            {
                productFaq.TenantId = (int?)AbpSession.TenantId;
            }

            await _productFaqRepository.InsertAsync(productFaq);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductFaqs_Edit)]
        protected virtual async Task Update(CreateOrEditProductFaqDto input)
        {
            var productFaq = await _productFaqRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productFaq);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductFaqs_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productFaqRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductFaqsToExcel(GetAllProductFaqsForExcelInput input)
        {

            var filteredProductFaqs = _productFaqRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Question.Contains(input.Filter) || e.Answer.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.QuestionFilter), e => e.Question.Contains(input.QuestionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AnswerFilter), e => e.Answer.Contains(input.AnswerFilter))
                        .WhereIf(input.TemplateFilter.HasValue && input.TemplateFilter > -1, e => (input.TemplateFilter == 1 && e.Template) || (input.TemplateFilter == 0 && !e.Template))
                        .WhereIf(input.PublishFilter.HasValue && input.PublishFilter > -1, e => (input.PublishFilter == 1 && e.Publish) || (input.PublishFilter == 0 && !e.Publish))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var query = (from o in filteredProductFaqs
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetProductFaqForViewDto()
                         {
                             ProductFaq = new ProductFaqDto
                             {
                                 Question = o.Question,
                                 Answer = o.Answer,
                                 Template = o.Template,
                                 Publish = o.Publish,
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var productFaqListDtos = await query.ToListAsync();

            return _productFaqsExcelExporter.ExportToFile(productFaqListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductFaqs)]
        public async Task<PagedResultDto<ProductFaqProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductFaqProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductFaqProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductFaqProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}