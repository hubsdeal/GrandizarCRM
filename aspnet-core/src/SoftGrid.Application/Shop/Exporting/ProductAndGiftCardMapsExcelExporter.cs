using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductAndGiftCardMapsExcelExporter : NpoiExcelExporterBase, IProductAndGiftCardMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductAndGiftCardMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductAndGiftCardMapForViewDto> productAndGiftCardMaps)
        {
            return CreateExcelPackage(
                "ProductAndGiftCardMaps.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductAndGiftCardMaps"));

                    AddHeader(
                        sheet,
                        L("PurchaseAmount"),
                        L("GiftAmount"),
                        (L("Product")) + L("Name"),
                        (L("Currency")) + L("Name")
                        );

                    AddObjects(
                        sheet, productAndGiftCardMaps,
                        _ => _.ProductAndGiftCardMap.PurchaseAmount,
                        _ => _.ProductAndGiftCardMap.GiftAmount,
                        _ => _.ProductName,
                        _ => _.CurrencyName
                        );

                });
        }
    }
}