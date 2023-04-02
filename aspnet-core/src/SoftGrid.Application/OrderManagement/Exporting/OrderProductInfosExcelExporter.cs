using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.OrderManagement.Exporting
{
    public class OrderProductInfosExcelExporter : NpoiExcelExporterBase, IOrderProductInfosExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OrderProductInfosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOrderProductInfoForViewDto> orderProductInfos)
        {
            return CreateExcelPackage(
                "OrderProductInfos.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("OrderProductInfos"));

                    AddHeader(
                        sheet,
                        L("Quantity"),
                        L("UnitPrice"),
                        L("ByProductDiscountAmount"),
                        L("ByProductDiscountPercentage"),
                        L("ByProductTaxAmount"),
                        L("ByProductTotalAmount"),
                        (L("Order")) + L("InvoiceNumber"),
                        (L("Store")) + L("Name"),
                        (L("Product")) + L("Name"),
                        (L("MeasurementUnit")) + L("Name")
                        );

                    AddObjects(
                        sheet, orderProductInfos,
                        _ => _.OrderProductInfo.Quantity,
                        _ => _.OrderProductInfo.UnitPrice,
                        _ => _.OrderProductInfo.ByProductDiscountAmount,
                        _ => _.OrderProductInfo.ByProductDiscountPercentage,
                        _ => _.OrderProductInfo.ByProductTaxAmount,
                        _ => _.OrderProductInfo.ByProductTotalAmount,
                        _ => _.OrderInvoiceNumber,
                        _ => _.StoreName,
                        _ => _.ProductName,
                        _ => _.MeasurementUnitName
                        );

                });
        }
    }
}