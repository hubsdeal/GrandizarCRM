using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.CRM.Exporting
{
    public class ContactTaskMapsExcelExporter : NpoiExcelExporterBase, IContactTaskMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ContactTaskMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetContactTaskMapForViewDto> contactTaskMaps)
        {
            return CreateExcelPackage(
                    "ContactTaskMaps.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("ContactTaskMaps"));

                        AddHeader(
                            sheet,
                        (L("Contact")) + L("FullName"),
                        (L("TaskEvent")) + L("Name")
                            );

                        AddObjects(
                            sheet, contactTaskMaps,
                        _ => _.ContactFullName,
                        _ => _.TaskEventName
                            );

                    });

        }
    }
}