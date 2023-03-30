using System.Collections.Generic;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop.Exporting
{
    public interface IProductWholeSalePricesExcelExporter
    {
        FileDto ExportToFile(List<GetProductWholeSalePriceForViewDto> productWholeSalePrices);
    }
}