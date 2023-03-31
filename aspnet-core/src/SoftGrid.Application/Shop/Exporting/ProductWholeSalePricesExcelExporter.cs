using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductWholeSalePricesExcelExporter : NpoiExcelExporterBase, IProductWholeSalePricesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductWholeSalePricesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductWholeSalePriceForViewDto> productWholeSalePrices)
        {
            return CreateExcelPackage(
                "ProductWholeSalePrices.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductWholeSalePrices"));

                    AddHeader(
                        sheet,
                        L("Price"),
                        L("ExactQuantity"),
                        L("PackageInfo"),
                        L("PackageQuantity"),
                        L("WholeSaleSkuId"),
                        (L("Product")) + L("Name"),
                        (L("ProductWholeSaleQuantityType")) + L("Name"),
                        (L("MeasurementUnit")) + L("Name"),
                        (L("Currency")) + L("Name")
                        );

                    AddObjects(
                        sheet, productWholeSalePrices,
                        _ => _.ProductWholeSalePrice.Price,
                        _ => _.ProductWholeSalePrice.ExactQuantity,
                        _ => _.ProductWholeSalePrice.PackageInfo,
                        _ => _.ProductWholeSalePrice.PackageQuantity,
                        _ => _.ProductWholeSalePrice.WholeSaleSkuId,
                        _ => _.ProductName,
                        _ => _.ProductWholeSaleQuantityTypeName,
                        _ => _.MeasurementUnitName,
                        _ => _.CurrencyName
                        );

                });
        }
    }
}