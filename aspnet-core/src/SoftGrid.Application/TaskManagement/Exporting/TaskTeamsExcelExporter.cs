using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.TaskManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.TaskManagement.Exporting
{
    public class TaskTeamsExcelExporter : NpoiExcelExporterBase, ITaskTeamsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TaskTeamsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTaskTeamForViewDto> taskTeams)
        {
            return CreateExcelPackage(
                    "TaskTeams.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("TaskTeams"));

                        AddHeader(
                            sheet,
                        L("StartDate"),
                        L("StartTime"),
                        L("EndTime"),
                        L("HourMinutes"),
                        L("EndDate"),
                        L("IsPrimary"),
                        L("EstimatedHour"),
                        L("SubTaskTitle"),
                        (L("TaskEvent")) + L("Name"),
                        (L("Employee")) + L("Name"),
                        (L("Contact")) + L("FullName")
                            );

                        AddObjects(
                            sheet, taskTeams,
                        _ => _timeZoneConverter.Convert(_.TaskTeam.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.TaskTeam.StartTime,
                        _ => _.TaskTeam.EndTime,
                        _ => _.TaskTeam.HourMinutes,
                        _ => _timeZoneConverter.Convert(_.TaskTeam.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.TaskTeam.IsPrimary,
                        _ => _.TaskTeam.EstimatedHour,
                        _ => _.TaskTeam.SubTaskTitle,
                        _ => _.TaskEventName,
                        _ => _.EmployeeName,
                        _ => _.ContactFullName
                            );

                        for (var i = 1; i <= taskTeams.Count; i++)
                        {
                            SetCellDataFormat(sheet.GetRow(i).Cells[1 - 1], "yyyy-mm-dd");
                        }
                        sheet.AutoSizeColumn(1 - 1); for (var i = 1; i <= taskTeams.Count; i++)
                        {
                            SetCellDataFormat(sheet.GetRow(i).Cells[5 - 1], "yyyy-mm-dd");
                        }
                        sheet.AutoSizeColumn(5 - 1);
                    });

        }
    }
}