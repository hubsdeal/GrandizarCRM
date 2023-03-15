using System.Collections.Generic;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CRM.Exporting
{
    public interface IBusinessesExcelExporter
    {
        FileDto ExportToFile(List<GetBusinessForViewDto> businesses);
    }
}