using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.CRM.Exporting
{
    public class BusinessAccountTeamsExcelExporter : NpoiExcelExporterBase, IBusinessAccountTeamsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BusinessAccountTeamsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBusinessAccountTeamForViewDto> businessAccountTeams)
        {
            return CreateExcelPackage(
                "BusinessAccountTeams.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("BusinessAccountTeams"));

                    AddHeader(
                        sheet,
                        L("Primary"),
                        (L("Business")) + L("Name"),
                        (L("Employee")) + L("Name")
                        );

                    AddObjects(
                        sheet, businessAccountTeams,
                        _ => _.BusinessAccountTeam.Primary,
                        _ => _.BusinessName,
                        _ => _.EmployeeName
                        );

                });
        }
    }
}