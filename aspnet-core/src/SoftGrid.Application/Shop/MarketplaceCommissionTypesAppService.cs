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
    [AbpAuthorize(AppPermissions.Pages_MarketplaceCommissionTypes)]
    public class MarketplaceCommissionTypesAppService : SoftGridAppServiceBase, IMarketplaceCommissionTypesAppService
    {
        private readonly IRepository<MarketplaceCommissionType, long> _marketplaceCommissionTypeRepository;
        private readonly IMarketplaceCommissionTypesExcelExporter _marketplaceCommissionTypesExcelExporter;

        public MarketplaceCommissionTypesAppService(IRepository<MarketplaceCommissionType, long> marketplaceCommissionTypeRepository, IMarketplaceCommissionTypesExcelExporter marketplaceCommissionTypesExcelExporter)
        {
            _marketplaceCommissionTypeRepository = marketplaceCommissionTypeRepository;
            _marketplaceCommissionTypesExcelExporter = marketplaceCommissionTypesExcelExporter;

        }

        public async Task<PagedResultDto<GetMarketplaceCommissionTypeForViewDto>> GetAll(GetAllMarketplaceCommissionTypesInput input)
        {

            var filteredMarketplaceCommissionTypes = _marketplaceCommissionTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinPercentageFilter != null, e => e.Percentage >= input.MinPercentageFilter)
                        .WhereIf(input.MaxPercentageFilter != null, e => e.Percentage <= input.MaxPercentageFilter)
                        .WhereIf(input.MinFixedAmountFilter != null, e => e.FixedAmount >= input.MinFixedAmountFilter)
                        .WhereIf(input.MaxFixedAmountFilter != null, e => e.FixedAmount <= input.MaxFixedAmountFilter);

            var pagedAndFilteredMarketplaceCommissionTypes = filteredMarketplaceCommissionTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var marketplaceCommissionTypes = from o in pagedAndFilteredMarketplaceCommissionTypes
                                             select new
                                             {

                                                 o.Name,
                                                 o.Percentage,
                                                 o.FixedAmount,
                                                 Id = o.Id
                                             };

            var totalCount = await filteredMarketplaceCommissionTypes.CountAsync();

            var dbList = await marketplaceCommissionTypes.ToListAsync();
            var results = new List<GetMarketplaceCommissionTypeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetMarketplaceCommissionTypeForViewDto()
                {
                    MarketplaceCommissionType = new MarketplaceCommissionTypeDto
                    {

                        Name = o.Name,
                        Percentage = o.Percentage,
                        FixedAmount = o.FixedAmount,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetMarketplaceCommissionTypeForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetMarketplaceCommissionTypeForViewDto> GetMarketplaceCommissionTypeForView(long id)
        {
            var marketplaceCommissionType = await _marketplaceCommissionTypeRepository.GetAsync(id);

            var output = new GetMarketplaceCommissionTypeForViewDto { MarketplaceCommissionType = ObjectMapper.Map<MarketplaceCommissionTypeDto>(marketplaceCommissionType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MarketplaceCommissionTypes_Edit)]
        public async Task<GetMarketplaceCommissionTypeForEditOutput> GetMarketplaceCommissionTypeForEdit(EntityDto<long> input)
        {
            var marketplaceCommissionType = await _marketplaceCommissionTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMarketplaceCommissionTypeForEditOutput { MarketplaceCommissionType = ObjectMapper.Map<CreateOrEditMarketplaceCommissionTypeDto>(marketplaceCommissionType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMarketplaceCommissionTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_MarketplaceCommissionTypes_Create)]
        protected virtual async Task Create(CreateOrEditMarketplaceCommissionTypeDto input)
        {
            var marketplaceCommissionType = ObjectMapper.Map<MarketplaceCommissionType>(input);

            if (AbpSession.TenantId != null)
            {
                marketplaceCommissionType.TenantId = (int?)AbpSession.TenantId;
            }

            await _marketplaceCommissionTypeRepository.InsertAsync(marketplaceCommissionType);

        }

        [AbpAuthorize(AppPermissions.Pages_MarketplaceCommissionTypes_Edit)]
        protected virtual async Task Update(CreateOrEditMarketplaceCommissionTypeDto input)
        {
            var marketplaceCommissionType = await _marketplaceCommissionTypeRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, marketplaceCommissionType);

        }

        [AbpAuthorize(AppPermissions.Pages_MarketplaceCommissionTypes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _marketplaceCommissionTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMarketplaceCommissionTypesToExcel(GetAllMarketplaceCommissionTypesForExcelInput input)
        {

            var filteredMarketplaceCommissionTypes = _marketplaceCommissionTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinPercentageFilter != null, e => e.Percentage >= input.MinPercentageFilter)
                        .WhereIf(input.MaxPercentageFilter != null, e => e.Percentage <= input.MaxPercentageFilter)
                        .WhereIf(input.MinFixedAmountFilter != null, e => e.FixedAmount >= input.MinFixedAmountFilter)
                        .WhereIf(input.MaxFixedAmountFilter != null, e => e.FixedAmount <= input.MaxFixedAmountFilter);

            var query = (from o in filteredMarketplaceCommissionTypes
                         select new GetMarketplaceCommissionTypeForViewDto()
                         {
                             MarketplaceCommissionType = new MarketplaceCommissionTypeDto
                             {
                                 Name = o.Name,
                                 Percentage = o.Percentage,
                                 FixedAmount = o.FixedAmount,
                                 Id = o.Id
                             }
                         });

            var marketplaceCommissionTypeListDtos = await query.ToListAsync();

            return _marketplaceCommissionTypesExcelExporter.ExportToFile(marketplaceCommissionTypeListDtos);
        }

    }
}