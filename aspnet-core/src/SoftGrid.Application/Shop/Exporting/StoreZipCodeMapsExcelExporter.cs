using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoreZipCodeMapsExcelExporter : NpoiExcelExporterBase, IStoreZipCodeMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreZipCodeMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreZipCodeMapForViewDto> storeZipCodeMaps)
        {
            return CreateExcelPackage(
                "StoreZipCodeMaps.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("StoreZipCodeMaps"));

                    AddHeader(
                        sheet,
                        L("ZipCode"),
                        (L("Store")) + L("Name"),
                        (L("ZipCode")) + L("Name")
                        );

                    AddObjects(
                        sheet, storeZipCodeMaps,
                        _ => _.StoreZipCodeMap.ZipCode,
                        _ => _.StoreName,
                        _ => _.ZipCodeName
                        );

                });
        }
    }
}