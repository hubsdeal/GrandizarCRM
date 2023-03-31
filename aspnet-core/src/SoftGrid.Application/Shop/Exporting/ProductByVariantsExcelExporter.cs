using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductByVariantsExcelExporter : NpoiExcelExporterBase, IProductByVariantsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductByVariantsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductByVariantForViewDto> productByVariants)
        {
            return CreateExcelPackage(
                "ProductByVariants.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductByVariants"));

                    AddHeader(
                        sheet,
                        L("Price"),
                        L("DisplaySequence"),
                        L("Description"),
                        (L("Product")) + L("Name"),
                        (L("ProductVariant")) + L("Name"),
                        (L("ProductVariantCategory")) + L("Name"),
                        (L("MediaLibrary")) + L("Name")
                        );

                    AddObjects(
                        sheet, productByVariants,
                        _ => _.ProductByVariant.Price,
                        _ => _.ProductByVariant.DisplaySequence,
                        _ => _.ProductByVariant.Description,
                        _ => _.ProductName,
                        _ => _.ProductVariantName,
                        _ => _.ProductVariantCategoryName,
                        _ => _.MediaLibraryName
                        );

                });
        }
    }
}