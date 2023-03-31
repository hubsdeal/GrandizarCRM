using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.DiscountManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.DiscountManagement.Exporting
{
    public class DiscountCodeGeneratorsExcelExporter : NpoiExcelExporterBase, IDiscountCodeGeneratorsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DiscountCodeGeneratorsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDiscountCodeGeneratorForViewDto> discountCodeGenerators)
        {
            return CreateExcelPackage(
                "DiscountCodeGenerators.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("DiscountCodeGenerators"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("CouponCode"),
                        L("PercentageOrFixedAmount"),
                        L("DiscountPercentage"),
                        L("DiscountAmount"),
                        L("StartDate"),
                        L("EndDate"),
                        L("AdminNotes"),
                        L("IsActive"),
                        L("StartTime"),
                        L("EndTime")
                        );

                    AddObjects(
                        sheet, discountCodeGenerators,
                        _ => _.DiscountCodeGenerator.Name,
                        _ => _.DiscountCodeGenerator.CouponCode,
                        _ => _.DiscountCodeGenerator.PercentageOrFixedAmount,
                        _ => _.DiscountCodeGenerator.DiscountPercentage,
                        _ => _.DiscountCodeGenerator.DiscountAmount,
                        _ => _timeZoneConverter.Convert(_.DiscountCodeGenerator.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.DiscountCodeGenerator.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.DiscountCodeGenerator.AdminNotes,
                        _ => _.DiscountCodeGenerator.IsActive,
                        _ => _.DiscountCodeGenerator.StartTime,
                        _ => _.DiscountCodeGenerator.EndTime
                        );

                    for (var i = 1; i <= discountCodeGenerators.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[6], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(6); for (var i = 1; i <= discountCodeGenerators.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[7], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(7);
                });
        }
    }
}