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
    [AbpAuthorize(AppPermissions.Pages_LeadSources)]
    public class LeadSourcesAppService : SoftGridAppServiceBase, ILeadSourcesAppService
    {
        private readonly IRepository<LeadSource, long> _leadSourceRepository;
        private readonly ILeadSourcesExcelExporter _leadSourcesExcelExporter;

        public LeadSourcesAppService(IRepository<LeadSource, long> leadSourceRepository, ILeadSourcesExcelExporter leadSourcesExcelExporter)
        {
            _leadSourceRepository = leadSourceRepository;
            _leadSourcesExcelExporter = leadSourcesExcelExporter;

        }

        public async Task<PagedResultDto<GetLeadSourceForViewDto>> GetAll(GetAllLeadSourcesInput input)
        {

            var filteredLeadSources = _leadSourceRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredLeadSources = filteredLeadSources
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var leadSources = from o in pagedAndFilteredLeadSources
                              select new
                              {

                                  o.Name,
                                  Id = o.Id
                              };

            var totalCount = await filteredLeadSources.CountAsync();

            var dbList = await leadSources.ToListAsync();
            var results = new List<GetLeadSourceForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetLeadSourceForViewDto()
                {
                    LeadSource = new LeadSourceDto
                    {

                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetLeadSourceForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetLeadSourceForViewDto> GetLeadSourceForView(long id)
        {
            var leadSource = await _leadSourceRepository.GetAsync(id);

            var output = new GetLeadSourceForViewDto { LeadSource = ObjectMapper.Map<LeadSourceDto>(leadSource) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_LeadSources_Edit)]
        public async Task<GetLeadSourceForEditOutput> GetLeadSourceForEdit(EntityDto<long> input)
        {
            var leadSource = await _leadSourceRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLeadSourceForEditOutput { LeadSource = ObjectMapper.Map<CreateOrEditLeadSourceDto>(leadSource) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditLeadSourceDto input)
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

        [AbpAuthorize(AppPermissions.Pages_LeadSources_Create)]
        protected virtual async Task Create(CreateOrEditLeadSourceDto input)
        {
            var leadSource = ObjectMapper.Map<LeadSource>(input);

            if (AbpSession.TenantId != null)
            {
                leadSource.TenantId = (int?)AbpSession.TenantId;
            }

            await _leadSourceRepository.InsertAsync(leadSource);

        }

        [AbpAuthorize(AppPermissions.Pages_LeadSources_Edit)]
        protected virtual async Task Update(CreateOrEditLeadSourceDto input)
        {
            var leadSource = await _leadSourceRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, leadSource);

        }

        [AbpAuthorize(AppPermissions.Pages_LeadSources_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _leadSourceRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetLeadSourcesToExcel(GetAllLeadSourcesForExcelInput input)
        {

            var filteredLeadSources = _leadSourceRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredLeadSources
                         select new GetLeadSourceForViewDto()
                         {
                             LeadSource = new LeadSourceDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var leadSourceListDtos = await query.ToListAsync();

            return _leadSourcesExcelExporter.ExportToFile(leadSourceListDtos);
        }

    }
}