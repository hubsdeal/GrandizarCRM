using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.CRM.Exporting
{
    public class BusinessDocumentsExcelExporter : NpoiExcelExporterBase, IBusinessDocumentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BusinessDocumentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBusinessDocumentForViewDto> businessDocuments)
        {
            return CreateExcelPackage(
                    "BusinessDocuments.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("BusinessDocuments"));

                        AddHeader(
                            sheet,
                        L("DocumentTitle"),
                        L("FileBinaryObjectId"),
                        (L("Business")) + L("Name"),
                        (L("DocumentType")) + L("Name")
                            );

                        AddObjects(
                            sheet, businessDocuments,
                        _ => _.BusinessDocument.DocumentTitle,
                        _ => _.BusinessDocument.FileBinaryObjectId,
                        _ => _.BusinessName,
                        _ => _.DocumentTypeName
                            );

                    });

        }
    }
}