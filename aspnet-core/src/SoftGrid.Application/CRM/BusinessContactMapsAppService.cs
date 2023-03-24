using SoftGrid.CRM;
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
    [AbpAuthorize(AppPermissions.Pages_BusinessContactMaps)]
    public class BusinessContactMapsAppService : SoftGridAppServiceBase, IBusinessContactMapsAppService
    {
        private readonly IRepository<BusinessContactMap, long> _businessContactMapRepository;
        private readonly IBusinessContactMapsExcelExporter _businessContactMapsExcelExporter;
        private readonly IRepository<Business, long> _lookup_businessRepository;
        private readonly IRepository<Contact, long> _lookup_contactRepository;

        public BusinessContactMapsAppService(IRepository<BusinessContactMap, long> businessContactMapRepository, IBusinessContactMapsExcelExporter businessContactMapsExcelExporter, IRepository<Business, long> lookup_businessRepository, IRepository<Contact, long> lookup_contactRepository)
        {
            _businessContactMapRepository = businessContactMapRepository;
            _businessContactMapsExcelExporter = businessContactMapsExcelExporter;
            _lookup_businessRepository = lookup_businessRepository;
            _lookup_contactRepository = lookup_contactRepository;

        }

        public async Task<PagedResultDto<GetBusinessContactMapForViewDto>> GetAll(GetAllBusinessContactMapsInput input)
        {

            var filteredBusinessContactMaps = _businessContactMapRepository.GetAll()
                        .Include(e => e.BusinessFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter);

            var pagedAndFilteredBusinessContactMaps = filteredBusinessContactMaps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var businessContactMaps = from o in pagedAndFilteredBusinessContactMaps
                                      join o1 in _lookup_businessRepository.GetAll() on o.BusinessId equals o1.Id into j1
                                      from s1 in j1.DefaultIfEmpty()

                                      join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                                      from s2 in j2.DefaultIfEmpty()

                                      select new
                                      {

                                          Id = o.Id,
                                          BusinessName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                          ContactFullName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString()
                                      };

            var totalCount = await filteredBusinessContactMaps.CountAsync();

            var dbList = await businessContactMaps.ToListAsync();
            var results = new List<GetBusinessContactMapForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBusinessContactMapForViewDto()
                {
                    BusinessContactMap = new BusinessContactMapDto
                    {

                        Id = o.Id,
                    },
                    BusinessName = o.BusinessName,
                    ContactFullName = o.ContactFullName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetBusinessContactMapForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetBusinessContactMapForViewDto> GetBusinessContactMapForView(long id)
        {
            var businessContactMap = await _businessContactMapRepository.GetAsync(id);

            var output = new GetBusinessContactMapForViewDto { BusinessContactMap = ObjectMapper.Map<BusinessContactMapDto>(businessContactMap) };

            if (output.BusinessContactMap.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.BusinessContactMap.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            if (output.BusinessContactMap.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.BusinessContactMap.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessContactMaps_Edit)]
        public async Task<GetBusinessContactMapForEditOutput> GetBusinessContactMapForEdit(EntityDto<long> input)
        {
            var businessContactMap = await _businessContactMapRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBusinessContactMapForEditOutput { BusinessContactMap = ObjectMapper.Map<CreateOrEditBusinessContactMapDto>(businessContactMap) };

            if (output.BusinessContactMap.BusinessId != null)
            {
                var _lookupBusiness = await _lookup_businessRepository.FirstOrDefaultAsync((long)output.BusinessContactMap.BusinessId);
                output.BusinessName = _lookupBusiness?.Name?.ToString();
            }

            if (output.BusinessContactMap.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((long)output.BusinessContactMap.ContactId);
                output.ContactFullName = _lookupContact?.FullName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBusinessContactMapDto input)
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

        [AbpAuthorize(AppPermissions.Pages_BusinessContactMaps_Create)]
        protected virtual async Task Create(CreateOrEditBusinessContactMapDto input)
        {
            var businessContactMap = ObjectMapper.Map<BusinessContactMap>(input);

            if (AbpSession.TenantId != null)
            {
                businessContactMap.TenantId = (int?)AbpSession.TenantId;
            }

            await _businessContactMapRepository.InsertAsync(businessContactMap);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessContactMaps_Edit)]
        protected virtual async Task Update(CreateOrEditBusinessContactMapDto input)
        {
            var businessContactMap = await _businessContactMapRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, businessContactMap);

        }

        [AbpAuthorize(AppPermissions.Pages_BusinessContactMaps_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _businessContactMapRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetBusinessContactMapsToExcel(GetAllBusinessContactMapsForExcelInput input)
        {

            var filteredBusinessContactMaps = _businessContactMapRepository.GetAll()
                        .Include(e => e.BusinessFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BusinessNameFilter), e => e.BusinessFk != null && e.BusinessFk.Name == input.BusinessNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFullNameFilter), e => e.ContactFk != null && e.ContactFk.FullName == input.ContactFullNameFilter);

            var query = (from o in filteredBusinessContactMaps
                         join o1 in _lookup_businessRepository.GetAll() on o.BusinessId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetBusinessContactMapForViewDto()
                         {
                             BusinessContactMap = new BusinessContactMapDto
                             {
                                 Id = o.Id
                             },
                             BusinessName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ContactFullName = s2 == null || s2.FullName == null ? "" : s2.FullName.ToString()
                         });

            var businessContactMapListDtos = await query.ToListAsync();

            return _businessContactMapsExcelExporter.ExportToFile(businessContactMapListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessContactMaps)]
        public async Task<PagedResultDto<BusinessContactMapBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_businessRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var businessList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BusinessContactMapBusinessLookupTableDto>();
            foreach (var business in businessList)
            {
                lookupTableDtoList.Add(new BusinessContactMapBusinessLookupTableDto
                {
                    Id = business.Id,
                    DisplayName = business.Name?.ToString()
                });
            }

            return new PagedResultDto<BusinessContactMapBusinessLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_BusinessContactMaps)]
        public async Task<PagedResultDto<BusinessContactMapContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.FullName != null && e.FullName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BusinessContactMapContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new BusinessContactMapContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.FullName?.ToString()
                });
            }

            return new PagedResultDto<BusinessContactMapContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}