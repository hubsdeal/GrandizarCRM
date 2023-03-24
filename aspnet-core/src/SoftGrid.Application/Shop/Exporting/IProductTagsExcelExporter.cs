using System.Collections.Generic;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop.Exporting
{
    public interface IProductTagsExcelExporter
    {
        FileDto ExportToFile(List<GetProductTagForViewDto> productTags);
    }
}