using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CRM
{
    public interface IBusinessTagsAppService : IApplicationService
    {
        Task<PagedResultDto<GetBusinessTagForViewDto>> GetAll(GetAllBusinessTagsInput input);

        Task<GetBusinessTagForViewDto> GetBusinessTagForView(long id);

        Task<GetBusinessTagForEditOutput> GetBusinessTagForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditBusinessTagDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetBusinessTagsToExcel(GetAllBusinessTagsForExcelInput input);

        Task<PagedResultDto<BusinessTagBusinessLookupTableDto>> GetAllBusinessForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<BusinessTagMasterTagCategoryLookupTableDto>> GetAllMasterTagCategoryForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<BusinessTagMasterTagLookupTableDto>> GetAllMasterTagForLookupTable(GetAllForLookupTableInput input);

    }
}