using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductUpsellRelatedProductsExcelExporter : NpoiExcelExporterBase, IProductUpsellRelatedProductsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductUpsellRelatedProductsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductUpsellRelatedProductForViewDto> productUpsellRelatedProducts)
        {
            return CreateExcelPackage(
                "ProductUpsellRelatedProducts.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductUpsellRelatedProducts"));

                    AddHeader(
                        sheet,
                        L("RelatedProductId"),
                        L("DisplaySequence"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, productUpsellRelatedProducts,
                        _ => _.ProductUpsellRelatedProduct.RelatedProductId,
                        _ => _.ProductUpsellRelatedProduct.DisplaySequence,
                        _ => _.ProductName
                        );

                });
        }
    }
}