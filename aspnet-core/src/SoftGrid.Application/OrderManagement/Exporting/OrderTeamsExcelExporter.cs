using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.OrderManagement.Exporting
{
    public class OrderTeamsExcelExporter : NpoiExcelExporterBase, IOrderTeamsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OrderTeamsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOrderTeamForViewDto> orderTeams)
        {
            return CreateExcelPackage(
                "OrderTeams.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("OrderTeams"));

                    AddHeader(
                        sheet,
                        (L("Order")) + L("InvoiceNumber"),
                        (L("Employee")) + L("Name")
                        );

                    AddObjects(
                        sheet, orderTeams,
                        _ => _.OrderInvoiceNumber,
                        _ => _.EmployeeName
                        );

                });
        }
    }
}