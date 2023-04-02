using SoftGrid.DiscountManagement;
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
    [AbpAuthorize(AppPermissions.Pages_DiscountCodeByCustomers)]
    public class DiscountCodeByCustomersAppService : SoftGridAppServiceBase, IDiscountCodeByCustomersAppService
    {
        private readonly IRepository<DiscountCodeByCustomer, long> _discountCodeByCustomerRepository;
        private readonly IDiscountCodeByCustomersExcelExporter _discountCodeByCustomersExcelExporter;
        private readonly IRepository<DiscountCodeGenerator, long> _lookup_discountCodeGeneratorRepository;
        private readonly IRepository<Contact, long> _lookup_contactRepository;

        public DiscountCodeByCustomersAppService(IRepository<DiscountCodeByCustomer, long> discountCodeByCustomerRepository, IDiscountCodeByCustomersExcelExporter discountCodeByCustomersExcelExporter, IRepository<DiscountCodeGenerator, long> lookup_discountCodeGeneratorRepository, IRepository<Contact, long> lookup_contactRepository)
        {
            _discountCodeByCustomerRepository = discountCodeByCustomerRepository;
            _discountCodeByCustomersExcelExporter = discountCodeByCustomersExcelExporter;
            _lookup_discountCodeGeneratorRepository = lookup_discountCodeGeneratorRepository;
            _lookup_contactRepository = lookup_contactRepository;

        }

        public async Task<PagedResultDto<GetDiscountCodeByCustomerForViewDto>> GetAll(GetAllDiscountCodeByCustomersInput input)
        {

            var filteredDiscountCodeByCustomers = _discountCodeByCustomerRepository.GetAll()
                        .Include(e => e.DiscountCodeGeneratorFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DiscountCodeGeneratorNameFilter), e => e.DiscountCodeGeneratorFk != null && e.DiscountCodeGeneratorFk.Name == input.DiscountCodeGeneratorNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter);

            var pagedAndFilteredDiscountCodeByCustomers = filteredDiscountCodeByCustomers
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var discountCodeByCustomers = from o in pagedAndFilteredDiscountCodeByCustomers
                                          join o1 in _lookup_discountCodeGeneratorRepository.GetAll() on o.DiscountCodeGeneratorId equals o1.Id into j1
                                          from s1 in j1.DefaultIfEmpty()

                                          join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                                          from s2 in j2.DefaultIfEmpty()

                                          select new
                                          {

                                              Id = o.Id,
                                              DiscountCodeGeneratorName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                              ContactFullName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString()
                                          };

            var totalCount = await filteredDiscountCodeByCustomers.CountAsync();

            var dbList = await discountCodeByCustomers.ToListAsync();
            var results = new List<GetDiscountCodeByCustomerForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDiscountCodeByCustomerForViewDto()
                {
                    DiscountCodeByCustomer = new DiscountCodeByCustomerDto
                    {

                        Id = o.Id,
                    },
                    DiscountCodeGeneratorName = o.DiscountCodeGeneratorName,
                    ContactFullName = o.ContactFullName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDiscountCodeByCustomerForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetDiscountCodeByCustomerForViewDto> GetDiscountCodeByCustomerForView(long id)
        {
            var discountCodeByCustomer = await _discountCodeByCustomerRepository.GetAsync(id);

            var output = new GetDiscountCodeByCustomerForViewDto { DiscountCodeByCustomer = ObjectMapper.Map<DiscountCodeByCustomerDto>(discountCodeByCustomer) };

            if (output.DiscountCodeByCustomer.DiscountCodeGeneratorId != null)
            {
                var _lookupDiscountCodeGenerator = await _lookup_discountCodeGeneratorRepository.FirstOrDefaultAsync((long)output.DiscountCodeByCustomer.DiscountCodeGeneratorId);
                output.DiscountCodeGeneratorName = _lookupDiscountCodeGenerator?.Name?.ToString();
            }

            if (output.DiscountCodeByCustomer.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.DiscountCodeByCustomer.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeByCustomers_Edit)]
        public async Task<GetDiscountCodeByCustomerForEditOutput> GetDiscountCodeByCustomerForEdit(EntityDto<long> input)
        {
            var discountCodeByCustomer = await _discountCodeByCustomerRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDiscountCodeByCustomerForEditOutput { DiscountCodeByCustomer = ObjectMapper.Map<CreateOrEditDiscountCodeByCustomerDto>(discountCodeByCustomer) };

            if (output.DiscountCodeByCustomer.DiscountCodeGeneratorId != null)
            {
                var _lookupDiscountCodeGenerator = await _lookup_discountCodeGeneratorRepository.FirstOrDefaultAsync((long)output.DiscountCodeByCustomer.DiscountCodeGeneratorId);
                output.DiscountCodeGeneratorName = _lookupDiscountCodeGenerator?.Name?.ToString();
            }

            if (output.DiscountCodeByCustomer.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.DiscountCodeByCustomer.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDiscountCodeByCustomerDto input)
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

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeByCustomers_Create)]
        protected virtual async Task Create(CreateOrEditDiscountCodeByCustomerDto input)
        {
            var discountCodeByCustomer = ObjectMapper.Map<DiscountCodeByCustomer>(input);

            if (AbpSession.TenantId != null)
            {
                discountCodeByCustomer.TenantId = (int?)AbpSession.TenantId;
            }

            await _discountCodeByCustomerRepository.InsertAsync(discountCodeByCustomer);

        }

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeByCustomers_Edit)]
        protected virtual async Task Update(CreateOrEditDiscountCodeByCustomerDto input)
        {
            var discountCodeByCustomer = await _discountCodeByCustomerRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, discountCodeByCustomer);

        }

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeByCustomers_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _discountCodeByCustomerRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDiscountCodeByCustomersToExcel(GetAllDiscountCodeByCustomersForExcelInput input)
        {

            var filteredDiscountCodeByCustomers = _discountCodeByCustomerRepository.GetAll()
                        .Include(e => e.DiscountCodeGeneratorFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DiscountCodeGeneratorNameFilter), e => e.DiscountCodeGeneratorFk != null && e.DiscountCodeGeneratorFk.Name == input.DiscountCodeGeneratorNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter);

            var query = (from o in filteredDiscountCodeByCustomers
                         join o1 in _lookup_discountCodeGeneratorRepository.GetAll() on o.DiscountCodeGeneratorId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetDiscountCodeByCustomerForViewDto()
                         {
                             DiscountCodeByCustomer = new DiscountCodeByCustomerDto
                             {
                                 Id = o.Id
                             },
                             DiscountCodeGeneratorName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ContactFullName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString()
                         });

            var discountCodeByCustomerListDtos = await query.ToListAsync();

            return _discountCodeByCustomersExcelExporter.ExportToFile(discountCodeByCustomerListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeByCustomers)]
        public async Task<PagedResultDto<DiscountCodeByCustomerDiscountCodeGeneratorLookupTableDto>> GetAllDiscountCodeGeneratorForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_discountCodeGeneratorRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var discountCodeGeneratorList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<DiscountCodeByCustomerDiscountCodeGeneratorLookupTableDto>();
            foreach (var discountCodeGenerator in discountCodeGeneratorList)
            {
                lookupTableDtoList.Add(new DiscountCodeByCustomerDiscountCodeGeneratorLookupTableDto
                {
                    Id = discountCodeGenerator.Id,
                    DisplayName = discountCodeGenerator.Name?.ToString()
                });
            }

            return new PagedResultDto<DiscountCodeByCustomerDiscountCodeGeneratorLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeByCustomers)]
        public async Task<PagedResultDto<DiscountCodeByCustomerContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<DiscountCodeByCustomerContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new DiscountCodeByCustomerContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<DiscountCodeByCustomerContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}