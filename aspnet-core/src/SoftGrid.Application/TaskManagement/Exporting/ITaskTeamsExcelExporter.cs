using System.Collections.Generic;
using SoftGrid.TaskManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.TaskManagement.Exporting
{
    public interface ITaskTeamsExcelExporter
    {
        FileDto ExportToFile(List<GetTaskTeamForViewDto> taskTeams);
    }
}