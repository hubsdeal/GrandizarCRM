using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoreAccountTeamsExcelExporter : NpoiExcelExporterBase, IStoreAccountTeamsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreAccountTeamsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreAccountTeamForViewDto> storeAccountTeams)
        {
            return CreateExcelPackage(
                "StoreAccountTeams.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("StoreAccountTeams"));

                    AddHeader(
                        sheet,
                        L("Primary"),
                        L("Active"),
                        L("OrderEmailNotification"),
                        L("OrderSmsNotification"),
                        (L("Store")) + L("Name"),
                        (L("Employee")) + L("Name")
                        );

                    AddObjects(
                        sheet, storeAccountTeams,
                        _ => _.StoreAccountTeam.Primary,
                        _ => _.StoreAccountTeam.Active,
                        _ => _.StoreAccountTeam.OrderEmailNotification,
                        _ => _.StoreAccountTeam.OrderSmsNotification,
                        _ => _.StoreName,
                        _ => _.EmployeeName
                        );

                });
        }
    }
}