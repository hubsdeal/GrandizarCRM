using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductSubscriptionMapsExcelExporter : NpoiExcelExporterBase, IProductSubscriptionMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductSubscriptionMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductSubscriptionMapForViewDto> productSubscriptionMaps)
        {
            return CreateExcelPackage(
                "ProductSubscriptionMaps.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductSubscriptionMaps"));

                    AddHeader(
                        sheet,
                        L("DiscountPercentage"),
                        L("DiscountAmount"),
                        L("Price"),
                        (L("Product")) + L("Name"),
                        (L("SubscriptionType")) + L("Name")
                        );

                    AddObjects(
                        sheet, productSubscriptionMaps,
                        _ => _.ProductSubscriptionMap.DiscountPercentage,
                        _ => _.ProductSubscriptionMap.DiscountAmount,
                        _ => _.ProductSubscriptionMap.Price,
                        _ => _.ProductName,
                        _ => _.SubscriptionTypeName
                        );

                });
        }
    }
}