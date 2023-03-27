using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.SalesLeadManagement.Exporting
{
    public class LeadContactsExcelExporter : NpoiExcelExporterBase, ILeadContactsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LeadContactsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLeadContactForViewDto> leadContacts)
        {
            return CreateExcelPackage(
                "LeadContacts.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("LeadContacts"));

                    AddHeader(
                        sheet,
                        L("Notes"),
                        L("InfluenceScore"),
                        (L("Lead")) + L("Title"),
                        (L("Contact")) + L("FullName")
                        );

                    AddObjects(
                        sheet, leadContacts,
                        _ => _.LeadContact.Notes,
                        _ => _.LeadContact.InfluenceScore,
                        _ => _.LeadTitle,
                        _ => _.ContactFullName
                        );

                });
        }
    }
}