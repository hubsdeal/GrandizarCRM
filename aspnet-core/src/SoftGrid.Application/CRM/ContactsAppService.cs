using SoftGrid.Authorization.Users;
using SoftGrid.LookupData;

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
    [AbpAuthorize(AppPermissions.Pages_Contacts)]
    public class ContactsAppService : SoftGridAppServiceBase, IContactsAppService
    {
        private readonly IRepository<Contact, long> _contactRepository;
        private readonly IContactsExcelExporter _contactsExcelExporter;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<Country, long> _lookup_countryRepository;
        private readonly IRepository<State, long> _lookup_stateRepository;
        private readonly IRepository<MembershipType, long> _lookup_membershipTypeRepository;

        public ContactsAppService(IRepository<Contact, long> contactRepository, IContactsExcelExporter contactsExcelExporter, IRepository<User, long> lookup_userRepository, IRepository<Country, long> lookup_countryRepository, IRepository<State, long> lookup_stateRepository, IRepository<MembershipType, long> lookup_membershipTypeRepository)
        {
            _contactRepository = contactRepository;
            _contactsExcelExporter = contactsExcelExporter;
            _lookup_userRepository = lookup_userRepository;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_stateRepository = lookup_stateRepository;
            _lookup_membershipTypeRepository = lookup_membershipTypeRepository;

        }

        public async Task<PagedResultDto<GetContactForViewDto>> GetAll(GetAllContactsInput input)
        {

            var filteredContacts = _contactRepository.GetAll()
                        .Include(e => e.ReferredByUserFk)
                        .Include(e => e.CountryFk)
                        .Include(e => e.StateFk)
                        .Include(e => e.MembershipTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FullName.Contains(input.Filter) || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) || e.FullAddress.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.ZipCode.Contains(input.Filter) || e.City.Contains(input.Filter) || e.Mobile.Contains(input.Filter) || e.OfficePhone.Contains(input.Filter) || e.CountryCode.Contains(input.Filter) || e.PersonalEmail.Contains(input.Filter) || e.BusinessEmail.Contains(input.Filter) || e.JobTitle.Contains(input.Filter) || e.CompanyName.Contains(input.Filter) || e.Profile.Contains(input.Filter) || e.AiDataTag.Contains(input.Filter) || e.Facebook.Contains(input.Filter) || e.LinkedIn.Contains(input.Filter) || e.Fax.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FullNameFilter), e => e.FullName.Contains(input.FullNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter), e => e.FirstName.Contains(input.FirstNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter), e => e.LastName.Contains(input.LastNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FullAddressFilter), e => e.FullAddress.Contains(input.FullAddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.Contains(input.CityFilter))
                        .WhereIf(input.MinDateOfBirthFilter != null, e => e.DateOfBirth >= input.MinDateOfBirthFilter)
                        .WhereIf(input.MaxDateOfBirthFilter != null, e => e.DateOfBirth <= input.MaxDateOfBirthFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryCodeFilter), e => e.CountryCode.Contains(input.CountryCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PersonalEmailFilter), e => e.PersonalEmail.Contains(input.PersonalEmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessEmailFilter), e => e.BusinessEmail.Contains(input.BusinessEmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobTitleFilter), e => e.JobTitle.Contains(input.JobTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CompanyNameFilter), e => e.CompanyName.Contains(input.CompanyNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProfileFilter), e => e.Profile.Contains(input.ProfileFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AiDataTagFilter), e => e.AiDataTag.Contains(input.AiDataTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FacebookFilter), e => e.Facebook.Contains(input.FacebookFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LinkedInFilter), e => e.LinkedIn.Contains(input.LinkedInFilter))
                        .WhereIf(input.ReferredFilter.HasValue && input.ReferredFilter > -1, e => (input.ReferredFilter == 1 && e.Referred) || (input.ReferredFilter == 0 && !e.Referred))
                        .WhereIf(input.VerifiedFilter.HasValue && input.VerifiedFilter > -1, e => (input.VerifiedFilter == 1 && e.Verified) || (input.VerifiedFilter == 0 && !e.Verified))
                        .WhereIf(input.MinScoreFilter != null, e => e.Score >= input.MinScoreFilter)
                        .WhereIf(input.MaxScoreFilter != null, e => e.Score <= input.MaxScoreFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.ReferredByUserFk != null && e.ReferredByUserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MembershipTypeNameFilter), e => e.MembershipTypeFk != null && e.MembershipTypeFk.Name == input.MembershipTypeNameFilter);

            var pagedAndFilteredContacts = filteredContacts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var contacts = from o in pagedAndFilteredContacts
                           join o1 in _lookup_userRepository.GetAll() on o.ReferredByUserId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()

                           join o2 in _lookup_countryRepository.GetAll() on o.CountryId equals o2.Id into j2
                           from s2 in j2.DefaultIfEmpty()

                           join o3 in _lookup_stateRepository.GetAll() on o.StateId equals o3.Id into j3
                           from s3 in j3.DefaultIfEmpty()

                           join o4 in _lookup_membershipTypeRepository.GetAll() on o.MembershipTypeId equals o4.Id into j4
                           from s4 in j4.DefaultIfEmpty()

                           select new
                           {

                               o.FullName,
                               o.FirstName,
                               o.LastName,
                               o.FullAddress,
                               o.Address,
                               o.ZipCode,
                               o.City,
                               o.DateOfBirth,
                               o.Mobile,
                               o.OfficePhone,
                               o.CountryCode,
                               o.PersonalEmail,
                               o.BusinessEmail,
                               o.JobTitle,
                               o.CompanyName,
                               o.AiDataTag,
                               o.Facebook,
                               o.LinkedIn,
                               o.Referred,
                               o.Verified,
                               o.Score,
                               Id = o.Id,
                               UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                               CountryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                               StateName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                               MembershipTypeName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                           };

            var totalCount = await filteredContacts.CountAsync();

            var dbList = await contacts.ToListAsync();
            var results = new List<GetContactForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetContactForViewDto()
                {
                    Contact = new ContactDto
                    {

                        FullName = o.FullName,
                        FirstName = o.FirstName,
                        LastName = o.LastName,
                        FullAddress = o.FullAddress,
                        Address = o.Address,
                        ZipCode = o.ZipCode,
                        City = o.City,
                        DateOfBirth = o.DateOfBirth,
                        Mobile = o.Mobile,
                        OfficePhone = o.OfficePhone,
                        CountryCode = o.CountryCode,
                        PersonalEmail = o.PersonalEmail,
                        BusinessEmail = o.BusinessEmail,
                        JobTitle = o.JobTitle,
                        CompanyName = o.CompanyName,
                        AiDataTag = o.AiDataTag,
                        Facebook = o.Facebook,
                        LinkedIn = o.LinkedIn,
                        Referred = o.Referred,
                        Verified = o.Verified,
                        Score = o.Score,
                        Id = o.Id,
                    },
                    UserName = o.UserName,
                    CountryName = o.CountryName,
                    StateName = o.StateName,
                    MembershipTypeName = o.MembershipTypeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetContactForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetContactForViewDto> GetContactForView(long id)
        {
            var contact = await _contactRepository.GetAsync(id);

            var output = new GetContactForViewDto { Contact = ObjectMapper.Map<ContactDto>(contact) };

            if (output.Contact.ReferredByUserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Contact.ReferredByUserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.Contact.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.Contact.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.Contact.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.Contact.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            if (output.Contact.MembershipTypeId != null)
            {
                var _lookupMembershipType = await _lookup_membershipTypeRepository.FirstOrDefaultAsync((long)output.Contact.MembershipTypeId);
                output.MembershipTypeName = _lookupMembershipType?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Contacts_Edit)]
        public async Task<GetContactForEditOutput> GetContactForEdit(EntityDto<long> input)
        {
            var contact = await _contactRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetContactForEditOutput { Contact = ObjectMapper.Map<CreateOrEditContactDto>(contact) };

            if (output.Contact.ReferredByUserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Contact.ReferredByUserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.Contact.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.Contact.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.Contact.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.Contact.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            if (output.Contact.MembershipTypeId != null)
            {
                var _lookupMembershipType = await _lookup_membershipTypeRepository.FirstOrDefaultAsync((long)output.Contact.MembershipTypeId);
                output.MembershipTypeName = _lookupMembershipType?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditContactDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Contacts_Create)]
        protected virtual async Task Create(CreateOrEditContactDto input)
        {
            var contact = ObjectMapper.Map<Contact>(input);

            if (AbpSession.TenantId != null)
            {
                contact.TenantId = (int?)AbpSession.TenantId;
            }

            await _contactRepository.InsertAsync(contact);

        }

        [AbpAuthorize(AppPermissions.Pages_Contacts_Edit)]
        protected virtual async Task Update(CreateOrEditContactDto input)
        {
            var contact = await _contactRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, contact);

        }

        [AbpAuthorize(AppPermissions.Pages_Contacts_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _contactRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetContactsToExcel(GetAllContactsForExcelInput input)
        {

            var filteredContacts = _contactRepository.GetAll()
                        .Include(e => e.ReferredByUserFk)
                        .Include(e => e.CountryFk)
                        .Include(e => e.StateFk)
                        .Include(e => e.MembershipTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FullName.Contains(input.Filter) || e.FirstName.Contains(input.Filter) || e.LastName.Contains(input.Filter) || e.FullAddress.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.ZipCode.Contains(input.Filter) || e.City.Contains(input.Filter) || e.Mobile.Contains(input.Filter) || e.OfficePhone.Contains(input.Filter) || e.CountryCode.Contains(input.Filter) || e.PersonalEmail.Contains(input.Filter) || e.BusinessEmail.Contains(input.Filter) || e.JobTitle.Contains(input.Filter) || e.CompanyName.Contains(input.Filter) || e.Profile.Contains(input.Filter) || e.AiDataTag.Contains(input.Filter) || e.Facebook.Contains(input.Filter) || e.LinkedIn.Contains(input.Filter) || e.Fax.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FullNameFilter), e => e.FullName.Contains(input.FullNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FirstNameFilter), e => e.FirstName.Contains(input.FirstNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LastNameFilter), e => e.LastName.Contains(input.LastNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FullAddressFilter), e => e.FullAddress.Contains(input.FullAddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ZipCodeFilter), e => e.ZipCode.Contains(input.ZipCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.Contains(input.CityFilter))
                        .WhereIf(input.MinDateOfBirthFilter != null, e => e.DateOfBirth >= input.MinDateOfBirthFilter)
                        .WhereIf(input.MaxDateOfBirthFilter != null, e => e.DateOfBirth <= input.MaxDateOfBirthFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryCodeFilter), e => e.CountryCode.Contains(input.CountryCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PersonalEmailFilter), e => e.PersonalEmail.Contains(input.PersonalEmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessEmailFilter), e => e.BusinessEmail.Contains(input.BusinessEmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.JobTitleFilter), e => e.JobTitle.Contains(input.JobTitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CompanyNameFilter), e => e.CompanyName.Contains(input.CompanyNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProfileFilter), e => e.Profile.Contains(input.ProfileFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AiDataTagFilter), e => e.AiDataTag.Contains(input.AiDataTagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FacebookFilter), e => e.Facebook.Contains(input.FacebookFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LinkedInFilter), e => e.LinkedIn.Contains(input.LinkedInFilter))
                        .WhereIf(input.ReferredFilter.HasValue && input.ReferredFilter > -1, e => (input.ReferredFilter == 1 && e.Referred) || (input.ReferredFilter == 0 && !e.Referred))
                        .WhereIf(input.VerifiedFilter.HasValue && input.VerifiedFilter > -1, e => (input.VerifiedFilter == 1 && e.Verified) || (input.VerifiedFilter == 0 && !e.Verified))
                        .WhereIf(input.MinScoreFilter != null, e => e.Score >= input.MinScoreFilter)
                        .WhereIf(input.MaxScoreFilter != null, e => e.Score <= input.MaxScoreFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.ReferredByUserFk != null && e.ReferredByUserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MembershipTypeNameFilter), e => e.MembershipTypeFk != null && e.MembershipTypeFk.Name == input.MembershipTypeNameFilter);

            var query = (from o in filteredContacts
                         join o1 in _lookup_userRepository.GetAll() on o.ReferredByUserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_countryRepository.GetAll() on o.CountryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_stateRepository.GetAll() on o.StateId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_membershipTypeRepository.GetAll() on o.MembershipTypeId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetContactForViewDto()
                         {
                             Contact = new ContactDto
                             {
                                 FullName = o.FullName,
                                 FirstName = o.FirstName,
                                 LastName = o.LastName,
                                 FullAddress = o.FullAddress,
                                 Address = o.Address,
                                 ZipCode = o.ZipCode,
                                 City = o.City,
                                 DateOfBirth = o.DateOfBirth,
                                 Mobile = o.Mobile,
                                 OfficePhone = o.OfficePhone,
                                 CountryCode = o.CountryCode,
                                 PersonalEmail = o.PersonalEmail,
                                 BusinessEmail = o.BusinessEmail,
                                 JobTitle = o.JobTitle,
                                 CompanyName = o.CompanyName,
                                 AiDataTag = o.AiDataTag,
                                 Facebook = o.Facebook,
                                 LinkedIn = o.LinkedIn,
                                 Referred = o.Referred,
                                 Verified = o.Verified,
                                 Score = o.Score,
                                 Id = o.Id
                             },
                             UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             CountryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             StateName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             MembershipTypeName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                         });

            var contactListDtos = await query.ToListAsync();

            return _contactsExcelExporter.ExportToFile(contactListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Contacts)]
        public async Task<PagedResultDto<ContactUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ContactUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new ContactUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<ContactUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        [AbpAuthorize(AppPermissions.Pages_Contacts)]
        public async Task<List<ContactCountryLookupTableDto>> GetAllCountryForTableDropdown()
        {
            return await _lookup_countryRepository.GetAll()
                .Select(country => new ContactCountryLookupTableDto
                {
                    Id = country.Id,
                    DisplayName = country == null || country.Name == null ? "" : country.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Contacts)]
        public async Task<List<ContactStateLookupTableDto>> GetAllStateForTableDropdown()
        {
            return await _lookup_stateRepository.GetAll()
                .Select(state => new ContactStateLookupTableDto
                {
                    Id = state.Id,
                    DisplayName = state == null || state.Name == null ? "" : state.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Contacts)]
        public async Task<List<ContactMembershipTypeLookupTableDto>> GetAllMembershipTypeForTableDropdown()
        {
            return await _lookup_membershipTypeRepository.GetAll()
                .Select(membershipType => new ContactMembershipTypeLookupTableDto
                {
                    Id = membershipType.Id,
                    DisplayName = membershipType == null || membershipType.Name == null ? "" : membershipType.Name.ToString()
                }).ToListAsync();
        }

    }
}