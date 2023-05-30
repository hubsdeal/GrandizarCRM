using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.WidgetManagement.Exporting
{
    public class StoreWidgetProductMapsExcelExporter : NpoiExcelExporterBase, IStoreWidgetProductMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreWidgetProductMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreWidgetProductMapForViewDto> storeWidgetProductMaps)
        {
            return CreateExcelPackage(
                    "StoreWidgetProductMaps.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("StoreWidgetProductMaps"));

                        AddHeader(
                            sheet,
                        L("DisplaySequence"),
                        (L("StoreWidgetMap")) + L("CustomName"),
                        (L("Product")) + L("Name")
                            );

                        AddObjects(
                            sheet, storeWidgetProductMaps,
                        _ => _.StoreWidgetProductMap.DisplaySequence,
                        _ => _.StoreWidgetMapCustomName,
                        _ => _.ProductName
                            );

                    });

        }
    }
}