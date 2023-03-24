using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.CRM.Exporting
{
    public class BusinessTagsExcelExporter : NpoiExcelExporterBase, IBusinessTagsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BusinessTagsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBusinessTagForViewDto> businessTags)
        {
            return CreateExcelPackage(
                "BusinessTags.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("BusinessTags"));

                    AddHeader(
                        sheet,
                        L("CustomTag"),
                        L("TagValue"),
                        L("Verified"),
                        L("Sequence"),
                        (L("Business")) + L("Name"),
                        (L("MasterTagCategory")) + L("Name"),
                        (L("MasterTag")) + L("Name")
                        );

                    AddObjects(
                        sheet, businessTags,
                        _ => _.BusinessTag.CustomTag,
                        _ => _.BusinessTag.TagValue,
                        _ => _.BusinessTag.Verified,
                        _ => _.BusinessTag.Sequence,
                        _ => _.BusinessName,
                        _ => _.MasterTagCategoryName,
                        _ => _.MasterTagName
                        );

                });
        }
    }
}