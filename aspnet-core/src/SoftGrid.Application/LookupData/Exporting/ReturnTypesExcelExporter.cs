using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.LookupData.Exporting
{
    public class ReturnTypesExcelExporter : NpoiExcelExporterBase, IReturnTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ReturnTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetReturnTypeForViewDto> returnTypes)
        {
            return CreateExcelPackage(
                "ReturnTypes.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ReturnTypes"));

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, returnTypes,
                        _ => _.ReturnType.Name
                        );

                });
        }
    }
}