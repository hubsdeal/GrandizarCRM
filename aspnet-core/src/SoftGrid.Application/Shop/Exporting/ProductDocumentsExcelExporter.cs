using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductDocumentsExcelExporter : NpoiExcelExporterBase, IProductDocumentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductDocumentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductDocumentForViewDto> productDocuments)
        {
            return CreateExcelPackage(
                    "ProductDocuments.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("ProductDocuments"));

                        AddHeader(
                            sheet,
                        L("DocumentTitle"),
                        L("FileBinaryObjectId"),
                        (L("Product")) + L("Name"),
                        (L("DocumentType")) + L("Name")
                            );

                        AddObjects(
                            sheet, productDocuments,
                        _ => _.ProductDocument.DocumentTitle,
                        _ => _.ProductDocument.FileBinaryObjectId,
                        _ => _.ProductName,
                        _ => _.DocumentTypeName
                            );

                    });

        }
    }
}