using System.Collections.Generic;
using SoftGrid.DiscountManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.DiscountManagement.Exporting
{
    public interface IDiscountCodeGeneratorsExcelExporter
    {
        FileDto ExportToFile(List<GetDiscountCodeGeneratorForViewDto> discountCodeGenerators);
    }
}