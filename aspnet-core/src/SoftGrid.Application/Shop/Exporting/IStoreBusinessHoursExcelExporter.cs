using System.Collections.Generic;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop.Exporting
{
    public interface IStoreBusinessHoursExcelExporter
    {
        FileDto ExportToFile(List<GetStoreBusinessHourForViewDto> storeBusinessHours);
    }
}