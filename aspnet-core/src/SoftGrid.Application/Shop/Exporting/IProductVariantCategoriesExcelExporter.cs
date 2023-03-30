using System.Collections.Generic;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop.Exporting
{
    public interface IProductVariantCategoriesExcelExporter
    {
        FileDto ExportToFile(List<GetProductVariantCategoryForViewDto> productVariantCategories);
    }
}