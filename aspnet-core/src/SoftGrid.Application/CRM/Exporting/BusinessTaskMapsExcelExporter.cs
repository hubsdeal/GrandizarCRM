using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.CRM.Exporting
{
    public class BusinessTaskMapsExcelExporter : NpoiExcelExporterBase, IBusinessTaskMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BusinessTaskMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBusinessTaskMapForViewDto> businessTaskMaps)
        {
            return CreateExcelPackage(
                "BusinessTaskMaps.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("BusinessTaskMaps"));

                    AddHeader(
                        sheet,
                        (L("Business")) + L("Name"),
                        (L("TaskEvent")) + L("Name")
                        );

                    AddObjects(
                        sheet, businessTaskMaps,
                        _ => _.BusinessName,
                        _ => _.TaskEventName
                        );

                });
        }
    }
}