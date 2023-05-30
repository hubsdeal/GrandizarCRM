using System.Collections.Generic;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.WidgetManagement.Exporting
{
    public interface IStoreWidgetProductMapsExcelExporter
    {
        FileDto ExportToFile(List<GetStoreWidgetProductMapForViewDto> storeWidgetProductMaps);
    }
}