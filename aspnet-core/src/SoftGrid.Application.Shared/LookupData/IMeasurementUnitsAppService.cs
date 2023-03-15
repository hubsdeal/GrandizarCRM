using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData
{
    public interface IMeasurementUnitsAppService : IApplicationService
    {
        Task<PagedResultDto<GetMeasurementUnitForViewDto>> GetAll(GetAllMeasurementUnitsInput input);

        Task<GetMeasurementUnitForViewDto> GetMeasurementUnitForView(long id);

        Task<GetMeasurementUnitForEditOutput> GetMeasurementUnitForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditMeasurementUnitDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetMeasurementUnitsToExcel(GetAllMeasurementUnitsForExcelInput input);

    }
}