using System.Collections.Generic;
using SoftGrid.TaskManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.TaskManagement.Exporting
{
    public interface ITaskDocumentsExcelExporter
    {
        FileDto ExportToFile(List<GetTaskDocumentForViewDto> taskDocuments);
    }
}