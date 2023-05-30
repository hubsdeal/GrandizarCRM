using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.WidgetManagement.Exporting
{
    public class HubWidgetContentMapsExcelExporter : NpoiExcelExporterBase, IHubWidgetContentMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HubWidgetContentMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHubWidgetContentMapForViewDto> hubWidgetContentMaps)
        {
            return CreateExcelPackage(
                    "HubWidgetContentMaps.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("HubWidgetContentMaps"));

                        AddHeader(
                            sheet,
                        L("DisplaySequence"),
                        (L("HubWidgetMap")) + L("CustomName"),
                        (L("Content")) + L("Title")
                            );

                        AddObjects(
                            sheet, hubWidgetContentMaps,
                        _ => _.HubWidgetContentMap.DisplaySequence,
                        _ => _.HubWidgetMapCustomName,
                        _ => _.ContentTitle
                            );

                    });

        }
    }
}