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
    [AbpAuthorize(AppPermissions.Pages_StoreMasterThemes)]
    public class StoreMasterThemesAppService : SoftGridAppServiceBase, IStoreMasterThemesAppService
    {
        private readonly IRepository<StoreMasterTheme, long> _storeMasterThemeRepository;
        private readonly IStoreMasterThemesExcelExporter _storeMasterThemesExcelExporter;

        public StoreMasterThemesAppService(IRepository<StoreMasterTheme, long> storeMasterThemeRepository, IStoreMasterThemesExcelExporter storeMasterThemesExcelExporter)
        {
            _storeMasterThemeRepository = storeMasterThemeRepository;
            _storeMasterThemesExcelExporter = storeMasterThemesExcelExporter;

        }

        public async Task<PagedResultDto<GetStoreMasterThemeForViewDto>> GetAll(GetAllStoreMasterThemesInput input)
        {

            var filteredStoreMasterThemes = _storeMasterThemeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.ThemeCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ThemeCodeFilter), e => e.ThemeCode.Contains(input.ThemeCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ThumbnailImageIdFilter.ToString()), e => e.ThumbnailImageId.ToString() == input.ThumbnailImageIdFilter.ToString());

            var pagedAndFilteredStoreMasterThemes = filteredStoreMasterThemes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeMasterThemes = from o in pagedAndFilteredStoreMasterThemes
                                    select new
                                    {

                                        o.Name,
                                        o.Description,
                                        o.ThemeCode,
                                        o.ThumbnailImageId,
                                        Id = o.Id
                                    };

            var totalCount = await filteredStoreMasterThemes.CountAsync();

            var dbList = await storeMasterThemes.ToListAsync();
            var results = new List<GetStoreMasterThemeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreMasterThemeForViewDto()
                {
                    StoreMasterTheme = new StoreMasterThemeDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        ThemeCode = o.ThemeCode,
                        ThumbnailImageId = o.ThumbnailImageId,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreMasterThemeForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreMasterThemeForViewDto> GetStoreMasterThemeForView(long id)
        {
            var storeMasterTheme = await _storeMasterThemeRepository.GetAsync(id);

            var output = new GetStoreMasterThemeForViewDto { StoreMasterTheme = ObjectMapper.Map<StoreMasterThemeDto>(storeMasterTheme) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreMasterThemes_Edit)]
        public async Task<GetStoreMasterThemeForEditOutput> GetStoreMasterThemeForEdit(EntityDto<long> input)
        {
            var storeMasterTheme = await _storeMasterThemeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreMasterThemeForEditOutput { StoreMasterTheme = ObjectMapper.Map<CreateOrEditStoreMasterThemeDto>(storeMasterTheme) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreMasterThemeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreMasterThemes_Create)]
        protected virtual async Task Create(CreateOrEditStoreMasterThemeDto input)
        {
            var storeMasterTheme = ObjectMapper.Map<StoreMasterTheme>(input);

            if (AbpSession.TenantId != null)
            {
                storeMasterTheme.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeMasterThemeRepository.InsertAsync(storeMasterTheme);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreMasterThemes_Edit)]
        protected virtual async Task Update(CreateOrEditStoreMasterThemeDto input)
        {
            var storeMasterTheme = await _storeMasterThemeRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeMasterTheme);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreMasterThemes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeMasterThemeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreMasterThemesToExcel(GetAllStoreMasterThemesForExcelInput input)
        {

            var filteredStoreMasterThemes = _storeMasterThemeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.ThemeCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ThemeCodeFilter), e => e.ThemeCode.Contains(input.ThemeCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ThumbnailImageIdFilter.ToString()), e => e.ThumbnailImageId.ToString() == input.ThumbnailImageIdFilter.ToString());

            var query = (from o in filteredStoreMasterThemes
                         select new GetStoreMasterThemeForViewDto()
                         {
                             StoreMasterTheme = new StoreMasterThemeDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 ThemeCode = o.ThemeCode,
                                 ThumbnailImageId = o.ThumbnailImageId,
                                 Id = o.Id
                             }
                         });

            var storeMasterThemeListDtos = await query.ToListAsync();

            return _storeMasterThemesExcelExporter.ExportToFile(storeMasterThemeListDtos);
        }

    }
}