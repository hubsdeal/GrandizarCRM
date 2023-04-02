using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.OrderManagement.Exporting
{
    public class OrderDeliveryInfosExcelExporter : NpoiExcelExporterBase, IOrderDeliveryInfosExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OrderDeliveryInfosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOrderDeliveryInfoForViewDto> orderDeliveryInfos)
        {
            return CreateExcelPackage(
                "OrderDeliveryInfos.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("OrderDeliveryInfos"));

                    AddHeader(
                        sheet,
                        L("TrackingNumber"),
                        L("TotalWeight"),
                        L("DeliveryProviderId"),
                        L("DispatchDate"),
                        L("DispatchTime"),
                        L("DeliverToCustomerDate"),
                        L("DeliverToCustomerTime"),
                        L("DeliveryNotes"),
                        L("CustomerAcknowledged"),
                        L("CustomerSignature"),
                        L("CateringDate"),
                        L("CateringTime"),
                        L("DeliveryDate"),
                        L("DeliveryTime"),
                        L("DineInDate"),
                        L("DineInTime"),
                        L("IncludedChildren"),
                        L("IsAsap"),
                        L("IsPickupCatering"),
                        L("NumberOfGuests"),
                        L("PickupDate"),
                        L("PickupTime"),
                        (L("Employee")) + L("Name"),
                        (L("Order")) + L("InvoiceNumber")
                        );

                    AddObjects(
                        sheet, orderDeliveryInfos,
                        _ => _.OrderDeliveryInfo.TrackingNumber,
                        _ => _.OrderDeliveryInfo.TotalWeight,
                        _ => _.OrderDeliveryInfo.DeliveryProviderId,
                        _ => _timeZoneConverter.Convert(_.OrderDeliveryInfo.DispatchDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.OrderDeliveryInfo.DispatchTime,
                        _ => _timeZoneConverter.Convert(_.OrderDeliveryInfo.DeliverToCustomerDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.OrderDeliveryInfo.DeliverToCustomerTime,
                        _ => _.OrderDeliveryInfo.DeliveryNotes,
                        _ => _.OrderDeliveryInfo.CustomerAcknowledged,
                        _ => _.OrderDeliveryInfo.CustomerSignature,
                        _ => _timeZoneConverter.Convert(_.OrderDeliveryInfo.CateringDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.OrderDeliveryInfo.CateringTime,
                        _ => _timeZoneConverter.Convert(_.OrderDeliveryInfo.DeliveryDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.OrderDeliveryInfo.DeliveryTime,
                        _ => _timeZoneConverter.Convert(_.OrderDeliveryInfo.DineInDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.OrderDeliveryInfo.DineInTime,
                        _ => _.OrderDeliveryInfo.IncludedChildren,
                        _ => _.OrderDeliveryInfo.IsAsap,
                        _ => _.OrderDeliveryInfo.IsPickupCatering,
                        _ => _.OrderDeliveryInfo.NumberOfGuests,
                        _ => _timeZoneConverter.Convert(_.OrderDeliveryInfo.PickupDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.OrderDeliveryInfo.PickupTime,
                        _ => _.EmployeeName,
                        _ => _.OrderInvoiceNumber
                        );

                    for (var i = 1; i <= orderDeliveryInfos.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(4); for (var i = 1; i <= orderDeliveryInfos.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[6], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(6); for (var i = 1; i <= orderDeliveryInfos.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[11], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(11); for (var i = 1; i <= orderDeliveryInfos.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[13], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(13); for (var i = 1; i <= orderDeliveryInfos.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[15], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(15); for (var i = 1; i <= orderDeliveryInfos.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[21], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(21);
                });
        }
    }
}