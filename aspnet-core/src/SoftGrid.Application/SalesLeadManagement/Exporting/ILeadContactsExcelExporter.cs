using System.Collections.Generic;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.SalesLeadManagement.Exporting
{
    public interface ILeadContactsExcelExporter
    {
        FileDto ExportToFile(List<GetLeadContactForViewDto> leadContacts);
    }
}