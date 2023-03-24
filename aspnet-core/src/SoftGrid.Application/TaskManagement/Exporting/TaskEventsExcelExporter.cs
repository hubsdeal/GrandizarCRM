using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.TaskManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.TaskManagement.Exporting
{
    public class TaskEventsExcelExporter : NpoiExcelExporterBase, ITaskEventsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TaskEventsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTaskEventForViewDto> taskEvents)
        {
            return CreateExcelPackage(
                "TaskEvents.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("TaskEvents"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Status"),
                        L("Priority"),
                        L("EventDate"),
                        L("StartTime"),
                        L("EndTime"),
                        L("Template"),
                        L("ActualTime"),
                        L("EndDate"),
                        L("EstimatedTime"),
                        L("HourAndMinutes"),
                        (L("TaskStatus")) + L("Name")
                        );

                    AddObjects(
                        sheet, taskEvents,
                        _ => _.TaskEvent.Name,
                        _ => _.TaskEvent.Description,
                        _ => _.TaskEvent.Status,
                        _ => _.TaskEvent.Priority,
                        _ => _timeZoneConverter.Convert(_.TaskEvent.EventDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.TaskEvent.StartTime,
                        _ => _.TaskEvent.EndTime,
                        _ => _.TaskEvent.Template,
                        _ => _.TaskEvent.ActualTime,
                        _ => _timeZoneConverter.Convert(_.TaskEvent.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.TaskEvent.EstimatedTime,
                        _ => _.TaskEvent.HourAndMinutes,
                        _ => _.TaskStatusName
                        );

                    for (var i = 1; i <= taskEvents.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[5], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(5); for (var i = 1; i <= taskEvents.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[10], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(10);
                });
        }
    }
}