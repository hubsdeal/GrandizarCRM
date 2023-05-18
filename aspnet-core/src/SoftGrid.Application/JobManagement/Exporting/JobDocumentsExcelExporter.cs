using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.JobManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.JobManagement.Exporting
{
    public class JobDocumentsExcelExporter : NpoiExcelExporterBase, IJobDocumentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public JobDocumentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetJobDocumentForViewDto> jobDocuments)
        {
            return CreateExcelPackage(
                    "JobDocuments.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("JobDocuments"));

                        AddHeader(
                            sheet,
                        L("DocumentTitle"),
                        L("FileBinaryObjectId"),
                        (L("Job")) + L("Title"),
                        (L("DocumentType")) + L("Name")
                            );

                        AddObjects(
                            sheet, jobDocuments,
                        _ => _.JobDocument.DocumentTitle,
                        _ => _.JobDocument.FileBinaryObjectId,
                        _ => _.JobTitle,
                        _ => _.DocumentTypeName
                            );

                    });

        }
    }
}