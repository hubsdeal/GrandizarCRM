using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.LookupData.Exporting
{
    public class CountriesExcelExporter : NpoiExcelExporterBase, ICountriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CountriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCountryForViewDto> countries)
        {
            return CreateExcelPackage(
                "Countries.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Countries"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Ticker"),
                        L("FlagIcon"),
                        L("PhoneCode")
                        );

                    AddObjects(
                        sheet, countries,
                        _ => _.Country.Name,
                        _ => _.Country.Ticker,
                        _ => _.Country.FlagIcon,
                        _ => _.Country.PhoneCode
                        );

                });
        }
    }
}