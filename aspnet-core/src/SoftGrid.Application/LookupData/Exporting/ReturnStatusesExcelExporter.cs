using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.LookupData.Exporting
{
    public class ReturnStatusesExcelExporter : NpoiExcelExporterBase, IReturnStatusesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ReturnStatusesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetReturnStatusForViewDto> returnStatuses)
        {
            return CreateExcelPackage(
                "ReturnStatuses.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ReturnStatuses"));

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, returnStatuses,
                        _ => _.ReturnStatus.Name
                        );

                });
        }
    }
}