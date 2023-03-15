using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.LookupData.Exporting
{
    public class CountiesExcelExporter : NpoiExcelExporterBase, ICountiesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CountiesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCountyForViewDto> counties)
        {
            return CreateExcelPackage(
                "Counties.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Counties"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        (L("Country")) + L("Name"),
                        (L("State")) + L("Name")
                        );

                    AddObjects(
                        sheet, counties,
                        _ => _.County.Name,
                        _ => _.CountryName,
                        _ => _.StateName
                        );

                });
        }
    }
}