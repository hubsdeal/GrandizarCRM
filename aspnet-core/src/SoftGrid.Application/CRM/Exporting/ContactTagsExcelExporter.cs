using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.CRM.Exporting
{
    public class ContactTagsExcelExporter : NpoiExcelExporterBase, IContactTagsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ContactTagsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetContactTagForViewDto> contactTags)
        {
            return CreateExcelPackage(
                "ContactTags.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ContactTags"));

                    AddHeader(
                        sheet,
                        L("CustomTag"),
                        L("TagValue"),
                        L("Verified"),
                        L("Sequence"),
                        (L("Contact")) + L("FullName"),
                        (L("MasterTagCategory")) + L("Name"),
                        (L("MasterTag")) + L("Name")
                        );

                    AddObjects(
                        sheet, contactTags,
                        _ => _.ContactTag.CustomTag,
                        _ => _.ContactTag.TagValue,
                        _ => _.ContactTag.Verified,
                        _ => _.ContactTag.Sequence,
                        _ => _.ContactFullName,
                        _ => _.MasterTagCategoryName,
                        _ => _.MasterTagName
                        );

                });
        }
    }
}