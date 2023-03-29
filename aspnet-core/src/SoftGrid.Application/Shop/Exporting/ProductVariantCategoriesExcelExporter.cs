using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductVariantCategoriesExcelExporter : NpoiExcelExporterBase, IProductVariantCategoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductVariantCategoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductVariantCategoryForViewDto> productVariantCategories)
        {
            return CreateExcelPackage(
                "ProductVariantCategories.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductVariantCategories"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        (L("Store")) + L("Name")
                        );

                    AddObjects(
                        sheet, productVariantCategories,
                        _ => _.ProductVariantCategory.Name,
                        _ => _.StoreName
                        );

                });
        }
    }
}