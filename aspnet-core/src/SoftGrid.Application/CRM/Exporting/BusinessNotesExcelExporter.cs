using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.CRM.Exporting
{
    public class BusinessNotesExcelExporter : NpoiExcelExporterBase, IBusinessNotesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BusinessNotesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBusinessNoteForViewDto> businessNotes)
        {
            return CreateExcelPackage(
                "BusinessNotes.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("BusinessNotes"));

                    AddHeader(
                        sheet,
                        L("Notes"),
                        (L("Business")) + L("Name")
                        );

                    AddObjects(
                        sheet, businessNotes,
                        _ => _.BusinessNote.Notes,
                        _ => _.BusinessName
                        );

                });
        }
    }
}