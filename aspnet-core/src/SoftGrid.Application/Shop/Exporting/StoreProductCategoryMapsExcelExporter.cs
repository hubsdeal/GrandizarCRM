using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoreProductCategoryMapsExcelExporter : NpoiExcelExporterBase, IStoreProductCategoryMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreProductCategoryMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreProductCategoryMapForViewDto> storeProductCategoryMaps)
        {
            return CreateExcelPackage(
                "StoreProductCategoryMaps.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("StoreProductCategoryMaps"));

                    AddHeader(
                        sheet,
                        L("Published"),
                        L("DisplaySequence"),
                        (L("Store")) + L("Name"),
                        (L("ProductCategory")) + L("Name")
                        );

                    AddObjects(
                        sheet, storeProductCategoryMaps,
                        _ => _.StoreProductCategoryMap.Published,
                        _ => _.StoreProductCategoryMap.DisplaySequence,
                        _ => _.StoreName,
                        _ => _.ProductCategoryName
                        );

                });
        }
    }
}