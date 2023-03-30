using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Territory.Exporting
{
    public class HubSalesProjectionsExcelExporter : NpoiExcelExporterBase, IHubSalesProjectionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HubSalesProjectionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHubSalesProjectionForViewDto> hubSalesProjections)
        {
            return CreateExcelPackage(
                "HubSalesProjections.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("HubSalesProjections"));

                    AddHeader(
                        sheet,
                        L("DurationTypeId"),
                        L("StartDate"),
                        L("EndDate"),
                        L("ExpectedSalesAmount"),
                        L("ActualSalesAmount"),
                        (L("Hub")) + L("Name"),
                        (L("ProductCategory")) + L("Name"),
                        (L("Store")) + L("Name"),
                        (L("Currency")) + L("Name")
                        );

                    AddObjects(
                        sheet, hubSalesProjections,
                        _ => _.HubSalesProjection.DurationTypeId,
                        _ => _timeZoneConverter.Convert(_.HubSalesProjection.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.HubSalesProjection.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.HubSalesProjection.ExpectedSalesAmount,
                        _ => _.HubSalesProjection.ActualSalesAmount,
                        _ => _.HubName,
                        _ => _.ProductCategoryName,
                        _ => _.StoreName,
                        _ => _.CurrencyName
                        );

                    for (var i = 1; i <= hubSalesProjections.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[2], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(2); for (var i = 1; i <= hubSalesProjections.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3);
                });
        }
    }
}