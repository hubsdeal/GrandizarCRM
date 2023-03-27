using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoreRelevantStoresExcelExporter : NpoiExcelExporterBase, IStoreRelevantStoresExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreRelevantStoresExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreRelevantStoreForViewDto> storeRelevantStores)
        {
            return CreateExcelPackage(
                "StoreRelevantStores.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("StoreRelevantStores"));

                    AddHeader(
                        sheet,
                        L("RelevantStoreId"),
                        (L("Store")) + L("Name")
                        );

                    AddObjects(
                        sheet, storeRelevantStores,
                        _ => _.StoreRelevantStore.RelevantStoreId,
                        _ => _.StoreName
                        );

                });
        }
    }
}