using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.TaskManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.TaskManagement.Exporting
{
    public class StoreTaskMapsExcelExporter : NpoiExcelExporterBase, IStoreTaskMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreTaskMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreTaskMapForViewDto> storeTaskMaps)
        {
            return CreateExcelPackage(
                    "StoreTaskMaps.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("StoreTaskMaps"));

                        AddHeader(
                            sheet,
                        (L("Store")) + L("Name"),
                        (L("TaskEvent")) + L("Name")
                            );

                        AddObjects(
                            sheet, storeTaskMaps,
                        _ => _.StoreName,
                        _ => _.TaskEventName
                            );

                    });

        }
    }
}