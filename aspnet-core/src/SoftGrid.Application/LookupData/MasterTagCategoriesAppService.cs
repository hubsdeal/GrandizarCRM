using SoftGrid.LookupData;

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
    [AbpAuthorize(AppPermissions.Pages_MasterTagCategories)]
    public class MasterTagCategoriesAppService : SoftGridAppServiceBase, IMasterTagCategoriesAppService
    {
        private readonly IRepository<MasterTagCategory, long> _masterTagCategoryRepository;
        private readonly IMasterTagCategoriesExcelExporter _masterTagCategoriesExcelExporter;
        private readonly IRepository<MediaLibrary, long> _lookup_mediaLibraryRepository;

        public MasterTagCategoriesAppService(IRepository<MasterTagCategory, long> masterTagCategoryRepository, IMasterTagCategoriesExcelExporter masterTagCategoriesExcelExporter, IRepository<MediaLibrary, long> lookup_mediaLibraryRepository)
        {
            _masterTagCategoryRepository = masterTagCategoryRepository;
            _masterTagCategoriesExcelExporter = masterTagCategoriesExcelExporter;
            _lookup_mediaLibraryRepository = lookup_mediaLibraryRepository;

        }

        public async Task<PagedResultDto<GetMasterTagCategoryForViewDto>> GetAll(GetAllMasterTagCategoriesInput input)
        {

            var filteredMasterTagCategories = _masterTagCategoryRepository.GetAll()
                        .Include(e => e.PictureMediaLibraryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.PictureMediaLibraryFk != null && e.PictureMediaLibraryFk.Name == input.MediaLibraryNameFilter);

            var pagedAndFilteredMasterTagCategories = filteredMasterTagCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var masterTagCategories = from o in pagedAndFilteredMasterTagCategories
                                      join o1 in _lookup_mediaLibraryRepository.GetAll() on o.PictureMediaLibraryId equals o1.Id into j1
                                      from s1 in j1.DefaultIfEmpty()

                                      select new
                                      {

                                          o.Name,
                                          o.Description,
                                          Id = o.Id,
                                          MediaLibraryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                      };

            var totalCount = await filteredMasterTagCategories.CountAsync();

            var dbList = await masterTagCategories.ToListAsync();
            var results = new List<GetMasterTagCategoryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetMasterTagCategoryForViewDto()
                {
                    MasterTagCategory = new MasterTagCategoryDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Id = o.Id,
                    },
                    MediaLibraryName = o.MediaLibraryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetMasterTagCategoryForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetMasterTagCategoryForViewDto> GetMasterTagCategoryForView(long id)
        {
            var masterTagCategory = await _masterTagCategoryRepository.GetAsync(id);

            var output = new GetMasterTagCategoryForViewDto { MasterTagCategory = ObjectMapper.Map<MasterTagCategoryDto>(masterTagCategory) };

            if (output.MasterTagCategory.PictureMediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.MasterTagCategory.PictureMediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MasterTagCategories_Edit)]
        public async Task<GetMasterTagCategoryForEditOutput> GetMasterTagCategoryForEdit(EntityDto<long> input)
        {
            var masterTagCategory = await _masterTagCategoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMasterTagCategoryForEditOutput { MasterTagCategory = ObjectMapper.Map<CreateOrEditMasterTagCategoryDto>(masterTagCategory) };

            if (output.MasterTagCategory.PictureMediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.MasterTagCategory.PictureMediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMasterTagCategoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_MasterTagCategories_Create)]
        protected virtual async Task Create(CreateOrEditMasterTagCategoryDto input)
        {
            var masterTagCategory = ObjectMapper.Map<MasterTagCategory>(input);

            if (AbpSession.TenantId != null)
            {
                masterTagCategory.TenantId = (int?)AbpSession.TenantId;
            }

            await _masterTagCategoryRepository.InsertAsync(masterTagCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_MasterTagCategories_Edit)]
        protected virtual async Task Update(CreateOrEditMasterTagCategoryDto input)
        {
            var masterTagCategory = await _masterTagCategoryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, masterTagCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_MasterTagCategories_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _masterTagCategoryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMasterTagCategoriesToExcel(GetAllMasterTagCategoriesForExcelInput input)
        {

            var filteredMasterTagCategories = _masterTagCategoryRepository.GetAll()
                        .Include(e => e.PictureMediaLibraryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.PictureMediaLibraryFk != null && e.PictureMediaLibraryFk.Name == input.MediaLibraryNameFilter);

            var query = (from o in filteredMasterTagCategories
                         join o1 in _lookup_mediaLibraryRepository.GetAll() on o.PictureMediaLibraryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetMasterTagCategoryForViewDto()
                         {
                             MasterTagCategory = new MasterTagCategoryDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Id = o.Id
                             },
                             MediaLibraryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var masterTagCategoryListDtos = await query.ToListAsync();

            return _masterTagCategoriesExcelExporter.ExportToFile(masterTagCategoryListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_MasterTagCategories)]
        public async Task<PagedResultDto<MasterTagCategoryMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_mediaLibraryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var mediaLibraryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<MasterTagCategoryMediaLibraryLookupTableDto>();
            foreach (var mediaLibrary in mediaLibraryList)
            {
                lookupTableDtoList.Add(new MasterTagCategoryMediaLibraryLookupTableDto
                {
                    Id = mediaLibrary.Id,
                    DisplayName = mediaLibrary.Name?.ToString()
                });
            }

            return new PagedResultDto<MasterTagCategoryMediaLibraryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}