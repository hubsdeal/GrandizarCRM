using System.Collections.Generic;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.SalesLeadManagement.Exporting
{
    public interface ILeadTagsExcelExporter
    {
        FileDto ExportToFile(List<GetLeadTagForViewDto> leadTags);
    }
}