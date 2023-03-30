using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.LookupData.Exporting;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.LookupData
{
    [AbpAuthorize(AppPermissions.Pages_SubscriptionTypes)]
    public class SubscriptionTypesAppService : SoftGridAppServiceBase, ISubscriptionTypesAppService
    {
        private readonly IRepository<SubscriptionType, long> _subscriptionTypeRepository;
        private readonly ISubscriptionTypesExcelExporter _subscriptionTypesExcelExporter;

        public SubscriptionTypesAppService(IRepository<SubscriptionType, long> subscriptionTypeRepository, ISubscriptionTypesExcelExporter subscriptionTypesExcelExporter)
        {
            _subscriptionTypeRepository = subscriptionTypeRepository;
            _subscriptionTypesExcelExporter = subscriptionTypesExcelExporter;

        }

        public async Task<PagedResultDto<GetSubscriptionTypeForViewDto>> GetAll(GetAllSubscriptionTypesInput input)
        {

            var filteredSubscriptionTypes = _subscriptionTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinNumberOfDaysFilter != null, e => e.NumberOfDays >= input.MinNumberOfDaysFilter)
                        .WhereIf(input.MaxNumberOfDaysFilter != null, e => e.NumberOfDays <= input.MaxNumberOfDaysFilter);

            var pagedAndFilteredSubscriptionTypes = filteredSubscriptionTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var subscriptionTypes = from o in pagedAndFilteredSubscriptionTypes
                                    select new
                                    {

                                        o.Name,
                                        o.NumberOfDays,
                                        Id = o.Id
                                    };

            var totalCount = await filteredSubscriptionTypes.CountAsync();

            var dbList = await subscriptionTypes.ToListAsync();
            var results = new List<GetSubscriptionTypeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSubscriptionTypeForViewDto()
                {
                    SubscriptionType = new SubscriptionTypeDto
                    {

                        Name = o.Name,
                        NumberOfDays = o.NumberOfDays,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetSubscriptionTypeForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetSubscriptionTypeForViewDto> GetSubscriptionTypeForView(long id)
        {
            var subscriptionType = await _subscriptionTypeRepository.GetAsync(id);

            var output = new GetSubscriptionTypeForViewDto { SubscriptionType = ObjectMapper.Map<SubscriptionTypeDto>(subscriptionType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SubscriptionTypes_Edit)]
        public async Task<GetSubscriptionTypeForEditOutput> GetSubscriptionTypeForEdit(EntityDto<long> input)
        {
            var subscriptionType = await _subscriptionTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSubscriptionTypeForEditOutput { SubscriptionType = ObjectMapper.Map<CreateOrEditSubscriptionTypeDto>(subscriptionType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSubscriptionTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_SubscriptionTypes_Create)]
        protected virtual async Task Create(CreateOrEditSubscriptionTypeDto input)
        {
            var subscriptionType = ObjectMapper.Map<SubscriptionType>(input);

            if (AbpSession.TenantId != null)
            {
                subscriptionType.TenantId = (int?)AbpSession.TenantId;
            }

            await _subscriptionTypeRepository.InsertAsync(subscriptionType);

        }

        [AbpAuthorize(AppPermissions.Pages_SubscriptionTypes_Edit)]
        protected virtual async Task Update(CreateOrEditSubscriptionTypeDto input)
        {
            var subscriptionType = await _subscriptionTypeRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, subscriptionType);

        }

        [AbpAuthorize(AppPermissions.Pages_SubscriptionTypes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _subscriptionTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetSubscriptionTypesToExcel(GetAllSubscriptionTypesForExcelInput input)
        {

            var filteredSubscriptionTypes = _subscriptionTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinNumberOfDaysFilter != null, e => e.NumberOfDays >= input.MinNumberOfDaysFilter)
                        .WhereIf(input.MaxNumberOfDaysFilter != null, e => e.NumberOfDays <= input.MaxNumberOfDaysFilter);

            var query = (from o in filteredSubscriptionTypes
                         select new GetSubscriptionTypeForViewDto()
                         {
                             SubscriptionType = new SubscriptionTypeDto
                             {
                                 Name = o.Name,
                                 NumberOfDays = o.NumberOfDays,
                                 Id = o.Id
                             }
                         });

            var subscriptionTypeListDtos = await query.ToListAsync();

            return _subscriptionTypesExcelExporter.ExportToFile(subscriptionTypeListDtos);
        }

    }
}