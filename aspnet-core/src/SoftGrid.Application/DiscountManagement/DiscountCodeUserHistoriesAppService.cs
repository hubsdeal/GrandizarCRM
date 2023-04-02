using SoftGrid.DiscountManagement;
using SoftGrid.OrderManagement;
using SoftGrid.CRM;

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
    [AbpAuthorize(AppPermissions.Pages_DiscountCodeUserHistories)]
    public class DiscountCodeUserHistoriesAppService : SoftGridAppServiceBase, IDiscountCodeUserHistoriesAppService
    {
        private readonly IRepository<DiscountCodeUserHistory, long> _discountCodeUserHistoryRepository;
        private readonly IDiscountCodeUserHistoriesExcelExporter _discountCodeUserHistoriesExcelExporter;
        private readonly IRepository<DiscountCodeGenerator, long> _lookup_discountCodeGeneratorRepository;
        private readonly IRepository<Order, long> _lookup_orderRepository;
        private readonly IRepository<Contact, long> _lookup_contactRepository;

        public DiscountCodeUserHistoriesAppService(IRepository<DiscountCodeUserHistory, long> discountCodeUserHistoryRepository, IDiscountCodeUserHistoriesExcelExporter discountCodeUserHistoriesExcelExporter, IRepository<DiscountCodeGenerator, long> lookup_discountCodeGeneratorRepository, IRepository<Order, long> lookup_orderRepository, IRepository<Contact, long> lookup_contactRepository)
        {
            _discountCodeUserHistoryRepository = discountCodeUserHistoryRepository;
            _discountCodeUserHistoriesExcelExporter = discountCodeUserHistoriesExcelExporter;
            _lookup_discountCodeGeneratorRepository = lookup_discountCodeGeneratorRepository;
            _lookup_orderRepository = lookup_orderRepository;
            _lookup_contactRepository = lookup_contactRepository;

        }

        public async Task<PagedResultDto<GetDiscountCodeUserHistoryForViewDto>> GetAll(GetAllDiscountCodeUserHistoriesInput input)
        {

            var filteredDiscountCodeUserHistories = _discountCodeUserHistoryRepository.GetAll()
                        .Include(e => e.DiscountCodeGeneratorFk)
                        .Include(e => e.OrderFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.MinDateFilter != null, e => e.Date >= input.MinDateFilter)
                        .WhereIf(input.MaxDateFilter != null, e => e.Date <= input.MaxDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DiscountCodeGeneratorNameFilter), e => e.DiscountCodeGeneratorFk != null && e.DiscountCodeGeneratorFk.Name == input.DiscountCodeGeneratorNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderInvoiceNumberFilter), e => e.OrderFk != null && e.OrderFk.InvoiceNumber == input.OrderInvoiceNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter);

            var pagedAndFilteredDiscountCodeUserHistories = filteredDiscountCodeUserHistories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var discountCodeUserHistories = from o in pagedAndFilteredDiscountCodeUserHistories
                                            join o1 in _lookup_discountCodeGeneratorRepository.GetAll() on o.DiscountCodeGeneratorId equals o1.Id into j1
                                            from s1 in j1.DefaultIfEmpty()

                                            join o2 in _lookup_orderRepository.GetAll() on o.OrderId equals o2.Id into j2
                                            from s2 in j2.DefaultIfEmpty()

                                            join o3 in _lookup_contactRepository.GetAll() on o.ContactId equals o3.Id into j3
                                            from s3 in j3.DefaultIfEmpty()

                                            select new
                                            {

                                                o.Amount,
                                                o.Date,
                                                Id = o.Id,
                                                DiscountCodeGeneratorName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                                OrderInvoiceNumber = s2 == null || s2.InvoiceNumber == null ? "" : s2.InvoiceNumber.ToString(),
                                                ContactFullName = s3 == null || s3.FullName == null ? "" : s3.FullName.ToString()
                                            };

            var totalCount = await filteredDiscountCodeUserHistories.CountAsync();

            var dbList = await discountCodeUserHistories.ToListAsync();
            var results = new List<GetDiscountCodeUserHistoryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDiscountCodeUserHistoryForViewDto()
                {
                    DiscountCodeUserHistory = new DiscountCodeUserHistoryDto
                    {

                        Amount = o.Amount,
                        Date = o.Date,
                        Id = o.Id,
                    },
                    DiscountCodeGeneratorName = o.DiscountCodeGeneratorName,
                    OrderInvoiceNumber = o.OrderInvoiceNumber,
                    ContactFullName = o.ContactFullName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDiscountCodeUserHistoryForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetDiscountCodeUserHistoryForViewDto> GetDiscountCodeUserHistoryForView(long id)
        {
            var discountCodeUserHistory = await _discountCodeUserHistoryRepository.GetAsync(id);

            var output = new GetDiscountCodeUserHistoryForViewDto { DiscountCodeUserHistory = ObjectMapper.Map<DiscountCodeUserHistoryDto>(discountCodeUserHistory) };

            if (output.DiscountCodeUserHistory.DiscountCodeGeneratorId != null)
            {
                var _lookupDiscountCodeGenerator = await _lookup_discountCodeGeneratorRepository.FirstOrDefaultAsync((long)output.DiscountCodeUserHistory.DiscountCodeGeneratorId);
                output.DiscountCodeGeneratorName = _lookupDiscountCodeGenerator?.Name?.ToString();
            }

            if (output.DiscountCodeUserHistory.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((long)output.DiscountCodeUserHistory.OrderId);
                output.OrderInvoiceNumber = _lookupOrder?.InvoiceNumber?.ToString();
            }

            if (output.DiscountCodeUserHistory.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.DiscountCodeUserHistory.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeUserHistories_Edit)]
        public async Task<GetDiscountCodeUserHistoryForEditOutput> GetDiscountCodeUserHistoryForEdit(EntityDto<long> input)
        {
            var discountCodeUserHistory = await _discountCodeUserHistoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDiscountCodeUserHistoryForEditOutput { DiscountCodeUserHistory = ObjectMapper.Map<CreateOrEditDiscountCodeUserHistoryDto>(discountCodeUserHistory) };

            if (output.DiscountCodeUserHistory.DiscountCodeGeneratorId != null)
            {
                var _lookupDiscountCodeGenerator = await _lookup_discountCodeGeneratorRepository.FirstOrDefaultAsync((long)output.DiscountCodeUserHistory.DiscountCodeGeneratorId);
                output.DiscountCodeGeneratorName = _lookupDiscountCodeGenerator?.Name?.ToString();
            }

            if (output.DiscountCodeUserHistory.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((long)output.DiscountCodeUserHistory.OrderId);
                output.OrderInvoiceNumber = _lookupOrder?.InvoiceNumber?.ToString();
            }

            if (output.DiscountCodeUserHistory.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.DiscountCodeUserHistory.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDiscountCodeUserHistoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeUserHistories_Create)]
        protected virtual async Task Create(CreateOrEditDiscountCodeUserHistoryDto input)
        {
            var discountCodeUserHistory = ObjectMapper.Map<DiscountCodeUserHistory>(input);

            if (AbpSession.TenantId != null)
            {
                discountCodeUserHistory.TenantId = (int?)AbpSession.TenantId;
            }

            await _discountCodeUserHistoryRepository.InsertAsync(discountCodeUserHistory);

        }

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeUserHistories_Edit)]
        protected virtual async Task Update(CreateOrEditDiscountCodeUserHistoryDto input)
        {
            var discountCodeUserHistory = await _discountCodeUserHistoryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, discountCodeUserHistory);

        }

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeUserHistories_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _discountCodeUserHistoryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDiscountCodeUserHistoriesToExcel(GetAllDiscountCodeUserHistoriesForExcelInput input)
        {

            var filteredDiscountCodeUserHistories = _discountCodeUserHistoryRepository.GetAll()
                        .Include(e => e.DiscountCodeGeneratorFk)
                        .Include(e => e.OrderFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.MinDateFilter != null, e => e.Date >= input.MinDateFilter)
                        .WhereIf(input.MaxDateFilter != null, e => e.Date <= input.MaxDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DiscountCodeGeneratorNameFilter), e => e.DiscountCodeGeneratorFk != null && e.DiscountCodeGeneratorFk.Name == input.DiscountCodeGeneratorNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderInvoiceNumberFilter), e => e.OrderFk != null && e.OrderFk.InvoiceNumber == input.OrderInvoiceNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter);

            var query = (from o in filteredDiscountCodeUserHistories
                         join o1 in _lookup_discountCodeGeneratorRepository.GetAll() on o.DiscountCodeGeneratorId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_orderRepository.GetAll() on o.OrderId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_contactRepository.GetAll() on o.ContactId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetDiscountCodeUserHistoryForViewDto()
                         {
                             DiscountCodeUserHistory = new DiscountCodeUserHistoryDto
                             {
                                 Amount = o.Amount,
                                 Date = o.Date,
                                 Id = o.Id
                             },
                             DiscountCodeGeneratorName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             OrderInvoiceNumber = s2 == null || s2.InvoiceNumber == null ? "" : s2.InvoiceNumber.ToString(),
                             ContactFullName = s3 == null || s3.FullName == null ? "" : s3.FullName.ToString()
                         });

            var discountCodeUserHistoryListDtos = await query.ToListAsync();

            return _discountCodeUserHistoriesExcelExporter.ExportToFile(discountCodeUserHistoryListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeUserHistories)]
        public async Task<PagedResultDto<DiscountCodeUserHistoryDiscountCodeGeneratorLookupTableDto>> GetAllDiscountCodeGeneratorForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_discountCodeGeneratorRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var discountCodeGeneratorList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<DiscountCodeUserHistoryDiscountCodeGeneratorLookupTableDto>();
            foreach (var discountCodeGenerator in discountCodeGeneratorList)
            {
                lookupTableDtoList.Add(new DiscountCodeUserHistoryDiscountCodeGeneratorLookupTableDto
                {
                    Id = discountCodeGenerator.Id,
                    DisplayName = discountCodeGenerator.Name?.ToString()
                });
            }

            return new PagedResultDto<DiscountCodeUserHistoryDiscountCodeGeneratorLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeUserHistories)]
        public async Task<PagedResultDto<DiscountCodeUserHistoryOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_orderRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.InvoiceNumber != null && e.InvoiceNumber.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var orderList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<DiscountCodeUserHistoryOrderLookupTableDto>();
            foreach (var order in orderList)
            {
                lookupTableDtoList.Add(new DiscountCodeUserHistoryOrderLookupTableDto
                {
                    Id = order.Id,
                    DisplayName = order.InvoiceNumber?.ToString()
                });
            }

            return new PagedResultDto<DiscountCodeUserHistoryOrderLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeUserHistories)]
        public async Task<PagedResultDto<DiscountCodeUserHistoryContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<DiscountCodeUserHistoryContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new DiscountCodeUserHistoryContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<DiscountCodeUserHistoryContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}