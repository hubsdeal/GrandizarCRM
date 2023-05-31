using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.WidgetManagement.Exporting
{
    public class HubWidgetStoreMapsExcelExporter : NpoiExcelExporterBase, IHubWidgetStoreMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HubWidgetStoreMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHubWidgetStoreMapForViewDto> hubWidgetStoreMaps)
        {
            return CreateExcelPackage(
                    "HubWidgetStoreMaps.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("HubWidgetStoreMaps"));

                        AddHeader(
                            sheet,
                        L("DisplaySequence"),
                        (L("HubWidgetMap")) + L("CustomName"),
                        (L("Store")) + L("Name")
                            );

                        AddObjects(
                            sheet, hubWidgetStoreMaps,
                        _ => _.HubWidgetStoreMap.DisplaySequence,
                        _ => _.HubWidgetMapCustomName,
                        _ => _.StoreName
                            );

                    });

        }
    }
}