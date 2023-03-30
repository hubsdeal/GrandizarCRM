using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Territory.Exporting
{
    public class HubContactsExcelExporter : NpoiExcelExporterBase, IHubContactsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HubContactsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHubContactForViewDto> hubContacts)
        {
            return CreateExcelPackage(
                "HubContacts.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("HubContacts"));

                    AddHeader(
                        sheet,
                        L("DisplayScore"),
                        (L("Hub")) + L("Name"),
                        (L("Contact")) + L("FullName")
                        );

                    AddObjects(
                        sheet, hubContacts,
                        _ => _.HubContact.DisplayScore,
                        _ => _.HubName,
                        _ => _.ContactFullName
                        );

                });
        }
    }
}