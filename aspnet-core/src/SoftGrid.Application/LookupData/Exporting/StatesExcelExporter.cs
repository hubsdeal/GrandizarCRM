using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.LookupData.Exporting
{
    public class StatesExcelExporter : NpoiExcelExporterBase, IStatesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StatesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStateForViewDto> states)
        {
            return CreateExcelPackage(
                "States.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("States"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Ticker"),
                        (L("Country")) + L("Name")
                        );

                    AddObjects(
                        sheet, states,
                        _ => _.State.Name,
                        _ => _.State.Ticker,
                        _ => _.CountryName
                        );

                });
        }
    }
}