using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Territory.Exporting
{
    public class HubZipCodeMapsExcelExporter : NpoiExcelExporterBase, IHubZipCodeMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HubZipCodeMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHubZipCodeMapForViewDto> hubZipCodeMaps)
        {
            return CreateExcelPackage(
                "HubZipCodeMaps.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("HubZipCodeMaps"));

                    AddHeader(
                        sheet,
                        L("CityName"),
                        L("ZipCode"),
                        (L("Hub")) + L("Name"),
                        (L("City")) + L("Name"),
                        (L("ZipCode")) + L("Name")
                        );

                    AddObjects(
                        sheet, hubZipCodeMaps,
                        _ => _.HubZipCodeMap.CityName,
                        _ => _.HubZipCodeMap.ZipCode,
                        _ => _.HubName,
                        _ => _.CityName,
                        _ => _.ZipCodeName
                        );

                });
        }
    }
}