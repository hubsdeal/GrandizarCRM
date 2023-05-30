using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.WidgetManagement.Exporting
{
    public class HubWidgetMapsExcelExporter : NpoiExcelExporterBase, IHubWidgetMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HubWidgetMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHubWidgetMapForViewDto> hubWidgetMaps)
        {
            return CreateExcelPackage(
                    "HubWidgetMaps.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("HubWidgetMaps"));

                        AddHeader(
                            sheet,
                        L("CustomName"),
                        L("DisplaySequence"),
                        L("WidgetTypeId"),
                        (L("Hub")) + L("Name"),
                        (L("MasterWidget")) + L("Name")
                            );

                        AddObjects(
                            sheet, hubWidgetMaps,
                        _ => _.HubWidgetMap.CustomName,
                        _ => _.HubWidgetMap.DisplaySequence,
                        _ => _.HubWidgetMap.WidgetTypeId,
                        _ => _.HubName,
                        _ => _.MasterWidgetName
                            );

                    });

        }
    }
}