using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Territory.Exporting
{
    public class HubAccountTeamsExcelExporter : NpoiExcelExporterBase, IHubAccountTeamsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HubAccountTeamsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHubAccountTeamForViewDto> hubAccountTeams)
        {
            return CreateExcelPackage(
                "HubAccountTeams.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("HubAccountTeams"));

                    AddHeader(
                        sheet,
                        L("PrimaryManager"),
                        L("StartDate"),
                        L("EndDate"),
                        (L("Hub")) + L("Name"),
                        (L("Employee")) + L("Name"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, hubAccountTeams,
                        _ => _.HubAccountTeam.PrimaryManager,
                        _ => _timeZoneConverter.Convert(_.HubAccountTeam.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.HubAccountTeam.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.HubName,
                        _ => _.EmployeeName,
                        _ => _.UserName
                        );

                    for (var i = 1; i <= hubAccountTeams.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[2], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(2); for (var i = 1; i <= hubAccountTeams.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3);
                });
        }
    }
}