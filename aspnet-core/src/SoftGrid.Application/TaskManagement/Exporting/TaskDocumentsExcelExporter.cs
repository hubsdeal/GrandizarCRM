using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.TaskManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.TaskManagement.Exporting
{
    public class TaskDocumentsExcelExporter : NpoiExcelExporterBase, ITaskDocumentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TaskDocumentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTaskDocumentForViewDto> taskDocuments)
        {
            return CreateExcelPackage(
                    "TaskDocuments.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("TaskDocuments"));

                        AddHeader(
                            sheet,
                        L("DocumentTitle"),
                        L("FileBinaryObjectId"),
                        (L("TaskEvent")) + L("Name"),
                        (L("DocumentType")) + L("Name")
                            );

                        AddObjects(
                            sheet, taskDocuments,
                        _ => _.TaskDocument.DocumentTitle,
                        _ => _.TaskDocument.FileBinaryObjectId,
                        _ => _.TaskEventName,
                        _ => _.DocumentTypeName
                            );

                    });

        }
    }
}