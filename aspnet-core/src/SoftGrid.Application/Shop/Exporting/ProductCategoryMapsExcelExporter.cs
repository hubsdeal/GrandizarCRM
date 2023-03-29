using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductCategoryMapsExcelExporter : NpoiExcelExporterBase, IProductCategoryMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductCategoryMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductCategoryMapForViewDto> productCategoryMaps)
        {
            return CreateExcelPackage(
                "ProductCategoryMaps.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductCategoryMaps"));

                    AddHeader(
                        sheet,
                        (L("Product")) + L("Name"),
                        (L("ProductCategory")) + L("Name")
                        );

                    AddObjects(
                        sheet, productCategoryMaps,
                        _ => _.ProductName,
                        _ => _.ProductCategoryName
                        );

                });
        }
    }
}