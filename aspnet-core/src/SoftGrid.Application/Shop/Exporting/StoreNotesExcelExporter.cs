using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoreNotesExcelExporter : NpoiExcelExporterBase, IStoreNotesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreNotesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreNoteForViewDto> storeNotes)
        {
            return CreateExcelPackage(
                "StoreNotes.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("StoreNotes"));

                    AddHeader(
                        sheet,
                        L("Notes"),
                        (L("Store")) + L("Name")
                        );

                    AddObjects(
                        sheet, storeNotes,
                        _ => _.StoreNote.Notes,
                        _ => _.StoreName
                        );

                });
        }
    }
}