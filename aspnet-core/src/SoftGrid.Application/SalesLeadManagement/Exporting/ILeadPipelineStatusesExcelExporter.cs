using System.Collections.Generic;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.SalesLeadManagement.Exporting
{
    public interface ILeadPipelineStatusesExcelExporter
    {
        FileDto ExportToFile(List<GetLeadPipelineStatusForViewDto> leadPipelineStatuses);
    }
}