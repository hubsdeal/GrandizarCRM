using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.JobManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.JobManagement.Exporting
{
    public class JobStatusTypesExcelExporter : NpoiExcelExporterBase, IJobStatusTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public JobStatusTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetJobStatusTypeForViewDto> jobStatusTypes)
        {
            return CreateExcelPackage(
                "JobStatusTypes.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("JobStatusTypes"));

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, jobStatusTypes,
                        _ => _.JobStatusType.Name
                        );

                });
        }
    }
}