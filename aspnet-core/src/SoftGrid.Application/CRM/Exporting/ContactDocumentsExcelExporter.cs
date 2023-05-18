using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.CRM.Exporting
{
    public class ContactDocumentsExcelExporter : NpoiExcelExporterBase, IContactDocumentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ContactDocumentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetContactDocumentForViewDto> contactDocuments)
        {
            return CreateExcelPackage(
                    "ContactDocuments.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("ContactDocuments"));

                        AddHeader(
                            sheet,
                        L("DocumentTitle"),
                        L("FileBinaryObjectId"),
                        (L("Contact")) + L("FullName"),
                        (L("DocumentType")) + L("Name")
                            );

                        AddObjects(
                            sheet, contactDocuments,
                        _ => _.ContactDocument.DocumentTitle,
                        _ => _.ContactDocument.FileBinaryObjectId,
                        _ => _.ContactFullName,
                        _ => _.DocumentTypeName
                            );

                    });

        }
    }
}