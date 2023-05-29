using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.TaskManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.TaskManagement.Exporting
{
    public class TaskWorkItemsExcelExporter : NpoiExcelExporterBase, ITaskWorkItemsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TaskWorkItemsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTaskWorkItemForViewDto> taskWorkItems)
        {
            return CreateExcelPackage(
                    "TaskWorkItems.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("TaskWorkItems"));

                        AddHeader(
                            sheet,
                        L("Name"),
                        L("EstimatedHours"),
                        L("ActualHours"),
                        L("StartDate"),
                        L("EndDate"),
                        L("StartTime"),
                        L("EndTime"),
                        L("OpenOrClosed"),
                        L("CompletionPercentage"),
                        (L("TaskEvent")) + L("Name"),
                        (L("Employee")) + L("Name")
                            );

                        AddObjects(
                            sheet, taskWorkItems,
                        _ => _.TaskWorkItem.Name,
                        _ => _.TaskWorkItem.EstimatedHours,
                        _ => _.TaskWorkItem.ActualHours,
                        _ => _timeZoneConverter.Convert(_.TaskWorkItem.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.TaskWorkItem.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.TaskWorkItem.StartTime,
                        _ => _.TaskWorkItem.EndTime,
                        _ => _.TaskWorkItem.OpenOrClosed,
                        _ => _.TaskWorkItem.CompletionPercentage,
                        _ => _.TaskEventName,
                        _ => _.EmployeeName
                            );

                        for (var i = 1; i <= taskWorkItems.Count; i++)
                        {
                            SetCellDataFormat(sheet.GetRow(i).Cells[4 - 1], "yyyy-mm-dd");
                        }
                        sheet.AutoSizeColumn(4 - 1); for (var i = 1; i <= taskWorkItems.Count; i++)
                        {
                            SetCellDataFormat(sheet.GetRow(i).Cells[5 - 1], "yyyy-mm-dd");
                        }
                        sheet.AutoSizeColumn(5 - 1);
                    });

        }
    }
}