using System.Collections.Generic;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CRM.Exporting
{
    public interface IBusinessAccountTeamsExcelExporter
    {
        FileDto ExportToFile(List<GetBusinessAccountTeamForViewDto> businessAccountTeams);
    }
}