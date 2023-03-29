using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductCustomerQueriesExcelExporter : NpoiExcelExporterBase, IProductCustomerQueriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductCustomerQueriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductCustomerQueryForViewDto> productCustomerQueries)
        {
            return CreateExcelPackage(
                "ProductCustomerQueries.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductCustomerQueries"));

                    AddHeader(
                        sheet,
                        L("Question"),
                        L("Answer"),
                        L("AnswerDate"),
                        L("AnswerTime"),
                        L("Publish"),
                        (L("Product")) + L("Name"),
                        (L("Contact")) + L("FullName"),
                        (L("Employee")) + L("Name")
                        );

                    AddObjects(
                        sheet, productCustomerQueries,
                        _ => _.ProductCustomerQuery.Question,
                        _ => _.ProductCustomerQuery.Answer,
                        _ => _timeZoneConverter.Convert(_.ProductCustomerQuery.AnswerDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ProductCustomerQuery.AnswerTime,
                        _ => _.ProductCustomerQuery.Publish,
                        _ => _.ProductName,
                        _ => _.ContactFullName,
                        _ => _.EmployeeName
                        );

                    for (var i = 1; i <= productCustomerQueries.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3);
                });
        }
    }
}