using System.Collections.Generic;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData.Exporting
{
    public interface IMeasurementUnitsExcelExporter
    {
        FileDto ExportToFile(List<GetMeasurementUnitForViewDto> measurementUnits);
    }
}