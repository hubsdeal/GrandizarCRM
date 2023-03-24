using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoreProductMapsExcelExporter : NpoiExcelExporterBase, IStoreProductMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreProductMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreProductMapForViewDto> storeProductMaps)
        {
            return CreateExcelPackage(
                "StoreProductMaps.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("StoreProductMaps"));

                    AddHeader(
                        sheet,
                        L("Published"),
                        L("DisplaySequence"),
                        (L("Store")) + L("Name"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, storeProductMaps,
                        _ => _.StoreProductMap.Published,
                        _ => _.StoreProductMap.DisplaySequence,
                        _ => _.StoreName,
                        _ => _.ProductName
                        );

                });
        }
    }
}