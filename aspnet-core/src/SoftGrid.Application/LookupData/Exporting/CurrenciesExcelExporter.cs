using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.LookupData.Exporting
{
    public class CurrenciesExcelExporter : NpoiExcelExporterBase, ICurrenciesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CurrenciesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCurrencyForViewDto> currencies)
        {
            return CreateExcelPackage(
                "Currencies.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Currencies"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Ticker"),
                        L("Icon")
                        );

                    AddObjects(
                        sheet, currencies,
                        _ => _.Currency.Name,
                        _ => _.Currency.Ticker,
                        _ => _.Currency.Icon
                        );

                });
        }
    }
}