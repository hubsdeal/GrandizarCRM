using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductTaskMapsExcelExporter : NpoiExcelExporterBase, IProductTaskMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductTaskMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductTaskMapForViewDto> productTaskMaps)
        {
            return CreateExcelPackage(
                    "ProductTaskMaps.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("ProductTaskMaps"));

                        AddHeader(
                            sheet,
                        (L("Product")) + L("Name"),
                        (L("TaskEvent")) + L("Name"),
                        (L("ProductCategory")) + L("Name")
                            );

                        AddObjects(
                            sheet, productTaskMaps,
                        _ => _.ProductName,
                        _ => _.TaskEventName,
                        _ => _.ProductCategoryName
                            );

                    });

        }
    }
}