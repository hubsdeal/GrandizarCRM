using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IStoreTagSettingCategoriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetStoreTagSettingCategoryForViewDto>> GetAll(GetAllStoreTagSettingCategoriesInput input);

        Task<GetStoreTagSettingCategoryForViewDto> GetStoreTagSettingCategoryForView(long id);

        Task<GetStoreTagSettingCategoryForEditOutput> GetStoreTagSettingCategoryForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditStoreTagSettingCategoryDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetStoreTagSettingCategoriesToExcel(GetAllStoreTagSettingCategoriesForExcelInput input);

    }
}