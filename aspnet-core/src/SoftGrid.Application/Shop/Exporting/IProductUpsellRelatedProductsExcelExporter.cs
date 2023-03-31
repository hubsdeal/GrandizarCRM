using System.Collections.Generic;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop.Exporting
{
    public interface IProductUpsellRelatedProductsExcelExporter
    {
        FileDto ExportToFile(List<GetProductUpsellRelatedProductForViewDto> productUpsellRelatedProducts);
    }
}