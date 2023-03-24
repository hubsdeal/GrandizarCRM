using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.JobManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.JobManagement.Exporting
{
    public class JobTagsExcelExporter : NpoiExcelExporterBase, IJobTagsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public JobTagsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetJobTagForViewDto> jobTags)
        {
            return CreateExcelPackage(
                "JobTags.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("JobTags"));

                    AddHeader(
                        sheet,
                        L("CustomTag"),
                        L("TagValue"),
                        L("Verified"),
                        L("Sequence"),
                        (L("Job")) + L("Title"),
                        (L("MasterTagCategory")) + L("Name"),
                        (L("MasterTag")) + L("Name")
                        );

                    AddObjects(
                        sheet, jobTags,
                        _ => _.JobTag.CustomTag,
                        _ => _.JobTag.TagValue,
                        _ => _.JobTag.Verified,
                        _ => _.JobTag.Sequence,
                        _ => _.JobTitle,
                        _ => _.MasterTagCategoryName,
                        _ => _.MasterTagName
                        );

                });
        }
    }
}