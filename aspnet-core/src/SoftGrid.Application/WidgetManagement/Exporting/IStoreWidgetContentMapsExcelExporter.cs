using System.Collections.Generic;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.WidgetManagement.Exporting
{
    public interface IStoreWidgetContentMapsExcelExporter
    {
        FileDto ExportToFile(List<GetStoreWidgetContentMapForViewDto> storeWidgetContentMaps);
    }
}