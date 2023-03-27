using System.Collections.Generic;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop.Exporting
{
    public interface IStoreZipCodeMapsExcelExporter
    {
        FileDto ExportToFile(List<GetStoreZipCodeMapForViewDto> storeZipCodeMaps);
    }
}