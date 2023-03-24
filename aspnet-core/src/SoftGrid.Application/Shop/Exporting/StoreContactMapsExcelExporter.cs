using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoreContactMapsExcelExporter : NpoiExcelExporterBase, IStoreContactMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreContactMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreContactMapForViewDto> storeContactMaps)
        {
            return CreateExcelPackage(
                "StoreContactMaps.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("StoreContactMaps"));

                    AddHeader(
                        sheet,
                        L("PaidCustomer"),
                        L("LifeTimeSalesAmount"),
                        (L("Store")) + L("Name"),
                        (L("Contact")) + L("FullName")
                        );

                    AddObjects(
                        sheet, storeContactMaps,
                        _ => _.StoreContactMap.PaidCustomer,
                        _ => _.StoreContactMap.LifeTimeSalesAmount,
                        _ => _.StoreName,
                        _ => _.ContactFullName
                        );

                });
        }
    }
}