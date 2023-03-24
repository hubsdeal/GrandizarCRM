using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoreBusinessCustomerMapsExcelExporter : NpoiExcelExporterBase, IStoreBusinessCustomerMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreBusinessCustomerMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreBusinessCustomerMapForViewDto> storeBusinessCustomerMaps)
        {
            return CreateExcelPackage(
                "StoreBusinessCustomerMaps.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("StoreBusinessCustomerMaps"));

                    AddHeader(
                        sheet,
                        L("PaidCustomer"),
                        L("LifeTimeSalesAmount"),
                        (L("Store")) + L("Name"),
                        (L("Business")) + L("Name")
                        );

                    AddObjects(
                        sheet, storeBusinessCustomerMaps,
                        _ => _.StoreBusinessCustomerMap.PaidCustomer,
                        _ => _.StoreBusinessCustomerMap.LifeTimeSalesAmount,
                        _ => _.StoreName,
                        _ => _.BusinessName
                        );

                });
        }
    }
}