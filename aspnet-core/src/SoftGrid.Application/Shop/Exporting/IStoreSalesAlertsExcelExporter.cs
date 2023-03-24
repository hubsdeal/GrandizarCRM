using System.Collections.Generic;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop.Exporting
{
    public interface IStoreSalesAlertsExcelExporter
    {
        FileDto ExportToFile(List<GetStoreSalesAlertForViewDto> storeSalesAlerts);
    }
}