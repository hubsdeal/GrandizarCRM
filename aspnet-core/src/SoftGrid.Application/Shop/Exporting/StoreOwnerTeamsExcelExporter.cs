using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoreOwnerTeamsExcelExporter : NpoiExcelExporterBase, IStoreOwnerTeamsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreOwnerTeamsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreOwnerTeamForViewDto> storeOwnerTeams)
        {
            return CreateExcelPackage(
                "StoreOwnerTeams.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("StoreOwnerTeams"));

                    AddHeader(
                        sheet,
                        L("Active"),
                        L("Primary"),
                        L("OrderEmailNotification"),
                        L("OrderSmsNotification"),
                        (L("Store")) + L("Name"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, storeOwnerTeams,
                        _ => _.StoreOwnerTeam.Active,
                        _ => _.StoreOwnerTeam.Primary,
                        _ => _.StoreOwnerTeam.OrderEmailNotification,
                        _ => _.StoreOwnerTeam.OrderSmsNotification,
                        _ => _.StoreName,
                        _ => _.UserName
                        );

                });
        }
    }
}