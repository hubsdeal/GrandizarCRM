using System.Collections.Generic;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.OrderManagement.Exporting
{
    public interface IOrderStatusesExcelExporter
    {
        FileDto ExportToFile(List<GetOrderStatusForViewDto> orderStatuses);
    }
}