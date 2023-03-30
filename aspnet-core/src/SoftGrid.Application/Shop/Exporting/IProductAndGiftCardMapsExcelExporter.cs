using System.Collections.Generic;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop.Exporting
{
    public interface IProductAndGiftCardMapsExcelExporter
    {
        FileDto ExportToFile(List<GetProductAndGiftCardMapForViewDto> productAndGiftCardMaps);
    }
}