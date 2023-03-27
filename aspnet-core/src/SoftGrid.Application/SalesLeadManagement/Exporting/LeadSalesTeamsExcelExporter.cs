using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.SalesLeadManagement.Exporting
{
    public class LeadSalesTeamsExcelExporter : NpoiExcelExporterBase, ILeadSalesTeamsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LeadSalesTeamsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLeadSalesTeamForViewDto> leadSalesTeams)
        {
            return CreateExcelPackage(
                "LeadSalesTeams.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("LeadSalesTeams"));

                    AddHeader(
                        sheet,
                        L("Primary"),
                        L("AssignedDate"),
                        (L("Lead")) + L("FirstName"),
                        (L("Employee")) + L("Name")
                        );

                    AddObjects(
                        sheet, leadSalesTeams,
                        _ => _.LeadSalesTeam.Primary,
                        _ => _timeZoneConverter.Convert(_.LeadSalesTeam.AssignedDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.LeadFirstName,
                        _ => _.EmployeeName
                        );

                    for (var i = 1; i <= leadSalesTeams.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[2], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(2);
                });
        }
    }
}