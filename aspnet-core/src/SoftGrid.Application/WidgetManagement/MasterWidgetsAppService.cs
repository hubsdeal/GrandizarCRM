using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.WidgetManagement.Exporting;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.WidgetManagement
{
    [AbpAuthorize(AppPermissions.Pages_MasterWidgets)]
    public class MasterWidgetsAppService : SoftGridAppServiceBase, IMasterWidgetsAppService
    {
        private readonly IRepository<MasterWidget, long> _masterWidgetRepository;
        private readonly IMasterWidgetsExcelExporter _masterWidgetsExcelExporter;

        public MasterWidgetsAppService(IRepository<MasterWidget, long> masterWidgetRepository, IMasterWidgetsExcelExporter masterWidgetsExcelExporter)
        {
            _masterWidgetRepository = masterWidgetRepository;
            _masterWidgetsExcelExporter = masterWidgetsExcelExporter;

        }

        public async Task<PagedResultDto<GetMasterWidgetForViewDto>> GetAll(GetAllMasterWidgetsInput input)
        {

            var filteredMasterWidgets = _masterWidgetRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.DesignCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DesignCodeFilter), e => e.DesignCode.Contains(input.DesignCodeFilter))
                        .WhereIf(input.PublishFilter.HasValue && input.PublishFilter > -1, e => (input.PublishFilter == 1 && e.Publish) || (input.PublishFilter == 0 && !e.Publish))
                        .WhereIf(input.MinInternalDisplayNumberFilter != null, e => e.InternalDisplayNumber >= input.MinInternalDisplayNumberFilter)
                        .WhereIf(input.MaxInternalDisplayNumberFilter != null, e => e.InternalDisplayNumber <= input.MaxInternalDisplayNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ThumbnailImageIdFilter.ToString()), e => e.ThumbnailImageId.ToString() == input.ThumbnailImageIdFilter.ToString());

            var pagedAndFilteredMasterWidgets = filteredMasterWidgets
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var masterWidgets = from o in pagedAndFilteredMasterWidgets
                                select new
                                {

                                    o.Name,
                                    o.Description,
                                    o.DesignCode,
                                    o.Publish,
                                    o.InternalDisplayNumber,
                                    o.ThumbnailImageId,
                                    Id = o.Id
                                };

            var totalCount = await filteredMasterWidgets.CountAsync();

            var dbList = await masterWidgets.ToListAsync();
            var results = new List<GetMasterWidgetForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetMasterWidgetForViewDto()
                {
                    MasterWidget = new MasterWidgetDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        DesignCode = o.DesignCode,
                        Publish = o.Publish,
                        InternalDisplayNumber = o.InternalDisplayNumber,
                        ThumbnailImageId = o.ThumbnailImageId,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetMasterWidgetForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetMasterWidgetForViewDto> GetMasterWidgetForView(long id)
        {
            var masterWidget = await _masterWidgetRepository.GetAsync(id);

            var output = new GetMasterWidgetForViewDto { MasterWidget = ObjectMapper.Map<MasterWidgetDto>(masterWidget) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MasterWidgets_Edit)]
        public async Task<GetMasterWidgetForEditOutput> GetMasterWidgetForEdit(EntityDto<long> input)
        {
            var masterWidget = await _masterWidgetRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMasterWidgetForEditOutput { MasterWidget = ObjectMapper.Map<CreateOrEditMasterWidgetDto>(masterWidget) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMasterWidgetDto input)
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

        [AbpAuthorize(AppPermissions.Pages_MasterWidgets_Create)]
        protected virtual async Task Create(CreateOrEditMasterWidgetDto input)
        {
            var masterWidget = ObjectMapper.Map<MasterWidget>(input);

            if (AbpSession.TenantId != null)
            {
                masterWidget.TenantId = (int?)AbpSession.TenantId;
            }

            await _masterWidgetRepository.InsertAsync(masterWidget);

        }

        [AbpAuthorize(AppPermissions.Pages_MasterWidgets_Edit)]
        protected virtual async Task Update(CreateOrEditMasterWidgetDto input)
        {
            var masterWidget = await _masterWidgetRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, masterWidget);

        }

        [AbpAuthorize(AppPermissions.Pages_MasterWidgets_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _masterWidgetRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMasterWidgetsToExcel(GetAllMasterWidgetsForExcelInput input)
        {

            var filteredMasterWidgets = _masterWidgetRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.DesignCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DesignCodeFilter), e => e.DesignCode.Contains(input.DesignCodeFilter))
                        .WhereIf(input.PublishFilter.HasValue && input.PublishFilter > -1, e => (input.PublishFilter == 1 && e.Publish) || (input.PublishFilter == 0 && !e.Publish))
                        .WhereIf(input.MinInternalDisplayNumberFilter != null, e => e.InternalDisplayNumber >= input.MinInternalDisplayNumberFilter)
                        .WhereIf(input.MaxInternalDisplayNumberFilter != null, e => e.InternalDisplayNumber <= input.MaxInternalDisplayNumberFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ThumbnailImageIdFilter.ToString()), e => e.ThumbnailImageId.ToString() == input.ThumbnailImageIdFilter.ToString());

            var query = (from o in filteredMasterWidgets
                         select new GetMasterWidgetForViewDto()
                         {
                             MasterWidget = new MasterWidgetDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 DesignCode = o.DesignCode,
                                 Publish = o.Publish,
                                 InternalDisplayNumber = o.InternalDisplayNumber,
                                 ThumbnailImageId = o.ThumbnailImageId,
                                 Id = o.Id
                             }
                         });

            var masterWidgetListDtos = await query.ToListAsync();

            return _masterWidgetsExcelExporter.ExportToFile(masterWidgetListDtos);
        }

    }
}