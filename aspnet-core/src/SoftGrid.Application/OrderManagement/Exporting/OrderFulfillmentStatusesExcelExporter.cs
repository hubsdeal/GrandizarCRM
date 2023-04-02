using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.OrderManagement.Exporting
{
    public class OrderFulfillmentStatusesExcelExporter : NpoiExcelExporterBase, IOrderFulfillmentStatusesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OrderFulfillmentStatusesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOrderFulfillmentStatusForViewDto> orderFulfillmentStatuses)
        {
            return CreateExcelPackage(
                "OrderFulfillmentStatuses.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("OrderFulfillmentStatuses"));

                    AddHeader(
                        sheet,
                        L("EstimatedTime"),
                        L("ActualTime"),
                        (L("OrderStatus")) + L("Name"),
                        (L("Order")) + L("InvoiceNumber"),
                        (L("Employee")) + L("Name")
                        );

                    AddObjects(
                        sheet, orderFulfillmentStatuses,
                        _ => _timeZoneConverter.Convert(_.OrderFulfillmentStatus.EstimatedTime, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.OrderFulfillmentStatus.ActualTime, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.OrderStatusName,
                        _ => _.OrderInvoiceNumber,
                        _ => _.EmployeeName
                        );

                    for (var i = 1; i <= orderFulfillmentStatuses.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[1], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(1); for (var i = 1; i <= orderFulfillmentStatuses.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[2], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(2);
                });
        }
    }
}