using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductNotesExcelExporter : NpoiExcelExporterBase, IProductNotesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductNotesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductNoteForViewDto> productNotes)
        {
            return CreateExcelPackage(
                "ProductNotes.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductNotes"));

                    AddHeader(
                        sheet,
                        L("Notes"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, productNotes,
                        _ => _.ProductNote.Notes,
                        _ => _.ProductName
                        );

                });
        }
    }
}