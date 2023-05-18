using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Territory.Exporting
{
    public class HubDocumentsExcelExporter : NpoiExcelExporterBase, IHubDocumentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HubDocumentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHubDocumentForViewDto> hubDocuments)
        {
            return CreateExcelPackage(
                    "HubDocuments.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("HubDocuments"));

                        AddHeader(
                            sheet,
                        L("DocumentTitle"),
                        L("FileBinaryObjectId"),
                        (L("Hub")) + L("Name"),
                        (L("DocumentType")) + L("Name")
                            );

                        AddObjects(
                            sheet, hubDocuments,
                        _ => _.HubDocument.DocumentTitle,
                        _ => _.HubDocument.FileBinaryObjectId,
                        _ => _.HubName,
                        _ => _.DocumentTypeName
                            );

                    });

        }
    }
}