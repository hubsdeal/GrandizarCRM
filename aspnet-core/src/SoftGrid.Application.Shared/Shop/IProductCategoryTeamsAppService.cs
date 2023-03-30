using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop
{
    public interface IProductCategoryTeamsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductCategoryTeamForViewDto>> GetAll(GetAllProductCategoryTeamsInput input);

        Task<GetProductCategoryTeamForViewDto> GetProductCategoryTeamForView(long id);

        Task<GetProductCategoryTeamForEditOutput> GetProductCategoryTeamForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductCategoryTeamDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductCategoryTeamsToExcel(GetAllProductCategoryTeamsForExcelInput input);

        Task<PagedResultDto<ProductCategoryTeamProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<ProductCategoryTeamEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input);

    }
}