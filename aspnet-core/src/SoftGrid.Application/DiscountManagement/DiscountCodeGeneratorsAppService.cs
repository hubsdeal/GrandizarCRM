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
    [AbpAuthorize(AppPermissions.Pages_DiscountCodeGenerators)]
    public class DiscountCodeGeneratorsAppService : SoftGridAppServiceBase, IDiscountCodeGeneratorsAppService
    {
        private readonly IRepository<DiscountCodeGenerator, long> _discountCodeGeneratorRepository;
        private readonly IDiscountCodeGeneratorsExcelExporter _discountCodeGeneratorsExcelExporter;

        public DiscountCodeGeneratorsAppService(IRepository<DiscountCodeGenerator, long> discountCodeGeneratorRepository, IDiscountCodeGeneratorsExcelExporter discountCodeGeneratorsExcelExporter)
        {
            _discountCodeGeneratorRepository = discountCodeGeneratorRepository;
            _discountCodeGeneratorsExcelExporter = discountCodeGeneratorsExcelExporter;

        }

        public async Task<PagedResultDto<GetDiscountCodeGeneratorForViewDto>> GetAll(GetAllDiscountCodeGeneratorsInput input)
        {

            var filteredDiscountCodeGenerators = _discountCodeGeneratorRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.CouponCode.Contains(input.Filter) || e.AdminNotes.Contains(input.Filter) || e.StartTime.Contains(input.Filter) || e.EndTime.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CouponCodeFilter), e => e.CouponCode.Contains(input.CouponCodeFilter))
                        .WhereIf(input.PercentageOrFixedAmountFilter.HasValue && input.PercentageOrFixedAmountFilter > -1, e => (input.PercentageOrFixedAmountFilter == 1 && e.PercentageOrFixedAmount) || (input.PercentageOrFixedAmountFilter == 0 && !e.PercentageOrFixedAmount))
                        .WhereIf(input.MinDiscountPercentageFilter != null, e => e.DiscountPercentage >= input.MinDiscountPercentageFilter)
                        .WhereIf(input.MaxDiscountPercentageFilter != null, e => e.DiscountPercentage <= input.MaxDiscountPercentageFilter)
                        .WhereIf(input.MinDiscountAmountFilter != null, e => e.DiscountAmount >= input.MinDiscountAmountFilter)
                        .WhereIf(input.MaxDiscountAmountFilter != null, e => e.DiscountAmount <= input.MaxDiscountAmountFilter)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdminNotesFilter), e => e.AdminNotes.Contains(input.AdminNotesFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StartTimeFilter), e => e.StartTime.Contains(input.StartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EndTimeFilter), e => e.EndTime.Contains(input.EndTimeFilter));

            var pagedAndFilteredDiscountCodeGenerators = filteredDiscountCodeGenerators
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var discountCodeGenerators = from o in pagedAndFilteredDiscountCodeGenerators
                                         select new
                                         {

                                             o.Name,
                                             o.CouponCode,
                                             o.PercentageOrFixedAmount,
                                             o.DiscountPercentage,
                                             o.DiscountAmount,
                                             o.StartDate,
                                             o.EndDate,
                                             o.AdminNotes,
                                             o.IsActive,
                                             o.StartTime,
                                             o.EndTime,
                                             Id = o.Id
                                         };

            var totalCount = await filteredDiscountCodeGenerators.CountAsync();

            var dbList = await discountCodeGenerators.ToListAsync();
            var results = new List<GetDiscountCodeGeneratorForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDiscountCodeGeneratorForViewDto()
                {
                    DiscountCodeGenerator = new DiscountCodeGeneratorDto
                    {

                        Name = o.Name,
                        CouponCode = o.CouponCode,
                        PercentageOrFixedAmount = o.PercentageOrFixedAmount,
                        DiscountPercentage = o.DiscountPercentage,
                        DiscountAmount = o.DiscountAmount,
                        StartDate = o.StartDate,
                        EndDate = o.EndDate,
                        AdminNotes = o.AdminNotes,
                        IsActive = o.IsActive,
                        StartTime = o.StartTime,
                        EndTime = o.EndTime,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDiscountCodeGeneratorForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetDiscountCodeGeneratorForViewDto> GetDiscountCodeGeneratorForView(long id)
        {
            var discountCodeGenerator = await _discountCodeGeneratorRepository.GetAsync(id);

            var output = new GetDiscountCodeGeneratorForViewDto { DiscountCodeGenerator = ObjectMapper.Map<DiscountCodeGeneratorDto>(discountCodeGenerator) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeGenerators_Edit)]
        public async Task<GetDiscountCodeGeneratorForEditOutput> GetDiscountCodeGeneratorForEdit(EntityDto<long> input)
        {
            var discountCodeGenerator = await _discountCodeGeneratorRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDiscountCodeGeneratorForEditOutput { DiscountCodeGenerator = ObjectMapper.Map<CreateOrEditDiscountCodeGeneratorDto>(discountCodeGenerator) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDiscountCodeGeneratorDto input)
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

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeGenerators_Create)]
        protected virtual async Task Create(CreateOrEditDiscountCodeGeneratorDto input)
        {
            var discountCodeGenerator = ObjectMapper.Map<DiscountCodeGenerator>(input);

            if (AbpSession.TenantId != null)
            {
                discountCodeGenerator.TenantId = (int?)AbpSession.TenantId;
            }

            await _discountCodeGeneratorRepository.InsertAsync(discountCodeGenerator);

        }

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeGenerators_Edit)]
        protected virtual async Task Update(CreateOrEditDiscountCodeGeneratorDto input)
        {
            var discountCodeGenerator = await _discountCodeGeneratorRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, discountCodeGenerator);

        }

        [AbpAuthorize(AppPermissions.Pages_DiscountCodeGenerators_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _discountCodeGeneratorRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDiscountCodeGeneratorsToExcel(GetAllDiscountCodeGeneratorsForExcelInput input)
        {

            var filteredDiscountCodeGenerators = _discountCodeGeneratorRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.CouponCode.Contains(input.Filter) || e.AdminNotes.Contains(input.Filter) || e.StartTime.Contains(input.Filter) || e.EndTime.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CouponCodeFilter), e => e.CouponCode.Contains(input.CouponCodeFilter))
                        .WhereIf(input.PercentageOrFixedAmountFilter.HasValue && input.PercentageOrFixedAmountFilter > -1, e => (input.PercentageOrFixedAmountFilter == 1 && e.PercentageOrFixedAmount) || (input.PercentageOrFixedAmountFilter == 0 && !e.PercentageOrFixedAmount))
                        .WhereIf(input.MinDiscountPercentageFilter != null, e => e.DiscountPercentage >= input.MinDiscountPercentageFilter)
                        .WhereIf(input.MaxDiscountPercentageFilter != null, e => e.DiscountPercentage <= input.MaxDiscountPercentageFilter)
                        .WhereIf(input.MinDiscountAmountFilter != null, e => e.DiscountAmount >= input.MinDiscountAmountFilter)
                        .WhereIf(input.MaxDiscountAmountFilter != null, e => e.DiscountAmount <= input.MaxDiscountAmountFilter)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdminNotesFilter), e => e.AdminNotes.Contains(input.AdminNotesFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StartTimeFilter), e => e.StartTime.Contains(input.StartTimeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EndTimeFilter), e => e.EndTime.Contains(input.EndTimeFilter));

            var query = (from o in filteredDiscountCodeGenerators
                         select new GetDiscountCodeGeneratorForViewDto()
                         {
                             DiscountCodeGenerator = new DiscountCodeGeneratorDto
                             {
                                 Name = o.Name,
                                 CouponCode = o.CouponCode,
                                 PercentageOrFixedAmount = o.PercentageOrFixedAmount,
                                 DiscountPercentage = o.DiscountPercentage,
                                 DiscountAmount = o.DiscountAmount,
                                 StartDate = o.StartDate,
                                 EndDate = o.EndDate,
                                 AdminNotes = o.AdminNotes,
                                 IsActive = o.IsActive,
                                 StartTime = o.StartTime,
                                 EndTime = o.EndTime,
                                 Id = o.Id
                             }
                         });

            var discountCodeGeneratorListDtos = await query.ToListAsync();

            return _discountCodeGeneratorsExcelExporter.ExportToFile(discountCodeGeneratorListDtos);
        }

    }
}