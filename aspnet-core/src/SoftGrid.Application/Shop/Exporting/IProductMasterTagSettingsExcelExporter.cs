using System.Collections.Generic;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop.Exporting
{
    public interface IProductMasterTagSettingsExcelExporter
    {
        FileDto ExportToFile(List<GetProductMasterTagSettingForViewDto> productMasterTagSettings);
    }
}