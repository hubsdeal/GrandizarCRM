using System.Collections.Generic;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Territory.Exporting
{
    public interface IHubContactsExcelExporter
    {
        FileDto ExportToFile(List<GetHubContactForViewDto> hubContacts);
    }
}