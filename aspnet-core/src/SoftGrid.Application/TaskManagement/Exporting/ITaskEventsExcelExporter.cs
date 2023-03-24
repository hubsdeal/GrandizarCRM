using System.Collections.Generic;
using SoftGrid.TaskManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.TaskManagement.Exporting
{
    public interface ITaskEventsExcelExporter
    {
        FileDto ExportToFile(List<GetTaskEventForViewDto> taskEvents);
    }
}