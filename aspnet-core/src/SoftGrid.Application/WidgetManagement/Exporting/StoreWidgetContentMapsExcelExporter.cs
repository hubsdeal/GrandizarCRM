using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.WidgetManagement.Exporting
{
    public class StoreWidgetContentMapsExcelExporter : NpoiExcelExporterBase, IStoreWidgetContentMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreWidgetContentMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreWidgetContentMapForViewDto> storeWidgetContentMaps)
        {
            return CreateExcelPackage(
                    "StoreWidgetContentMaps.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("StoreWidgetContentMaps"));

                        AddHeader(
                            sheet,
                        L("DisplaySequence"),
                        (L("StoreWidgetMap")) + L("CustomName"),
                        (L("Content")) + L("Title")
                            );

                        AddObjects(
                            sheet, storeWidgetContentMaps,
                        _ => _.StoreWidgetContentMap.DisplaySequence,
                        _ => _.StoreWidgetMapCustomName,
                        _ => _.ContentTitle
                            );

                    });

        }
    }
}