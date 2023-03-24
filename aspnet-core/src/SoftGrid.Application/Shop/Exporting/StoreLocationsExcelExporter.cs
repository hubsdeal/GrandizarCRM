using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoreLocationsExcelExporter : NpoiExcelExporterBase, IStoreLocationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreLocationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreLocationForViewDto> storeLocations)
        {
            return CreateExcelPackage(
                "StoreLocations.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("StoreLocations"));

                    AddHeader(
                        sheet,
                        L("LocationName"),
                        L("FullAddress"),
                        L("Latitude"),
                        L("Longitude"),
                        L("Address"),
                        L("Mobile"),
                        L("Email"),
                        L("ZipCode"),
                        (L("City")) + L("Name"),
                        (L("State")) + L("Name"),
                        (L("Country")) + L("Name"),
                        (L("Store")) + L("Name")
                        );

                    AddObjects(
                        sheet, storeLocations,
                        _ => _.StoreLocation.LocationName,
                        _ => _.StoreLocation.FullAddress,
                        _ => _.StoreLocation.Latitude,
                        _ => _.StoreLocation.Longitude,
                        _ => _.StoreLocation.Address,
                        _ => _.StoreLocation.Mobile,
                        _ => _.StoreLocation.Email,
                        _ => _.StoreLocation.ZipCode,
                        _ => _.CityName,
                        _ => _.StateName,
                        _ => _.CountryName,
                        _ => _.StoreName
                        );

                });
        }
    }
}