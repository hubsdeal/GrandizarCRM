using System.Collections.Generic;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Territory.Exporting
{
    public interface IHubSalesProjectionsExcelExporter
    {
        FileDto ExportToFile(List<GetHubSalesProjectionForViewDto> hubSalesProjections);
    }
}