using System.Collections.Generic;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop.Exporting
{
    public interface IStoreLocationsExcelExporter
    {
        FileDto ExportToFile(List<GetStoreLocationForViewDto> storeLocations);
    }
}