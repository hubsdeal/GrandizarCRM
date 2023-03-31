using SoftGrid.CRM;
using SoftGrid.Authorization.Users;
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
    [AbpAuthorize(AppPermissions.Pages_CustomerWallets)]
    public class CustomerWalletsAppService : SoftGridAppServiceBase, ICustomerWalletsAppService
    {
        private readonly IRepository<CustomerWallet, long> _customerWalletRepository;
        private readonly ICustomerWalletsExcelExporter _customerWalletsExcelExporter;
        private readonly IRepository<Contact, long> _lookup_contactRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<Currency, long> _lookup_currencyRepository;

        public CustomerWalletsAppService(IRepository<CustomerWallet, long> customerWalletRepository, ICustomerWalletsExcelExporter customerWalletsExcelExporter, IRepository<Contact, long> lookup_contactRepository, IRepository<User, long> lookup_userRepository, IRepository<Currency, long> lookup_currencyRepository)
        {
            _customerWalletRepository = customerWalletRepository;
            _customerWalletsExcelExporter = customerWalletsExcelExporter;
            _lookup_contactRepository = lookup_contactRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_currencyRepository = lookup_currencyRepository;

        }

        public async Task<PagedResultDto<GetCustomerWalletForViewDto>> GetAll(GetAllCustomerWalletsInput input)
        {

            var filteredCustomerWallets = _customerWalletRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .Include(e => e.UserFk)
                        .Include(e => e.CurrencyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinWalletOpeningDateFilter != null, e => e.WalletOpeningDate >= input.MinWalletOpeningDateFilter)
                        .WhereIf(input.MaxWalletOpeningDateFilter != null, e => e.WalletOpeningDate <= input.MaxWalletOpeningDateFilter)
                        .WhereIf(input.MinBalanceDateFilter != null, e => e.BalanceDate >= input.MinBalanceDateFilter)
                        .WhereIf(input.MaxBalanceDateFilter != null, e => e.BalanceDate <= input.MaxBalanceDateFilter)
                        .WhereIf(input.MinBalanceAmountFilter != null, e => e.BalanceAmount >= input.MinBalanceAmountFilter)
                        .WhereIf(input.MaxBalanceAmountFilter != null, e => e.BalanceAmount <= input.MaxBalanceAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyNameFilter), e => e.CurrencyFk != null && e.CurrencyFk.Name == input.CurrencyNameFilter);

            var pagedAndFilteredCustomerWallets = filteredCustomerWallets
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var customerWallets = from o in pagedAndFilteredCustomerWallets
                                  join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                                  from s1 in j1.DefaultIfEmpty()

                                  join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                                  from s2 in j2.DefaultIfEmpty()

                                  join o3 in _lookup_currencyRepository.GetAll() on o.CurrencyId equals o3.Id into j3
                                  from s3 in j3.DefaultIfEmpty()

                                  select new
                                  {

                                      o.WalletOpeningDate,
                                      o.BalanceDate,
                                      o.BalanceAmount,
                                      Id = o.Id,
                                      ContactFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                                      UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                      CurrencyName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                                  };

            var totalCount = await filteredCustomerWallets.CountAsync();

            var dbList = await customerWallets.ToListAsync();
            var results = new List<GetCustomerWalletForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCustomerWalletForViewDto()
                {
                    CustomerWallet = new CustomerWalletDto
                    {

                        WalletOpeningDate = o.WalletOpeningDate,
                        BalanceDate = o.BalanceDate,
                        BalanceAmount = o.BalanceAmount,
                        Id = o.Id,
                    },
                    ContactFullName = o.ContactFullName,
                    UserName = o.UserName,
                    CurrencyName = o.CurrencyName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCustomerWalletForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCustomerWalletForViewDto> GetCustomerWalletForView(long id)
        {
            var customerWallet = await _customerWalletRepository.GetAsync(id);

            var output = new GetCustomerWalletForViewDto { CustomerWallet = ObjectMapper.Map<CustomerWalletDto>(customerWallet) };

            if (output.CustomerWallet.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.CustomerWallet.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.CustomerWallet.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.CustomerWallet.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.CustomerWallet.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((long)output.CustomerWallet.CurrencyId);
                output.CurrencyName = _lookupCurrency?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CustomerWallets_Edit)]
        public async Task<GetCustomerWalletForEditOutput> GetCustomerWalletForEdit(EntityDto<long> input)
        {
            var customerWallet = await _customerWalletRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCustomerWalletForEditOutput { CustomerWallet = ObjectMapper.Map<CreateOrEditCustomerWalletDto>(customerWallet) };

            if (output.CustomerWallet.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.CustomerWallet.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            if (output.CustomerWallet.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.CustomerWallet.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.CustomerWallet.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((long)output.CustomerWallet.CurrencyId);
                output.CurrencyName = _lookupCurrency?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCustomerWalletDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CustomerWallets_Create)]
        protected virtual async Task Create(CreateOrEditCustomerWalletDto input)
        {
            var customerWallet = ObjectMapper.Map<CustomerWallet>(input);

            if (AbpSession.TenantId != null)
            {
                customerWallet.TenantId = (int?)AbpSession.TenantId;
            }

            await _customerWalletRepository.InsertAsync(customerWallet);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerWallets_Edit)]
        protected virtual async Task Update(CreateOrEditCustomerWalletDto input)
        {
            var customerWallet = await _customerWalletRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, customerWallet);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerWallets_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _customerWalletRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCustomerWalletsToExcel(GetAllCustomerWalletsForExcelInput input)
        {

            var filteredCustomerWallets = _customerWalletRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .Include(e => e.UserFk)
                        .Include(e => e.CurrencyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinWalletOpeningDateFilter != null, e => e.WalletOpeningDate >= input.MinWalletOpeningDateFilter)
                        .WhereIf(input.MaxWalletOpeningDateFilter != null, e => e.WalletOpeningDate <= input.MaxWalletOpeningDateFilter)
                        .WhereIf(input.MinBalanceDateFilter != null, e => e.BalanceDate >= input.MinBalanceDateFilter)
                        .WhereIf(input.MaxBalanceDateFilter != null, e => e.BalanceDate <= input.MaxBalanceDateFilter)
                        .WhereIf(input.MinBalanceAmountFilter != null, e => e.BalanceAmount >= input.MinBalanceAmountFilter)
                        .WhereIf(input.MaxBalanceAmountFilter != null, e => e.BalanceAmount <= input.MaxBalanceAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyNameFilter), e => e.CurrencyFk != null && e.CurrencyFk.Name == input.CurrencyNameFilter);

            var query = (from o in filteredCustomerWallets
                         join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_currencyRepository.GetAll() on o.CurrencyId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetCustomerWalletForViewDto()
                         {
                             CustomerWallet = new CustomerWalletDto
                             {
                                 WalletOpeningDate = o.WalletOpeningDate,
                                 BalanceDate = o.BalanceDate,
                                 BalanceAmount = o.BalanceAmount,
                                 Id = o.Id
                             },
                             ContactFullName = s1 == null || s1.FullName == null ? "" : s1.FullName.ToString(),
                             UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             CurrencyName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var customerWalletListDtos = await query.ToListAsync();

            return _customerWalletsExcelExporter.ExportToFile(customerWalletListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_CustomerWallets)]
        public async Task<PagedResultDto<CustomerWalletContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CustomerWalletContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new CustomerWalletContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<CustomerWalletContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_CustomerWallets)]
        public async Task<PagedResultDto<CustomerWalletUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CustomerWalletUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new CustomerWalletUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<CustomerWalletUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_CustomerWallets)]
        public async Task<PagedResultDto<CustomerWalletCurrencyLookupTableDto>> GetAllCurrencyForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_currencyRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var currencyList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CustomerWalletCurrencyLookupTableDto>();
            foreach (var currency in currencyList)
            {
                lookupTableDtoList.Add(new CustomerWalletCurrencyLookupTableDto
                {
                    Id = currency.Id,
                    DisplayName = currency.Name?.ToString()
                });
            }

            return new PagedResultDto<CustomerWalletCurrencyLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}