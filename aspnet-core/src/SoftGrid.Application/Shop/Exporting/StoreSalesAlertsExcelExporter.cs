using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoreSalesAlertsExcelExporter : NpoiExcelExporterBase, IStoreSalesAlertsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreSalesAlertsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreSalesAlertForViewDto> storeSalesAlerts)
        {
            return CreateExcelPackage(
                "StoreSalesAlerts.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("StoreSalesAlerts"));

                    AddHeader(
                        sheet,
                        L("Message"),
                        L("Current"),
                        L("StartDate"),
                        L("EndDate"),
                        (L("Store")) + L("Name")
                        );

                    AddObjects(
                        sheet, storeSalesAlerts,
                        _ => _.StoreSalesAlert.Message,
                        _ => _.StoreSalesAlert.Current,
                        _ => _timeZoneConverter.Convert(_.StoreSalesAlert.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.StoreSalesAlert.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.StoreName
                        );

                    for (var i = 1; i <= storeSalesAlerts.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3); for (var i = 1; i <= storeSalesAlerts.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(4);
                });
        }
    }
}