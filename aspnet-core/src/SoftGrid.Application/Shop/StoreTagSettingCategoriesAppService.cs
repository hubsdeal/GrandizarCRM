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
    [AbpAuthorize(AppPermissions.Pages_StoreTagSettingCategories)]
    public class StoreTagSettingCategoriesAppService : SoftGridAppServiceBase, IStoreTagSettingCategoriesAppService
    {
        private readonly IRepository<StoreTagSettingCategory, long> _storeTagSettingCategoryRepository;
        private readonly IStoreTagSettingCategoriesExcelExporter _storeTagSettingCategoriesExcelExporter;

        public StoreTagSettingCategoriesAppService(IRepository<StoreTagSettingCategory, long> storeTagSettingCategoryRepository, IStoreTagSettingCategoriesExcelExporter storeTagSettingCategoriesExcelExporter)
        {
            _storeTagSettingCategoryRepository = storeTagSettingCategoryRepository;
            _storeTagSettingCategoriesExcelExporter = storeTagSettingCategoriesExcelExporter;

        }

        public async Task<PagedResultDto<GetStoreTagSettingCategoryForViewDto>> GetAll(GetAllStoreTagSettingCategoriesInput input)
        {

            var filteredStoreTagSettingCategories = _storeTagSettingCategoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ImageIdFilter.ToString()), e => e.ImageId.ToString() == input.ImageIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter));

            var pagedAndFilteredStoreTagSettingCategories = filteredStoreTagSettingCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeTagSettingCategories = from o in pagedAndFilteredStoreTagSettingCategories
                                            select new
                                            {

                                                o.Name,
                                                o.ImageId,
                                                o.Description,
                                                Id = o.Id
                                            };

            var totalCount = await filteredStoreTagSettingCategories.CountAsync();

            var dbList = await storeTagSettingCategories.ToListAsync();
            var results = new List<GetStoreTagSettingCategoryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreTagSettingCategoryForViewDto()
                {
                    StoreTagSettingCategory = new StoreTagSettingCategoryDto
                    {

                        Name = o.Name,
                        ImageId = o.ImageId,
                        Description = o.Description,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreTagSettingCategoryForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreTagSettingCategoryForViewDto> GetStoreTagSettingCategoryForView(long id)
        {
            var storeTagSettingCategory = await _storeTagSettingCategoryRepository.GetAsync(id);

            var output = new GetStoreTagSettingCategoryForViewDto { StoreTagSettingCategory = ObjectMapper.Map<StoreTagSettingCategoryDto>(storeTagSettingCategory) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreTagSettingCategories_Edit)]
        public async Task<GetStoreTagSettingCategoryForEditOutput> GetStoreTagSettingCategoryForEdit(EntityDto<long> input)
        {
            var storeTagSettingCategory = await _storeTagSettingCategoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreTagSettingCategoryForEditOutput { StoreTagSettingCategory = ObjectMapper.Map<CreateOrEditStoreTagSettingCategoryDto>(storeTagSettingCategory) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreTagSettingCategoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreTagSettingCategories_Create)]
        protected virtual async Task Create(CreateOrEditStoreTagSettingCategoryDto input)
        {
            var storeTagSettingCategory = ObjectMapper.Map<StoreTagSettingCategory>(input);

            if (AbpSession.TenantId != null)
            {
                storeTagSettingCategory.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeTagSettingCategoryRepository.InsertAsync(storeTagSettingCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreTagSettingCategories_Edit)]
        protected virtual async Task Update(CreateOrEditStoreTagSettingCategoryDto input)
        {
            var storeTagSettingCategory = await _storeTagSettingCategoryRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeTagSettingCategory);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreTagSettingCategories_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeTagSettingCategoryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreTagSettingCategoriesToExcel(GetAllStoreTagSettingCategoriesForExcelInput input)
        {

            var filteredStoreTagSettingCategories = _storeTagSettingCategoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ImageIdFilter.ToString()), e => e.ImageId.ToString() == input.ImageIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter));

            var query = (from o in filteredStoreTagSettingCategories
                         select new GetStoreTagSettingCategoryForViewDto()
                         {
                             StoreTagSettingCategory = new StoreTagSettingCategoryDto
                             {
                                 Name = o.Name,
                                 ImageId = o.ImageId,
                                 Description = o.Description,
                                 Id = o.Id
                             }
                         });

            var storeTagSettingCategoryListDtos = await query.ToListAsync();

            return _storeTagSettingCategoriesExcelExporter.ExportToFile(storeTagSettingCategoryListDtos);
        }

    }
}