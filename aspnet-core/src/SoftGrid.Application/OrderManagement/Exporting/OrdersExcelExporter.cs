using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.OrderManagement.Exporting
{
    public class OrdersExcelExporter : NpoiExcelExporterBase, IOrdersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OrdersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOrderForViewDto> orders)
        {
            return CreateExcelPackage(
                "Orders.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Orders"));

                    AddHeader(
                        sheet,
                        L("InvoiceNumber"),
                        L("DeliveryOrPickup"),
                        L("PaymentCompleted"),
                        L("FullName"),
                        L("DeliveryAddress"),
                        L("City"),
                        L("ZipCode"),
                        L("Notes"),
                        L("DeliveryFee"),
                        L("SubTotalExcludedTax"),
                        L("TotalDiscountAmount"),
                        L("TotalTaxAmount"),
                        L("TotalAmount"),
                        L("Email"),
                        L("DiscountAmountByCode"),
                        L("GratuityAmount"),
                        L("GratuityPercentage"),
                        L("ServiceCharge"),
                        (L("State")) + L("Name"),
                        (L("Country")) + L("Name"),
                        (L("Contact")) + L("FullName"),
                        (L("OrderStatus")) + L("Name"),
                        (L("Currency")) + L("Name"),
                        (L("Store")) + L("Name"),
                        (L("OrderSalesChannel")) + L("Name")
                        );

                    AddObjects(
                        sheet, orders,
                        _ => _.Order.InvoiceNumber,
                        _ => _.Order.DeliveryOrPickup,
                        _ => _.Order.PaymentCompleted,
                        _ => _.Order.FullName,
                        _ => _.Order.DeliveryAddress,
                        _ => _.Order.City,
                        _ => _.Order.ZipCode,
                        _ => _.Order.Notes,
                        _ => _.Order.DeliveryFee,
                        _ => _.Order.SubTotalExcludedTax,
                        _ => _.Order.TotalDiscountAmount,
                        _ => _.Order.TotalTaxAmount,
                        _ => _.Order.TotalAmount,
                        _ => _.Order.Email,
                        _ => _.Order.DiscountAmountByCode,
                        _ => _.Order.GratuityAmount,
                        _ => _.Order.GratuityPercentage,
                        _ => _.Order.ServiceCharge,
                        _ => _.StateName,
                        _ => _.CountryName,
                        _ => _.ContactFullName,
                        _ => _.OrderStatusName,
                        _ => _.CurrencyName,
                        _ => _.StoreName,
                        _ => _.OrderSalesChannelName
                        );

                });
        }
    }
}