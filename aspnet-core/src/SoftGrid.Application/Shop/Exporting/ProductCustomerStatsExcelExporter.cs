using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductCustomerStatsExcelExporter : NpoiExcelExporterBase, IProductCustomerStatsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductCustomerStatsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductCustomerStatForViewDto> productCustomerStats)
        {
            return CreateExcelPackage(
                "ProductCustomerStats.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductCustomerStats"));

                    AddHeader(
                        sheet,
                        L("Clicked"),
                        L("WishedOrFavorite"),
                        L("Purchased"),
                        L("PurchasedQuantity"),
                        L("Shared"),
                        L("PageLink"),
                        L("AppOrWeb"),
                        L("QuitFromLink"),
                        (L("Product")) + L("Name"),
                        (L("Contact")) + L("FullName"),
                        (L("Store")) + L("Name"),
                        (L("Hub")) + L("Name"),
                        (L("SocialMedia")) + L("Name")
                        );

                    AddObjects(
                        sheet, productCustomerStats,
                        _ => _.ProductCustomerStat.Clicked,
                        _ => _.ProductCustomerStat.WishedOrFavorite,
                        _ => _.ProductCustomerStat.Purchased,
                        _ => _.ProductCustomerStat.PurchasedQuantity,
                        _ => _.ProductCustomerStat.Shared,
                        _ => _.ProductCustomerStat.PageLink,
                        _ => _.ProductCustomerStat.AppOrWeb,
                        _ => _.ProductCustomerStat.QuitFromLink,
                        _ => _.ProductName,
                        _ => _.ContactFullName,
                        _ => _.StoreName,
                        _ => _.HubName,
                        _ => _.SocialMediaName
                        );

                });
        }
    }
}