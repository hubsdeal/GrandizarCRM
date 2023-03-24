using System.Collections.Generic;
using SoftGrid.TaskManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.TaskManagement.Exporting
{
    public interface ITaskTagsExcelExporter
    {
        FileDto ExportToFile(List<GetTaskTagForViewDto> taskTags);
    }
}