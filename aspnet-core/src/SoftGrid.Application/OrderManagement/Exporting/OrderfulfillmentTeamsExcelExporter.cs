using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.OrderManagement.Exporting
{
    public class OrderfulfillmentTeamsExcelExporter : NpoiExcelExporterBase, IOrderfulfillmentTeamsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OrderfulfillmentTeamsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOrderfulfillmentTeamForViewDto> orderfulfillmentTeams)
        {
            return CreateExcelPackage(
                "OrderfulfillmentTeams.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("OrderfulfillmentTeams"));

                    AddHeader(
                        sheet,
                        (L("Order")) + L("FullName"),
                        (L("Employee")) + L("Name"),
                        (L("Contact")) + L("FullName"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, orderfulfillmentTeams,
                        _ => _.OrderFullName,
                        _ => _.EmployeeName,
                        _ => _.ContactFullName,
                        _ => _.UserName
                        );

                });
        }
    }
}