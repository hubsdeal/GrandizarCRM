using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;

using Microsoft.EntityFrameworkCore;

using SoftGrid.Authorization;
using SoftGrid.CRM;
using SoftGrid.Dto;
using SoftGrid.JobManagement.Dtos;
using SoftGrid.JobManagement.Exporting;
using SoftGrid.LookupData;
using SoftGrid.Shop;

using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace SoftGrid.JobManagement
{
    [AbpAuthorize(AppPermissions.Pages_Jobs)]
    public class JobsAppService : SoftGridAppServiceBase, IJobsAppService
    {
        private readonly IRepository<Job, long> _jobRepository;
        private readonly IJobsExcelExporter _jobsExcelExporter;
        private readonly IRepository<MasterTagCategory, long> _lookup_masterTagCategoryRepository;
        private readonly IRepository<MasterTag, long> _lookup_masterTagRepository;
        private readonly IRepository<ProductCategory, long> _lookup_productCategoryRepository;
        private readonly IRepository<Currency, long> _lookup_currencyRepository;
        private readonly IRepository<Business, long> _lookup_businessRepository;
        private readonly IRepository<Country, long> _lookup_countryRepository;
        private readonly IRepository<State, long> _lookup_stateRepository;
        private readonly IRepository<City, long> _lookup_cityRepository;
        private readonly IRepository<JobStatusType, long> _lookup_jobStatusTypeRepository;
        private readonly IRepository<Store, long> _lookup_storeRepository;

        public JobsAppService(IRepository<Job, long> jobRepository, IJobsExcelExporter jobsExcelExporter, IRepository<MasterTagCategory, long> lookup_masterTagCategoryRepository, IRepository<MasterTag, long> lookup_masterTagRepository, IRepository<ProductCategory, long> lookup_productCategoryRepository, IRepository<Currency, long> lookup_currencyRepository, IRepository<Business, long> lookup_businessRepository, IRepository<Country, long> lookup_countryRepository, IRepository<State, long> lookup_stateRepository, IRepository<City, long> lookup_cityRepository, IRepository<JobStatusType, long> lookup_jobStatusTypeRepository, IRepository<Store, long> lookup_storeRepository)
        {
            _jobRepository = jobRepository;
            _jobsExcelExporter = jobsExcelExporter;
            _lookup_masterTagCategoryRepository = lookup_masterTagCategoryRepository;
            _lookup_masterTagRepository = lookup_masterTagRepository;
            _lookup_productCategoryRepository = lookup_productCategoryRepository;
            _lookup_currencyRepository = lookup_currencyRepository;
            _lookup_businessRepository = lookup_businessRepository;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_stateRepository = lookup_stateRepository;
            _lookup_cityRepository = lookup_cityRepository;
            _lookup_jobStatusTypeRepository = lookup_jobStatusTypeRepository;
            _lookup_storeRepository = lookup_storeRepository;

        }

        public async Task<PagedResultDto<GetJobForViewDto>> GetAll(GetAllJobsInput input)
        {

            var filteredJobs = _jobRepository.GetAll()
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.MasterTagFk)
                        .Include(e => e.ProductCategoryFk)
                        .Include(e => e.CurrencyFk)
                        .Include(e => e.BusinessFk)
                        .Include(e => e.CountryFk)
                        .Include(e => e.StateFk)
                        .Include(e => e.CityFk)
                        .Include(e => e.JobStatusTypeFk)
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) || e.SalaryOrStaffingRate.Contains(input.Filter) || e.ReferralPoints.Contains(input.Filter) || e.MinimumExperience.Contains(input.Filter) || e.MaximumExperience.Contains(input.Filter) || e.JobDescription.Contains(input.Filter) || e.JobLocationFullAddress.Contains(input.Filter) || e.ZipCode.Contains(input.Filter) || e.InternalJobDescription.Contains(input.Filter) || e.CityLocation.Contains(input.Filter) || e.Url.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title.Contains(input.TitleFilter))
                        .WhereIf(input.FullTimeJobOrGigWorkProjectFilter.HasValue && input.FullTimeJobOrGigWorkProjectFilter > -1, e => (input.FullTimeJobOrGigWorkProjectFilter == 1 && e.FullTimeJobOrGigWorkProject) || (input.FullTimeJobOrGigWorkProjectFilter == 0 && !e.FullTimeJobOrGigWorkProject))
                        .WhereIf(input.RemoteWorkOrOnSiteWorkFilter.HasValue && input.RemoteWorkOrOnSiteWorkFilter > -1, e => (input.RemoteWorkOrOnSiteWorkFilter == 1 && e.RemoteWorkOrOnSiteWork) || (input.RemoteWorkOrOnSiteWorkFilter == 0 && !e.RemoteWorkOrOnSiteWork))
                        .WhereIf(input.SalaryBasedOrFixedPriceFilter.HasValue && input.SalaryBasedOrFixedPriceFilter > -1, e => (input.SalaryBasedOrFixedPriceFilter == 1 && e.SalaryBasedOrFixedPrice) || (input.SalaryBasedOrFixedPriceFilter == 0 && !e.SalaryBasedOrFixedPrice))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SalaryOrStaffingRateFilter), e => e.SalaryOrStaffingRate.Contains(input.SalaryOrStaffingRateFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReferralPointsFilter), e => e.ReferralPoints.Contains(input.ReferralPointsFilter))
                        .WhereIf(input.TemplateFilter.HasValue && input.TemplateFilter > -1, e => (input.TemplateFilter == 1 && e.Template) || (input.TemplateFilter == 0 && !e.Template))
                        .WhereIf(input.MinNumberOfJobsFilter != null, e => e.NumberOfJobs >= input.MinNumberOfJobsFilter)
                        .WhereIf(input.MaxNumberOfJobsFilter != null, e => e.NumberOfJobs <= input.MaxNumberOfJobsFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MinimumExperienceFilter), e => e.MinimumExperience.Contains(input.MinimumExperienceFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MaximumExperienceFilter), e => e.MaximumExperience.Contains(input.MaximumExperienceFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobDescriptionFilter), e => e.JobDescription.Contains(input.JobDescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobLocationFullAddressFilter), e => e.JobLocationFullAddress.Contains(input.JobLocationFullAddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter))
                        .WhereIf(input.MinLatitudeFilter != null, e => e.Latitude >= input.MinLatitudeFilter)
                        .WhereIf(input.MaxLatitudeFilter != null, e => e.Latitude <= input.MaxLatitudeFilter)
                        .WhereIf(input.MinLongitudeFilter != null, e => e.Longitude >= input.MinLongitudeFilter)
                        .WhereIf(input.MaxLongitudeFilter != null, e => e.Longitude <= input.MaxLongitudeFilter)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinHireByDateFilter != null, e => e.HireByDate >= input.MinHireByDateFilter)
                        .WhereIf(input.MaxHireByDateFilter != null, e => e.HireByDate <= input.MaxHireByDateFilter)
                        .WhereIf(input.MinPublishDateFilter != null, e => e.PublishDate >= input.MinPublishDateFilter)
                        .WhereIf(input.MaxPublishDateFilter != null, e => e.PublishDate <= input.MaxPublishDateFilter)
                        .WhereIf(input.MinExpirationDateFilter != null, e => e.ExpirationDate >= input.MinExpirationDateFilter)
                        .WhereIf(input.MaxExpirationDateFilter != null, e => e.ExpirationDate <= input.MaxExpirationDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InternalJobDescriptionFilter), e => e.InternalJobDescription.Contains(input.InternalJobDescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityLocationFilter), e => e.CityLocation.Contains(input.CityLocationFilter))
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UrlFilter), e => e.Url.Contains(input.UrlFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.MasterTagFk != null && e.MasterTagFk.Name == input.MasterTagNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyNameFilter), e => e.CurrencyFk != null && e.CurrencyFk.Name == input.CurrencyNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityNameFilter), e => e.CityFk != null && e.CityFk.Name == input.CityNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobStatusTypeNameFilter), e => e.JobStatusTypeFk != null && e.JobStatusTypeFk.Name == input.JobStatusTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var pagedAndFilteredJobs = filteredJobs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var jobs = from o in pagedAndFilteredJobs
                       join o1 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o1.Id into j1
                       from s1 in j1.DefaultIfEmpty()

                       join o2 in _lookup_masterTagRepository.GetAll() on o.MasterTagId equals o2.Id into j2
                       from s2 in j2.DefaultIfEmpty()

                       join o3 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o3.Id into j3
                       from s3 in j3.DefaultIfEmpty()

                       join o4 in _lookup_currencyRepository.GetAll() on o.CurrencyId equals o4.Id into j4
                       from s4 in j4.DefaultIfEmpty()

                       join o5 in _lookup_businessRepository.GetAll() on o.BusinessId equals o5.Id into j5
                       from s5 in j5.DefaultIfEmpty()

                       join o6 in _lookup_countryRepository.GetAll() on o.CountryId equals o6.Id into j6
                       from s6 in j6.DefaultIfEmpty()

                       join o7 in _lookup_stateRepository.GetAll() on o.StateId equals o7.Id into j7
                       from s7 in j7.DefaultIfEmpty()

                       join o8 in _lookup_cityRepository.GetAll() on o.CityId equals o8.Id into j8
                       from s8 in j8.DefaultIfEmpty()

                       join o9 in _lookup_jobStatusTypeRepository.GetAll() on o.JobStatusTypeId equals o9.Id into j9
                       from s9 in j9.DefaultIfEmpty()

                       join o10 in _lookup_storeRepository.GetAll() on o.StoreId equals o10.Id into j10
                       from s10 in j10.DefaultIfEmpty()

                       select new
                       {

                           o.Title,
                           o.FullTimeJobOrGigWorkProject,
                           o.RemoteWorkOrOnSiteWork,
                           o.SalaryBasedOrFixedPrice,
                           o.SalaryOrStaffingRate,
                           o.ReferralPoints,
                           o.Template,
                           o.NumberOfJobs,
                           o.MinimumExperience,
                           o.MaximumExperience,
                           o.JobDescription,
                           o.JobLocationFullAddress,
                           o.ZipCode,
                           o.Latitude,
                           o.Longitude,
                           o.StartDate,
                           o.HireByDate,
                           o.PublishDate,
                           o.ExpirationDate,
                           o.InternalJobDescription,
                           o.CityLocation,
                           o.Published,
                           o.Url,
                           Id = o.Id,
                           MasterTagCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                           MasterTagName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                           ProductCategoryName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                           CurrencyName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                           BusinessName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
                           CountryName = s6 == null || s6.Name == null ? "" : s6.Name.ToString(),
                           StateName = s7 == null || s7.Name == null ? "" : s7.Name.ToString(),
                           CityName = s8 == null || s8.Name == null ? "" : s8.Name.ToString(),
                           JobStatusTypeName = s9 == null || s9.Name == null ? "" : s9.Name.ToString(),
                           StoreName = s10 == null || s10.Name == null ? "" : s10.Name.ToString()
                       };

            var totalCount = await filteredJobs.CountAsync();

            var dbList = await jobs.ToListAsync();
            var results = new List<GetJobForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetJobForViewDto()
                {
                    Job = new JobDto
                    {

                        Title = o.Title,
                        FullTimeJobOrGigWorkProject = o.FullTimeJobOrGigWorkProject,
                        RemoteWorkOrOnSiteWork = o.RemoteWorkOrOnSiteWork,
                        SalaryBasedOrFixedPrice = o.SalaryBasedOrFixedPrice,
                        SalaryOrStaffingRate = o.SalaryOrStaffingRate,
                        ReferralPoints = o.ReferralPoints,
                        Template = o.Template,
                        NumberOfJobs = o.NumberOfJobs,
                        MinimumExperience = o.MinimumExperience,
                        MaximumExperience = o.MaximumExperience,
                        JobDescription = o.JobDescription,
                        JobLocationFullAddress = o.JobLocationFullAddress,
                        ZipCode = o.ZipCode,
                        Latitude = o.Latitude,
                        Longitude = o.Longitude,
                        StartDate = o.StartDate,
                        HireByDate = o.HireByDate,
                        PublishDate = o.PublishDate,
                        ExpirationDate = o.ExpirationDate,
                        InternalJobDescription = o.InternalJobDescription,
                        CityLocation = o.CityLocation,
                        Published = o.Published,
                        Url = o.Url,
                        Id = o.Id,
                    },
                    MasterTagCategoryName = o.MasterTagCategoryName,
                    MasterTagName = o.MasterTagName,
                    ProductCategoryName = o.ProductCategoryName,
                    CurrencyName = o.CurrencyName,
                    BusinessName = o.BusinessName,
                    CountryName = o.CountryName,
                    StateName = o.StateName,
                    CityName = o.CityName,
                    JobStatusTypeName = o.JobStatusTypeName,
                    StoreName = o.StoreName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetJobForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetJobForViewDto> GetJobForView(long id)
        {
            var job = await _jobRepository.GetAsync(id);

            var output = new GetJobForViewDto { Job = ObjectMapper.Map<JobDto>(job) };

            if (output.Job.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.Job.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.Job.MasterTagId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.Job.MasterTagId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            if (output.Job.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.Job.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            if (output.Job.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((long)output.Job.CurrencyId);
                output.CurrencyName = _lookupCurrency?.Name?.ToString();
            }

            if (output.Job.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.Job.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            if (output.Job.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.Job.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.Job.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.Job.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            if (output.Job.CityId != null)
            {
                var _lookupCity = await _lookup_cityRepository.FirstOrDefaultAsync((long)output.Job.CityId);
                output.CityName = _lookupCity?.Name?.ToString();
            }

            if (output.Job.JobStatusTypeId != null)
            {
                var _lookupJobStatusType = await _lookup_jobStatusTypeRepository.FirstOrDefaultAsync((long)output.Job.JobStatusTypeId);
                output.JobStatusTypeName = _lookupJobStatusType?.Name?.ToString();
            }

            if (output.Job.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.Job.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Jobs_Edit)]
        public async Task<GetJobForEditOutput> GetJobForEdit(EntityDto<long> input)
        {
            var job = await _jobRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetJobForEditOutput { Job = ObjectMapper.Map<CreateOrEditJobDto>(job) };

            if (output.Job.MasterTagCategoryId != null)
            {
                var _lookupMasterTagCategory = await _lookup_masterTagCategoryRepository.FirstOrDefaultAsync((long)output.Job.MasterTagCategoryId);
                output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
            }

            if (output.Job.MasterTagId != null)
            {
                var _lookupMasterTag = await _lookup_masterTagRepository.FirstOrDefaultAsync((long)output.Job.MasterTagId);
                output.MasterTagName = _lookupMasterTag?.Name?.ToString();
            }

            if (output.Job.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.Job.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            if (output.Job.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((long)output.Job.CurrencyId);
                output.CurrencyName = _lookupCurrency?.Name?.ToString();
            }

            if (output.Job.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.Job.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            if (output.Job.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.Job.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.Job.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.Job.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            if (output.Job.CityId != null)
            {
                var _lookupCity = await _lookup_cityRepository.FirstOrDefaultAsync((long)output.Job.CityId);
                output.CityName = _lookupCity?.Name?.ToString();
            }

            if (output.Job.JobStatusTypeId != null)
            {
                var _lookupJobStatusType = await _lookup_jobStatusTypeRepository.FirstOrDefaultAsync((long)output.Job.JobStatusTypeId);
                output.JobStatusTypeName = _lookupJobStatusType?.Name?.ToString();
            }

            if (output.Job.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.Job.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditJobDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Jobs_Create)]
        protected virtual async Task Create(CreateOrEditJobDto input)
        {
            var job = ObjectMapper.Map<Job>(input);

            if (AbpSession.TenantId != null)
            {
                job.TenantId = (int?)AbpSession.TenantId;
            }

            await _jobRepository.InsertAsync(job);

        }

        [AbpAuthorize(AppPermissions.Pages_Jobs_Edit)]
        protected virtual async Task Update(CreateOrEditJobDto input)
        {
            var job = await _jobRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, job);

        }

        [AbpAuthorize(AppPermissions.Pages_Jobs_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _jobRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetJobsToExcel(GetAllJobsForExcelInput input)
        {

            var filteredJobs = _jobRepository.GetAll()
                        .Include(e => e.MasterTagCategoryFk)
                        .Include(e => e.MasterTagFk)
                        .Include(e => e.ProductCategoryFk)
                        .Include(e => e.CurrencyFk)
                        .Include(e => e.BusinessFk)
                        .Include(e => e.CountryFk)
                        .Include(e => e.StateFk)
                        .Include(e => e.CityFk)
                        .Include(e => e.JobStatusTypeFk)
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) || e.SalaryOrStaffingRate.Contains(input.Filter) || e.ReferralPoints.Contains(input.Filter) || e.MinimumExperience.Contains(input.Filter) || e.MaximumExperience.Contains(input.Filter) || e.JobDescription.Contains(input.Filter) || e.JobLocationFullAddress.Contains(input.Filter) || e.ZipCode.Contains(input.Filter) || e.InternalJobDescription.Contains(input.Filter) || e.CityLocation.Contains(input.Filter) || e.Url.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title.Contains(input.TitleFilter))
                        .WhereIf(input.FullTimeJobOrGigWorkProjectFilter.HasValue && input.FullTimeJobOrGigWorkProjectFilter > -1, e => (input.FullTimeJobOrGigWorkProjectFilter == 1 && e.FullTimeJobOrGigWorkProject) || (input.FullTimeJobOrGigWorkProjectFilter == 0 && !e.FullTimeJobOrGigWorkProject))
                        .WhereIf(input.RemoteWorkOrOnSiteWorkFilter.HasValue && input.RemoteWorkOrOnSiteWorkFilter > -1, e => (input.RemoteWorkOrOnSiteWorkFilter == 1 && e.RemoteWorkOrOnSiteWork) || (input.RemoteWorkOrOnSiteWorkFilter == 0 && !e.RemoteWorkOrOnSiteWork))
                        .WhereIf(input.SalaryBasedOrFixedPriceFilter.HasValue && input.SalaryBasedOrFixedPriceFilter > -1, e => (input.SalaryBasedOrFixedPriceFilter == 1 && e.SalaryBasedOrFixedPrice) || (input.SalaryBasedOrFixedPriceFilter == 0 && !e.SalaryBasedOrFixedPrice))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SalaryOrStaffingRateFilter), e => e.SalaryOrStaffingRate.Contains(input.SalaryOrStaffingRateFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReferralPointsFilter), e => e.ReferralPoints.Contains(input.ReferralPointsFilter))
                        .WhereIf(input.TemplateFilter.HasValue && input.TemplateFilter > -1, e => (input.TemplateFilter == 1 && e.Template) || (input.TemplateFilter == 0 && !e.Template))
                        .WhereIf(input.MinNumberOfJobsFilter != null, e => e.NumberOfJobs >= input.MinNumberOfJobsFilter)
                        .WhereIf(input.MaxNumberOfJobsFilter != null, e => e.NumberOfJobs <= input.MaxNumberOfJobsFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MinimumExperienceFilter), e => e.MinimumExperience.Contains(input.MinimumExperienceFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MaximumExperienceFilter), e => e.MaximumExperience.Contains(input.MaximumExperienceFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobDescriptionFilter), e => e.JobDescription.Contains(input.JobDescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobLocationFullAddressFilter), e => e.JobLocationFullAddress.Contains(input.JobLocationFullAddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter))
                        .WhereIf(input.MinLatitudeFilter != null, e => e.Latitude >= input.MinLatitudeFilter)
                        .WhereIf(input.MaxLatitudeFilter != null, e => e.Latitude <= input.MaxLatitudeFilter)
                        .WhereIf(input.MinLongitudeFilter != null, e => e.Longitude >= input.MinLongitudeFilter)
                        .WhereIf(input.MaxLongitudeFilter != null, e => e.Longitude <= input.MaxLongitudeFilter)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinHireByDateFilter != null, e => e.HireByDate >= input.MinHireByDateFilter)
                        .WhereIf(input.MaxHireByDateFilter != null, e => e.HireByDate <= input.MaxHireByDateFilter)
                        .WhereIf(input.MinPublishDateFilter != null, e => e.PublishDate >= input.MinPublishDateFilter)
                        .WhereIf(input.MaxPublishDateFilter != null, e => e.PublishDate <= input.MaxPublishDateFilter)
                        .WhereIf(input.MinExpirationDateFilter != null, e => e.ExpirationDate >= input.MinExpirationDateFilter)
                        .WhereIf(input.MaxExpirationDateFilter != null, e => e.ExpirationDate <= input.MaxExpirationDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InternalJobDescriptionFilter), e => e.InternalJobDescription.Contains(input.InternalJobDescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityLocationFilter), e => e.CityLocation.Contains(input.CityLocationFilter))
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UrlFilter), e => e.Url.Contains(input.UrlFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagCategoryNameFilter), e => e.MasterTagCategoryFk != null && e.MasterTagCategoryFk.Name == input.MasterTagCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MasterTagNameFilter), e => e.MasterTagFk != null && e.MasterTagFk.Name == input.MasterTagNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyNameFilter), e => e.CurrencyFk != null && e.CurrencyFk.Name == input.CurrencyNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityNameFilter), e => e.CityFk != null && e.CityFk.Name == input.CityNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobStatusTypeNameFilter), e => e.JobStatusTypeFk != null && e.JobStatusTypeFk.Name == input.JobStatusTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var query = (from o in filteredJobs
                         join o1 in _lookup_masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_masterTagRepository.GetAll() on o.MasterTagId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_currencyRepository.GetAll() on o.CurrencyId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         join o5 in _lookup_businessRepository.GetAll() on o.BusinessId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()

                         join o6 in _lookup_countryRepository.GetAll() on o.CountryId equals o6.Id into j6
                         from s6 in j6.DefaultIfEmpty()

                         join o7 in _lookup_stateRepository.GetAll() on o.StateId equals o7.Id into j7
                         from s7 in j7.DefaultIfEmpty()

                         join o8 in _lookup_cityRepository.GetAll() on o.CityId equals o8.Id into j8
                         from s8 in j8.DefaultIfEmpty()

                         join o9 in _lookup_jobStatusTypeRepository.GetAll() on o.JobStatusTypeId equals o9.Id into j9
                         from s9 in j9.DefaultIfEmpty()

                         join o10 in _lookup_storeRepository.GetAll() on o.StoreId equals o10.Id into j10
                         from s10 in j10.DefaultIfEmpty()

                         select new GetJobForViewDto()
                         {
                             Job = new JobDto
                             {
                                 Title = o.Title,
                                 FullTimeJobOrGigWorkProject = o.FullTimeJobOrGigWorkProject,
                                 RemoteWorkOrOnSiteWork = o.RemoteWorkOrOnSiteWork,
                                 SalaryBasedOrFixedPrice = o.SalaryBasedOrFixedPrice,
                                 SalaryOrStaffingRate = o.SalaryOrStaffingRate,
                                 ReferralPoints = o.ReferralPoints,
                                 Template = o.Template,
                                 NumberOfJobs = o.NumberOfJobs,
                                 MinimumExperience = o.MinimumExperience,
                                 MaximumExperience = o.MaximumExperience,
                                 JobDescription = o.JobDescription,
                                 JobLocationFullAddress = o.JobLocationFullAddress,
                                 ZipCode = o.ZipCode,
                                 Latitude = o.Latitude,
                                 Longitude = o.Longitude,
                                 StartDate = o.StartDate,
                                 HireByDate = o.HireByDate,
                                 PublishDate = o.PublishDate,
                                 ExpirationDate = o.ExpirationDate,
                                 InternalJobDescription = o.InternalJobDescription,
                                 CityLocation = o.CityLocation,
                                 Published = o.Published,
                                 Url = o.Url,
                                 Id = o.Id
                             },
                             MasterTagCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MasterTagName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             ProductCategoryName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             CurrencyName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                             BusinessName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
                             CountryName = s6 == null || s6.Name == null ? "" : s6.Name.ToString(),
                             StateName = s7 == null || s7.Name == null ? "" : s7.Name.ToString(),
                             CityName = s8 == null || s8.Name == null ? "" : s8.Name.ToString(),
                             JobStatusTypeName = s9 == null || s9.Name == null ? "" : s9.Name.ToString(),
                             StoreName = s10 == null || s10.Name == null ? "" : s10.Name.ToString()
                         });

            var jobListDtos = await query.ToListAsync();

            return _jobsExcelExporter.ExportToFile(jobListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Jobs)]
        public async Task<PagedResultDto<JobMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<JobMasterTagCategoryLookupTableDto>();
            foreach (var masterTagCategory in masterTagCategoryList)
            {
                lookupTableDtoList.Add(new JobMasterTagCategoryLookupTableDto
                {
                    Id = masterTagCategory.Id,
                    DisplayName = masterTagCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<JobMasterTagCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Jobs)]
        public async Task<PagedResultDto<JobMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_masterTagRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var masterTagList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<JobMasterTagLookupTableDto>();
            foreach (var masterTag in masterTagList)
            {
                lookupTableDtoList.Add(new JobMasterTagLookupTableDto
                {
                    Id = masterTag.Id,
                    DisplayName = masterTag.Name?.ToString()
                });
            }

            return new PagedResultDto<JobMasterTagLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Jobs)]
        public async Task<PagedResultDto<JobProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<JobProductCategoryLookupTableDto>();
            foreach (var productCategory in productCategoryList)
            {
                lookupTableDtoList.Add(new JobProductCategoryLookupTableDto
                {
                    Id = productCategory.Id,
                    DisplayName = productCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<JobProductCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Jobs)]
        public async Task<PagedResultDto<JobCurrencyLookupTableDto>> GetAllCurrencyForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_currencyRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var currencyList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<JobCurrencyLookupTableDto>();
            foreach (var currency in currencyList)
            {
                lookupTableDtoList.Add(new JobCurrencyLookupTableDto
                {
                    Id = currency.Id,
                    DisplayName = currency.Name?.ToString()
                });
            }

            return new PagedResultDto<JobCurrencyLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Jobs)]
        public async Task<PagedResultDto<JobBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_businessRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var businessList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<JobBusinessLookupTableDto>();
            foreach (var business in businessList)
            {
                lookupTableDtoList.Add(new JobBusinessLookupTableDto
                {
                    Id = business.Id,
                    DisplayName = business.Name?.ToString()
                });
            }

            return new PagedResultDto<JobBusinessLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Jobs)]
        public async Task<PagedResultDto<JobCountryLookupTableDto>> GetAllCountryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_countryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var countryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<JobCountryLookupTableDto>();
            foreach (var country in countryList)
            {
                lookupTableDtoList.Add(new JobCountryLookupTableDto
                {
                    Id = country.Id,
                    DisplayName = country.Name?.ToString()
                });
            }

            return new PagedResultDto<JobCountryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Jobs)]
        public async Task<PagedResultDto<JobStateLookupTableDto>> GetAllStateForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_stateRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var stateList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<JobStateLookupTableDto>();
            foreach (var state in stateList)
            {
                lookupTableDtoList.Add(new JobStateLookupTableDto
                {
                    Id = state.Id,
                    DisplayName = state.Name?.ToString()
                });
            }

            return new PagedResultDto<JobStateLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Jobs)]
        public async Task<PagedResultDto<JobCityLookupTableDto>> GetAllCityForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_cityRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var cityList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<JobCityLookupTableDto>();
            foreach (var city in cityList)
            {
                lookupTableDtoList.Add(new JobCityLookupTableDto
                {
                    Id = city.Id,
                    DisplayName = city.Name?.ToString()
                });
            }

            return new PagedResultDto<JobCityLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Jobs)]
        public async Task<PagedResultDto<JobJobStatusTypeLookupTableDto>> GetAllJobStatusTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_jobStatusTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var jobStatusTypeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<JobJobStatusTypeLookupTableDto>();
            foreach (var jobStatusType in jobStatusTypeList)
            {
                lookupTableDtoList.Add(new JobJobStatusTypeLookupTableDto
                {
                    Id = jobStatusType.Id,
                    DisplayName = jobStatusType.Name?.ToString()
                });
            }

            return new PagedResultDto<JobJobStatusTypeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Jobs)]
        public async Task<PagedResultDto<JobStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<JobStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new JobStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<JobStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }



        public async Task<PagedResultDto<JobMasterTagLookupTableDto>> GetAllJobCategory()
        {
            var query = _lookup_masterTagRepository.GetAll().Where(e => e.MasterTagCategoryId == (long)MasterTagCategoryEnum.JobCategory);

            var totalCount = await query.CountAsync();

            var masterTagList = await query
                .ToListAsync();

            var lookupTableDtoList = new List<JobMasterTagLookupTableDto>();
            foreach (var masterTag in masterTagList)
            {
                lookupTableDtoList.Add(new JobMasterTagLookupTableDto
                {
                    Id = masterTag.Id,
                    DisplayName = masterTag.Name?.ToString()
                });
            }

            return new PagedResultDto<JobMasterTagLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }


        public async Task<PagedResultDto<JobMasterTagLookupTableDto>> GetAllJobType()
        {
            var query = _lookup_masterTagRepository.GetAll().Where(e => e.MasterTagCategoryId == (long)MasterTagCategoryEnum.JobType);

            var totalCount = await query.CountAsync();

            var masterTagList = await query
                .ToListAsync();

            var lookupTableDtoList = new List<JobMasterTagLookupTableDto>();
            foreach (var masterTag in masterTagList)
            {
                lookupTableDtoList.Add(new JobMasterTagLookupTableDto
                {
                    Id = masterTag.Id,
                    DisplayName = masterTag.Name?.ToString()
                });
            }

            return new PagedResultDto<JobMasterTagLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}