using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.JobManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.JobManagement.Exporting
{
    public class JobTaskMapsExcelExporter : NpoiExcelExporterBase, IJobTaskMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public JobTaskMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetJobTaskMapForViewDto> jobTaskMaps)
        {
            return CreateExcelPackage(
                    "JobTaskMaps.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("JobTaskMaps"));

                        AddHeader(
                            sheet,
                        (L("Job")) + L("Title"),
                        (L("TaskEvent")) + L("Name")
                            );

                        AddObjects(
                            sheet, jobTaskMaps,
                        _ => _.JobTitle,
                        _ => _.TaskEventName
                            );

                    });

        }
    }
}