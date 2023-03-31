using System.Collections.Generic;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.OrderManagement.Exporting
{
    public interface IOrdersExcelExporter
    {
        FileDto ExportToFile(List<GetOrderForViewDto> orders);
    }
}