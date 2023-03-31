using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductCategoryVariantMapsExcelExporter : NpoiExcelExporterBase, IProductCategoryVariantMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductCategoryVariantMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductCategoryVariantMapForViewDto> productCategoryVariantMaps)
        {
            return CreateExcelPackage(
                "ProductCategoryVariantMaps.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductCategoryVariantMaps"));

                    AddHeader(
                        sheet,
                        (L("ProductCategory")) + L("Name"),
                        (L("ProductVariantCategory")) + L("Name")
                        );

                    AddObjects(
                        sheet, productCategoryVariantMaps,
                        _ => _.ProductCategoryName,
                        _ => _.ProductVariantCategoryName
                        );

                });
        }
    }
}