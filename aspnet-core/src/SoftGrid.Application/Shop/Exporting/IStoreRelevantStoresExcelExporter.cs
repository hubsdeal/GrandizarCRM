using System.Collections.Generic;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop.Exporting
{
    public interface IStoreRelevantStoresExcelExporter
    {
        FileDto ExportToFile(List<GetStoreRelevantStoreForViewDto> storeRelevantStores);
    }
}