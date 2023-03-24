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
    [AbpAuthorize(AppPermissions.Pages_StoreSalesAlerts)]
    public class StoreSalesAlertsAppService : SoftGridAppServiceBase, IStoreSalesAlertsAppService
    {
        private readonly IRepository<StoreSalesAlert, long> _storeSalesAlertRepository;
        private readonly IStoreSalesAlertsExcelExporter _storeSalesAlertsExcelExporter;
        private readonly IRepository<Store, long> _lookup_storeRepository;

        public StoreSalesAlertsAppService(IRepository<StoreSalesAlert, long> storeSalesAlertRepository, IStoreSalesAlertsExcelExporter storeSalesAlertsExcelExporter, IRepository<Store, long> lookup_storeRepository)
        {
            _storeSalesAlertRepository = storeSalesAlertRepository;
            _storeSalesAlertsExcelExporter = storeSalesAlertsExcelExporter;
            _lookup_storeRepository = lookup_storeRepository;

        }

        public async Task<PagedResultDto<GetStoreSalesAlertForViewDto>> GetAll(GetAllStoreSalesAlertsInput input)
        {

            var filteredStoreSalesAlerts = _storeSalesAlertRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Message.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MessageFilter), e => e.Message.Contains(input.MessageFilter))
                        .WhereIf(input.CurrentFilter.HasValue && input.CurrentFilter > -1, e => (input.CurrentFilter == 1 && e.Current) || (input.CurrentFilter == 0 && !e.Current))
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var pagedAndFilteredStoreSalesAlerts = filteredStoreSalesAlerts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeSalesAlerts = from o in pagedAndFilteredStoreSalesAlerts
                                   join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                                   from s1 in j1.DefaultIfEmpty()

                                   select new
                                   {

                                       o.Message,
                                       o.Current,
                                       o.StartDate,
                                       o.EndDate,
                                       Id = o.Id,
                                       StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                   };

            var totalCount = await filteredStoreSalesAlerts.CountAsync();

            var dbList = await storeSalesAlerts.ToListAsync();
            var results = new List<GetStoreSalesAlertForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreSalesAlertForViewDto()
                {
                    StoreSalesAlert = new StoreSalesAlertDto
                    {

                        Message = o.Message,
                        Current = o.Current,
                        StartDate = o.StartDate,
                        EndDate = o.EndDate,
                        Id = o.Id,
                    },
                    StoreName = o.StoreName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreSalesAlertForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreSalesAlertForViewDto> GetStoreSalesAlertForView(long id)
        {
            var storeSalesAlert = await _storeSalesAlertRepository.GetAsync(id);

            var output = new GetStoreSalesAlertForViewDto { StoreSalesAlert = ObjectMapper.Map<StoreSalesAlertDto>(storeSalesAlert) };

            if (output.StoreSalesAlert.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreSalesAlert.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreSalesAlerts_Edit)]
        public async Task<GetStoreSalesAlertForEditOutput> GetStoreSalesAlertForEdit(EntityDto<long> input)
        {
            var storeSalesAlert = await _storeSalesAlertRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreSalesAlertForEditOutput { StoreSalesAlert = ObjectMapper.Map<CreateOrEditStoreSalesAlertDto>(storeSalesAlert) };

            if (output.StoreSalesAlert.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreSalesAlert.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreSalesAlertDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreSalesAlerts_Create)]
        protected virtual async Task Create(CreateOrEditStoreSalesAlertDto input)
        {
            var storeSalesAlert = ObjectMapper.Map<StoreSalesAlert>(input);

            if (AbpSession.TenantId != null)
            {
                storeSalesAlert.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeSalesAlertRepository.InsertAsync(storeSalesAlert);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreSalesAlerts_Edit)]
        protected virtual async Task Update(CreateOrEditStoreSalesAlertDto input)
        {
            var storeSalesAlert = await _storeSalesAlertRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeSalesAlert);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreSalesAlerts_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeSalesAlertRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreSalesAlertsToExcel(GetAllStoreSalesAlertsForExcelInput input)
        {

            var filteredStoreSalesAlerts = _storeSalesAlertRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Message.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MessageFilter), e => e.Message.Contains(input.MessageFilter))
                        .WhereIf(input.CurrentFilter.HasValue && input.CurrentFilter > -1, e => (input.CurrentFilter == 1 && e.Current) || (input.CurrentFilter == 0 && !e.Current))
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter);

            var query = (from o in filteredStoreSalesAlerts
                         join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetStoreSalesAlertForViewDto()
                         {
                             StoreSalesAlert = new StoreSalesAlertDto
                             {
                                 Message = o.Message,
                                 Current = o.Current,
                                 StartDate = o.StartDate,
                                 EndDate = o.EndDate,
                                 Id = o.Id
                             },
                             StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var storeSalesAlertListDtos = await query.ToListAsync();

            return _storeSalesAlertsExcelExporter.ExportToFile(storeSalesAlertListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreSalesAlerts)]
        public async Task<PagedResultDto<StoreSalesAlertStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreSalesAlertStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreSalesAlertStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreSalesAlertStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}