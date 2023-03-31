using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.DiscountManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.DiscountManagement.Exporting
{
    public class DiscountCodeUserHistoriesExcelExporter : NpoiExcelExporterBase, IDiscountCodeUserHistoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DiscountCodeUserHistoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDiscountCodeUserHistoryForViewDto> discountCodeUserHistories)
        {
            return CreateExcelPackage(
                "DiscountCodeUserHistories.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("DiscountCodeUserHistories"));

                    AddHeader(
                        sheet,
                        L("Amount"),
                        L("Date"),
                        (L("DiscountCodeGenerator")) + L("Name"),
                        (L("Order")) + L("InvoiceNumber"),
                        (L("Contact")) + L("FullName")
                        );

                    AddObjects(
                        sheet, discountCodeUserHistories,
                        _ => _timeZoneConverter.Convert(_.DiscountCodeUserHistory.Amount, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.DiscountCodeUserHistory.Date, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.DiscountCodeGeneratorName,
                        _ => _.OrderInvoiceNumber,
                        _ => _.ContactFullName
                        );

                    for (var i = 1; i <= discountCodeUserHistories.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[1], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(1); for (var i = 1; i <= discountCodeUserHistories.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[2], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(2);
                });
        }
    }
}