using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductFlashSaleProductMapsExcelExporter : NpoiExcelExporterBase, IProductFlashSaleProductMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductFlashSaleProductMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductFlashSaleProductMapForViewDto> productFlashSaleProductMaps)
        {
            return CreateExcelPackage(
                "ProductFlashSaleProductMaps.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductFlashSaleProductMaps"));

                    AddHeader(
                        sheet,
                        L("FlashSalePrice"),
                        L("DiscountPercentage"),
                        L("DiscountAmount"),
                        L("EndDate"),
                        L("EndTime"),
                        L("StartDate"),
                        L("StartTime"),
                        (L("Product")) + L("Name"),
                        (L("Store")) + L("Name"),
                        (L("MembershipType")) + L("Name")
                        );

                    AddObjects(
                        sheet, productFlashSaleProductMaps,
                        _ => _.ProductFlashSaleProductMap.FlashSalePrice,
                        _ => _.ProductFlashSaleProductMap.DiscountPercentage,
                        _ => _.ProductFlashSaleProductMap.DiscountAmount,
                        _ => _timeZoneConverter.Convert(_.ProductFlashSaleProductMap.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ProductFlashSaleProductMap.EndTime,
                        _ => _timeZoneConverter.Convert(_.ProductFlashSaleProductMap.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ProductFlashSaleProductMap.StartTime,
                        _ => _.ProductName,
                        _ => _.StoreName,
                        _ => _.MembershipTypeName
                        );

                    for (var i = 1; i <= productFlashSaleProductMaps.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(4); for (var i = 1; i <= productFlashSaleProductMaps.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[6], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(6);
                });
        }
    }
}