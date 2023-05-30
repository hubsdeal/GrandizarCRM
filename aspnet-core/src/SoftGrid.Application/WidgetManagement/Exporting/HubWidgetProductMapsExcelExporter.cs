using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.WidgetManagement.Exporting
{
    public class HubWidgetProductMapsExcelExporter : NpoiExcelExporterBase, IHubWidgetProductMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HubWidgetProductMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHubWidgetProductMapForViewDto> hubWidgetProductMaps)
        {
            return CreateExcelPackage(
                    "HubWidgetProductMaps.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("HubWidgetProductMaps"));

                        AddHeader(
                            sheet,
                        L("DisplaySequence"),
                        (L("HubWidgetMap")) + L("CustomName"),
                        (L("Product")) + L("Name")
                            );

                        AddObjects(
                            sheet, hubWidgetProductMaps,
                        _ => _.HubWidgetProductMap.DisplaySequence,
                        _ => _.HubWidgetMapCustomName,
                        _ => _.ProductName
                            );

                    });

        }
    }
}