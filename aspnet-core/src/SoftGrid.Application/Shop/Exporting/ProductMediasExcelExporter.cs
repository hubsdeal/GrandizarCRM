using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductMediasExcelExporter : NpoiExcelExporterBase, IProductMediasExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductMediasExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductMediaForViewDto> productMedias)
        {
            return CreateExcelPackage(
                "ProductMedias.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductMedias"));

                    AddHeader(
                        sheet,
                        L("DisplaySequence"),
                        (L("Product")) + L("Name"),
                        (L("MediaLibrary")) + L("Name")
                        );

                    AddObjects(
                        sheet, productMedias,
                        _ => _.ProductMedia.DisplaySequence,
                        _ => _.ProductName,
                        _ => _.MediaLibraryName
                        );

                });
        }
    }
}