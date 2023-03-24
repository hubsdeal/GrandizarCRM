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
    [AbpAuthorize(AppPermissions.Pages_StoreBankAccounts)]
    public class StoreBankAccountsAppService : SoftGridAppServiceBase, IStoreBankAccountsAppService
    {
        private readonly IRepository<StoreBankAccount, long> _storeBankAccountRepository;
        private readonly IStoreBankAccountsExcelExporter _storeBankAccountsExcelExporter;
        private readonly IRepository<Store, long> _lookup_storeRepository;

        public StoreBankAccountsAppService(IRepository<StoreBankAccount, long> storeBankAccountRepository, IStoreBankAccountsExcelExporter storeBankAccountsExcelExporter, IRepository<Store, long> lookup_storeRepository)
        {
            _storeBankAccountRepository = storeBankAccountRepository;
            _storeBankAccountsExcelExporter = storeBankAccountsExcelExporter;
            _lookup_storeRepository = lookup_storeRepository;

        }

        public async Task<PagedResultDto<GetStoreBankAccountForViewDto>> GetAll(GetAllStoreBankAccountsInput input)
        {

            var filteredStoreBankAccounts = _storeBankAccountRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AccountName.Contains(input.Filter) || e.AccountNo.Contains(input.Filter) || e.BankName.Contains(input.Filter) || e.RoutingNo.Contains(input.Filter) || e.BankAddress.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountNameFilter), e => e.AccountName.Contains(input.AccountNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountNoFilter), e => e.AccountNo.Contains(input.AccountNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BankNameFilter), e => e.BankName.Contains(input.BankNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RoutingNoFilter), e => e.RoutingNo.Contains(input.RoutingNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BankAddressFilter), e => e.BankAddress.Contains(input.BankAddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var pagedAndFilteredStoreBankAccounts = filteredStoreBankAccounts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeBankAccounts = from o in pagedAndFilteredStoreBankAccounts
                                    join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    select new
                                    {

                                        o.AccountName,
                                        o.AccountNo,
                                        o.BankName,
                                        o.RoutingNo,
                                        o.BankAddress,
                                        Id = o.Id,
                                        StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                    };

            var totalCount = await filteredStoreBankAccounts.CountAsync();

            var dbList = await storeBankAccounts.ToListAsync();
            var results = new List<GetStoreBankAccountForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreBankAccountForViewDto()
                {
                    StoreBankAccount = new StoreBankAccountDto
                    {

                        AccountName = o.AccountName,
                        AccountNo = o.AccountNo,
                        BankName = o.BankName,
                        RoutingNo = o.RoutingNo,
                        BankAddress = o.BankAddress,
                        Id = o.Id,
                    },
                    StoreName = o.StoreName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreBankAccountForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreBankAccountForViewDto> GetStoreBankAccountForView(long id)
        {
            var storeBankAccount = await _storeBankAccountRepository.GetAsync(id);

            var output = new GetStoreBankAccountForViewDto { StoreBankAccount = ObjectMapper.Map<StoreBankAccountDto>(storeBankAccount) };

            if (output.StoreBankAccount.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreBankAccount.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreBankAccounts_Edit)]
        public async Task<GetStoreBankAccountForEditOutput> GetStoreBankAccountForEdit(EntityDto<long> input)
        {
            var storeBankAccount = await _storeBankAccountRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreBankAccountForEditOutput { StoreBankAccount = ObjectMapper.Map<CreateOrEditStoreBankAccountDto>(storeBankAccount) };

            if (output.StoreBankAccount.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreBankAccount.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreBankAccountDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreBankAccounts_Create)]
        protected virtual async Task Create(CreateOrEditStoreBankAccountDto input)
        {
            var storeBankAccount = ObjectMapper.Map<StoreBankAccount>(input);

            if (AbpSession.TenantId != null)
            {
                storeBankAccount.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeBankAccountRepository.InsertAsync(storeBankAccount);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreBankAccounts_Edit)]
        protected virtual async Task Update(CreateOrEditStoreBankAccountDto input)
        {
            var storeBankAccount = await _storeBankAccountRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeBankAccount);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreBankAccounts_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeBankAccountRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreBankAccountsToExcel(GetAllStoreBankAccountsForExcelInput input)
        {

            var filteredStoreBankAccounts = _storeBankAccountRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AccountName.Contains(input.Filter) || e.AccountNo.Contains(input.Filter) || e.BankName.Contains(input.Filter) || e.RoutingNo.Contains(input.Filter) || e.BankAddress.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountNameFilter), e => e.AccountName.Contains(input.AccountNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountNoFilter), e => e.AccountNo.Contains(input.AccountNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BankNameFilter), e => e.BankName.Contains(input.BankNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RoutingNoFilter), e => e.RoutingNo.Contains(input.RoutingNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BankAddressFilter), e => e.BankAddress.Contains(input.BankAddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var query = (from o in filteredStoreBankAccounts
                         join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetStoreBankAccountForViewDto()
                         {
                             StoreBankAccount = new StoreBankAccountDto
                             {
                                 AccountName = o.AccountName,
                                 AccountNo = o.AccountNo,
                                 BankName = o.BankName,
                                 RoutingNo = o.RoutingNo,
                                 BankAddress = o.BankAddress,
                                 Id = o.Id
                             },
                             StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var storeBankAccountListDtos = await query.ToListAsync();

            return _storeBankAccountsExcelExporter.ExportToFile(storeBankAccountListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreBankAccounts)]
        public async Task<PagedResultDto<StoreBankAccountStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreBankAccountStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreBankAccountStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreBankAccountStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}