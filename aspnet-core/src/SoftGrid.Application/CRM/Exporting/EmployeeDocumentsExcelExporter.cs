using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.CRM.Exporting
{
    public class EmployeeDocumentsExcelExporter : NpoiExcelExporterBase, IEmployeeDocumentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeeDocumentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeeDocumentForViewDto> employeeDocuments)
        {
            return CreateExcelPackage(
                    "EmployeeDocuments.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("EmployeeDocuments"));

                        AddHeader(
                            sheet,
                        L("DocumentTitle"),
                        L("FileBinaryObjectId"),
                        (L("Employee")) + L("Name"),
                        (L("DocumentType")) + L("Name")
                            );

                        AddObjects(
                            sheet, employeeDocuments,
                        _ => _.EmployeeDocument.DocumentTitle,
                        _ => _.EmployeeDocument.FileBinaryObjectId,
                        _ => _.EmployeeName,
                        _ => _.DocumentTypeName
                            );

                    });

        }
    }
}