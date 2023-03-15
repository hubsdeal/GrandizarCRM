using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.LookupData.Exporting
{
    public class CitiesExcelExporter : NpoiExcelExporterBase, ICitiesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CitiesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCityForViewDto> cities)
        {
            return CreateExcelPackage(
                "Cities.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Cities"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        (L("Country")) + L("Name"),
                        (L("State")) + L("Name"),
                        (L("County")) + L("Name")
                        );

                    AddObjects(
                        sheet, cities,
                        _ => _.City.Name,
                        _ => _.CountryName,
                        _ => _.StateName,
                        _ => _.CountyName
                        );

                });
        }
    }
}