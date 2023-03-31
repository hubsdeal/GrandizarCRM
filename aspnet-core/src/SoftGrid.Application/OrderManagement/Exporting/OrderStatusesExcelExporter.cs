using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.OrderManagement.Exporting
{
    public class OrderStatusesExcelExporter : NpoiExcelExporterBase, IOrderStatusesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OrderStatusesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOrderStatusForViewDto> orderStatuses)
        {
            return CreateExcelPackage(
                "OrderStatuses.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("OrderStatuses"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("SequenceNo"),
                        L("ColorCode"),
                        L("Message"),
                        L("DeliveryOrPickup"),
                        (L("Role")) + L("Name")
                        );

                    AddObjects(
                        sheet, orderStatuses,
                        _ => _.OrderStatus.Name,
                        _ => _.OrderStatus.Description,
                        _ => _.OrderStatus.SequenceNo,
                        _ => _.OrderStatus.ColorCode,
                        _ => _.OrderStatus.Message,
                        _ => _.OrderStatus.DeliveryOrPickup,
                        _ => _.RoleName
                        );

                });
        }
    }
}