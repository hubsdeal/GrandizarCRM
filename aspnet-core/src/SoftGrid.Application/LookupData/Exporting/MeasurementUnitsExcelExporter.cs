using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.LookupData.Exporting
{
    public class MeasurementUnitsExcelExporter : NpoiExcelExporterBase, IMeasurementUnitsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MeasurementUnitsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMeasurementUnitForViewDto> measurementUnits)
        {
            return CreateExcelPackage(
                "MeasurementUnits.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("MeasurementUnits"));

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, measurementUnits,
                        _ => _.MeasurementUnit.Name
                        );

                });
        }
    }
}