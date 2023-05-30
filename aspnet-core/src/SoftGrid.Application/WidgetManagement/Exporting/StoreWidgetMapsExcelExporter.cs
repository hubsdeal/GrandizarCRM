using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.WidgetManagement.Exporting
{
    public class StoreWidgetMapsExcelExporter : NpoiExcelExporterBase, IStoreWidgetMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreWidgetMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreWidgetMapForViewDto> storeWidgetMaps)
        {
            return CreateExcelPackage(
                    "StoreWidgetMaps.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("StoreWidgetMaps"));

                        AddHeader(
                            sheet,
                        L("DisplaySequence"),
                        L("WidgetTypeId"),
                        L("CustomName"),
                        (L("MasterWidget")) + L("Name"),
                        (L("Store")) + L("Name")
                            );

                        AddObjects(
                            sheet, storeWidgetMaps,
                        _ => _.StoreWidgetMap.DisplaySequence,
                        _ => _.StoreWidgetMap.WidgetTypeId,
                        _ => _.StoreWidgetMap.CustomName,
                        _ => _.MasterWidgetName,
                        _ => _.StoreName
                            );

                    });

        }
    }
}