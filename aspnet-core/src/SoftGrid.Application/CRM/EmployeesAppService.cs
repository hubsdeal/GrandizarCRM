using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.CRM;

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
    [AbpAuthorize(AppPermissions.Pages_Employees)]
    public class EmployeesAppService : SoftGridAppServiceBase, IEmployeesAppService
    {
        private readonly IRepository<Employee, long> _employeeRepository;
        private readonly IEmployeesExcelExporter _employeesExcelExporter;
        private readonly IRepository<State, long> _lookup_stateRepository;
        private readonly IRepository<Country, long> _lookup_countryRepository;
        private readonly IRepository<Contact, long> _lookup_contactRepository;

        public EmployeesAppService(IRepository<Employee, long> employeeRepository, IEmployeesExcelExporter employeesExcelExporter, IRepository<State, long> lookup_stateRepository, IRepository<Country, long> lookup_countryRepository, IRepository<Contact, long> lookup_contactRepository)
        {
            _employeeRepository = employeeRepository;
            _employeesExcelExporter = employeesExcelExporter;
            _lookup_stateRepository = lookup_stateRepository;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_contactRepository = lookup_contactRepository;

        }

        public async Task<PagedResultDto<GetEmployeeForViewDto>> GetAll(GetAllEmployeesInput input)
        {

            var filteredEmployees = _employeeRepository.GetAll()
                        .Include(e => e.StateFk)
                        .Include(e => e.CountryFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) || e.FullAddress.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.ZipCode.Contains(input.Filter) || e.City.Contains(input.Filter) || e.Mobile.Contains(input.Filter) || e.OfficePhone.Contains(input.Filter) || e.PersonalEmail.Contains(input.Filter) || e.BusinessEmail.Contains(input.Filter) || e.JobTitle.Contains(input.Filter) || e.CompanyName.Contains(input.Filter) || e.Profile.Contains(input.Filter) || e.Facebook.Contains(input.Filter) || e.LinkedIn.Contains(input.Filter) || e.Fax.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter), e => e.FirstName.Contains(input.FirstNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter), e => e.LastName.Contains(input.LastNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FullAddressFilter), e => e.FullAddress.Contains(input.FullAddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.Contains(input.CityFilter))
                        .WhereIf(input.MinDateOfBirthFilter != null, e => e.DateOfBirth >= input.MinDateOfBirthFilter)
                        .WhereIf(input.MaxDateOfBirthFilter != null, e => e.DateOfBirth <= input.MaxDateOfBirthFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MobileFilter), e => e.Mobile.Contains(input.MobileFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OfficePhoneFilter), e => e.OfficePhone.Contains(input.OfficePhoneFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PersonalEmailFilter), e => e.PersonalEmail.Contains(input.PersonalEmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessEmailFilter), e => e.BusinessEmail.Contains(input.BusinessEmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobTitleFilter), e => e.JobTitle.Contains(input.JobTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CompanyNameFilter), e => e.CompanyName.Contains(input.CompanyNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProfileFilter), e => e.Profile.Contains(input.ProfileFilter))
                        .WhereIf(input.MinHireDateFilter != null, e => e.HireDate >= input.MinHireDateFilter)
                        .WhereIf(input.MaxHireDateFilter != null, e => e.HireDate <= input.MaxHireDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FacebookFilter), e => e.Facebook.Contains(input.FacebookFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LinkedInFilter), e => e.LinkedIn.Contains(input.LinkedInFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FaxFilter), e => e.Fax.Contains(input.FaxFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProfilePictureIdFilter.ToString()), e => e.ProfilePictureId.ToString() == input.ProfilePictureIdFilter.ToString())
                        .WhereIf(input.CurrentEmployeeFilter.HasValue && input.CurrentEmployeeFilter > -1, e => (input.CurrentEmployeeFilter == 1 && e.CurrentEmployee) || (input.CurrentEmployeeFilter == 0 && !e.CurrentEmployee))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter);

            var pagedAndFilteredEmployees = filteredEmployees
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var employees = from o in pagedAndFilteredEmployees
                            join o1 in _lookup_stateRepository.GetAll() on o.StateId equals o1.Id into j1
                            from s1 in j1.DefaultIfEmpty()

                            join o2 in _lookup_countryRepository.GetAll() on o.CountryId equals o2.Id into j2
                            from s2 in j2.DefaultIfEmpty()

                            join o3 in _lookup_contactRepository.GetAll() on o.ContactId equals o3.Id into j3
                            from s3 in j3.DefaultIfEmpty()

                            select new
                            {

                                o.Name,
                                o.FirstName,
                                o.LastName,
                                o.FullAddress,
                                o.Address,
                                o.ZipCode,
                                o.City,
                                o.DateOfBirth,
                                o.Mobile,
                                o.OfficePhone,
                                o.PersonalEmail,
                                o.BusinessEmail,
                                o.JobTitle,
                                o.CompanyName,
                                o.Profile,
                                o.HireDate,
                                o.Facebook,
                                o.LinkedIn,
                                o.Fax,
                                o.ProfilePictureId,
                                o.CurrentEmployee,
                                Id = o.Id,
                                StateName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                CountryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                ContactFullName = s3 == null || s3.FullName == null ? "" : s3.FullName.ToString()
                            };

            var totalCount = await filteredEmployees.CountAsync();

            var dbList = await employees.ToListAsync();
            var results = new List<GetEmployeeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetEmployeeForViewDto()
                {
                    Employee = new EmployeeDto
                    {

                        Name = o.Name,
                        FirstName = o.FirstName,
                        LastName = o.LastName,
                        FullAddress = o.FullAddress,
                        Address = o.Address,
                        ZipCode = o.ZipCode,
                        City = o.City,
                        DateOfBirth = o.DateOfBirth,
                        Mobile = o.Mobile,
                        OfficePhone = o.OfficePhone,
                        PersonalEmail = o.PersonalEmail,
                        BusinessEmail = o.BusinessEmail,
                        JobTitle = o.JobTitle,
                        CompanyName = o.CompanyName,
                        Profile = o.Profile,
                        HireDate = o.HireDate,
                        Facebook = o.Facebook,
                        LinkedIn = o.LinkedIn,
                        Fax = o.Fax,
                        ProfilePictureId = o.ProfilePictureId,
                        CurrentEmployee = o.CurrentEmployee,
                        Id = o.Id,
                    },
                    StateName = o.StateName,
                    CountryName = o.CountryName,
                    ContactFullName = o.ContactFullName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetEmployeeForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetEmployeeForViewDto> GetEmployeeForView(long id)
        {
            var employee = await _employeeRepository.GetAsync(id);

            var output = new GetEmployeeForViewDto { Employee = ObjectMapper.Map<EmployeeDto>(employee) };

            if (output.Employee.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.Employee.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            if (output.Employee.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.Employee.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.Employee.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.Employee.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Employees_Edit)]
        public async Task<GetEmployeeForEditOutput> GetEmployeeForEdit(EntityDto<long> input)
        {
            var employee = await _employeeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetEmployeeForEditOutput { Employee = ObjectMapper.Map<CreateOrEditEmployeeDto>(employee) };

            if (output.Employee.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.Employee.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            if (output.Employee.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.Employee.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.Employee.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.Employee.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditEmployeeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Employees_Create)]
        protected virtual async Task Create(CreateOrEditEmployeeDto input)
        {
            var employee = ObjectMapper.Map<Employee>(input);

            if (AbpSession.TenantId != null)
            {
                employee.TenantId = (int?)AbpSession.TenantId;
            }

            await _employeeRepository.InsertAsync(employee);

        }

        [AbpAuthorize(AppPermissions.Pages_Employees_Edit)]
        protected virtual async Task Update(CreateOrEditEmployeeDto input)
        {
            var employee = await _employeeRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, employee);

        }

        [AbpAuthorize(AppPermissions.Pages_Employees_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _employeeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetEmployeesToExcel(GetAllEmployeesForExcelInput input)
        {

            var filteredEmployees = _employeeRepository.GetAll()
                        .Include(e => e.StateFk)
                        .Include(e => e.CountryFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) || e.FullAddress.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.ZipCode.Contains(input.Filter) || e.City.Contains(input.Filter) || e.Mobile.Contains(input.Filter) || e.OfficePhone.Contains(input.Filter) || e.PersonalEmail.Contains(input.Filter) || e.BusinessEmail.Contains(input.Filter) || e.JobTitle.Contains(input.Filter) || e.CompanyName.Contains(input.Filter) || e.Profile.Contains(input.Filter) || e.Facebook.Contains(input.Filter) || e.LinkedIn.Contains(input.Filter) || e.Fax.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter), e => e.FirstName.Contains(input.FirstNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter), e => e.LastName.Contains(input.LastNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FullAddressFilter), e => e.FullAddress.Contains(input.FullAddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.Contains(input.CityFilter))
                        .WhereIf(input.MinDateOfBirthFilter != null, e => e.DateOfBirth >= input.MinDateOfBirthFilter)
                        .WhereIf(input.MaxDateOfBirthFilter != null, e => e.DateOfBirth <= input.MaxDateOfBirthFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MobileFilter), e => e.Mobile.Contains(input.MobileFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OfficePhoneFilter), e => e.OfficePhone.Contains(input.OfficePhoneFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PersonalEmailFilter), e => e.PersonalEmail.Contains(input.PersonalEmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessEmailFilter), e => e.BusinessEmail.Contains(input.BusinessEmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobTitleFilter), e => e.JobTitle.Contains(input.JobTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CompanyNameFilter), e => e.CompanyName.Contains(input.CompanyNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProfileFilter), e => e.Profile.Contains(input.ProfileFilter))
                        .WhereIf(input.MinHireDateFilter != null, e => e.HireDate >= input.MinHireDateFilter)
                        .WhereIf(input.MaxHireDateFilter != null, e => e.HireDate <= input.MaxHireDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FacebookFilter), e => e.Facebook.Contains(input.FacebookFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LinkedInFilter), e => e.LinkedIn.Contains(input.LinkedInFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FaxFilter), e => e.Fax.Contains(input.FaxFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProfilePictureIdFilter.ToString()), e => e.ProfilePictureId.ToString() == input.ProfilePictureIdFilter.ToString())
                        .WhereIf(input.CurrentEmployeeFilter.HasValue && input.CurrentEmployeeFilter > -1, e => (input.CurrentEmployeeFilter == 1 && e.CurrentEmployee) || (input.CurrentEmployeeFilter == 0 && !e.CurrentEmployee))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter);

            var query = (from o in filteredEmployees
                         join o1 in _lookup_stateRepository.GetAll() on o.StateId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_countryRepository.GetAll() on o.CountryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_contactRepository.GetAll() on o.ContactId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetEmployeeForViewDto()
                         {
                             Employee = new EmployeeDto
                             {
                                 Name = o.Name,
                                 FirstName = o.FirstName,
                                 LastName = o.LastName,
                                 FullAddress = o.FullAddress,
                                 Address = o.Address,
                                 ZipCode = o.ZipCode,
                                 City = o.City,
                                 DateOfBirth = o.DateOfBirth,
                                 Mobile = o.Mobile,
                                 OfficePhone = o.OfficePhone,
                                 PersonalEmail = o.PersonalEmail,
                                 BusinessEmail = o.BusinessEmail,
                                 JobTitle = o.JobTitle,
                                 CompanyName = o.CompanyName,
                                 Profile = o.Profile,
                                 HireDate = o.HireDate,
                                 Facebook = o.Facebook,
                                 LinkedIn = o.LinkedIn,
                                 Fax = o.Fax,
                                 ProfilePictureId = o.ProfilePictureId,
                                 CurrentEmployee = o.CurrentEmployee,
                                 Id = o.Id
                             },
                             StateName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             CountryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             ContactFullName = s3 == null || s3.FullName == null ? "" : s3.FullName.ToString()
                         });

            var employeeListDtos = await query.ToListAsync();

            return _employeesExcelExporter.ExportToFile(employeeListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Employees)]
        public async Task<PagedResultDto<EmployeeStateLookupTableDto>> GetAllStateForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_stateRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var stateList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<EmployeeStateLookupTableDto>();
            foreach (var state in stateList)
            {
                lookupTableDtoList.Add(new EmployeeStateLookupTableDto
                {
                    Id = state.Id,
                    DisplayName = state.Name?.ToString()
                });
            }

            return new PagedResultDto<EmployeeStateLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Employees)]
        public async Task<PagedResultDto<EmployeeCountryLookupTableDto>> GetAllCountryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_countryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var countryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<EmployeeCountryLookupTableDto>();
            foreach (var country in countryList)
            {
                lookupTableDtoList.Add(new EmployeeCountryLookupTableDto
                {
                    Id = country.Id,
                    DisplayName = country.Name?.ToString()
                });
            }

            return new PagedResultDto<EmployeeCountryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Employees)]
        public async Task<PagedResultDto<EmployeeContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<EmployeeContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new EmployeeContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<EmployeeContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}