using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.SalesLeadManagement.Exporting
{
    public class LeadTagsExcelExporter : NpoiExcelExporterBase, ILeadTagsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LeadTagsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLeadTagForViewDto> leadTags)
        {
            return CreateExcelPackage(
                "LeadTags.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("LeadTags"));

                    AddHeader(
                        sheet,
                        L("CustomTag"),
                        L("TagValue"),
                        L("DisplaySequence"),
                        (L("Lead")) + L("Title"),
                        (L("MasterTagCategory")) + L("Name"),
                        (L("MasterTag")) + L("Name")
                        );

                    AddObjects(
                        sheet, leadTags,
                        _ => _.LeadTag.CustomTag,
                        _ => _.LeadTag.TagValue,
                        _ => _.LeadTag.DisplaySequence,
                        _ => _.LeadTitle,
                        _ => _.MasterTagCategoryName,
                        _ => _.MasterTagName
                        );

                });
        }
    }
}