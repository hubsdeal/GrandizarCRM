using System.Collections.Generic;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop.Exporting
{
    public interface IProductVariantsExcelExporter
    {
        FileDto ExportToFile(List<GetProductVariantForViewDto> productVariants);
    }
}