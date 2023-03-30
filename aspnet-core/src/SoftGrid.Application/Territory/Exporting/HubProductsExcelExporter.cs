using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Territory.Exporting
{
    public class HubProductsExcelExporter : NpoiExcelExporterBase, IHubProductsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HubProductsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHubProductForViewDto> hubProducts)
        {
            return CreateExcelPackage(
                "HubProducts.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("HubProducts"));

                    AddHeader(
                        sheet,
                        L("Published"),
                        L("DisplayScore"),
                        (L("Hub")) + L("Name"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, hubProducts,
                        _ => _.HubProduct.Published,
                        _ => _.HubProduct.DisplayScore,
                        _ => _.HubName,
                        _ => _.ProductName
                        );

                });
        }
    }
}