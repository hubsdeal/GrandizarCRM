using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.SalesLeadManagement.Exporting
{
    public class LeadNotesExcelExporter : NpoiExcelExporterBase, ILeadNotesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LeadNotesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLeadNoteForViewDto> leadNotes)
        {
            return CreateExcelPackage(
                "LeadNotes.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("LeadNotes"));

                    AddHeader(
                        sheet,
                        L("Notes"),
                        (L("Lead")) + L("Title")
                        );

                    AddObjects(
                        sheet, leadNotes,
                        _ => _.LeadNote.Notes,
                        _ => _.LeadTitle
                        );

                });
        }
    }
}