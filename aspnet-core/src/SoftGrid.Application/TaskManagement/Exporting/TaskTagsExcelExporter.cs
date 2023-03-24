using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.TaskManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.TaskManagement.Exporting
{
    public class TaskTagsExcelExporter : NpoiExcelExporterBase, ITaskTagsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TaskTagsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTaskTagForViewDto> taskTags)
        {
            return CreateExcelPackage(
                "TaskTags.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("TaskTags"));

                    AddHeader(
                        sheet,
                        L("CustomTag"),
                        L("TagValue"),
                        L("Verfied"),
                        L("Sequence"),
                        (L("TaskEvent")) + L("Name"),
                        (L("MasterTagCategory")) + L("Name"),
                        (L("MasterTag")) + L("Name")
                        );

                    AddObjects(
                        sheet, taskTags,
                        _ => _.TaskTag.CustomTag,
                        _ => _.TaskTag.TagValue,
                        _ => _.TaskTag.Verfied,
                        _ => _.TaskTag.Sequence,
                        _ => _.TaskEventName,
                        _ => _.MasterTagCategoryName,
                        _ => _.MasterTagName
                        );

                });
        }
    }
}