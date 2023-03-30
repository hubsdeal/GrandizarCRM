using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductPackagesExcelExporter : NpoiExcelExporterBase, IProductPackagesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductPackagesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductPackageForViewDto> productPackages)
        {
            return CreateExcelPackage(
                "ProductPackages.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductPackages"));

                    AddHeader(
                        sheet,
                        L("PackageProductId"),
                        L("DisplaySequence"),
                        L("Price"),
                        L("Quantity"),
                        (L("Product")) + L("Name"),
                        (L("MediaLibrary")) + L("Name")
                        );

                    AddObjects(
                        sheet, productPackages,
                        _ => _.ProductPackage.PackageProductId,
                        _ => _.ProductPackage.DisplaySequence,
                        _ => _.ProductPackage.Price,
                        _ => _.ProductPackage.Quantity,
                        _ => _.ProductName,
                        _ => _.MediaLibraryName
                        );

                });
        }
    }
}