using System.Collections.Generic;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData.Exporting
{
    public interface ICurrenciesExcelExporter
    {
        FileDto ExportToFile(List<GetCurrencyForViewDto> currencies);
    }
}