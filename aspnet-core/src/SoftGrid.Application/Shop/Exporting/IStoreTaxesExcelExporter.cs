using System.Collections.Generic;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop.Exporting
{
    public interface IStoreTaxesExcelExporter
    {
        FileDto ExportToFile(List<GetStoreTaxForViewDto> storeTaxes);
    }
}