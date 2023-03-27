using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.SalesLeadManagement.Exporting;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.SalesLeadManagement
{
    [AbpAuthorize(AppPermissions.Pages_LeadPipelineStages)]
    public class LeadPipelineStagesAppService : SoftGridAppServiceBase, ILeadPipelineStagesAppService
    {
        private readonly IRepository<LeadPipelineStage, long> _leadPipelineStageRepository;
        private readonly ILeadPipelineStagesExcelExporter _leadPipelineStagesExcelExporter;

        public LeadPipelineStagesAppService(IRepository<LeadPipelineStage, long> leadPipelineStageRepository, ILeadPipelineStagesExcelExporter leadPipelineStagesExcelExporter)
        {
            _leadPipelineStageRepository = leadPipelineStageRepository;
            _leadPipelineStagesExcelExporter = leadPipelineStagesExcelExporter;

        }

        public async Task<PagedResultDto<GetLeadPipelineStageForViewDto>> GetAll(GetAllLeadPipelineStagesInput input)
        {

            var filteredLeadPipelineStages = _leadPipelineStageRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinStageOrderFilter != null, e => e.StageOrder >= input.MinStageOrderFilter)
                        .WhereIf(input.MaxStageOrderFilter != null, e => e.StageOrder <= input.MaxStageOrderFilter);

            var pagedAndFilteredLeadPipelineStages = filteredLeadPipelineStages
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var leadPipelineStages = from o in pagedAndFilteredLeadPipelineStages
                                     select new
                                     {

                                         o.Name,
                                         o.StageOrder,
                                         Id = o.Id
                                     };

            var totalCount = await filteredLeadPipelineStages.CountAsync();

            var dbList = await leadPipelineStages.ToListAsync();
            var results = new List<GetLeadPipelineStageForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetLeadPipelineStageForViewDto()
                {
                    LeadPipelineStage = new LeadPipelineStageDto
                    {

                        Name = o.Name,
                        StageOrder = o.StageOrder,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetLeadPipelineStageForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetLeadPipelineStageForViewDto> GetLeadPipelineStageForView(long id)
        {
            var leadPipelineStage = await _leadPipelineStageRepository.GetAsync(id);

            var output = new GetLeadPipelineStageForViewDto { LeadPipelineStage = ObjectMapper.Map<LeadPipelineStageDto>(leadPipelineStage) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_LeadPipelineStages_Edit)]
        public async Task<GetLeadPipelineStageForEditOutput> GetLeadPipelineStageForEdit(EntityDto<long> input)
        {
            var leadPipelineStage = await _leadPipelineStageRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLeadPipelineStageForEditOutput { LeadPipelineStage = ObjectMapper.Map<CreateOrEditLeadPipelineStageDto>(leadPipelineStage) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditLeadPipelineStageDto input)
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

        [AbpAuthorize(AppPermissions.Pages_LeadPipelineStages_Create)]
        protected virtual async Task Create(CreateOrEditLeadPipelineStageDto input)
        {
            var leadPipelineStage = ObjectMapper.Map<LeadPipelineStage>(input);

            if (AbpSession.TenantId != null)
            {
                leadPipelineStage.TenantId = (int?)AbpSession.TenantId;
            }

            await _leadPipelineStageRepository.InsertAsync(leadPipelineStage);

        }

        [AbpAuthorize(AppPermissions.Pages_LeadPipelineStages_Edit)]
        protected virtual async Task Update(CreateOrEditLeadPipelineStageDto input)
        {
            var leadPipelineStage = await _leadPipelineStageRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, leadPipelineStage);

        }

        [AbpAuthorize(AppPermissions.Pages_LeadPipelineStages_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _leadPipelineStageRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetLeadPipelineStagesToExcel(GetAllLeadPipelineStagesForExcelInput input)
        {

            var filteredLeadPipelineStages = _leadPipelineStageRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinStageOrderFilter != null, e => e.StageOrder >= input.MinStageOrderFilter)
                        .WhereIf(input.MaxStageOrderFilter != null, e => e.StageOrder <= input.MaxStageOrderFilter);

            var query = (from o in filteredLeadPipelineStages
                         select new GetLeadPipelineStageForViewDto()
                         {
                             LeadPipelineStage = new LeadPipelineStageDto
                             {
                                 Name = o.Name,
                                 StageOrder = o.StageOrder,
                                 Id = o.Id
                             }
                         });

            var leadPipelineStageListDtos = await query.ToListAsync();

            return _leadPipelineStagesExcelExporter.ExportToFile(leadPipelineStageListDtos);
        }

    }
}