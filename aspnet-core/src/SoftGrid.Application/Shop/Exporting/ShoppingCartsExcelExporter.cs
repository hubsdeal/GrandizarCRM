using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ShoppingCartsExcelExporter : NpoiExcelExporterBase, IShoppingCartsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ShoppingCartsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetShoppingCartForViewDto> shoppingCarts)
        {
            return CreateExcelPackage(
                    "ShoppingCarts.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("ShoppingCarts"));

                        AddHeader(
                            sheet,
                        L("Quantity"),
                        L("UnitPrice"),
                        L("TotalAmount"),
                        L("UnitTotalPrice"),
                        L("UnitDiscountAmount"),
                        (L("Contact")) + L("FullName"),
                        (L("Order")) + L("InvoiceNumber"),
                        (L("Store")) + L("Name"),
                        (L("Product")) + L("Name"),
                        (L("Currency")) + L("Name")
                            );

                        AddObjects(
                            sheet, shoppingCarts,
                        _ => _.ShoppingCart.Quantity,
                        _ => _.ShoppingCart.UnitPrice,
                        _ => _.ShoppingCart.TotalAmount,
                        _ => _.ShoppingCart.UnitTotalPrice,
                        _ => _.ShoppingCart.UnitDiscountAmount,
                        _ => _.ContactFullName,
                        _ => _.OrderInvoiceNumber,
                        _ => _.StoreName,
                        _ => _.ProductName,
                        _ => _.CurrencyName
                            );

                    });

        }
    }
}