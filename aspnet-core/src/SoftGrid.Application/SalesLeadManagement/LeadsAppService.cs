using SoftGrid.CRM;
using SoftGrid.CRM;
using SoftGrid.Shop;
using SoftGrid.Shop;
using SoftGrid.Shop;
using SoftGrid.CRM;
using SoftGrid.SalesLeadManagement;
using SoftGrid.SalesLeadManagement;
using SoftGrid.Territory;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.SalesLeadManagement.Exporting;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.SalesLeadManagement
{
    [AbpAuthorize(AppPermissions.Pages_Leads)]
    public class LeadsAppService : SoftGridAppServiceBase, ILeadsAppService
    {
        private readonly IRepository<Lead, long> _leadRepository;
        private readonly ILeadsExcelExporter _leadsExcelExporter;
        private readonly IRepository<Contact, long> _lookup_contactRepository;
        private readonly IRepository<Business, long> _lookup_businessRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;
        private readonly IRepository<ProductCategory, long> _lookup_productCategoryRepository;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<Employee, long> _lookup_employeeRepository;
        private readonly IRepository<LeadSource, long> _lookup_leadSourceRepository;
        private readonly IRepository<LeadPipelineStage, long> _lookup_leadPipelineStageRepository;
        private readonly IRepository<Hub, long> _lookup_hubRepository;

        public LeadsAppService(IRepository<Lead, long> leadRepository, ILeadsExcelExporter leadsExcelExporter, IRepository<Contact, long> lookup_contactRepository, IRepository<Business, long> lookup_businessRepository, IRepository<Product, long> lookup_productRepository, IRepository<ProductCategory, long> lookup_productCategoryRepository, IRepository<Store, long> lookup_storeRepository, IRepository<Employee, long> lookup_employeeRepository, IRepository<LeadSource, long> lookup_leadSourceRepository, IRepository<LeadPipelineStage, long> lookup_leadPipelineStageRepository, IRepository<Hub, long> lookup_hubRepository)
        {
            _leadRepository = leadRepository;
            _leadsExcelExporter = leadsExcelExporter;
            _lookup_contactRepository = lookup_contactRepository;
            _lookup_businessRepository = lookup_businessRepository;
            _lookup_productRepository = lookup_productRepository;
            _lookup_productCategoryRepository = lookup_productCategoryRepository;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_employeeRepository = lookup_employeeRepository;
            _lookup_leadSourceRepository = lookup_leadSourceRepository;
            _lookup_leadPipelineStageRepository = lookup_leadPipelineStageRepository;
            _lookup_hubRepository = lookup_hubRepository;

        }

        public async Task<PagedResultDto<GetLeadForViewDto>> GetAll(GetAllLeadsInput input)
        {

            var filteredLeads = _leadRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .Include(e => e.BusinessFk)
                        .Include(e => e.ProductFk)
                        .Include(e => e.ProductCategoryFk)
                        .Include(e => e.StoreFk)
                        .Include(e => e.EmployeeFk)
                        .Include(e => e.LeadSourceFk)
                        .Include(e => e.LeadPipelineStageFk)
                        .Include(e => e.HubFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.Phone.Contains(input.Filter) || e.Company.Contains(input.Filter) || e.JobTitle.Contains(input.Filter) || e.Industry.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title.Contains(input.TitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter), e => e.FirstName.Contains(input.FirstNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter), e => e.LastName.Contains(input.LastNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneFilter), e => e.Phone.Contains(input.PhoneFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CompanyFilter), e => e.Company.Contains(input.CompanyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobTitleFilter), e => e.JobTitle.Contains(input.JobTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IndustryFilter), e => e.Industry.Contains(input.IndustryFilter))
                        .WhereIf(input.MinLeadScoreFilter != null, e => e.LeadScore >= input.MinLeadScoreFilter)
                        .WhereIf(input.MaxLeadScoreFilter != null, e => e.LeadScore <= input.MaxLeadScoreFilter)
                        .WhereIf(input.MinExpectedSalesAmountFilter != null, e => e.ExpectedSalesAmount >= input.MinExpectedSalesAmountFilter)
                        .WhereIf(input.MaxExpectedSalesAmountFilter != null, e => e.ExpectedSalesAmount <= input.MaxExpectedSalesAmountFilter)
                        .WhereIf(input.MinCreatedDateFilter != null, e => e.CreatedDate >= input.MinCreatedDateFilter)
                        .WhereIf(input.MaxCreatedDateFilter != null, e => e.CreatedDate <= input.MaxCreatedDateFilter)
                        .WhereIf(input.MinExpectedClosingDateFilter != null, e => e.ExpectedClosingDate >= input.MinExpectedClosingDateFilter)
                        .WhereIf(input.MaxExpectedClosingDateFilter != null, e => e.ExpectedClosingDate <= input.MaxExpectedClosingDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LeadSourceNameFilter), e => e.LeadSourceFk != null && e.LeadSourceFk.Name == input.LeadSourceNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LeadPipelineStageNameFilter), e => e.LeadPipelineStageFk != null && e.LeadPipelineStageFk.Name == input.LeadPipelineStageNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter);

            var pagedAndFilteredLeads = filteredLeads
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var leads = from o in pagedAndFilteredLeads
                        join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                        from s1 in j1.DefaultIfEmpty()

                        join o2 in _lookup_businessRepository.GetAll() on o.BusinessId equals o2.Id into j2
                        from s2 in j2.DefaultIfEmpty()

                        join o3 in _lookup_productRepository.GetAll() on o.ProductId equals o3.Id into j3
                        from s3 in j3.DefaultIfEmpty()

                        join o4 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o4.Id into j4
                        from s4 in j4.DefaultIfEmpty()

                        join o5 in _lookup_storeRepository.GetAll() on o.StoreId equals o5.Id into j5
                        from s5 in j5.DefaultIfEmpty()

                        join o6 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o6.Id into j6
                        from s6 in j6.DefaultIfEmpty()

                        join o7 in _lookup_leadSourceRepository.GetAll() on o.LeadSourceId equals o7.Id into j7
                        from s7 in j7.DefaultIfEmpty()

                        join o8 in _lookup_leadPipelineStageRepository.GetAll() on o.LeadPipelineStageId equals o8.Id into j8
                        from s8 in j8.DefaultIfEmpty()

                        join o9 in _lookup_hubRepository.GetAll() on o.HubId equals o9.Id into j9
                        from s9 in j9.DefaultIfEmpty()

                        select new
                        {

                            o.Title,
                            o.FirstName,
                            o.LastName,
                            o.Email,
                            o.Phone,
                            o.Company,
                            o.JobTitle,
                            o.Industry,
                            o.LeadScore,
                            o.ExpectedSalesAmount,
                            o.CreatedDate,
                            o.ExpectedClosingDate,
                            Id = o.Id,
                            ContactFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                            BusinessName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                            ProductName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                            ProductCategoryName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                            StoreName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
                            EmployeeName = s6 == null || s6.Name == null ? "" : s6.Name.ToString(),
                            LeadSourceName = s7 == null || s7.Name == null ? "" : s7.Name.ToString(),
                            LeadPipelineStageName = s8 == null || s8.Name == null ? "" : s8.Name.ToString(),
                            HubName = s9 == null || s9.Name == null ? "" : s9.Name.ToString()
                        };

            var totalCount = await filteredLeads.CountAsync();

            var dbList = await leads.ToListAsync();
            var results = new List<GetLeadForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetLeadForViewDto()
                {
                    Lead = new LeadDto
                    {

                        Title = o.Title,
                        FirstName = o.FirstName,
                        LastName = o.LastName,
                        Email = o.Email,
                        Phone = o.Phone,
                        Company = o.Company,
                        JobTitle = o.JobTitle,
                        Industry = o.Industry,
                        LeadScore = o.LeadScore,
                        ExpectedSalesAmount = o.ExpectedSalesAmount,
                        CreatedDate = o.CreatedDate,
                        ExpectedClosingDate = o.ExpectedClosingDate,
                        Id = o.Id,
                    },
                    ContactFullName = o.ContactFullName,
                    BusinessName = o.BusinessName,
                    ProductName = o.ProductName,
                    ProductCategoryName = o.ProductCategoryName,
                    StoreName = o.StoreName,
                    EmployeeName = o.EmployeeName,
                    LeadSourceName = o.LeadSourceName,
                    LeadPipelineStageName = o.LeadPipelineStageName,
                    HubName = o.HubName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetLeadForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetLeadForViewDto> GetLeadForView(long id)
        {
            var lead = await _leadRepository.GetAsync(id);

            var output = new GetLeadForViewDto { Lead = ObjectMapper.Map<LeadDto>(lead) };

            if (output.Lead.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.Lead.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.Lead.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.Lead.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            if (output.Lead.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.Lead.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.Lead.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.Lead.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            if (output.Lead.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.Lead.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.Lead.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.Lead.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            if (output.Lead.LeadSourceId != null)
            {
                var _lookupLeadSource = await _lookup_leadSourceRepository.FirstOrDefaultAsync((long)output.Lead.LeadSourceId);
                output.LeadSourceName = _lookupLeadSource?.Name?.ToString();
            }

            if (output.Lead.LeadPipelineStageId != null)
            {
                var _lookupLeadPipelineStage = await _lookup_leadPipelineStageRepository.FirstOrDefaultAsync((long)output.Lead.LeadPipelineStageId);
                output.LeadPipelineStageName = _lookupLeadPipelineStage?.Name?.ToString();
            }

            if (output.Lead.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.Lead.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Leads_Edit)]
        public async Task<GetLeadForEditOutput> GetLeadForEdit(EntityDto<long> input)
        {
            var lead = await _leadRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLeadForEditOutput { Lead = ObjectMapper.Map<CreateOrEditLeadDto>(lead) };

            if (output.Lead.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.Lead.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.Lead.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.Lead.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            if (output.Lead.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.Lead.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.Lead.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.Lead.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            if (output.Lead.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.Lead.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.Lead.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.Lead.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            if (output.Lead.LeadSourceId != null)
            {
                var _lookupLeadSource = await _lookup_leadSourceRepository.FirstOrDefaultAsync((long)output.Lead.LeadSourceId);
                output.LeadSourceName = _lookupLeadSource?.Name?.ToString();
            }

            if (output.Lead.LeadPipelineStageId != null)
            {
                var _lookupLeadPipelineStage = await _lookup_leadPipelineStageRepository.FirstOrDefaultAsync((long)output.Lead.LeadPipelineStageId);
                output.LeadPipelineStageName = _lookupLeadPipelineStage?.Name?.ToString();
            }

            if (output.Lead.HubId != null)
            {
                var _lookupHub = await _lookup_hubRepository.FirstOrDefaultAsync((long)output.Lead.HubId);
                output.HubName = _lookupHub?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditLeadDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Leads_Create)]
        protected virtual async Task Create(CreateOrEditLeadDto input)
        {
            var lead = ObjectMapper.Map<Lead>(input);

            if (AbpSession.TenantId != null)
            {
                lead.TenantId = (int?)AbpSession.TenantId;
            }

            await _leadRepository.InsertAsync(lead);

        }

        [AbpAuthorize(AppPermissions.Pages_Leads_Edit)]
        protected virtual async Task Update(CreateOrEditLeadDto input)
        {
            var lead = await _leadRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, lead);

        }

        [AbpAuthorize(AppPermissions.Pages_Leads_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _leadRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetLeadsToExcel(GetAllLeadsForExcelInput input)
        {

            var filteredLeads = _leadRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .Include(e => e.BusinessFk)
                        .Include(e => e.ProductFk)
                        .Include(e => e.ProductCategoryFk)
                        .Include(e => e.StoreFk)
                        .Include(e => e.EmployeeFk)
                        .Include(e => e.LeadSourceFk)
                        .Include(e => e.LeadPipelineStageFk)
                        .Include(e => e.HubFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.Phone.Contains(input.Filter) || e.Company.Contains(input.Filter) || e.JobTitle.Contains(input.Filter) || e.Industry.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title.Contains(input.TitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter), e => e.FirstName.Contains(input.FirstNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter), e => e.LastName.Contains(input.LastNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneFilter), e => e.Phone.Contains(input.PhoneFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CompanyFilter), e => e.Company.Contains(input.CompanyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobTitleFilter), e => e.JobTitle.Contains(input.JobTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IndustryFilter), e => e.Industry.Contains(input.IndustryFilter))
                        .WhereIf(input.MinLeadScoreFilter != null, e => e.LeadScore >= input.MinLeadScoreFilter)
                        .WhereIf(input.MaxLeadScoreFilter != null, e => e.LeadScore <= input.MaxLeadScoreFilter)
                        .WhereIf(input.MinExpectedSalesAmountFilter != null, e => e.ExpectedSalesAmount >= input.MinExpectedSalesAmountFilter)
                        .WhereIf(input.MaxExpectedSalesAmountFilter != null, e => e.ExpectedSalesAmount <= input.MaxExpectedSalesAmountFilter)
                        .WhereIf(input.MinCreatedDateFilter != null, e => e.CreatedDate >= input.MinCreatedDateFilter)
                        .WhereIf(input.MaxCreatedDateFilter != null, e => e.CreatedDate <= input.MaxCreatedDateFilter)
                        .WhereIf(input.MinExpectedClosingDateFilter != null, e => e.ExpectedClosingDate >= input.MinExpectedClosingDateFilter)
                        .WhereIf(input.MaxExpectedClosingDateFilter != null, e => e.ExpectedClosingDate <= input.MaxExpectedClosingDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LeadSourceNameFilter), e => e.LeadSourceFk != null && e.LeadSourceFk.Name == input.LeadSourceNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LeadPipelineStageNameFilter), e => e.LeadPipelineStageFk != null && e.LeadPipelineStageFk.Name == input.LeadPipelineStageNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HubNameFilter), e => e.HubFk != null && e.HubFk.Name == input.HubNameFilter);

            var query = (from o in filteredLeads
                         join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_businessRepository.GetAll() on o.BusinessId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_productRepository.GetAll() on o.ProductId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         join o5 in _lookup_storeRepository.GetAll() on o.StoreId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()

                         join o6 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o6.Id into j6
                         from s6 in j6.DefaultIfEmpty()

                         join o7 in _lookup_leadSourceRepository.GetAll() on o.LeadSourceId equals o7.Id into j7
                         from s7 in j7.DefaultIfEmpty()

                         join o8 in _lookup_leadPipelineStageRepository.GetAll() on o.LeadPipelineStageId equals o8.Id into j8
                         from s8 in j8.DefaultIfEmpty()

                         join o9 in _lookup_hubRepository.GetAll() on o.HubId equals o9.Id into j9
                         from s9 in j9.DefaultIfEmpty()

                         select new GetLeadForViewDto()
                         {
                             Lead = new LeadDto
                             {
                                 Title = o.Title,
                                 FirstName = o.FirstName,
                                 LastName = o.LastName,
                                 Email = o.Email,
                                 Phone = o.Phone,
                                 Company = o.Company,
                                 JobTitle = o.JobTitle,
                                 Industry = o.Industry,
                                 LeadScore = o.LeadScore,
                                 ExpectedSalesAmount = o.ExpectedSalesAmount,
                                 CreatedDate = o.CreatedDate,
                                 ExpectedClosingDate = o.ExpectedClosingDate,
                                 Id = o.Id
                             },
                             ContactFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                             BusinessName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             ProductName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             ProductCategoryName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                             StoreName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
                             EmployeeName = s6 == null || s6.Name == null ? "" : s6.Name.ToString(),
                             LeadSourceName = s7 == null || s7.Name == null ? "" : s7.Name.ToString(),
                             LeadPipelineStageName = s8 == null || s8.Name == null ? "" : s8.Name.ToString(),
                             HubName = s9 == null || s9.Name == null ? "" : s9.Name.ToString()
                         });

            var leadListDtos = await query.ToListAsync();

            return _leadsExcelExporter.ExportToFile(leadListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Leads)]
        public async Task<PagedResultDto<LeadContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new LeadContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<LeadContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Leads)]
        public async Task<PagedResultDto<LeadBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_businessRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var businessList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadBusinessLookupTableDto>();
            foreach (var business in businessList)
            {
                lookupTableDtoList.Add(new LeadBusinessLookupTableDto
                {
                    Id = business.Id,
                    DisplayName = business.Name?.ToString()
                });
            }

            return new PagedResultDto<LeadBusinessLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Leads)]
        public async Task<PagedResultDto<LeadProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new LeadProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<LeadProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Leads)]
        public async Task<PagedResultDto<LeadProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadProductCategoryLookupTableDto>();
            foreach (var productCategory in productCategoryList)
            {
                lookupTableDtoList.Add(new LeadProductCategoryLookupTableDto
                {
                    Id = productCategory.Id,
                    DisplayName = productCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<LeadProductCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Leads)]
        public async Task<PagedResultDto<LeadStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new LeadStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<LeadStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Leads)]
        public async Task<PagedResultDto<LeadEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_employeeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var employeeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadEmployeeLookupTableDto>();
            foreach (var employee in employeeList)
            {
                lookupTableDtoList.Add(new LeadEmployeeLookupTableDto
                {
                    Id = employee.Id,
                    DisplayName = employee.Name?.ToString()
                });
            }

            return new PagedResultDto<LeadEmployeeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Leads)]
        public async Task<PagedResultDto<LeadLeadSourceLookupTableDto>> GetAllLeadSourceForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_leadSourceRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var leadSourceList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadLeadSourceLookupTableDto>();
            foreach (var leadSource in leadSourceList)
            {
                lookupTableDtoList.Add(new LeadLeadSourceLookupTableDto
                {
                    Id = leadSource.Id,
                    DisplayName = leadSource.Name?.ToString()
                });
            }

            return new PagedResultDto<LeadLeadSourceLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Leads)]
        public async Task<PagedResultDto<LeadLeadPipelineStageLookupTableDto>> GetAllLeadPipelineStageForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_leadPipelineStageRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var leadPipelineStageList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadLeadPipelineStageLookupTableDto>();
            foreach (var leadPipelineStage in leadPipelineStageList)
            {
                lookupTableDtoList.Add(new LeadLeadPipelineStageLookupTableDto
                {
                    Id = leadPipelineStage.Id,
                    DisplayName = leadPipelineStage.Name?.ToString()
                });
            }

            return new PagedResultDto<LeadLeadPipelineStageLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Leads)]
        public async Task<PagedResultDto<LeadHubLookupTableDto>> GetAllHubForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_hubRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var hubList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LeadHubLookupTableDto>();
            foreach (var hub in hubList)
            {
                lookupTableDtoList.Add(new LeadHubLookupTableDto
                {
                    Id = hub.Id,
                    DisplayName = hub.Name?.ToString()
                });
            }

            return new PagedResultDto<LeadHubLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}