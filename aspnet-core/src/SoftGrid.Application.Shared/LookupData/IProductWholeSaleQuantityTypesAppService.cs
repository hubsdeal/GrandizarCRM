using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData
{
    public interface IProductWholeSaleQuantityTypesAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductWholeSaleQuantityTypeForViewDto>> GetAll(GetAllProductWholeSaleQuantityTypesInput input);

        Task<GetProductWholeSaleQuantityTypeForViewDto> GetProductWholeSaleQuantityTypeForView(long id);

        Task<GetProductWholeSaleQuantityTypeForEditOutput> GetProductWholeSaleQuantityTypeForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditProductWholeSaleQuantityTypeDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetProductWholeSaleQuantityTypesToExcel(GetAllProductWholeSaleQuantityTypesForExcelInput input);

    }
}