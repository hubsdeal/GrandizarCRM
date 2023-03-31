using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.LookupData.Exporting
{
    public class ProductWholeSaleQuantityTypesExcelExporter : NpoiExcelExporterBase, IProductWholeSaleQuantityTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductWholeSaleQuantityTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductWholeSaleQuantityTypeForViewDto> productWholeSaleQuantityTypes)
        {
            return CreateExcelPackage(
                "ProductWholeSaleQuantityTypes.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductWholeSaleQuantityTypes"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("MinQty"),
                        L("MaxQty")
                        );

                    AddObjects(
                        sheet, productWholeSaleQuantityTypes,
                        _ => _.ProductWholeSaleQuantityType.Name,
                        _ => _.ProductWholeSaleQuantityType.MinQty,
                        _ => _.ProductWholeSaleQuantityType.MaxQty
                        );

                });
        }
    }
}