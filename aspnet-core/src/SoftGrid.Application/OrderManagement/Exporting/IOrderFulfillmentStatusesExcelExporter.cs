using System.Collections.Generic;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.OrderManagement.Exporting
{
    public interface IOrderFulfillmentStatusesExcelExporter
    {
        FileDto ExportToFile(List<GetOrderFulfillmentStatusForViewDto> orderFulfillmentStatuses);
    }
}