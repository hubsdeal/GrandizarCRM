using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductCrossSellProductsExcelExporter : NpoiExcelExporterBase, IProductCrossSellProductsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductCrossSellProductsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductCrossSellProductForViewDto> productCrossSellProducts)
        {
            return CreateExcelPackage(
                "ProductCrossSellProducts.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductCrossSellProducts"));

                    AddHeader(
                        sheet,
                        L("CrossProductId"),
                        L("CrossSellScore"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, productCrossSellProducts,
                        _ => _.ProductCrossSellProduct.CrossProductId,
                        _ => _.ProductCrossSellProduct.CrossSellScore,
                        _ => _.ProductName
                        );

                });
        }
    }
}