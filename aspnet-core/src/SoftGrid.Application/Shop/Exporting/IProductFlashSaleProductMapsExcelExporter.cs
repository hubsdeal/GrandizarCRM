using System.Collections.Generic;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop.Exporting
{
    public interface IProductFlashSaleProductMapsExcelExporter
    {
        FileDto ExportToFile(List<GetProductFlashSaleProductMapForViewDto> productFlashSaleProductMaps);
    }
}