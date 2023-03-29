using SoftGrid.Shop;
using SoftGrid.CRM;
using SoftGrid.CRM;

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
    [AbpAuthorize(AppPermissions.Pages_ProductCustomerQueries)]
    public class ProductCustomerQueriesAppService : SoftGridAppServiceBase, IProductCustomerQueriesAppService
    {
        private readonly IRepository<ProductCustomerQuery, long> _productCustomerQueryRepository;
        private readonly IProductCustomerQueriesExcelExporter _productCustomerQueriesExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<Contact, long> _lookup_contactRepository;
        private readonly IRepository<Employee, long> _lookup_employeeRepository;

        public ProductCustomerQueriesAppService(IRepository<ProductCustomerQuery, long> productCustomerQueryRepository, IProductCustomerQueriesExcelExporter productCustomerQueriesExcelExporter, IRepository<Product, long> lookup_productRepository, IRepository<Contact, long> lookup_contactRepository, IRepository<Employee, long> lookup_employeeRepository)
        {
            _productCustomerQueryRepository = productCustomerQueryRepository;
            _productCustomerQueriesExcelExporter = productCustomerQueriesExcelExporter;
            _lookup_productRepository = lookup_productRepository;
            _lookup_contactRepository = lookup_contactRepository;
            _lookup_employeeRepository = lookup_employeeRepository;

        }

        public async Task<PagedResultDto<GetProductCustomerQueryForViewDto>> GetAll(GetAllProductCustomerQueriesInput input)
        {

            var filteredProductCustomerQueries = _productCustomerQueryRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.ContactFk)
                        .Include(e => e.EmployeeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Question.Contains(input.Filter) || e.Answer.Contains(input.Filter) || e.AnswerTime.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.QuestionFilter), e => e.Question.Contains(input.QuestionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AnswerFilter), e => e.Answer.Contains(input.AnswerFilter))
                        .WhereIf(input.MinAnswerDateFilter != null, e => e.AnswerDate >= input.MinAnswerDateFilter)
                        .WhereIf(input.MaxAnswerDateFilter != null, e => e.AnswerDate <= input.MaxAnswerDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AnswerTimeFilter), e => e.AnswerTime.Contains(input.AnswerTimeFilter))
                        .WhereIf(input.PublishFilter.HasValue && input.PublishFilter > -1, e => (input.PublishFilter == 1 && e.Publish) || (input.PublishFilter == 0 && !e.Publish))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter);

            var pagedAndFilteredProductCustomerQueries = filteredProductCustomerQueries
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productCustomerQueries = from o in pagedAndFilteredProductCustomerQueries
                                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                         from s1 in j1.DefaultIfEmpty()

                                         join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                                         from s2 in j2.DefaultIfEmpty()

                                         join o3 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o3.Id into j3
                                         from s3 in j3.DefaultIfEmpty()

                                         select new
                                         {

                                             o.Question,
                                             o.Answer,
                                             o.AnswerDate,
                                             o.AnswerTime,
                                             o.Publish,
                                             Id = o.Id,
                                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                             ContactFullName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString(),
                                             EmployeeName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                                         };

            var totalCount = await filteredProductCustomerQueries.CountAsync();

            var dbList = await productCustomerQueries.ToListAsync();
            var results = new List<GetProductCustomerQueryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductCustomerQueryForViewDto()
                {
                    ProductCustomerQuery = new ProductCustomerQueryDto
                    {

                        Question = o.Question,
                        Answer = o.Answer,
                        AnswerDate = o.AnswerDate,
                        AnswerTime = o.AnswerTime,
                        Publish = o.Publish,
                        Id = o.Id,
                    },
                    ProductName = o.ProductName,
                    ContactFullName = o.ContactFullName,
                    EmployeeName = o.EmployeeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductCustomerQueryForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductCustomerQueryForViewDto> GetProductCustomerQueryForView(long id)
        {
            var productCustomerQuery = await _productCustomerQueryRepository.GetAsync(id);

            var output = new GetProductCustomerQueryForViewDto { ProductCustomerQuery = ObjectMapper.Map<ProductCustomerQueryDto>(productCustomerQuery) };

            if (output.ProductCustomerQuery.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductCustomerQuery.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductCustomerQuery.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.ProductCustomerQuery.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.ProductCustomerQuery.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.ProductCustomerQuery.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCustomerQueries_Edit)]
        public async Task<GetProductCustomerQueryForEditOutput> GetProductCustomerQueryForEdit(EntityDto<long> input)
        {
            var productCustomerQuery = await _productCustomerQueryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductCustomerQueryForEditOutput { ProductCustomerQuery = ObjectMapper.Map<CreateOrEditProductCustomerQueryDto>(productCustomerQuery) };

            if (output.ProductCustomerQuery.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductCustomerQuery.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductCustomerQuery.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.ProductCustomerQuery.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.ProductCustomerQuery.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.ProductCustomerQuery.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductCustomerQueryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductCustomerQueries_Create)]
        protected virtual async Task Create(CreateOrEditProductCustomerQueryDto input)
        {
            var productCustomerQuery = ObjectMapper.Map<ProductCustomerQuery>(input);

            if (AbpSession.TenantId != null)
            {
                productCustomerQuery.TenantId = (int?)AbpSession.TenantId;
            }

            await _productCustomerQueryRepository.InsertAsync(productCustomerQuery);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductCustomerQueries_Edit)]
        protected virtual async Task Update(CreateOrEditProductCustomerQueryDto input)
        {
            var productCustomerQuery = await _productCustomerQueryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productCustomerQuery);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductCustomerQueries_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productCustomerQueryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductCustomerQueriesToExcel(GetAllProductCustomerQueriesForExcelInput input)
        {

            var filteredProductCustomerQueries = _productCustomerQueryRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.ContactFk)
                        .Include(e => e.EmployeeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Question.Contains(input.Filter) || e.Answer.Contains(input.Filter) || e.AnswerTime.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.QuestionFilter), e => e.Question.Contains(input.QuestionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AnswerFilter), e => e.Answer.Contains(input.AnswerFilter))
                        .WhereIf(input.MinAnswerDateFilter != null, e => e.AnswerDate >= input.MinAnswerDateFilter)
                        .WhereIf(input.MaxAnswerDateFilter != null, e => e.AnswerDate <= input.MaxAnswerDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AnswerTimeFilter), e => e.AnswerTime.Contains(input.AnswerTimeFilter))
                        .WhereIf(input.PublishFilter.HasValue && input.PublishFilter > -1, e => (input.PublishFilter == 1 && e.Publish) || (input.PublishFilter == 0 && !e.Publish))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter);

            var query = (from o in filteredProductCustomerQueries
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetProductCustomerQueryForViewDto()
                         {
                             ProductCustomerQuery = new ProductCustomerQueryDto
                             {
                                 Question = o.Question,
                                 Answer = o.Answer,
                                 AnswerDate = o.AnswerDate,
                                 AnswerTime = o.AnswerTime,
                                 Publish = o.Publish,
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ContactFullName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString(),
                             EmployeeName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var productCustomerQueryListDtos = await query.ToListAsync();

            return _productCustomerQueriesExcelExporter.ExportToFile(productCustomerQueryListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCustomerQueries)]
        public async Task<PagedResultDto<ProductCustomerQueryProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductCustomerQueryProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductCustomerQueryProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductCustomerQueryProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCustomerQueries)]
        public async Task<PagedResultDto<ProductCustomerQueryContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductCustomerQueryContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new ProductCustomerQueryContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<ProductCustomerQueryContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCustomerQueries)]
        public async Task<PagedResultDto<ProductCustomerQueryEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_employeeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var employeeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductCustomerQueryEmployeeLookupTableDto>();
            foreach (var employee in employeeList)
            {
                lookupTableDtoList.Add(new ProductCustomerQueryEmployeeLookupTableDto
                {
                    Id = employee.Id,
                    DisplayName = employee.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductCustomerQueryEmployeeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}