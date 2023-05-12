using System.Collections.Generic;
using SoftGrid.TaskManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.TaskManagement.Exporting
{
    public interface IStoreTaskMapsExcelExporter
    {
        FileDto ExportToFile(List<GetStoreTaskMapForViewDto> storeTaskMaps);
    }
}